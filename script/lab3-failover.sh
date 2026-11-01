#!/usr/bin/env bash
# Демонстрація failover (Git Bash): під час потоку запитів зупиняємо один backend, потім піднімаємо.
# Використання: ./script/lab3-failover.sh      (змінні: URL, VICTIM, TOTAL, STOP_AT, START_AT)
URL="${URL:-http://localhost:8080/api/Companies/GetAll}"
VICTIM="${VICTIM:-companies-service-2}"
TOTAL="${TOTAL:-300}"
STOP_AT="${STOP_AT:-100}"
START_AT="${START_AT:-200}"

declare -A CODES INST
HDR="$(mktemp)"
errors5xx=0

for i in $(seq 1 "$TOTAL"); do
  if [ "$i" -eq "$STOP_AT" ]; then echo ">>> [$i] docker stop $VICTIM"; docker stop "$VICTIM" >/dev/null; fi
  if [ "$i" -eq "$START_AT" ]; then echo ">>> [$i] docker start $VICTIM"; docker start "$VICTIM" >/dev/null; fi

  out=$(curl -s -o /dev/null -D "$HDR" --max-time 10 -w '%{http_code} %{time_total}' "$URL")
  code="${out%% *}"; t="${out##* }"
  id=$(grep -i '^x-instance-id' "$HDR" | awk '{print $2}' | tr -d '\r')
  CODES[$code]=$(( ${CODES[$code]:-0} + 1 ))
  INST[${id:-none}]=$(( ${INST[${id:-none}]:-0} + 1 ))
  [ "$code" -ge 500 ] 2>/dev/null && errors5xx=$((errors5xx+1))
  # повільні відповіді (>0.5с) = момент, коли nginx робив retry на живий вузол
  awk -v t="$t" 'BEGIN{exit !(t>0.5)}' && echo "slow: req=$i code=$code inst=${id:-none} time=${t}s"
done
rm -f "$HDR"

echo; echo "=== HTTP-коди ==="; for k in "${!CODES[@]}"; do echo "$k: ${CODES[$k]}"; done
echo "=== Розподіл по інстансах ==="; for k in "${!INST[@]}"; do echo "$k: ${INST[$k]}"; done
echo "=== 5xx: $errors5xx ==="
