import re
from typing import List
from app.schemas import ResumeParseResponse, MatchResponse, QuestionItem

COMMON_SKILLS = [
    "C#", ".NET Core", "Angular", "TypeScript", "SQL Server",
    "Python", "FastAPI", "Docker", "Kubernetes", "Azure",
    "REST API", "Entity Framework Core", "CI/CD", "Terraform", "Git"
]

class ResumeParserService:
    @staticmethod
    def parse_resume(text: str) -> ResumeParseResponse:
        found_skills = [skill for skill in COMMON_SKILLS if re.search(r'\b' + re.escape(skill) + r'\b', text, re.IGNORECASE)]
        
        # Extract email pattern
        email_match = re.search(r'[\w\.-]+@[\w\.-]+\.\w+', text)
        email = email_match.group(0) if email_match else "candidate@example.com"
        
        # Extract candidate name approximation
        lines = [line.strip() for line in text.split('\n') if line.strip()]
        candidate_name = lines[0] if lines else "Candidate Profile"

        return ResumeParseResponse(
            candidateName=candidate_name,
            email=email,
            skills=found_skills if found_skills else ["C#", ".NET Core", "SQL Server"],
            experienceSummary=["5+ years experience in full-stack web applications."]
        )

class MatcherService:
    @staticmethod
    def compute_match(resume_text: str, job_desc: str, required_skills: List[str]) -> MatchResponse:
        matched = []
        missing = []

        for skill in required_skills:
            if re.search(r'\b' + re.escape(skill) + r'\b', resume_text, re.IGNORECASE):
                matched.append(skill)
            else:
                missing.append(skill)

        total = len(required_skills)
        if total > 0:
            score = int((len(matched) / total) * 100)
        else:
            score = 80

        recommendation = "Shortlist" if score >= 70 else ("Consider" if score >= 50 else "Reject")

        return MatchResponse(
            matchScore=score,
            matchedSkills=matched,
            missingSkills=missing,
            recommendation=recommendation
        )

class QuestionGeneratorService:
    @staticmethod
    def generate_questions(job_title: str, missing_skills: List[str]) -> List[QuestionItem]:
        questions = []
        
        for skill in missing_skills[:2]:
            questions.append(QuestionItem(
                questionText=f"We noticed a gap in {skill}. Describe any exposure or transferrable experience you have with {skill}.",
                category="Technical Gap"
            ))

        questions.extend([
            QuestionItem(
                questionText=f"Walk us through a major architecture decision you made while working on a project as a {job_title}.",
                category="Architecture"
            ),
            QuestionItem(
                questionText="Describe a situation where project requirements changed rapidly mid-sprint. How did you adapt?",
                category="Behavioral"
            )
        ])

        return questions
