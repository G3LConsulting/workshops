---

description: "Task list for Demo Task API implementation"
---

# Tasks: Demo Task API

**Input**: Design documents from `/specs/001-task-api/`
**Prerequisites**: plan.md (✓), spec.md (✓), research.md (✓), data-model.md (✓), contracts/ (✓), quickstart.md (✓)

**Tests**: Manual testing via Swagger UI (automated tests optional for 45-minute workshop)

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3, US4)
- Include exact file paths in descriptions

## Path Conventions

- **Single project**: `TaskApi/` at workspace root with Models/, Data/, Controllers/
- All paths relative to workspace root

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and basic structure

- [X] T001 Create .NET 8 Web API project using `dotnet new webapi -n TaskApi`
- [X] T002 Add NuGet package Microsoft.EntityFrameworkCore.InMemory to TaskApi.csproj
- [X] T003 Add NuGet package Swashbuckle.AspNetCore (verify it exists) to TaskApi.csproj
- [X] T004 [P] Create folder structure: TaskApi/Models/Entities, TaskApi/Models/DTOs, TaskApi/Data/Repositories, TaskApi/Controllers
- [X] T005 [P] Enable XML documentation in TaskApi.csproj with GenerateDocumentationFile property

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [X] T006 [P] Create TaskEntity class in TaskApi/Models/Entities/TaskEntity.cs with Id, Title, Description, DueDate, IsComplete, CreatedAt, UpdatedAt properties
- [X] T007 Create TaskDbContext in TaskApi/Data/TaskDbContext.cs with DbSet<TaskEntity> and OnModelCreating configuration
- [X] T008 [P] Create ITaskRepository interface in TaskApi/Data/Repositories/ITaskRepository.cs with GetAllAsync, GetByIdAsync, CreateAsync, UpdateAsync, DeleteAsync methods
- [X] T009 Configure DI in Program.cs: AddDbContext with In-Memory database, AddScoped for ITaskRepository
- [X] T010 Configure Swagger in Program.cs: AddSwaggerGen with XML comments, UseSwagger, UseSwaggerUI

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Create and List Tasks (Priority: P1) 🎯 MVP

**Goal**: Enable creating tasks and retrieving all tasks with filtering/search

**Independent Test**: POST a task, then GET /api/tasks to see it in the list. Filter by status and search by text.

### Implementation for User Story 1

- [X] T011 [P] [US1] Create TaskDto in TaskApi/Models/DTOs/TaskDto.cs with all 7 fields and XML comments
- [X] T012 [P] [US1] Create CreateTaskDto in TaskApi/Models/DTOs/CreateTaskDto.cs with Required, StringLength, MaxLength attributes
- [X] T013 [US1] Implement TaskRepository in TaskApi/Data/Repositories/TaskRepository.cs with constructor injecting TaskDbContext
- [X] T014 [US1] Implement GetAllAsync in TaskRepository with status filtering (complete/incomplete/all) and search logic
- [X] T015 [US1] Implement CreateAsync in TaskRepository with Guid generation, timestamp setting, IsComplete default
- [X] T016 [US1] Create TasksController in TaskApi/Controllers/TasksController.cs with [ApiController], [Route("api/[controller]")] attributes
- [X] T017 [US1] Implement GET /api/tasks endpoint with status and search query parameters, ProducesResponseType attributes
- [X] T018 [US1] Implement POST /api/tasks endpoint with CreateTaskDto validation, whitespace check, CreatedAtAction response
- [X] T019 [US1] Add MapToDto helper method in TasksController to convert TaskEntity to TaskDto

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently via Swagger

---

## Phase 4: User Story 2 - View and Update Individual Tasks (Priority: P2)

**Goal**: Enable retrieving and updating individual tasks by ID

**Independent Test**: Use GET /api/tasks/{id} to retrieve a specific task, then PUT /api/tasks/{id} to update it

### Implementation for User Story 2

- [X] T020 [P] [US2] Create UpdateTaskDto in TaskApi/Models/DTOs/UpdateTaskDto.cs with validation attributes and IsComplete field
- [X] T021 [US2] Implement GetByIdAsync in TaskRepository using FindAsync
- [X] T022 [US2] Implement UpdateAsync in TaskRepository with UpdatedAt timestamp refresh and SaveChangesAsync
- [X] T023 [US2] Implement GET /api/tasks/{id} endpoint in TasksController with 404 handling
- [X] T024 [US2] Implement PUT /api/tasks/{id} endpoint in TasksController with validation, 404 handling, entity update logic

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently

---

## Phase 5: User Story 3 - Mark Tasks Complete/Incomplete (Priority: P3)

**Goal**: Enable quick toggle of task completion status via PATCH endpoints

**Independent Test**: Create a task, use PATCH /api/tasks/{id}/complete, verify isComplete=true, then use /incomplete to toggle back

### Implementation for User Story 3

- [X] T025 [US3] Implement PATCH /api/tasks/{id}/complete endpoint in TasksController with GetByIdAsync, status update, UpdateAsync
- [X] T026 [US3] Implement PATCH /api/tasks/{id}/incomplete endpoint in TasksController with GetByIdAsync, status update, UpdateAsync
- [X] T027 [US3] Add ProducesResponseType attributes and XML comments to both PATCH endpoints

**Checkpoint**: All user stories except Delete should now be independently functional

---

## Phase 6: User Story 4 - Delete Tasks (Priority: P4)

**Goal**: Enable deletion of tasks by ID

**Independent Test**: Create a task, DELETE it, verify 204 response, then GET it to verify 404

### Implementation for User Story 4

- [X] T028 [US4] Implement DeleteAsync in TaskRepository with FindAsync and Remove operations
- [X] T029 [US4] Implement DELETE /api/tasks/{id} endpoint in TasksController with 404 handling and NoContent response
- [X] T030 [US4] Add ProducesResponseType attributes and XML comments to DELETE endpoint

**Checkpoint**: All user stories should now be independently functional - complete CRUD API

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [X] T031 [P] Verify all controller endpoints have [ProducesResponseType] attributes for 200/201/204/400/404 status codes
- [X] T032 [P] Verify all DTOs have XML comments with /// summary tags
- [X] T033 Run `dotnet build` and verify no warnings or errors
- [X] T034 Start application with `dotnet run` and access Swagger UI at http://localhost:5000/swagger
- [ ] T035 Execute manual test scenarios from quickstart.md (8 test cases covering all user stories)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3-6)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3 → P4)
- **Polish (Phase 7)**: Depends on all user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - Builds on P1 but independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Uses P2's GetByIdAsync but independently testable
- **User Story 4 (P4)**: Can start after Foundational (Phase 2) - Completely independent

### Within Each User Story

- DTOs can be created in parallel with repository methods (marked [P])
- Repository implementation must complete before controller implementation
- All endpoints in same controller can reference shared MapToDto helper
- Each story builds incrementally on repository interface

### Parallel Opportunities

- Phase 1 Setup: All tasks (T001-T005) can run in parallel except T004 must complete before folder-dependent tasks
- Phase 2 Foundational: T006 (Entity) and T008 (Interface) can run in parallel
- User Story 1: T011 (TaskDto) and T012 (CreateTaskDto) can run in parallel
- User Story 2: T020 (UpdateTaskDto) can run in parallel with T021-T022 (repository methods)
- Phase 7 Polish: T031 and T032 can run in parallel

---

## Parallel Example: User Story 1

```bash
# Launch DTOs in parallel:
Task: "Create TaskDto in TaskApi/Models/DTOs/TaskDto.cs with all 7 fields and XML comments"
Task: "Create CreateTaskDto in TaskApi/Models/DTOs/CreateTaskDto.cs with Required, StringLength, MaxLength attributes"

# Then implement repository (sequential):
Task: "Implement TaskRepository in TaskApi/Data/Repositories/TaskRepository.cs"
Task: "Implement GetAllAsync in TaskRepository"
Task: "Implement CreateAsync in TaskRepository"

# Then implement controller endpoints (sequential but could be parallel if different developers):
Task: "Create TasksController with base setup"
Task: "Implement GET /api/tasks endpoint"
Task: "Implement POST /api/tasks endpoint"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup (T001-T005) - 5 minutes
2. Complete Phase 2: Foundational (T006-T010) - 10 minutes
3. Complete Phase 3: User Story 1 (T011-T019) - 15 minutes
4. **STOP and VALIDATE**: Test User Story 1 independently via Swagger
5. Deploy/demo if ready - **MVP complete in ~30 minutes!**

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready (15 min)
2. Add User Story 1 → Test independently → Deploy/Demo (MVP at 30 min)
3. Add User Story 2 → Test independently → Deploy/Demo (35 min)
4. Add User Story 3 → Test independently → Deploy/Demo (38 min)
5. Add User Story 4 → Test independently → Deploy/Demo (40 min)
6. Polish & validate all scenarios → Complete (45 min)

### Parallel Team Strategy

With multiple developers (NOT typical for workshop, but possible):

1. Team completes Setup + Foundational together (15 min)
2. Once Foundational is done:
   - Developer A: User Story 1 (T011-T019)
   - Developer B: User Story 2 (T020-T024) 
   - Developer C: User Story 3 (T025-T027) + User Story 4 (T028-T030)
3. Stories complete and integrate independently
4. Team validates together in Polish phase

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Manual testing via Swagger required after each phase (no automated tests for workshop)
- Commit after each user story completion or logical group
- Stop at any checkpoint to validate story independently
- Total estimated time: 30-45 minutes for all user stories
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence

---

## Manual Testing Checklist (Per Quickstart.md)

After completing all user stories, execute these tests in Swagger UI:

1. ✅ **Test 1**: POST /api/tasks with valid data → Verify 201 Created
2. ✅ **Test 2**: GET /api/tasks → Verify array with created task
3. ✅ **Test 3**: GET /api/tasks/{id} → Verify 200 OK with task details
4. ✅ **Test 4**: PUT /api/tasks/{id} → Verify 200 OK with updates
5. ✅ **Test 5**: PATCH /api/tasks/{id}/complete → Verify isComplete=true
6. ✅ **Test 6**: GET /api/tasks?status=complete → Verify filtering works
7. ✅ **Test 7**: DELETE /api/tasks/{id} → Verify 204, then 404 on GET
8. ✅ **Test 8**: POST with empty title → Verify 400 Bad Request with validation errors

**All tests passing = Workshop complete! 🎉**
