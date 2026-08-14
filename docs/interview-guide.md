# Cognizant .NET + Angular Interview Preparation Guide

This guide equips you to defend and explain every single architectural choice, design pattern, and line of code in the **Smart Recruitment & Resume Screening Platform**.

---

## 1. Top Technical Questions & Answers

### Q1: Why Angular 22 and what are its key advantages?
- **Answer:** Angular 22 provides enterprise-grade Single Page Application structure with Google LTS support. Standalone components simplify the mental model without requiring legacy `NgModule` boilerplates. Features like signals for reactive state management, deferrable views (`@defer`) for lazy-loading UI components, and built-in CLI optimizations make it ideal for high-performance enterprise dashboards.

### Q2: Why ASP.NET Core (.NET 10) and Clean Architecture?
- **Answer:** .NET 10 provides high throughput (Minimal APIs / Web APIs), built-in dependency injection, and cross-platform container performance. Clean Architecture decouples domain business rules (`Recruitment.Domain`) from data access (`EF Core`) and outer frameworks (`API Controllers`). This ensures testability, maintainability, and clean separation of concerns.

### Q3: How is Authentication and Authorization implemented?
- **Answer:** 
  - **Authentication:** Stateless JWT bearer tokens issued upon validation of BCrypt-hashed user credentials.
  - **Authorization:** ASP.NET Core Role-Based Access Control (RBAC) via `[Authorize(Roles = "Recruiter")]` or `[Authorize(Roles = "Candidate")]` attributes on API controllers. Angular Angular route guards (`AuthGuard`) enforce navigation protection on the client side.

### Q4: Explain the AI microservice architecture and why Python/FastAPI was used.
- **Answer:** Python is the industry standard for NLP and LLM integrations. FastAPI provides asynchronous high-performance endpoint processing with automatic OpenAPI/Swagger generation and Pydantic validation. Isolating AI workloads into a microservice prevents LLM API latency from blocking core CRUD operations in .NET.

### Q5: How do EF Core and LINQ optimize database queries?
- **Answer:** EF Core utilizes Code-First migrations with explicit entity configurations (`IEntityTypeConfiguration`). We leverage `.AsNoTracking()` for read-only query performance, projection via `.Select()` to prevent over-fetching, and explicit include navigation properties to prevent N+1 query traps.

---

## 2. 2-Minute Elevator Pitch Script

> *"We built the **Smart Recruitment & Resume Screening Platform** — a full-stack, enterprise hiring solution designed with ASP.NET Core (.NET 10), Angular 22, and a Python FastAPI AI microservice.*
>
> *Recruiters can post job listings, review applicant pools, and view AI-derived match scoring. Candidates create profiles, upload resumes, browse positions, and receive actionable match feedback. When an applicant applies, our Python AI service parses the candidate's resume and compares it against the job description using LLM NLP algorithms. It computes a 0-100 fit score, highlights matched vs. missing skills, and auto-generates custom technical interview questions for HR.*
>
> *Architecturally, the backend adheres to Clean Architecture across Domain, Application, Infrastructure, and API projects. It uses EF Core with Microsoft SQL Server, secured via stateless JWT bearer authentication and role-based policies. The frontend is a modern Angular 22 SPA built with Reactive Forms and HTTP interceptors. The system is containerized with Docker, deployable to Azure Kubernetes Service (AKS), automated via GitHub Actions CI/CD pipelines, and provisioned with Terraform IaC.*
>
> *This platform demonstrates full-stack expertise across modern C#, TypeScript/Angular, Python microservices, relational schema design, JWT security, and cloud DevOps."*

---

## 3. Recommended Git Commit Style & Workflow

Follow Conventional Commits format:

```text
feat: initialize project structure with frontend, backend, and ai-service
feat(auth): implement JWT token generation and BCrypt password hashing
feat(jobs): add job posting CRUD endpoints in ASP.NET Core
feat(ai): integrate FastAPI resume parsing and match scoring endpoints
fix(applications): enforce duplicate application validation guard in ApplicationService
test(unit): add xUnit test coverage for AuthService and JobService
ci: configure GitHub Actions pipeline for multi-service build and Docker deployment
docs: add architecture diagrams and database ER schema
```
