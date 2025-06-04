#  Mock Dashboard README – FastAPI Twin Endpoint

## Overview

This module allows developers and testers to simulate telemetry data (temperature, occupancy, and light levels) for individual rooms. It connects to the Unity3D frontend through RESTful endpoints.

---

## 1. Running the Server

### Start FastAPI (with Uvicorn):

```bash
uvicorn main:app --reload
```

### Access the API documentation:

* Open your browser: [http://127.0.0.1:8000/docs](http://127.0.0.1:8000/docs)

---

## 2. Sending Mock Telemetry Data

```bash
curl -X POST http://127.0.0.1:8000/mock-twin/twin/Room101 \
-H "Content-Type: application/json" \
-d '{"temperature": 25.5, "occupancy": true, "lightLevel": 80}'
```

### Turning Alert Mode ON (Red Background in Unity):

```bash
curl -X POST http://127.0.0.1:8000/mock-twin/twin/Room101 \
-H "Content-Type: application/json" \
-d '{"temperature": 45.0, "occupancy": false, "lightLevel": 10}'
```

---

## 3. API Reference

| Route                  | Method | Description                         |
| ---------------------- | ------ | ----------------------------------- |
| `/mock-twin/twin/{id}` | POST   | Insert/update mock telemetry        |
| `/unity/{id}`          | GET    | Fetch mock data (used by Unity app) |

---

## 4. Sample Testing Scenarios

| Scenario            | Input JSON                                                    | Expected Unity Feedback       |
| ------------------- | ------------------------------------------------------------- | ----------------------------- |
| Normal Room         | `{ "temperature": 23, "occupancy": true, "lightLevel": 75 }`  | Green background, live values |
| Alert Trigger (Hot) | `{ "temperature": 45, "occupancy": false, "lightLevel": 10 }` | Red background, alert mode    |
| Low Light Condition | `{ "temperature": 21, "occupancy": true, "lightLevel": 5 }`   | UI dimmed background          |

---

## 5. Azure Digital Twins Compatibility (Future Upgrade)

To prepare for ADT migration:

* Mirror mock payload to match DTDL schema (temperature, occupancy, etc.)
* Replace POST call with Azure SDK method (`digitaltwins_client.update_digital_twin`)
* Ensure `az login` and tenant config is valid

---

## Maintainer Info

Maintainer: Tithir Mahmud Bakshi
FastAPI Version: 0.110+
Python: 3.10+
Last Updated: Sprint 6 (Week 12)
