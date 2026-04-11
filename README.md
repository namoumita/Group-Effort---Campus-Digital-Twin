# Campus Digital Twin — Real-Time IoT Analytics Dashboard

A collaborative project built for La Trobe University's Digital Innovation Hub (DIH), visualising live campus building data through an interactive dashboard connected to a Unity 3D model.

## What It Does
- Ingests live IoT sensor data every 5 seconds via a real-time data pipeline
- Displays building telemetry (environmental, occupancy, solar) on an interactive dashboard
- Connects backend data streams to a Unity 3D campus model via WebSocket integration
- Enables dynamic visual interactions using LeanTween animations

## Tech Stack
| Layer | Technology |
|---|---|
| Backend API | Python, FastAPI |
| Database | PostgreSQL |
| Real-time streaming | WebSocket, Kafka, QuestDB |
| Frontend / Visualisation | HTML, CSS, JavaScript |
| 3D Integration | Unity, custom DLL |
| Version Control | Git, GitHub |

## Repository Structure
- `digital_twin_fast_api/` — FastAPI backend and REST endpoints
- `Dashboard(UI&WebSocket&UnityIntegration)/` — Frontend dashboard and WebSocket client
- `Kafka-QuestDB-Integration/` — Streaming pipeline setup
- `database/` — PostgreSQL schema and setup scripts
- `DataPreparationScripts/` — Data cleaning and preparation
- `DigitalTwinUnity/` — Unity 3D model integration

## Key Features
- End-to-end data pipeline: IoT sensors → Kafka → QuestDB → FastAPI → Dashboard
- Live 5-second refresh cycle for real-time building telemetry
- Custom DLL bridging Python backend with Unity 3D visualisation
- Dataset validation notebooks for UniCon and UniSolar sensor data

## Team
This was a group project developed as part of the Master of Data Science programme at La Trobe University (February – June 2025).
