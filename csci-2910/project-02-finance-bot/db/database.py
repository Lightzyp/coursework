import aiosqlite

DB_PATH = "finance.db"

async def init_db():
    async with aiosqlite.connect(DB_PATH) as db:
        with open("db/schema.sql") as f:
            await db.executescript(f.read())
        await db.commit()