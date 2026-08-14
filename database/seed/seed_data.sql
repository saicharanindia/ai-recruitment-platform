USE RecruitDB;
GO

-- Seed Skills
INSERT INTO dbo.Skills (Name) VALUES 
('C#'), ('.NET Core'), ('Angular'), ('TypeScript'), ('SQL Server'),
('Python'), ('FastAPI'), ('Docker'), ('Kubernetes'), ('Azure'),
('REST API'), ('Entity Framework Core'), ('CI/CD'), ('Terraform');

-- Seed Users (Password is 'Password123!' hashed with BCrypt)
-- Hash: $2a$11$qE.xNlHn0fE/7QJz4Q6d0eW0E8h8xLzKkP0vN.2YkK0x6G5F2D.S. (Sample placeholder hash)
INSERT INTO dbo.Users (FullName, Email, PasswordHash, Role) VALUES
('Jane Doe', 'recruiter@cognizant.com', '$2a$11$qE.xNlHn0fE/7QJz4Q6d0eW0E8h8xLzKkP0vN.2YkK0x6G5F2D.S.', 'Recruiter'),
('John Candidate', 'john.candidate@example.com', '$2a$11$qE.xNlHn0fE/7QJz4Q6d0eW0E8h8xLzKkP0vN.2YkK0x6G5F2D.S.', 'Candidate'),
('Alice Smith', 'alice.smith@example.com', '$2a$11$qE.xNlHn0fE/7QJz4Q6d0eW0E8h8xLzKkP0vN.2YkK0x6G5F2D.S.', 'Candidate');

-- Seed Recruiter & Candidate Profiles
INSERT INTO dbo.Recruiters (UserId, Department, Title) VALUES
(1, 'Talent Acquisition', 'Senior Tech Recruiter');

INSERT INTO dbo.Candidates (UserId, Phone, ResumeLink) VALUES
(2, '+1-555-0199', 'uploads/resumes/john_candidate_resume.pdf'),
(3, '+1-555-0288', 'uploads/resumes/alice_smith_resume.pdf');

-- Seed Candidate Skills
INSERT INTO dbo.Candidate_Skills (CandidateId, SkillId) VALUES
(1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 11), -- John: C#, .NET, Angular, TS, SQL, REST
(2, 6), (2, 7), (2, 8), (2, 9), (2, 13);          -- Alice: Python, FastAPI, Docker, K8s, CI/CD

-- Seed Jobs
INSERT INTO dbo.Jobs (RecruiterId, Title, Description, Department, Location, EmploymentType, SalaryMin, SalaryMax, IsActive) VALUES
(1, 'Senior .NET + Angular Full Stack Developer', 'Seeking a seasoned C# .NET 10 & Angular 22 developer to build enterprise microservices.', 'Engineering', 'Hyderabad, India (Hybrid)', 'Full-time', 1200000, 1800000, 1),
(1, 'AI & Python Microservices Specialist', 'Focus on FastAPI, LLM integrations, OpenAI APIs, and containerized cloud deployment.', 'AI Innovation', 'Bangalore, India (Remote)', 'Full-time', 1400000, 2200000, 1);

-- Seed Job Required Skills
INSERT INTO dbo.Job_Skills (JobId, SkillId) VALUES
(1, 1), (1, 2), (1, 3), (1, 4), (1, 5), (1, 12),
(2, 6), (2, 7), (2, 8), (2, 9), (2, 10);

-- Seed Application
INSERT INTO dbo.Applications (JobId, CandidateId, Status, ResumeAtApply) VALUES
(1, 1, 'Shortlisted', 'John Candidate - Senior C#/.NET & Angular Developer with 5 years experience.'),
(2, 2, 'Pending', 'Alice Smith - Python FastAPI AI Engineer specializing in LLM microservices.');

-- Seed AI Match
INSERT INTO dbo.AI_Match (ApplicationId, MatchScore, MatchedSkills, MissingSkills) VALUES
(1, 88, '["C#", ".NET Core", "Angular", "TypeScript", "SQL Server"]', '["Entity Framework Core"]'),
(2, 92, '["Python", "FastAPI", "Docker", "Kubernetes"]', '["Azure"]');

-- Seed Interview Questions
INSERT INTO dbo.Interview_Questions (ApplicationId, QuestionText, Category) VALUES
(1, 'Explain how Angular 22 standalone components optimize bundle size and tree-shaking.', 'Technical'),
(1, 'How do you handle JWT refresh tokens securely in an ASP.NET Core Web API?', 'Technical'),
(2, 'Describe your experience using FastAPI background tasks vs Celery queues.', 'Technical');
GO
