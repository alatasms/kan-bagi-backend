from motor.motor_asyncio import AsyncIOMotorClient
from pymongo.errors import ServerSelectionTimeoutError  # Changed from ConnectionError
from dotenv import load_dotenv
import os

load_dotenv()

MONGO_URI = os.getenv("MONGO_URI", "mongodb://localhost:27017/chat_db")

class MongoDB:
    client: AsyncIOMotorClient = None
    db = None

    async def connect(self):
        try:
            self.client = AsyncIOMotorClient(MONGO_URI)
            self.db = self.client.get_database("chat_db")
            await self.client.admin.command("ping")  # Test connection
            print("MongoDB connection successful!")
        except ServerSelectionTimeoutError as e:  # Changed exception type
            print(f"MongoDB connection error: {e}")

    async def close(self):
        if self.client:
            self.client.close()
            print("MongoDB connection closed.")

mongo_db = MongoDB()