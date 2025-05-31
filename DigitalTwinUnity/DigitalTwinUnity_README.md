# Digital Twin Unity3D Project

This Unity3D project is part of the Campus Digital Twin system developed by Team Maverick for the CSE5IDP subject at La Trobe University. The Unity environment visualizes real-time infrastructure data such as occupancy, temperature, lighting, and energy usage streamed from a FastAPI backend.

---

## 🧩 Features

- Real-time 3D feedback based on sensor data
- Color-coded visual cues for occupancy and light levels
- Modular room configuration with support for multiple zones
- Data fetched from local FastAPI backend using REST endpoints

---

## 🛠️ Requirements

- Unity Editor 2022.3+ (LTS recommended)
- FastAPI backend running on `http://127.0.0.1:8000/`
- macOS / Windows compatible

---

## 🚀 How to Run

1. **Start the FastAPI Backend**
   Navigate to `digital_twin_fast_api/` and run:
   ```bash
   uvicorn app.main:app --reload
   ```

2. **Open Unity**
   - Launch Unity Hub
   - Add the `DigitalTwinUnity/` folder as a project
   - Open and load the main scene (e.g., `RoomScene.unity`)

3. **Trigger Real-Time Updates**
   Use `curl` or Postman to send a POST request:
   ```bash
   curl -X POST http://127.0.0.1:8000/mock-twin/twin/Room101 \
   -H "Content-Type: application/json" \
   -d '{"temperature": 28.5, "occupancy": true, "lightLevel": 90}'
   ```

4. **Watch the Scene React**
   - Rooms will update color or objects based on backend state
   - You can use `/unity/{twin_id}` to fetch status if needed

---

## 🗂️ Project Structure

```
DigitalTwinUnity/
├── Assets/                  # Main Unity assets
│   ├── Scenes/              # Unity scenes (e.g., RoomScene)
│   ├── Scripts/             # C# scripts for REST API calls, data parsing
│   └── Prefabs/             # Visual elements (rooms, indicators, etc.)
├── Packages/
├── ProjectSettings/
└── README.md                # This file
```

---

## 👨‍💻 Developers

- Tithir Mahmud Bakshi  
- Sayuru Gunawardana  
- Mashrukh Namooumita  
- Likith Reddy  
- Babanpreet Singh  
- Nafisa Ahmaad  

---

## 📬 Notes

- All data is simulated. For real-world usage, replace the backend with Azure Digital Twins endpoints or real IoT feeds.
- The Unity scene expects responses from `/twin/{id}` or `/unity/{id}`.
