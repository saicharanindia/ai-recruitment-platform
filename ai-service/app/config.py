import os
from pydantic_settings import BaseSettings

class Settings:
    OPENAI_API_KEY: str = os.getenv("OPENAI_API_KEY", "sk-mock-key")
    LLM_MODEL: str = os.getenv("LLM_MODEL", "gpt-4")
    ENVIRONMENT: str = os.getenv("ENVIRONMENT", "development")

settings = Settings()
