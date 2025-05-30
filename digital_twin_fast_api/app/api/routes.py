from fastapi import APIRouter
from app.api.endpoints import electricity, solar, water, weather, health

router = APIRouter()
router.include_router(electricity.router, prefix="/electricity", tags=["Electricity"])
router.include_router(solar.router, prefix="/solar", tags=["Solar"])
router.include_router(water.router, prefix="/water", tags=["Water"])
router.include_router(weather.router, prefix="/weather", tags=["Weather"])
router.include_router(health.router, prefix="/health", tags=["Health"])