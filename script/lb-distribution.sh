#!/usr/bin/env bash
# Розподіл N послідовних запитів по інстансах (Git Bash).
# Використання: ./script/lb-distribution.sh [URL] [N]
URL="${1:-http://localhost:8080/api/Companies/GetAll}"
N="${2:-100}"
for _ in $(seq 1 "$N"); do
  curl -s -o /dev/null -D - "$URL" | grep -i '^x-instance-id' | tr -d '\r' | awk '{print $2}'
done | sort | uniq -c
