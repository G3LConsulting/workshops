# Workshops Repository

This repository contains workshop materials for learning and mastering AI-assisted development tools, with a focus on GitHub Copilot and rapid prototyping with AI.

## Repository Structure

### 📂 `github-copilot/`

Workshop materials for mastering GitHub Copilot in software development.

**Duration:** 4 hours (Presentation + Hands-on Lab)

**Contents:**

- **`docs/`** - Workshop documentation
  - `workshop-structure.md` - Detailed 4-hour agenda covering LLMs, Copilot modes (Ask/Edit/Agent), context management, and fine-tuning
  - `presentation.pdf` - Slide deck for the workshop
- **`examples/`** - Templates and examples
  - `copilot-instructions-template.md` - Custom instructions template for fine-tuning Copilot behavior

**Topics Covered:**

- Introduction to Large Language Models (LLMs)
- GitHub Copilot features and capabilities
- Context management and reference syntax
- Copilot modes: Ask, Edit, and Agent
- Custom instructions and prompt engineering
- Hands-on kata using the SupermarketReceipt-Refactoring-Kata

---

### 📂 `rapid-prototyping/`

Workshop materials for rapid API development using AI-assisted coding and specification-driven development.

#### Subfolders

**`prompts/`** - Curated prompts and spec-kit methodology

- `presentation-prompts.md` - Example prompts demonstrating good vs. bad practices for API generation
- `speckitcommands.md` - Commands for using the spec-kit methodology
- **`spec-kit/`** - Specification templates for structured development
  - `constitution-example.md` - Comprehensive project constitution defining tech stack, architecture patterns, API standards, and best practices for fullstack projects (.NET + React/TypeScript)
  - `functional-description.md` - MVP functional specification for a Task Management API, designed as a 3-hour workshop exercise
  - `demo-description.md` - Additional specification examples

**`src/`** - Example implementations demonstrating AI-generated code
  - **`task-simple/`** - Basic Node.js/Express task API (structure reference, files may not exist)
    - Simple REST API implementation
    - Express.js with basic routing
    - Task controller and model examples
  - **`task-elaborate/`** - Advanced .NET 9 task management API
    - `.gitignore` - Standard .NET gitignore configuration
    - **`TaskApi/`** - Full-featured ASP.NET Core Web API
      - RESTful endpoints with pagination
      - Entity Framework Core with DbContext
      - DTOs for request/response handling
      - OpenAPI/Swagger documentation
      - Proper error handling and HTTP status codes
      - Controllers, Models, and Data layers

**`rapid-prototyping-workshop.pptx`** - Presentation slide deck

**Key Concepts:**
- Spec-Kit methodology: Using detailed specifications (constitution + functional description) to guide AI in generating production-quality code
- Demonstrates the difference between vague prompts and well-structured specifications
- Shows progressive complexity from simple Node.js API to enterprise .NET implementation
- Emphasizes validation, error handling, documentation, and best practices

---

## Workshop Philosophy

These workshops teach developers how to:
1. **Leverage AI effectively** - Understanding LLM capabilities and limitations
2. **Provide better context** - Using reference syntax, custom instructions, and specifications
3. **Structure projects properly** - Following architectural patterns and best practices
4. **Generate production-quality code** - Moving beyond simple examples to real-world implementations
5. **Maintain control** - Using AI as an assistant, not a replacement for engineering judgment

## Getting Started

Each workshop folder contains its own documentation and materials. Start by reviewing the relevant workshop structure document and presentation materials before diving into hands-on exercises.

## Prerequisites

- Visual Studio Code
- GitHub Copilot extension
- Basic understanding of software development concepts
- Familiarity with at least one programming language
