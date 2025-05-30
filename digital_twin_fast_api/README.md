#  Digital Twin Dummy Backend API

A lightweight FastAPI-based backend simulating **real-time infrastructure and operational (I&O) data** for campus buildings. Built for integration with Unity 3D visualisation, mobile frontends, and Azure Digital Twins.

---

## Features

✅ Simulated real-time sensor data  
✅ Endpoints for multiple utility types (electricity, solar, water, weather)  
✅ Static and streaming (SSE) endpoints  
✅ Easily extendable for real-world sensor or BMS integration  
✅ Designed to be integrated with Unity3D or other visualisation engines

---

## Project Structure

```bash
digital_twin/
│
├── app/
│   ├── api/
│   │   ├── routes.py
│   │   ├── endpoints/
│   │   │   ├── electricity.py
│   │   │   ├── solar.py
│   │   │   ├── water.py
│   │   │   ├── weather.py
│   │   │   └── health.py
│   │   └── utils.py
│   └── models/
│       └── sensor_data.py
│
├── main.py
├── requirements.txt
└── README.md


#Please run these for this to run the code in your system
#in terminal or command prompt, go to project directory
pip install -r requirements.txt

uvicorn main:app --reload

Open in browser: http://127.0.0.1:8000/docs

Health Check: GET /health

Static Data Endpoints:
GET /electricity
GET /solar
GET /water
GET /weather

Real-Time Streaming Endpoints (Server-Sent Events)
GET /electricity/stream
GET /solar/stream
GET /water/stream
GET /weather/stream







