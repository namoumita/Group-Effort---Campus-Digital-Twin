
from fastapi import APIRouter
from pydantic import BaseModel
from datetime import datetime
from typing import Dict

router = APIRouter()

# In-memory storage for "twins"
MOCK_TWINS: Dict[str, dict] = {}

# Model
class TwinData(BaseModel):
    temperature: float
    occupancy: bool
    lightLevel: int
    last_updated: str = datetime.now().isoformat()

@router.get("/twin/{twin_id}")
def get_twin(twin_id: str):
    return MOCK_TWINS.get(twin_id, {"error": "Twin not found"})

@router.post("/twin/{twin_id}")
def upsert_twin(twin_id: str, data: TwinData):
    MOCK_TWINS[twin_id] = data.dict()
    MOCK_TWINS[twin_id]["last_updated"] = datetime.now().isoformat()
    return {"status": "updated", "data": MOCK_TWINS[twin_id]}

@router.get("/unity/{twin_id}")
def unity_polling(twin_id: str):
    # Simulate Unity polling the backend for latest twin state
    twin = MOCK_TWINS.get(twin_id)
    if twin:
        return {
            "id": twin_id,
            "temperature": twin["temperature"],
            "occupancy": twin["occupancy"],
            "lightLevel": twin["lightLevel"]
        }
    return {"error": "No twin data available"}
