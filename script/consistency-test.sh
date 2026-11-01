#!/usr/bin/env bash
# Cross-Instance Consistency: POST -> inst1, GET -> inst2, PUT -> inst2, GET -> inst1
# Без sticky sessions: звертаємось напряму до кожного instance.
set -euo pipefail

I1=${I1:-http://localhost:5011}
I2=${I2:-http://localhost:5012}
API=api/Companies
EMAIL="acme-$RANDOM@test.com"

# call METHOD URL [JSON_BODY]  -> друкує instance + body, body зберігає в $BODY
call() {
  local method=$1 url=$2 data=${3:-}
  local resp
  if [[ -n "$data" ]]; then
    resp=$(curl -s -i -X "$method" "$url" -H 'Content-Type: application/json' -d "$data")
  else
    resp=$(curl -s -i -X "$method" "$url")
  fi
  local inst status
  inst=$(echo "$resp" | grep -i '^x-instance-id' | tr -d '\r' | awk '{print $2}')
  status=$(echo "$resp" | head -n1 | tr -d '\r')
  BODY=$(echo "$resp" | tail -n1 | tr -d '\r')
  echo "  [$method] $status | X-Instance-ID: ${inst:-?} | body: $BODY"
}

echo "1) POST -> instance 1"
call POST "$I1/$API/Create" "{\"name\":\"Acme\",\"email\":\"$EMAIL\",\"address\":\"Kyiv\"}"
ID=$(echo "$BODY" | tr -d '"')

echo "2) GET  -> instance 2 (має побачити запис, створений на instance 1)"
call GET "$I2/$API/GetById/$ID"
echo "$BODY" | grep -q "Acme" && echo "  PASS" || { echo "  FAIL"; exit 1; }

echo "3) PUT  -> instance 2"
call PUT "$I2/$API/Update" "{\"id\":\"$ID\",\"name\":\"Acme Updated\",\"email\":\"$EMAIL\",\"address\":\"Lviv\"}"

echo "4) GET  -> instance 1 (має побачити зміну, зроблену на instance 2)"
call GET "$I1/$API/GetById/$ID"
echo "$BODY" | grep -q "Acme Updated" && echo "  PASS: дані актуальні" || { echo "  FAIL: stale data"; exit 1; }

echo "Cross-instance consistency: OK"
