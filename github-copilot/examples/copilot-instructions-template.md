# Example: copilot-instructions.md

Place this file at `.github/copilot-instructions.md` in your repository root.
GitHub Copilot will automatically include these instructions in every interaction.

---

## Template for Your Projects

```markdown
# [Project Name] Copilot Guidelines

## Project Overview
[Brief description of what this project does - 1-2 sentences]

## Tech Stack
- [Primary language and version]
- [Framework and version]
- [Testing framework]
- [Key libraries]

## Architecture
- [Architecture pattern, e.g., Clean Architecture, CQRS, etc.]
- [Key architectural decisions]

## Coding Standards

### Naming Conventions
- [Class naming rules]
- [Method naming rules]
- [Variable naming rules]

### Code Style
- [Formatting preferences]
- [Import/using organization]
- [Comment style]

### Patterns to Follow
- [Preferred patterns]
- [Anti-patterns to avoid]

## Error Handling
- [How errors should be handled]
- [Logging requirements]

## Testing
- [Test naming convention]
- [Testing patterns]
- [Coverage requirements]

## Security
- [Security considerations]
- [Sensitive data handling]
```

---

# Example: TypeScript Projects

```markdown
# TypeScript Project Guidelines

## Project Overview
Enterprise TypeScript applications following Clean Architecture principles.

## Tech Stack
- Node.js 18+ with TypeScript 5
- Express or Fastify for APIs
- Prisma or TypeORM for data access
- Jest with ts-jest for testing

## Architecture
- Clean Architecture (Domain → Application → Infrastructure → Presentation)
- CQRS pattern for complex domains
- Repository pattern for data access
- Dependency injection with tsyringe or InversifyJS

## Coding Standards

### Naming Conventions
- camelCase for variables, functions, and methods
- PascalCase for classes, interfaces, types, and enums
- UPPER_SNAKE_CASE for constants
- Prefix interfaces with I only when needed for clarity

### Code Style
- Use explicit return types on all functions
- Prefer interfaces over type aliases for objects
- Use readonly for immutable properties
- Use const assertions where appropriate
- One class/interface per file

### Patterns to Follow
- Result pattern (neverthrow) for expected failures
- Factory functions for complex object creation
- Builder pattern for test data
- Null object pattern over null checks

### Patterns to Avoid
- Service Locator (use DI instead)
- Anemic domain models
- God classes
- any type (use unknown if needed)

## Error Handling
- Use Result<T, E> for business logic failures
- Throw errors only for unexpected failures
- Log all errors with correlation ID
- Return proper HTTP status codes for API errors

## Testing
- Test naming: should_expected_when_condition
- Arrange-Act-Assert structure
- One logical assertion per test
- Use factories/builders for test data setup
- Minimum 80% coverage on business logic

## Security
- Never log sensitive data (PII, credentials)
- Use parameterized queries (ORMs handle this)
- Validate all input at API boundary
- Use HTTPS only
```

---

## Example: SupermarketReceipt Kata

```markdown
# SupermarketReceipt Kata Guidelines

## Project Context
This is a refactoring exercise for a supermarket receipt system.
The goal is to improve code quality without changing behavior.
All existing tests must pass after each change.

## Tech Stack
- TypeScript 5
- Jest for testing

## Code Smells to Address
- Long methods (handleOffers is the main target)
- Complex conditionals and switch statements
- Magic numbers
- Mixed abstraction levels

## Refactoring Approach
1. Make small, incremental changes
2. Run tests after each change (npm test)
3. Extract methods before introducing patterns
4. Keep commits atomic and meaningful

## Testing Guidelines
- Use approval testing for receipt output
- Test each discount type independently
- Include edge cases (zero quantity, missing offers)
- Descriptive test names: should_applyDiscount_when_offerMatches

## Discount Types to Understand
- TenPercentDiscount: X% off total for product
- ThreeForTwo: Buy 3, pay for 2
- TwoForAmount: Buy 2 for fixed price
- FiveForAmount: Buy 5 for fixed price
- Bundle: (Future) % off when buying all bundle items
```

---

## File-Specific Instructions

Create additional files in `.github/instructions/` for targeted guidance:

### `.github/instructions/typescript.instructions.md`

```markdown
---
applyTo: "**/*.ts"
---
# TypeScript File Guidelines

- Use explicit return types on all functions
- Prefer interfaces over type aliases for objects
- Use readonly for immutable properties
- Use const assertions where appropriate
- JSDoc comments on public APIs
```

### `.github/instructions/tests.instructions.md`

```markdown
---
applyTo: "**/*.test.ts"
---
# Test File Guidelines

- Use describe/it blocks for structure
- One logical assertion per test
- Test name format: should_expected_when_condition
- Use test.each for parameterized tests
- Mock external dependencies with jest.mock()
```

### `.github/instructions/api.instructions.md`

```markdown
---
applyTo: "**/routes/**/*.ts"
---
# API Route Guidelines

- Use express.Router() for route definitions
- Return proper HTTP status codes
- Validate input with zod or joi
- Handle errors with middleware
- Use async/await for async operations
```

---

## MCP Server Configuration

MCP (Model Context Protocol) servers extend Copilot's capabilities by connecting to external tools and data sources. Configuration is stored in `.vscode/mcp.json`.

### Basic MCP Configuration Template

```json
{
  "inputs": [
    {
      "type": "promptString",
      "id": "github-token",
      "description": "GitHub Personal Access Token",
      "password": true
    }
  ],
  "servers": {
    "your-server-name": {
      "type": "http",
      "url": "https://your-server-url.com/mcp",
      "headers": {
        "Authorization": "Bearer ${input:github-token}"
      }
    }
  }
}
```

### Example: GitHub MCP Server (Remote)

```json
{
  "inputs": [
    {
      "type": "promptString",
      "id": "github-token",
      "description": "GitHub Personal Access Token",
      "password": true
    }
  ],
  "servers": {
    "github": {
      "type": "http",
      "url": "https://api.githubcopilot.com/mcp/v1",
      "headers": {
        "Authorization": "Bearer ${input:github-token}"
      }
    }
  }
}
```

**Capabilities**: Repository access, issue management, PR creation, code search across organization.

### Example: Playwright MCP Server (Local)

```json
{
  "servers": {
    "playwright": {
      "command": "npx",
      "args": ["@playwright/mcp@latest"]
    }
  }
}
```

**Capabilities**: Browser automation, web scraping, E2E testing, screenshot capture.

### Example: Azure MCP Server (Remote)

```json
{
  "servers": {
    "azure": {
      "type": "http",
      "url": "https://mcp.azure.com/v1",
      "headers": {
        "Authorization": "Bearer ${input:azure-token}"
      }
    }
  }
}
```

**Capabilities**: Azure resource management, deployment, monitoring, cost analysis.

### Example: PostgreSQL MCP Server (Local)

```json
{
  "inputs": [
    {
      "type": "promptString",
      "id": "postgres-connection",
      "description": "PostgreSQL Connection String",
      "password": true
    }
  ],
  "servers": {
    "postgres": {
      "command": "npx",
      "args": ["@modelcontextprotocol/server-postgres"],
      "env": {
        "DATABASE_URL": "${input:postgres-connection}"
      }
    }
  }
}
```

**Capabilities**: Database queries, schema analysis, data exploration.

### Example: Combined Configuration (Multiple Servers)

```json
{
  "inputs": [
    {
      "type": "promptString",
      "id": "github-token",
      "description": "GitHub PAT",
      "password": true
    },
    {
      "type": "promptString",
      "id": "db-connection",
      "description": "Database Connection String",
      "password": true
    }
  ],
  "servers": {
    "github": {
      "type": "http",
      "url": "https://api.githubcopilot.com/mcp/v1",
      "headers": {
        "Authorization": "Bearer ${input:github-token}"
      }
    },
    "playwright": {
      "command": "npx",
      "args": ["@playwright/mcp@latest"]
    },
    "database": {
      "command": "npx",
      "args": ["@modelcontextprotocol/server-postgres"],
      "env": {
        "DATABASE_URL": "${input:db-connection}"
      }
    }
  }
}
```

### MCP Configuration Tips

1. **Use input variables** for sensitive data (tokens, connection strings)
2. **Start with one server**, add more as needed
3. **Test each server** individually before combining
4. **Check the MCP Registry** for official server configurations: `github.com/modelcontextprotocol/servers`

### Discovering MCP Servers

**GitHub MCP Registry**: The official curated list of MCP servers

- Browse: <https://github.com/modelcontextprotocol/servers>
- One-click install available for many servers
- Community and partner-built options

**Popular MCP Servers for .NET Development**:

| Server | Use Case |
|--------|----------|
| GitHub | Repository management, PRs, issues |
| Playwright | Browser testing, web automation |
| Azure | Cloud resource management |
| PostgreSQL/SQL Server | Database operations |
| Docker | Container management |
| Filesystem | Local file access |

### Using MCP Servers in Copilot

1. Configure servers in `.vscode/mcp.json`
2. Open Copilot Chat → Switch to **Agent mode**
3. Click the **Tools** icon to see available MCP tools
4. Ask questions that leverage the tools:

```prompt
# GitHub MCP examples
"List open issues assigned to me"
"Create a feature branch called feature/new-discount"
"Search for authentication code in our organization"

# Playwright MCP examples  
"Take a screenshot of our login page"
"Test the checkout flow and report any errors"

# Database MCP examples
"Show me the schema of the Orders table"
"Find customers who placed orders last month"
```

---

## Custom Agents

GitHub Copilot supports custom agents that can be invoked with `@` mentions in chat. Agents can operate in two modes:

### Ask Mode Agents (Read-Only)

Ask mode agents provide information and answer questions without making changes to the codebase.

**Example**: Architecture Documentation Agent

Create `.github/copilot/agents/architecture.md`:

```markdown
# Architecture Documentation Agent

You are an architecture documentation expert for this codebase.

When users ask about architecture, design patterns, or technical decisions:

1. Analyze the codebase structure and identify the patterns used
2. Reference specific files and code examples
3. Explain the rationale behind architectural choices
4. Highlight dependencies and relationships between components
5. Suggest improvements only when explicitly asked

## Focus Areas

- Layer separation and dependencies
- Design patterns (Repository, Factory, Strategy, etc.)
- SOLID principles adherence
- Data flow and component interaction
- API design and boundaries

## Response Guidelines

- Always ground explanations in actual code from the workspace
- Use concrete examples with file paths and line numbers
- Provide visual diagrams when helpful (using Mermaid syntax)
- Link related components and dependencies
- Explain trade-offs in architectural decisions

## Example Prompts

- "Explain the architecture of this project"
- "What design patterns are used in the codebase?"
- "How does data flow through the application?"
- "Analyze the dependency structure"
```

**Usage Examples**:

```prompt
@architecture explain the repository pattern implementation
@architecture how are dependencies injected in this project?
@architecture what's the purpose of the service layer?
```

### Agent Mode Agents (Read-Write)

Agent mode agents can both analyze code and make changes to the workspace.

**Example**: Refactoring Agent

Create `.github/copilot/agents/refactor.md`:

```markdown
# Refactoring Agent

You are a refactoring expert focused on improving code quality through safe, incremental changes.

## Refactoring Process

### 1. Analyze
Identify code smells and improvement opportunities:
- Long methods (>20 lines)
- High cyclomatic complexity
- Duplicate code
- Poor naming
- Magic numbers
- God classes

### 2. Plan
Create a step-by-step refactoring plan:
- List specific changes
- Estimate risk level (low/medium/high)
- Identify test requirements
- Note dependencies

### 3. Execute
Make incremental changes:
- One refactoring at a time
- Run tests after each change
- Preserve existing behavior
- Keep commits atomic

### 4. Verify
Ensure quality improvements:
- All tests pass
- No new warnings or errors
- Code metrics improved
- Documentation updated

## Refactoring Patterns to Apply

- Extract Method
- Extract Class
- Rename Variable/Method/Class
- Introduce Parameter Object
- Replace Conditional with Polymorphism
- Replace Magic Number with Named Constant
- Simplify Conditional Expressions
- Remove Dead Code

## Safety Rules

- Never change public APIs without explicit permission
- Run tests after every change
- Stop immediately if tests fail
- Use TypeScript/language features appropriately
- Maintain backward compatibility
- Document significant changes

## Before Making Changes

Always explain:
1. What code smell you identified
2. What refactoring you'll apply
3. Why this improves the code
4. What risks exist (if any)

## Example Prompts

- "Refactor this file to reduce complexity"
- "Extract methods from this long function"
- "Remove code duplication in these files"
- "Improve naming in this class"
```

**Usage Examples**:

```prompt
@refactor analyze Checkout.ts for code smells
@refactor extract the discount calculation logic into separate methods
@refactor remove duplication between OrderService and InvoiceService
```

### Agent Configuration Guidelines

**Ask Mode** - Use when you want to:

- Provide documentation and explanations
- Answer questions about the codebase
- Analyze code without modifications
- Review and audit code

**Agent Mode** - Use when you want to:

- Make code changes
- Perform refactoring
- Generate new files
- Run tests and commands
- Fix errors automatically

### Activating Custom Agents

1. Create agent YAML files in `.github/copilot/agents/`
2. Reload VS Code or restart Copilot
3. Type `@` in Copilot Chat to see available agents
4. Select your custom agent and start chatting

**Note**: Custom agents require GitHub Copilot Enterprise or specific organization settings.
