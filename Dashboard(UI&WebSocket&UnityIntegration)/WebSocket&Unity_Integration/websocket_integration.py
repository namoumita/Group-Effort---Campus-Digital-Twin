# websocket_manager.py - Add this to your FastAPI project

from fastapi import WebSocket, WebSocketDisconnect
from typing import Dict, List
import json
import asyncio
from datetime import datetime

class ConnectionManager:
    def __init__(self):
        # Store active connections by type
        self.active_connections: Dict[str, List[WebSocket]] = {
            "dashboard": [],
            "unity": [],
            "mobile": []  # For future mobile app integration
        }
        
    async def connect(self, websocket: WebSocket, client_type: str):
        await websocket.accept()
        if client_type not in self.active_connections:
            self.active_connections[client_type] = []
        self.active_connections[client_type].append(websocket)
        print(f"New {client_type} client connected. Total: {len(self.active_connections[client_type])}")
        
    def disconnect(self, websocket: WebSocket, client_type: str):
        if client_type in self.active_connections:
            if websocket in self.active_connections[client_type]:
                self.active_connections[client_type].remove(websocket)
        print(f"{client_type} client disconnected. Remaining: {len(self.active_connections.get(client_type, []))}")
        
    async def send_personal_message(self, message: str, websocket: WebSocket):
        await websocket.send_text(message)
        
    async def broadcast_to_type(self, message: dict, client_type: str):
        """Send message to all clients of a specific type"""
        if client_type in self.active_connections:
            disconnected = []
            for connection in self.active_connections[client_type]:
                try:
                    await connection.send_text(json.dumps(message))
                except:
                    disconnected.append(connection)
            
            # Remove disconnected clients
            for conn in disconnected:
                self.active_connections[client_type].remove(conn)
                
    async def bridge_message(self, message: dict, from_type: str, to_type: str):
        """Send message from one client type to another"""
        message["from"] = from_type
        message["timestamp"] = datetime.now().isoformat()
        await self.broadcast_to_type(message, to_type)
        
    async def broadcast_to_all(self, message: dict):
        """Send message to all connected clients"""
        for client_type in self.active_connections:
            await self.broadcast_to_type(message, client_type)

# Create global connection manager instance
manager = ConnectionManager()

# Add this to your main.py or routes.py
from fastapi import FastAPI, WebSocket, WebSocketDisconnect

app = FastAPI()  # Your existing FastAPI app

@app.websocket("/ws/{client_type}")
async def websocket_endpoint(websocket: WebSocket, client_type: str):
    await manager.connect(websocket, client_type)
    try:
        while True:
            # Receive message from client
            data = await websocket.receive_text()
            message = json.loads(data)
            
            # Process different types of messages
            await handle_websocket_message(message, client_type, websocket)
            
    except WebSocketDisconnect:
        manager.disconnect(websocket, client_type)

async def handle_websocket_message(message: dict, from_client: str, websocket: WebSocket):
    """Handle incoming WebSocket messages and route them appropriately"""
    
    message_type = message.get("type")
    
    if message_type == "dashboard_click":
        # Dashboard clicked on something - send to Unity
        unity_message = {
            "type": "highlight_component",
            "component": message.get("component"),
            "data": message.get("data"),
            "action": "highlight"
        }
        await manager.bridge_message(unity_message, "dashboard", "unity")
        
    elif message_type == "unity_click":
        # Unity clicked on something - send to Dashboard
        dashboard_message = {
            "type": "focus_data",
            "component": message.get("component"),
            "data": message.get("data"),
            "action": "focus"
        }
        await manager.bridge_message(dashboard_message, "unity", "dashboard")
        
    elif message_type == "data_update":
        # Real-time data update - broadcast to all
        await manager.broadcast_to_all({
            "type": "data_update",
            "sensor": message.get("sensor"),
            "value": message.get("value"),
            "timestamp": datetime.now().isoformat()
        })
        
    elif message_type == "alert":
        # System alert - broadcast to all
        await manager.broadcast_to_all({
            "type": "alert",
            "severity": message.get("severity", "info"),
            "message": message.get("message"),
            "component": message.get("component")
        })
        
    elif message_type == "sync_request":
        # Client requesting sync - send current state
        current_state = await get_current_system_state()
        await manager.send_personal_message(json.dumps({
            "type": "sync_response",
            "state": current_state
        }), websocket)

async def get_current_system_state():
    """Get current state of all systems for sync"""
    # You would implement this to return current sensor values
    return {
        "electricity": {"current": 25.5, "status": "normal"},
        "solar": {"current": 18.2, "status": "good"},
        "water": {"current": 45.8, "status": "normal"},
        "temperature": {"current": 22.5, "humidity": 65, "status": "comfortable"}
    }

# Helper function to send data updates to all connected clients
async def broadcast_sensor_data(sensor_type: str, value: float, metadata: dict = None):
    """Call this function when you get new sensor data"""
    message = {
        "type": "sensor_update",
        "sensor": sensor_type,
        "value": value,
        "metadata": metadata or {},
        "timestamp": datetime.now().isoformat()
    }
    await manager.broadcast_to_all(message)

# Example: Modify your existing endpoint to also broadcast via WebSocket
@app.get("/api/electricity")
async def get_electricity_data():
    # Your existing code to get electricity data
    electricity_data = {"power": 25.5, "voltage": 240, "current": 10.6}
    
    # Broadcast to WebSocket clients
    await broadcast_sensor_data("electricity", electricity_data["power"], electricity_data)
    
    return electricity_data