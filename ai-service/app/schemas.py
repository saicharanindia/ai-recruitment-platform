from pydantic import BaseModel
from typing import List, Optional

class ResumeParseRequest(BaseModel):
    resumeText: str

class ResumeParseResponse(BaseModel):
    candidateName: str
    email: str
    skills: List[str]
    experienceSummary: List[str]

class MatchRequest(BaseModel):
    resumeText: str
    jobDescription: str
    requiredSkills: List[str]

class MatchResponse(BaseModel):
    matchScore: int
    matchedSkills: List[str]
    missingSkills: List[str]
    recommendation: str

class QuestionGenerateRequest(BaseModel):
    jobTitle: str
    missingSkills: List[str]

class QuestionItem(BaseModel):
    questionText: str
    category: str
