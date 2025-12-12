# Developer Workshops

A collection of workshop materials designed for developers to learn modern development practices, AI-assisted coding, and rapid application prototyping.

## 🎓 Available Workshops

### 1. Mastering GitHub Copilot

**Duration**: 4 hours  
**Format**: Presentation + Hands-on Lab  
**Target Audience**: Developers of all skill levels  
**Tools Required**: VS Code with GitHub Copilot extension

#### Overview
This comprehensive workshop teaches developers how to effectively collaborate with GitHub Copilot to enhance productivity and code quality. Through a combination of theory and hands-on practice, participants learn to leverage AI-assisted coding tools professionally.

#### Learning Objectives
- Understand Large Language Models (LLMs) fundamentals, training, and limitations
- Master GitHub Copilot's three interaction modes: Ask, Edit, and Agent
- Learn context management techniques using reference syntax (#file, @workspace, etc.)
- Customize Copilot behavior with custom instructions files
- Apply AI-assisted refactoring to legacy code

#### Workshop Structure

**Part 1: Foundations (1h 30min)**
- Introduction to LLMs: What they are, how they're trained, strengths and limitations
- GitHub Copilot overview: History, available models (GPT-4o, Claude, Gemini), pricing tiers
- Deep dive into Ask/Edit/Agent modes with live demonstrations
- When to use which mode for maximum effectiveness

**Part 2: Mastering Copilot (60min)**
- Context is King: How Copilot uses context and best practices for better output
- Reference syntax and custom instructions files
- Fine-tuning: Prompt files, custom agents, and extensions ecosystem
- Enterprise features and capabilities

**Part 3: Hands-on Lab (1h 35min)**
- Exercise 1: Code exploration using Ask mode to understand and identify code smells
- Exercise 2: Test generation with Copilot, including approval testing setup
- Exercise 3: Safe refactoring with Edit mode backed by comprehensive tests
- Uses the [SupermarketReceipt-Refactoring-Kata](https://github.com/emilybache/SupermarketReceipt-Refactoring-Kata) repository

#### Key Takeaways
- Effective prompt engineering for AI coding assistants
- Strategies for maintaining code quality while using AI tools
- Best practices for context management in large codebases
- Custom configuration patterns for team-wide consistency

---

### 2. Rapid Prototyping with AI-Assisted Development

**Duration**: 3-4 hours  
**Format**: Workshop with progressive exercises  
**Target Audience**: Developers familiar with REST APIs and modern web development  
**Tools Required**: GitHub Copilot, .NET 8 SDK, Node.js 18+

#### Overview
Learn specification-first development where detailed requirements and architecture decisions are documented upfront, then implemented efficiently using AI assistance. This workshop emphasizes the power of well-structured specifications in guiding AI tools to generate production-quality code.

#### Learning Objectives
- Master spec-driven development methodology
- Write effective prompts for API and full-stack application generation
- Implement complete REST APIs with authentication and validation
- Apply architectural patterns consistently using project constitutions
- Deliver working prototypes in hours instead of days

#### Core Concepts

**Specification-First Approach**
- Define comprehensive project constitutions covering architecture, coding standards, and patterns
- Document functional specifications with detailed API contracts
- Use specifications as living documentation and AI guidance

**The SPEC-KIT Pattern**
- **Constitution**: Project-wide standards defining architecture, tech stack, coding conventions, validation rules, and testing requirements
- **Functional Description**: Detailed feature specifications with API endpoints, request/response examples, and business rules
- **Progressive Implementation**: Start with 30-minute demos, scale to 3-hour full MVPs

#### Workshop Exercises

**Exercise 1: Prompt Engineering (30 minutes)**
- Compare ineffective vs. effective prompts
- Practice writing detailed API specifications
- Generate a simple CRUD API with validation and OpenAPI documentation

**Exercise 2: Project Constitution (45 minutes)**
- Create a full-stack project constitution
- Define backend standards (.NET 8, Entity Framework Core, FluentValidation)
- Define frontend standards (React 18+, TypeScript, Tailwind CSS)
- Implement consistent patterns across the stack

**Exercise 3: Complete API Implementation (2-3 hours)**
- Build a Task Management API from specification
- Implement authentication with JWT tokens
- Add authorization and user-specific data access
- Generate comprehensive test coverage
- Deploy with full OpenAPI documentation

#### Technology Stack Examples

**Backend**
- .NET 8 LTS with ASP.NET Core Web API
- Entity Framework Core with code-first migrations
- FluentValidation for input validation
- Repository and Unit of Work patterns
- JWT Bearer authentication

**Frontend**
- React 18+ with TypeScript 5.x
- Vite for build tooling
- TanStack Query for server state
- React Hook Form with Zod validation
- Tailwind CSS with Tailwind UI components

#### Key Takeaways
- How to write specifications that produce production-ready code
- Architectural patterns for maintainable full-stack applications
- Validation strategies at multiple layers (API, business logic, data access)
- Best practices for RESTful API design
- Rapid iteration cycles with AI-assisted development

## 🚀 Getting Started

### Prerequisites
- VS Code with GitHub Copilot extension
- GitHub Copilot subscription (Individual, Business, Enterprise, or Education)
- Node.js 18+ (for practical exercises)
- .NET 8 SDK (for rapid prototyping exercises)

### Using These Materials
1. Choose the workshop track relevant to your learning goals
2. Review the documentation in the corresponding folder
3. Follow the structured agenda provided in each workshop
4. Complete hands-on exercises at your own pace
5. Use the provided templates and examples as reference material

## 📖 Additional Resources

- [SupermarketReceipt-Refactoring-Kata](https://github.com/emilybache/SupermarketReceipt-Refactoring-Kata) - Used in GitHub Copilot workshop exercises
- GitHub Copilot Documentation
- Spec-Kit patterns and best practices

## 🤝 Contributing

These workshops are maintained by G3L Consulting. For questions, improvements, or suggestions, please open an issue or submit a pull request.

## 📝 License

Workshop materials are provided for educational purposes.

---

**Last Updated**: December 2025