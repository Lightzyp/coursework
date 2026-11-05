from fastapi import FastAPI
from db.database import init_db
from api.routes import finance

app = FastAPI()

@app.on_event("startup")
async def startup():
    await init_db()

app.include_router(finance.router)