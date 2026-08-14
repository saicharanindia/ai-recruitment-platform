from fastapi import FastAPI, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from typing import List
from app.schemas import (
    ResumeParseRequest, ResumeParseResponse,
    MatchRequest, MatchResponse,
    QuestionGenerateRequest, QuestionItem
)
from app.services.ai_logic import ResumeParserService, MatcherService, QuestionGeneratorService

app = FastAPI(
    title="Smart Recruitment AI Service",
    description="Microservice providing NLP resume parsing, job matching, and automated interview question generation.",
    version="1.0.0"
)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@app.get("/")
def health_check():
    return {"status": "online", "service": "AI Microservice", "version": "1.0.0"}

@app.post("/api/ai/parse-resume", response_model=ResumeParseResponse)
def parse_resume(request: ResumeParseRequest):
    if not request.resumeText:
        raise HTTPException(status_code=400, detail="Resume text is required")
    return ResumeParserService.parse_resume(request.resumeText)

@app.post("/api/ai/match", response_model=MatchResponse)
def match_resume(request: MatchRequest):
    return MatcherService.compute_match(
        request.resumeText,
        request.jobDescription,
        request.requiredSkills
    )

@app.post("/api/ai/generate-questions", response_model=List[QuestionItem])
def generate_questions(request: QuestionGenerateRequest):
    return QuestionGeneratorService.generate_questions(
        request.jobTitle,
        request.missingSkills
    )
