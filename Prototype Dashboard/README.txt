1) Understanding the Architecture



<Dashboard (Web) ↔ Communication Layer ↔ Unity 3D Model>


The communication happens through WebSockets between the web dashboard and Unity.



2) Communication Method

WebSockets 

Real-time bidirectional communication
Instant updates both ways
Better for live synchronization


3) Implementation Plan

Phase 1: Set Up WebSocket Server (FastAPI Side)
FastAPI will act as the bridge between dashboard and Unity.

Phase 2: Modify Dashboard to Send/Receive Events
The dashboard will send events like "user clicked electricity chart" and receive events like "Unity highlighted solar panel."

Phase 3: Unity Integration
Unity will connect to WebSocket server to send/receive commands.

4) Codes Include

WebSocket Integration for Dashboard-Unity Bridge
Real-Time Data Dashboard (updated version5)
Unity WebSocket Client for Dashboard Integration 


5) Implementation Instructions
Dashboard Side:

a) Add the WebSocket code to FastAPI project:

Copy the WebSocket manager code into a new file websocket_manager.py
Add the WebSocket endpoint to main.py
Install required dependency: pip install websockets


b) Test the enhanced dashboard:

The dashboard now has click handlers on each chart
When clicked a chart, it sends a message to Unity
WebSocket connection status is shown in top-right corner


For Unity Side:

a) Install required Unity packages:

WebSocketSharp: Window → Package Manager → Add package from git URL → https://github.com/sta/websocket-sharp.git
Newtonsoft.Json: Should be built-in, if not install via Package Manager


b) Add the Unity script:

Create the UnityWebSocketClient.cs script
Attach it to a GameObject in the scene
Assign the building component GameObjects in the inspector


c) Make building components clickable:

Add colliders to building parts that are desired to be clickable
The script handles click detection automatically

6) Testing the Integration
Test Scenario 1: Dashboard to Unity

Start the FastAPI server
Open the dashboard in browser
Start Unity with the WebSocket client
Click on any chart in the dashboard
Watch the corresponding building component highlight in Unity

Test Scenario 2: Unity to Dashboard

Click on a building component in Unity
Watch the dashboard focus on the corresponding chart
See the chart highlight and scroll into view












