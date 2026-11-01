// Навантажувальний тест через k6. Запуск (Windows cmd, з кореня репозиторію):
//   docker run --rm -i --network hlse-asv_backend -e VUS=50 -e DURATION=30s grafana/k6 run - < script\k6-load.js
// Змінні: VUS, DURATION, BASE_URL (за замовч. http://nginx), TARGET_PATH (за замовч. /api/Companies/GetAll)
import http from 'k6/http';
import { check } from 'k6';
import { Counter } from 'k6/metrics';

const hits = new Counter('hits');
const BASE = __ENV.BASE_URL || 'http://nginx';
const TARGET = __ENV.TARGET_PATH || '/api/Companies/GetAll';

export const options = {
  vus: Number(__ENV.VUS || 50),
  duration: __ENV.DURATION || '30s',
  summaryTrendStats: ['avg', 'med', 'p(90)', 'p(95)', 'p(99)', 'max'],
  thresholds: {
    // Порожні пороги змушують k6 показати розбивку лічильника по інстансах
    'hits{instance:company-1}': ['count>=0'],
    'hits{instance:company-2}': ['count>=0'],
    'hits{instance:company-3}': ['count>=0'],
    'hits{instance:unknown}': ['count>=0'],
    'http_req_failed': ['rate<1'],
  },
};

export default function () {
  const res = http.get(`${BASE}${TARGET}`);
  const id = res.headers['X-Instance-Id'] || res.headers['X-Instance-ID'] || 'unknown';
  hits.add(1, { instance: id });
  check(res, { 'status < 500': (r) => r.status > 0 && r.status < 500 });
}
