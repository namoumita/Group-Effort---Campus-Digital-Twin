from datetime import datetime, timedelta
import random
from typing import List
import asyncio
import json
from app.models.sensor_data import SensorData
from app.db.database import SessionLocal
from app.db.models import SensorDataDB

CAMPUSES = ["Bundoora", "City", "Shepparton", "Albury-Wodonga", "Bendigo"]
BUILDINGS = ["Science Wing", "Library", "Admin Block", "Engineering Block", "Student Center"]

def save_to_db(data: SensorData):
    db = SessionLocal()
    db_obj = SensorDataDB(
        timestamp=datetime.fromisoformat(data.timestamp),
        campus=data.campus,
        building=data.building,
        utility_type=data.utility_type,
        value=data.value
    )
    db.add(db_obj)
    db.commit()
    db.close()

def generate_dummy_data(utility_type: str, count: int = 10, campus=None, building=None) -> List[SensorData]:
    now = datetime.now()
    data = [
        SensorData(
            timestamp=(now - timedelta(minutes=random.randint(0, 1000))).isoformat(),
            campus=random.choice(CAMPUSES),
            building=random.choice(BUILDINGS),
            utility_type=utility_type,
            value=round(random.uniform(50.0, 500.0), 2)
        ) for _ in range(count)
    ]
    for entry in data:
        save_to_db(entry)
    if campus:
        data = [d for d in data if d.campus == campus]
    if building:
        data = [d for d in data if d.building == building]
    return data

async def generate_streaming_data(utility_type: str):
    while True:
        data = SensorData(
            timestamp=datetime.now().isoformat(),
            campus=random.choice(CAMPUSES),
            building=random.choice(BUILDINGS),
            utility_type=utility_type,
            value=round(random.uniform(50.0, 500.0), 2)
        )
        save_to_db(data)
        yield f"data: {json.dumps(data.dict())}\n\n"
        await asyncio.sleep(2)