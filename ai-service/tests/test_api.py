from fastapi.testclient import TestClient
from app.main import app

client = TestClient(app)

def test_health_check():
    response = client.get("/")
    assert response.status_code == 200
    assert response.json()["status"] == "online"

def test_parse_resume():
    payload = {"resumeText": "Jane Candidate\nEmail: jane@example.com\nSkills: C#, Angular, Python, Docker"}
    response = client.post("/api/ai/parse-resume", json=payload)
    assert response.status_code == 200
    data = response.json()
    assert "C#" in data["skills"]
    assert "Angular" in data["skills"]

def test_match_resume():
    payload = {
        "resumeText": "Experienced in C#, .NET Core, Angular, SQL Server",
        "jobDescription": "Looking for .NET developer with Angular and Docker",
        "requiredSkills": ["C#", "Angular", "Docker"]
    }
    response = client.post("/api/ai/match", json=payload)
    assert response.status_code == 200
    data = response.json()
    assert data["matchScore"] > 0
    assert "C#" in data["matchedSkills"]
    assert "Docker" in data["missingSkills"]

def test_generate_questions():
    payload = {
        "jobTitle": "Full Stack Engineer",
        "missingSkills": ["Kubernetes", "GraphQL"]
    }
    response = client.post("/api/ai/generate-questions", json=payload)
    assert response.status_code == 200
    questions = response.json()
    assert len(questions) >= 3
