from fastapi import FastAPI
from app.api.routes import router
from app.db.database import init_db

app = FastAPI(
    title="Digital Twin Backend",
    version="1.0.0",
    description="Provides real-time and historical infrastructure data for digital twin visualization."
)

@app.on_event("startup")
def startup_event():
    init_db()

app.include_router(router)