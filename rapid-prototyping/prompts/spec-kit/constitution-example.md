# Project Constitution

## Core Principles
- Maintain clear separation between frontend and backend with REST API as the contract
- Prioritize type safety, maintainability, and developer experience across the stack
- Follow industry best practices and established patterns consistently
- All code must be self-documenting with clear naming and appropriate comments
- Security and performance considerations are non-negotiable

## Backend Stack & Standards

### Technology Stack
- .NET 8.0 LTS (supported until November 2026)
- ASP.NET Core Web API with Minimal APIs or Controllers
- Entity Framework Core 8.x with code-first migrations
- PostgreSQL or SQL Server for production, SQLite for development/testing
- Serilog for structured logging
- FluentValidation for input validation
- AutoMapper for object mapping

### Architecture Patterns
- Repository pattern with generic base repository and specific implementations
- Unit of Work pattern for transaction management
- Dependency Injection using built-in .NET DI container
- CQRS pattern for complex operations (optional, as needed)
- Domain-driven design principles for business logic organization

### API Standards
- RESTful design following OpenAPI 3.0 specification
- Consistent HTTP status codes (200, 201, 204, 400, 401, 403, 404, 409, 422, 500)
- API versioning through URL path (/api/v1/)
- All endpoints must have Swagger/OpenAPI documentation
- Request/Response DTOs separate from domain entities
- Consistent error response format with problem details (RFC 7807)

### Validation & Security
- ALL incoming requests validated using FluentValidation
- Model binding validation as first defense layer
- Business rule validation in service layer
- JWT Bearer authentication for protected endpoints
- Role-based authorization using policies
- Rate limiting on all public endpoints
- CORS configuration explicit and restrictive
- SQL injection prevention through parameterized queries (EF Core default)
- Sensitive data never logged or exposed in responses

### Data Access Rules
- Repository interfaces in Core/Domain layer
- Repository implementations in Infrastructure layer
- All database operations must be async
- Use IQueryable for composable queries, materialize at service boundary
- Explicit loading or projection for related data (avoid lazy loading)
- Optimistic concurrency control for updates
- Soft deletes for audit trail (IsDeleted flag + DeletedAt timestamp)

### Testing Requirements
- Unit tests for business logic (minimum 80% coverage)
- Integration tests for all API endpoints
- Repository tests using in-memory database
- Use xUnit, FluentAssertions, and Moq/NSubstitute
- Test data builders following Builder pattern
- No test dependencies between test methods

## Frontend Stack & Standards

### Technology Stack
- React 18+ with TypeScript 5.x
- Vite for build tooling and development server
- React Router v6 for routing
- TanStack Query (React Query) for server state management
- Zustand or Context API for client state
- Axios for HTTP client with interceptors
- React Hook Form with Zod for form validation
- Tailwind CSS v3.x with Tailwind UI components
- ESLint + Prettier for code formatting

### TypeScript Configuration
- Strict mode enabled (`"strict": true`)
- No implicit any (`"noImplicitAny": true`)
- Strict null checks (`"strictNullChecks": true`)
- All API responses must have defined types
- Use discriminated unions for complex state
- Prefer interfaces for object shapes, types for unions/primitives
- Generic components where reusability matters

### Component Architecture
- Functional components with hooks (no class components)
- Custom hooks for shared logic (prefix with 'use')
- Presentational/Container component separation
- Props interfaces explicitly defined for all components
- Default props using destructuring with defaults
- Memoization (React.memo, useMemo, useCallback) where performance matters
- Error boundaries at route level minimum

### Tailwind UI Integration
- Use Tailwind UI components as base, customize through className props
- Maintain consistent spacing scale (Tailwind's default)
- Color palette defined in tailwind.config.js, no arbitrary colors
- Responsive design mobile-first (sm -> md -> lg -> xl)
- Dark mode support using Tailwind's dark: variant
- Component library wrapper for Tailwind UI components with TypeScript props
- Accessibility standards (WCAG 2.1 AA minimum)

### State Management Rules
- Server state managed exclusively through TanStack Query
- Client state minimal, using Zustand for global, Context for feature-specific
- Form state managed by React Hook Form
- URL state for filters, pagination, and shareable UI state
- No prop drilling beyond 2 levels (use Context or state management)
- Optimistic updates for better UX where appropriate

### API Integration Standards
- All API calls through centralized service layer
- Type-safe API client generated from OpenAPI spec (openapi-typescript)
- Request/response interceptors for auth tokens and error handling
- Retry logic with exponential backoff for failed requests
- Loading states for all async operations
- Error states with user-friendly messages
- Stale-while-revalidate strategy for cached data

### Performance Requirements
- Lighthouse score > 90 for performance
- Code splitting at route level minimum
- Lazy loading for images and heavy components
- Bundle size < 200KB for initial load (gzipped)
- Virtualization for large lists (react-window or TanStack Virtual)
- Web Vitals: LCP < 2.5s, FID < 100ms, CLS < 0.1

## Cross-Stack Standards

### Version Control
- Git with conventional commits specification
- Branch protection on main/master
- Pull requests required with at least one review
- Squash merge for feature branches
- Semantic versioning for releases

### Environment Configuration
- Environment variables for all configuration
- .env files for local development (never committed)
- Separate configs for development, staging, production
- Feature flags for progressive rollout
- Health check endpoints for monitoring

### Error Handling
- Structured error logging with correlation IDs
- User-friendly error messages (no stack traces)
- Graceful degradation where possible
- Retry mechanisms with circuit breakers
- Centralized error tracking (Sentry, Application Insights)

### Documentation
- README with setup instructions and architecture overview
- API documentation auto-generated from code
- Component documentation with Storybook (optional)
- Inline code comments for complex logic
- Architecture Decision Records (ADRs) for significant choices

### CI/CD Requirements
- Automated build on every push
- Tests run before merge to main
- Linting and formatting checks enforced
- Security scanning for dependencies
- Automated deployment to staging
- Manual approval for production deployment

## Development Workflow
- Local development with hot reload
- Docker containers for consistency
- Database migrations version controlled
- Seed data for development environment
- Pre-commit hooks for linting and formatting
- Code review checklist enforced

## Monitoring & Observability
- Structured logging with appropriate levels
- Distributed tracing for request flow
- Custom metrics for business KPIs
- Real user monitoring for frontend
- Alerts for error rates and performance degradation
- Regular performance profiling

## Security Requirements
- HTTPS only in production
- Security headers (CSP, HSTS, X-Frame-Options)
- Input sanitization on frontend and backend
- No sensitive data in URLs or logs
- Regular dependency updates
- OWASP Top 10 compliance
- Penetration testing for production releases