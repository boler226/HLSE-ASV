#!/usr/bin/env bash
# Restart / Instance Loss: аварійна зупинка companies-service-1 під час операцій.
# Усі запити йдуть через nginx (round-robin).
set -uo pipefail

LB=${LB:-http://localhost:8080}
API=api/Companies
IDS=()

create() {
  local n=$1
  local resp inst body
  resp=$(curl -s -i -X POST "$LB/$API/Create" -H 'Content-Type: application/json' \
        -d "{\"name\":\"Fail-$n\",\"email\":\"fail-$n-$RANDOM@test.com\",\"address\":\"Kyiv\"}")
  inst=$(echo "$resp" | grep -i '^x-instance-id' | tr -d '\r' | awk '{print $2}')
  body=$(echo "$resp" | tail -n1 | tr -d '\r"')
  echo "  create #$n -> ${inst:-NO-RESPONSE} id=$body"
  [[ -n "$body" && ${#body} -ge 32 ]] && IDS+=("$body")
}

echo "== Фаза 1: обидва instances живі =="
for i in 1 2 3 4; do create "$i"; done

echo "== docker kill companies-service-1 =="
docker kill companies-service-1 >/dev/null
sleep 1

echo "== Фаза 2: працює лише instance 2 =="
for i in 5 6 7 8; do create "$i"; done

echo "== Перевірка цілісності: читаємо всі створені записи =="
ok=0; fail=0
for id in "${IDS[@]}"; do
  resp=$(curl -s -i "$LB/$API/GetById/$id")
  status=$(echo "$resp" | head -n1 | tr -d '\r')
  inst=$(echo "$resp" | grep -i '^x-instance-id' | tr -d '\r' | awk '{print $2}')
  if echo "$status" | grep -q "200"; then ok=$((ok+1)); else fail=$((fail+1)); fi
  echo "  $id -> $status ($inst)"
done
echo "Створено: ${#IDS[@]} | Прочитано успішно: $ok | Втрачено: $fail"

echo "== Відновлення instance 1 =="
docker start companies-service-1 >/dev/null
echo "Готово. Зачекайте ~10 с і повторіть GET — обидва instances знову обслуговують запити."
