````markdown
# Tasks: API Unit Testing Coverage

**Input**: Design documents from `/specs/002-unit-tests/`
**Prerequisites**: plan.md, spec.md, research.md, quickstart.md

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `- [ ] [ID] [P?] [Story?] Description`

- **Checkbox**: `- [ ]` (markdown task checkbox)
- **[ID]**: Sequential task number (T001, T002, T003...)
- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2)
- Include exact file paths in descriptions

## Path Conventions

Based on plan.md structure:
- **API Project**: `TaskApi/` (existing)
- **Test Project**: `TaskApi.Tests/` (new)

---

## Phase 1: Setup (Test Project Infrastructure)

**Purpose**: Initialize test project and configure testing frameworks

- [X] T001 Create xUnit test project TaskApi.Tests in repository root
- [X] T002 Add NuGet package xunit version 2.6.2 to TaskApi.Tests/TaskApi.Tests.csproj
- [X] T003 [P] Add NuGet package xunit.runner.visualstudio version 2.5.4 to TaskApi.Tests/TaskApi.Tests.csproj
- [X] T004 [P] Add NuGet package Microsoft.NET.Test.Sdk version 17.8.0 to TaskApi.Tests/TaskApi.Tests.csproj
- [X] T005 [P] Add NuGet package Moq version 4.20.70 to TaskApi.Tests/TaskApi.Tests.csproj
- [X] T006 [P] Add NuGet package FluentAssertions version 6.12.0 to TaskApi.Tests/TaskApi.Tests.csproj
- [X] T007 [P] Add NuGet package coverlet.collector version 6.0.0 to TaskApi.Tests/TaskApi.Tests.csproj
- [X] T008 Add project reference to TaskApi/TaskApi.csproj from TaskApi.Tests/TaskApi.Tests.csproj
- [X] T009 Create directory structure TaskApi.Tests/Controllers/
- [X] T010 [P] Create directory structure TaskApi.Tests/Fixtures/
- [X] T011 [P] Create directory structure TaskApi.Tests/Mocks/

---

## Phase 2: Foundational (Test Infrastructure & Helpers)

**Purpose**: Core test infrastructure that ALL tests will depend on

**⚠️ CRITICAL**: No user story tests can be written until this phase is complete

- [X] T012 Create TestDataFixtures class in TaskApi.Tests/Fixtures/TestDataFixtures.cs with SampleTask() method
- [X] T013 Add SampleTask(Action<TaskEntity>) overload to TestDataFixtures for customization
- [X] T014 [P] Add SampleCreateDto() method to TestDataFixtures in TaskApi.Tests/Fixtures/TestDataFixtures.cs
- [X] T015 [P] Add SampleUpdateDto() method to TestDataFixtures in TaskApi.Tests/Fixtures/TestDataFixtures.cs
- [X] T016 Create MockTaskRepository helper class in TaskApi.Tests/Mocks/MockTaskRepository.cs
- [X] T017 Add CreateMockForGetById() method to MockTaskRepository for common GetById scenarios
- [X] T018 [P] Add CreateMockForGetAll() method to MockTaskRepository for common GetAll scenarios
- [X] T019 Create main test class TasksControllerTests in TaskApi.Tests/Controllers/TasksControllerTests.cs

**Checkpoint**: Foundation ready - user story test implementation can now begin in parallel

---

## Phase 3: User Story 1 - Validate Core CRUD Operations (Priority: P1) 🎯 MVP

**Goal**: Verify all basic CRUD operations work correctly with mocked dependencies

**Independent Test**: Run `dotnet test --filter "FullyQualifiedName~CreateTests|GetAllTests|GetByIdTests|UpdateTests|DeleteTests"` and verify 13+ tests pass

### Tests for Create Endpoint

- [X] T020 [P] [US1] Create nested class CreateTests in TaskApi.Tests/Controllers/TasksControllerTests.cs
- [X] T021 [P] [US1] Implement test Create_WithValidDto_Returns201Created in CreateTests
- [X] T022 [P] [US1] Implement test Create_WithValidDto_ReturnsTaskWithGeneratedId in CreateTests
- [X] T023 [P] [US1] Implement test Create_WithValidDto_CallsRepositoryCreateAsync in CreateTests

### Tests for GetAll Endpoint

- [X] T024 [P] [US1] Create nested class GetAllTests in TaskApi.Tests/Controllers/TasksControllerTests.cs
- [X] T025 [P] [US1] Implement test GetAll_WithNoFilters_Returns200OK in GetAllTests
- [X] T026 [P] [US1] Implement test GetAll_WithNoFilters_ReturnsAllTasks in GetAllTests
- [X] T027 [P] [US1] Implement test GetAll_WithNoFilters_MapsEntitiesToDtos in GetAllTests

### Tests for GetById Endpoint

- [X] T028 [P] [US1] Create nested class GetByIdTests in TaskApi.Tests/Controllers/TasksControllerTests.cs
- [X] T029 [P] [US1] Implement test GetById_WithValidId_Returns200OK in GetByIdTests
- [X] T030 [P] [US1] Implement test GetById_WithValidId_ReturnsCorrectTask in GetByIdTests
- [X] T031 [P] [US1] Implement test GetById_WithValidId_MapsDtoCorrectly in GetByIdTests

### Tests for Update Endpoint

- [X] T032 [P] [US1] Create nested class UpdateTests in TaskApi.Tests/Controllers/TasksControllerTests.cs
- [X] T033 [P] [US1] Implement test Update_WithValidData_Returns200OK in UpdateTests
- [X] T034 [P] [US1] Implement test Update_WithValidData_ReturnsUpdatedTask in UpdateTests
- [X] T035 [P] [US1] Implement test Update_WithValidData_CallsRepositoryUpdateAsync in UpdateTests

### Tests for Delete Endpoint

- [X] T036 [P] [US1] Create nested class DeleteTests in TaskApi.Tests/Controllers/TasksControllerTests.cs
- [X] T037 [P] [US1] Implement test Delete_WithValidId_Returns204NoContent in DeleteTests
- [X] T038 [P] [US1] Implement test Delete_WithValidId_CallsRepositoryDeleteAsync in DeleteTests

**Checkpoint**: At this point, User Story 1 should be fully tested (13 tests for core CRUD operations)

---

## Phase 4: User Story 2 - Validate Error Handling and Edge Cases (Priority: P2)

**Goal**: Verify API handles invalid inputs and error conditions gracefully

**Independent Test**: Run `dotnet test --filter "FullyQualifiedName~Validation|NotFound|Error"` and verify 8+ tests pass

### Validation Error Tests

- [X] T039 [P] [US2] Implement test Create_WithWhitespaceTitle_Returns400BadRequest in CreateTests
- [X] T040 [P] [US2] Implement test Create_WithWhitespaceTitle_ReturnsValidationErrors in CreateTests
- [X] T041 [P] [US2] Implement test Update_WithWhitespaceTitle_Returns400BadRequest in UpdateTests
- [X] T042 [P] [US2] Implement test Update_WithWhitespaceTitle_ReturnsValidationErrors in UpdateTests

### Not Found Error Tests

- [X] T043 [P] [US2] Implement test GetById_WithNonExistentId_Returns404NotFound in GetByIdTests
- [X] T044 [P] [US2] Implement test GetById_WithNonExistentId_ReturnsErrorMessage in GetByIdTests
- [X] T045 [P] [US2] Implement test Update_WithNonExistentId_Returns404NotFound in UpdateTests
- [X] T046 [P] [US2] Implement test Delete_WithNonExistentId_Returns404NotFound in DeleteTests

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently (21 tests total)

---

## Phase 5: User Story 3 - Validate Status Toggle Operations (Priority: P3)

**Goal**: Verify task completion status can be toggled correctly

**Independent Test**: Run `dotnet test --filter "FullyQualifiedName~MarkComplete|MarkIncomplete"` and verify 6+ tests pass

### Tests for MarkComplete Endpoint

- [X] T047 [P] [US3] Create nested class MarkCompleteTests in TaskApi.Tests/Controllers/TasksControllerTests.cs
- [X] T048 [P] [US3] Implement test MarkComplete_WithValidId_Returns200OK in MarkCompleteTests
- [X] T049 [P] [US3] Implement test MarkComplete_WithValidId_ReturnsTaskMarkedComplete in MarkCompleteTests
- [X] T050 [P] [US3] Implement test MarkComplete_WithNonExistentId_Returns404NotFound in MarkCompleteTests

### Tests for MarkIncomplete Endpoint

- [X] T051 [P] [US3] Create nested class MarkIncompleteTests in TaskApi.Tests/Controllers/TasksControllerTests.cs
- [X] T052 [P] [US3] Implement test MarkIncomplete_WithValidId_Returns200OK in MarkIncompleteTests
- [X] T053 [P] [US3] Implement test MarkIncomplete_WithValidId_ReturnsTaskMarkedIncomplete in MarkIncompleteTests

**Checkpoint**: All user stories should now be independently functional (27 tests total)

---

## Phase 6: User Story 4 - Validate Query Filtering (Priority: P3)

**Goal**: Verify query parameters for filtering work correctly

**Independent Test**: Run `dotnet test --filter "FullyQualifiedName~GetAllTests"` and verify 4+ additional filtering tests pass

### Query Parameter Tests

- [X] T054 [P] [US4] Implement test GetAll_WithStatusFilter_ReturnsOnlyMatchingStatus in GetAllTests
- [X] T055 [P] [US4] Implement test GetAll_WithSearchFilter_ReturnsOnlyMatchingSearch in GetAllTests
- [X] T056 [P] [US4] Implement test GetAll_WithBothFilters_ReturnsTasksMatchingBoth in GetAllTests
- [X] T057 [P] [US4] Implement test GetAll_WithStatusFilter_CallsRepositoryWithCorrectParameters in GetAllTests

**Checkpoint**: All acceptance scenarios from spec.md should now have test coverage (31+ tests total)

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Improvements and validation across all tests

- [X] T058 Run all tests with `dotnet test` and verify 31+ tests pass
- [X] T059 Run tests with coverage `dotnet test --collect:"XPlat Code Coverage"` and verify 80%+ coverage
- [X] T060 Verify all tests execute in under 2 seconds total
- [X] T061 Verify all test names follow MethodName_Scenario_ExpectedBehavior convention
- [X] T062 [P] Verify all tests use FluentAssertions for assertions
- [X] T063 [P] Verify all tests follow Arrange-Act-Assert pattern with clear comments
- [X] T064 Review quickstart.md examples match actual test implementation
- [X] T065 Add XML documentation comments to TestDataFixtures methods
- [X] T066 [P] Add XML documentation comments to MockTaskRepository methods
- [X] T067 Run `dotnet test --logger "console;verbosity=detailed"` to verify no warnings or errors

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion (T001-T011) - BLOCKS all user story tests
- **User Story 1 (Phase 3)**: Depends on Foundational completion (T012-T019)
- **User Story 2 (Phase 4)**: Depends on Foundational completion (T012-T019) + User Story 1 test classes
- **User Story 3 (Phase 5)**: Depends on Foundational completion (T012-T019)
- **User Story 4 (Phase 6)**: Depends on Foundational completion (T012-T019) + GetAllTests class from US1
- **Polish (Phase 7)**: Depends on all user story tests being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - Creates base test classes
- **User Story 2 (P2)**: Can add tests to existing classes from US1 - Independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Creates new test classes - Independently testable
- **User Story 4 (P3)**: Adds tests to GetAllTests from US1 - Independently testable

### Within Each User Story

- Nested test classes before individual test methods
- All tests within a nested class can run in parallel (marked [P])
- Each test is isolated and can be run independently

### Parallel Opportunities

**Phase 1 - Setup (Parallel groups)**:
- T002 (xunit package) can run alone
- T003-T007 (all other packages) can run in parallel after T002
- T009-T011 (directory creation) can run in parallel

**Phase 2 - Foundational (Parallel groups)**:
- T012-T013 (TaskEntity fixtures) sequential
- T014-T015 (DTO fixtures) can run in parallel after T012-T013
- T016-T018 (Mock helpers) sequential
- T019 (main test class) after all fixtures/mocks

**Phase 3 - User Story 1**:
- All test class creations (T020, T024, T028, T032, T036) can run in parallel
- All test implementations within each class can run in parallel

**Phase 4 - User Story 2**:
- All validation tests (T039-T042) can run in parallel
- All not-found tests (T043-T046) can run in parallel

**Phase 5 - User Story 3**:
- T047 (class creation) then T048-T050 in parallel
- T051 (class creation) then T052-T053 in parallel
- Both endpoint groups can run in parallel

**Phase 6 - User Story 4**:
- All filtering tests (T054-T057) can run in parallel

**Phase 7 - Polish**:
- T058-T061 (test execution and verification) sequential
- T062-T063 (assertion verification) can run in parallel
- T064-T066 (documentation) can run in parallel
- T067 (final verification) after all

---

## Parallel Example: User Story 1

```bash
# Step 1: Create all test classes in parallel
Task T020: "Create nested class CreateTests"
Task T024: "Create nested class GetAllTests"
Task T028: "Create nested class GetByIdTests"
Task T032: "Create nested class UpdateTests"
Task T036: "Create nested class DeleteTests"

# Step 2: Implement all Create tests in parallel
Task T021: "Implement test Create_WithValidDto_Returns201Created"
Task T022: "Implement test Create_WithValidDto_ReturnsTaskWithGeneratedId"
Task T023: "Implement test Create_WithValidDto_CallsRepositoryCreateAsync"

# Step 3: Continue with other endpoints...
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup (T001-T011)
2. Complete Phase 2: Foundational (T012-T019) - CRITICAL
3. Complete Phase 3: User Story 1 (T020-T038)
4. **STOP and VALIDATE**: Run `dotnet test` - should have 13+ passing tests
5. Check coverage - should be 60%+ for basic CRUD
6. Can demo/deploy basic test coverage

### Incremental Delivery

1. Setup + Foundational → Test infrastructure ready (T001-T019)
2. Add User Story 1 → 13 tests, 60%+ coverage (T020-T038)
3. Add User Story 2 → 21 tests, 70%+ coverage (T039-T046)
4. Add User Story 3 → 27 tests, 75%+ coverage (T047-T053)
5. Add User Story 4 → 31+ tests, 80%+ coverage (T054-T057)
6. Polish → Final validation, documentation (T058-T067)

### Parallel Team Strategy

With multiple developers after Foundational phase completes:

1. Team completes Setup + Foundational together (T001-T019)
2. Once T019 is done:
   - **Developer A**: User Story 1 - Create tests (T020-T038)
   - **Developer B**: User Story 3 - Status toggle tests (T047-T053)
   - **Developer C**: Polish - Set up fixtures/mocks enhancements
3. Then add User Story 2 and 4 tests to existing classes

---

## Success Metrics Validation

After completing all phases, verify against spec.md success criteria:

- **SC-001**: ✅ All seven endpoints have 3+ tests each (31+ tests total)
- **SC-002**: ✅ 80%+ code coverage of TasksController (run T059)
- **SC-003**: ✅ All tests execute in <2 seconds (run T060)
- **SC-004**: ✅ Zero external dependencies (all mocks, no database)
- **SC-005**: ✅ Every success path has passing test
- **SC-006**: ✅ Every error path (400, 404) has passing test
- **SC-007**: ✅ 100% validation rules have test coverage
- **SC-008**: ✅ Consistent naming and AAA pattern (run T061, T063)

---

## Notes

- All tasks follow strict checkbox format: `- [ ] [ID] [P?] [Story?] Description with file path`
- [P] tasks operate on different files with no dependencies
- [Story] labels (US1, US2, US3, US4) map to user stories from spec.md
- Each phase builds on previous phases
- Tests are isolated and can run in any order
- Verify tests with `dotnet test --logger "console;verbosity=detailed"`
- Track coverage with `dotnet test --collect:"XPlat Code Coverage"`
- Commit after each logical group of related tasks
- Reference quickstart.md for testing patterns and examples

````