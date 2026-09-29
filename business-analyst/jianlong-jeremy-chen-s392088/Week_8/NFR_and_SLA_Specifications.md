# 01 - Enterprise Non-Functional Requirements (NFRs) & SLA Specifications

**Module:** PRT681 / PRT585 - Business Analyst (BA)  
**Week:** Week 5  
**Objective:** Define enterprise-grade quality benchmarks, constraints, and service level agreements (SLAs) for the "Reorder Past Meal & Checkout" feature.

---

## 1. 3-Line Structure Notes
* **What it is:** Non-Functional Requirements (NFRs) specify system operational qualities, performance thresholds, security baselines, and architectural constraints rather than specific user behaviors.
* **Why it is used:** To prevent system crashes under heavy load, define measurable technical Service Level Agreements (SLAs) for engineering teams, and avoid post-launch latency or security vulnerabilities.
* **Simple Example:** Mandating that the checkout reorder endpoint handles $\ge 500\text{ requests/sec}$ with a 95th percentile response latency under 2.0 seconds while enforcing TLS 1.3 encryption.

---

## 2. NFR Specifications & SLA Matrix

| Category | NFR ID | Requirement Specification | SLA Target / Benchmark | Verification Method |
| :--- | :--- | :--- | :--- | :--- |
| **Performance** | `NFR-PERF-01` | Reorder API end-to-end response latency | Latency $< 2.0\text{ s}$ under standard load (95th percentile) | JMeter / Postman Load Testing |
| **Concurrency** | `NFR-PERF-02` | Peak lunch-hour concurrent order throughput | Support $\ge 500\text{ TPS}$ without dropped requests or timeouts | Stepped Concurrency Load Test |
| **Availability** | `NFR-AVAIL-01` | Customer-facing ordering service uptime | High availability $\ge 99.9\%$ during peak dining hours | Cloud Monitoring Dashboard (Grafana) |
| **Security** | `NFR-SEC-01` | Data-in-transit and payment token encryption | Enforce TLS 1.3 for transport; AES-256 for data at rest | Security Audit & SSL Probe Inspection |
| **Data Integrity**| `NFR-DATA-01` | Inventory check and cart sync consistency | Zero duplicate allocations of sold-out items; read lag $< 500\text{ ms}$ | Transaction Isolation Test & SQL Verification |

---

## 3. BA Acceptance & Sign-off Criteria
1. Performance metrics must pass a 1-hour sustained load test in the staging environment with an error rate $< 0.1\%$.
2. Security scans must show zero critical or high-severity CVE vulnerabilities before release authorization.
3. Cloud infrastructure must implement auto-scaling policies, spinning up new instances within 90 seconds if CPU utilization exceeds 75%.