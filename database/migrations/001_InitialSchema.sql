-- ============================================================
-- Database Schema Script for Smart Recruitment Platform
-- Target Database: SQL Server 2025 / 2022
-- ============================================================

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'RecruitDB')
BEGIN
    CREATE DATABASE RecruitDB;
END
GO

USE RecruitDB;
GO

-- 1. Users Table
IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users (
        UserId INT IDENTITY(1,1) PRIMARY KEY,
        FullName NVARCHAR(150) NOT NULL,
        Email NVARCHAR(150) NOT NULL UNIQUE,
        PasswordHash NVARCHAR(256) NOT NULL,
        Role NVARCHAR(50) NOT NULL CHECK (Role IN ('Candidate', 'Recruiter')),
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- 2. Candidates Table
IF OBJECT_ID('dbo.Candidates', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Candidates (
        CandidateId INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL UNIQUE FOREIGN KEY REFERENCES dbo.Users(UserId) ON DELETE CASCADE,
        Phone NVARCHAR(30) NULL,
        ResumeLink NVARCHAR(500) NULL,
        JoinedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- 3. Recruiters Table
IF OBJECT_ID('dbo.Recruiters', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Recruiters (
        RecruiterId INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL UNIQUE FOREIGN KEY REFERENCES dbo.Users(UserId) ON DELETE CASCADE,
        Department NVARCHAR(100) NOT NULL,
        Title NVARCHAR(100) NOT NULL
    );
END
GO

-- 4. Jobs Table
IF OBJECT_ID('dbo.Jobs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Jobs (
        JobId INT IDENTITY(1,1) PRIMARY KEY,
        RecruiterId INT NOT NULL FOREIGN KEY REFERENCES dbo.Recruiters(RecruiterId),
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NOT NULL,
        Department NVARCHAR(100) NOT NULL,
        Location NVARCHAR(100) NOT NULL,
        EmploymentType NVARCHAR(50) NOT NULL DEFAULT 'Full-time',
        SalaryMin FLOAT NULL,
        SalaryMax FLOAT NULL,
        CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ClosingDate DATETIME2 NULL,
        IsActive BIT NOT NULL DEFAULT 1
    );
END
GO

-- 5. Resumes Table
IF OBJECT_ID('dbo.Resumes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Resumes (
        ResumeId INT IDENTITY(1,1) PRIMARY KEY,
        CandidateId INT NOT NULL FOREIGN KEY REFERENCES dbo.Candidates(CandidateId) ON DELETE CASCADE,
        FilePath NVARCHAR(500) NOT NULL,
        UploadedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

-- 6. Skills Table
IF OBJECT_ID('dbo.Skills', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Skills (
        SkillId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL UNIQUE
    );
END
GO

-- 7. Candidate_Skills Table
IF OBJECT_ID('dbo.Candidate_Skills', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Candidate_Skills (
        CandidateSkillId INT IDENTITY(1,1) PRIMARY KEY,
        CandidateId INT NOT NULL FOREIGN KEY REFERENCES dbo.Candidates(CandidateId) ON DELETE CASCADE,
        SkillId INT NOT NULL FOREIGN KEY REFERENCES dbo.Skills(SkillId) ON DELETE CASCADE,
        CONSTRAINT UQ_Candidate_Skill UNIQUE(CandidateId, SkillId)
    );
END
GO

-- 8. Job_Skills Table
IF OBJECT_ID('dbo.Job_Skills', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Job_Skills (
        JobSkillId INT IDENTITY(1,1) PRIMARY KEY,
        JobId INT NOT NULL FOREIGN KEY REFERENCES dbo.Jobs(JobId) ON DELETE CASCADE,
        SkillId INT NOT NULL FOREIGN KEY REFERENCES dbo.Skills(SkillId) ON DELETE CASCADE,
        CONSTRAINT UQ_Job_Skill UNIQUE(JobId, SkillId)
    );
END
GO

-- 9. Applications Table
IF OBJECT_ID('dbo.Applications', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Applications (
        ApplicationId INT IDENTITY(1,1) PRIMARY KEY,
        JobId INT NOT NULL FOREIGN KEY REFERENCES dbo.Jobs(JobId),
        CandidateId INT NOT NULL FOREIGN KEY REFERENCES dbo.Candidates(CandidateId),
        AppliedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        Status NVARCHAR(50) NOT NULL DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Shortlisted', 'Rejected')),
        ResumeAtApply NVARCHAR(MAX) NULL,
        CONSTRAINT UQ_Candidate_Job_Application UNIQUE(CandidateId, JobId)
    );
END
GO

-- 10. Interview_Questions Table
IF OBJECT_ID('dbo.Interview_Questions', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Interview_Questions (
        QuestionId INT IDENTITY(1,1) PRIMARY KEY,
        ApplicationId INT NOT NULL FOREIGN KEY REFERENCES dbo.Applications(ApplicationId) ON DELETE CASCADE,
        QuestionText NVARCHAR(MAX) NOT NULL,
        Category NVARCHAR(50) NOT NULL DEFAULT 'Technical'
    );
END
GO

-- 11. AI_Match Table
IF OBJECT_ID('dbo.AI_Match', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AI_Match (
        MatchId INT IDENTITY(1,1) PRIMARY KEY,
        ApplicationId INT NOT NULL UNIQUE FOREIGN KEY REFERENCES dbo.Applications(ApplicationId) ON DELETE CASCADE,
        MatchScore INT NOT NULL CHECK (MatchScore BETWEEN 0 AND 100),
        MatchedSkills NVARCHAR(MAX) NULL,
        MissingSkills NVARCHAR(MAX) NULL,
        CalculatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO
