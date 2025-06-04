# 🎮 Unity User Manual – Campus Digital Twin

## Purpose

This manual provides step-by-step instructions to operate, modify, and maintain the Unity3D frontend for the Campus Digital Twin system developed by Team Maverick. This expands the existing project README into a full user guide.

---

## 1. Project Structure

| Folder     | Purpose                                        |
| ---------- | ---------------------------------------------- |
| `Assets/`  | Core Unity assets, including models and scenes |
| `Scenes/`  | Contains `RoomScene.unity` (main environment)  |
| `Scripts/` | Unity C# scripts (e.g., API polling, visuals)  |
| `Prefabs/` | Visual indicators for temperature, light, etc  |
| `Models/`  | Imported 3D mesh assets (download separately)  |

---

## 2. Running the Unity App

1. **Start FastAPI Backend**

   ```bash
   cd digital_twin_fast_api
   uvicorn app.main:app --reload
   ```
2. **Open Unity**

   * Launch Unity Hub
   * Add the `DigitalTwinUnity/` folder as a project
   * Load the main scene (`RoomScene.unity` in `Scenes/`)
3. **Enter Play Mode**

   * Click ▶️ (Play) to simulate room telemetry updates

---

## 3. Live Data Integration

* Script: `UnityWebRequestHandler.cs`
* REST API Endpoint: `http://127.0.0.1:8000/unity/Room101`

### Sample Trigger

```bash
curl -X POST http://127.0.0.1:8000/mock-twin/twin/Room101 \
-H "Content-Type: application/json" \
-d '{"temperature": 28.5, "occupancy": true, "lightLevel": 90}'
```

> ✅ The Unity dashboard will change visuals based on data.

---

## 4. Scene Behavior & Feedback

| Input Condition   | Visual Feedback               |
| ----------------- | ----------------------------- |
| Temperature > 40  | Background turns red (alert)  |
| Light level < 10  | Scene dims to low-light mode  |
| Occupancy = false | Lights turn off, idle overlay |
| Normal conditions | Green-lit normal operation    |

---

## 5. Common Issues & Fixes

| Issue                    | Resolution                                                      |
| ------------------------ | --------------------------------------------------------------- |
| Scene not updating       | Ensure backend is running and room ID matches API               |
| Unity asset missing      | Download `Textured_mesh_1_Selection.obj` (see Notes section)    |
| Red background always on | Check telemetry: high temp (> 40°C) triggers alert mode         |
| Data doesn't sync        | Confirm URL matches Unity script and CORS is enabled in FastAPI |

---

## 6. Adding New Models

1. Download `.fbx` or `.obj` asset
2. Place it in `Assets/Models/`
3. Drag it into the active scene
4. Adjust position, materials, and lighting
5. Save scene
6. Commit with Git LFS:

```bash
git lfs track "*.fbx"
git add . && git commit -m "Add model"
```

---

## 7. Exporting the Build

* Open **File > Build Settings**
* Platform: `PC, Mac & Linux Standalone`
* Target Folder: `Build/`
* Click **Build and Run**

---

## 8. Developer & Credits

* Maintainer: Tithir Mahmud Bakshi
* Environment: Unity 2022.3+, macOS/Windows
* Course: CSE5IDP – La Trobe University

---


## 9 Notes

- All data is simulated. For real-world usage, replace the backend with Azure Digital Twins endpoints or real IoT feeds.
- The Unity scene expects responses from `/twin/{id}` or `/unity/{id}`.
- > ⚠️ Note: Due to GitHub's file size limits, the 3D mesh `Textured_mesh_1_Selection.obj` (328MB) is stored externally.  
[Download it here](https://drive.google.com/drive/folders/1G-Lqvh6PigunnbMpi9_WNhfoY3tVTbXR?usp=sharing) and place it in:
`DigitalTwinUnity/Assets/Models/`

