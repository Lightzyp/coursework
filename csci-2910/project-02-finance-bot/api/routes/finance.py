from fastapi import APIRouter
import aiosqlite

router = APIRouter()

DB_PATH = "finance.db"

@router.get("/price/{symbol}")
async def get_price(symbol: str):
    # fake data for now
    prices = {
        "AAPL": 215.44,
        "TSLA": 178.20,
        "BTC": 68250.15
    }

    return {
        "symbol": symbol.upper(),
        "price": prices.get(symbol.upper(), None)
    }

@router.post("/watchlist/{user_id}/{symbol}")
async def add_watchlist(user_id: str, symbol: str):
    async with aiosqlite.connect(DB_PATH) as db:
        await db.execute(
            "INSERT INTO watchlists (user_id, symbol) VALUES (?, ?)",
            (user_id, symbol.upper())
        )
        await db.commit()

    return {"message": "Added"}

@router.get("/watchlist/{user_id}")
async def get_watchlist(user_id: str):
    async with aiosqlite.connect(DB_PATH) as db:
        cursor = await db.execute(
            "SELECT symbol FROM watchlists WHERE user_id = ?",
            (user_id,)
        )
        rows = await cursor.fetchall()

    return {"symbols": [r[0] for r in rows]}