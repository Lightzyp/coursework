import os
import discord
from discord.ext import commands
from dotenv import load_dotenv
from db.database import init_db

load_dotenv()
TOKEN = os.getenv("DISCORD_TOKEN")

intents = discord.Intents.default()

class FinanceBot(commands.Bot):
    def __init__(self):
        super().__init__(command_prefix="!", intents=intents)

    async def setup_hook(self):
        await init_db()
        await self.load_extension("cogs.finance")
        synced = await self.tree.sync()
        print(f"Synced {len(synced)} slash command(s).")

bot = FinanceBot()

@bot.event
async def on_ready():
    print(f"{bot.user} is now online!")

bot.run(TOKEN)