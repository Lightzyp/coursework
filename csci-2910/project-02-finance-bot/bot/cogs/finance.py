import discord
from discord.ext import commands
from discord import app_commands

API_URL = "http://127.0.0.1:8000"

class Finance(commands.Cog):
    def __init__(self, bot):
        self.bot = bot

    @app_commands.command(name="price")
    async def price(self, interaction: discord.Interaction, symbol: str):
        await interaction.response.defer()

        async with self.bot.session.get(f"{API_URL}/price/{symbol}") as resp:
            data = await resp.json()

        if data["price"] is None:
            await interaction.followup.send("Invalid symbol.")
            return

        await interaction.followup.send(
            f"{data['symbol']} price: ${data['price']}"
        )

    @app_commands.command(name="watchlist_add")
    async def add(self, interaction: discord.Interaction, symbol: str):
        async with self.bot.session.post(
            f"{API_URL}/watchlist/{interaction.user.id}/{symbol}"
        ):
            pass

        await interaction.response.send_message("Added to watchlist!")

    @app_commands.command(name="watchlist_view")
    async def view(self, interaction: discord.Interaction):
        async with self.bot.session.get(
            f"{API_URL}/watchlist/{interaction.user.id}"
        ) as resp:
            data = await resp.json()

        if not data["symbols"]:
            await interaction.response.send_message("Empty watchlist.")
            return

        await interaction.response.send_message(
            "\n".join(data["symbols"])
        )

async def setup(bot):
    await bot.add_cog(Finance(bot))