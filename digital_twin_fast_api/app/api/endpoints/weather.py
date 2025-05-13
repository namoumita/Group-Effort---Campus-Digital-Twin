from fastapi import APIRouter, Query
from fastapi.responses import StreamingResponse
from typing import List, Optional
from app.models.sensor_data import SensorData
from app.api.utils import generate_dummy_data, generate_streaming_data, save_to_db

router = APIRouter()

@router.get("/", response_model=List[SensorData])
def get_weather_data(count: int = 10, campus: Optional[str] = Query(None), building: Optional[str] = Query(None)):
    return generate_dummy_data("weather", count, campus, building)

@router.get("/stream")
async def stream_weather_data():
    return StreamingResponse(generate_streaming_data("weather"), media_type="text/event-stream")

@router.post("/unity", status_code=201)
def push_weather_data(data: SensorData):
    save_to_db(data)
    return {"message": "Weather data received."}