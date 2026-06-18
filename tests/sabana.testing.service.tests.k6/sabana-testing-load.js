/**
 * sabana.testing.service - Pruebas de Carga con k6
 * Equivalente al plan JMeter: sabana-testing-load.jmx
 * Unidad 5 - Luis Alberto Rojas Adames
 *
 * Escenario 1: Carga Normal  → 10 VUs, ramp 30s, duración 60s
 * Escenario 2: Carga Alta    → 50 VUs, ramp 10s, duración 60s
 *
 * Ejecución:
 *   k6 run sabana-testing-load.js
 *
 * Con variables de entorno:
 *   k6 run -e HOST=localhost -e PORT=5121 sabana-testing-load.js
 *
 * Exportar resultados a CSV:
 *   k6 run --out csv=results/k6-results.csv sabana-testing-load.js
 */

import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend, Counter } from 'k6/metrics';

// ─── Configuración de host ────────────────────────────────────────────────────
const HOST = __ENV.HOST || 'localhost';
const PORT = __ENV.PORT || '5121';
const BASE_URL = `http://${HOST}:${PORT}`;

// ─── Métricas personalizadas ──────────────────────────────────────────────────
const getProductsTrend = new Trend('get_products_duration', true);
const postProductsTrend = new Trend('post_products_duration', true);
const failedRequests = new Counter('failed_requests');

// ─── Definición de escenarios ─────────────────────────────────────────────────
export const options = {
  scenarios: {
    // Escenario 1 - Carga Normal (10 usuarios)
    escenario1_carga_normal: {
      executor: 'ramping-vus',
      startVUs: 0,
      stages: [
        { duration: '15s', target: 10 }, // ramp-up: 0 → 10 VUs en 15s
        { duration: '40s', target: 10 }, // sostener 10 VUs por 40s
        { duration: '5s',  target: 0  }, // ramp-down
      ],
      gracefulRampDown: '5s',
      tags: { escenario: 'escenario1_carga_normal' },
    },

    // Escenario 2 - Carga Alta (50 usuarios)
    escenario2_carga_alta: {
      executor: 'ramping-vus',
      startVUs: 0,
      stages: [
        { duration: '10s', target: 50 }, // ramp-up: 0 → 50 VUs en 10s
        { duration: '45s', target: 50 }, // sostener 50 VUs por 45s
        { duration: '5s',  target: 0  }, // ramp-down
      ],
      gracefulRampDown: '5s',
      tags: { escenario: 'escenario2_carga_alta' },
    },
  },

  // Umbrales equivalentes a las aserciones JMeter
  thresholds: {
    // Todos los requests deben responder dentro de 10s (response_timeout de JMeter)
    http_req_duration: ['p(95)<10000'],
    // Tasa de fallos debe ser 0 (on_sample_error: continue pero se reporta)
    failed_requests: ['count==0'],
    // Tiempos específicos por endpoint
    get_products_duration:  ['p(95)<10000'],
    post_products_duration: ['p(95)<10000'],
  },
};

// ─── Cabeceras HTTP (equivalente al HeaderManager de JMeter) ──────────────────
const headers = {
  'Content-Type': 'application/json',
  'Accept':       'application/json',
};

// ─── Función principal (ejecutada por cada VU en cada iteración) ──────────────
export default function () {
  // ── GET /api/products ──────────────────────────────────────────────────────
  const getRes = http.get(`${BASE_URL}/api/products`, {
    headers,
    timeout: '10s',    // response_timeout = 10000ms
    tags: { name: 'GET /api/products' },
  });

  getProductsTrend.add(getRes.timings.duration);

  const getOk = check(getRes, {
    // Assert HTTP 200 (equivalente a ResponseAssertion de JMeter)
    'GET /api/products → HTTP 200': (r) => r.status === 200,
  });

  if (!getOk) {
    failedRequests.add(1);
    console.error(`[GET /api/products] Esperaba HTTP 200, obtuvo: ${getRes.status}`);
  }

  // ── POST /api/products ─────────────────────────────────────────────────────
  // Equivalente a: "Producto-${__threadNum}-${__Random(1000,9999)}"
  const vuId    = __VU;
  const randNum = Math.floor(Math.random() * (9999 - 1000 + 1)) + 1000;
  const price   = Math.floor(Math.random() * 500) + 1; // __Random(1,500)

  const payload = JSON.stringify({
    name:  `Producto-${vuId}-${randNum}`,
    price: price,
  });

  const postRes = http.post(`${BASE_URL}/api/products`, payload, {
    headers,
    timeout: '10s',
    tags: { name: 'POST /api/products' },
  });

  postProductsTrend.add(postRes.timings.duration);

  const postOk = check(postRes, {
    // Assert HTTP 201 (equivalente a ResponseAssertion de JMeter)
    'POST /api/products → HTTP 201': (r) => r.status === 201,
  });

  if (!postOk) {
    failedRequests.add(1);
    console.error(`[POST /api/products] Esperaba HTTP 201, obtuvo: ${postRes.status}`);
  }

  // Pequeña pausa entre iteraciones (think time)
  sleep(1);
}
