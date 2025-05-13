from pydantic import BaseModel

class SensorData(BaseModel):
    timestamp: str
    campus: str
    building: str
    utility_type: str
    value: float