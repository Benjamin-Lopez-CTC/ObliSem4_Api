# Project Context & Handover: ObliSem4Api (.NET 10 Web API)

## 1. Project Overview & Environment
- **Type:** Academic Project (Semester 4)
- **Framework:** .NET 10 Web API
- **ORM:** Entity Framework Core
- **Database:** MySQL
- **MySQL Provider:** Microting fork of Pomelo EF Core (used to resolve .NET 10 primitive / `DateOnly` casting bugs).
- **Architecture:** Controller -> Service -> Repository -> EF Core DbContext.

---

## 2. Core Domain Entities & Relationships

### `Users`
- `int Id`: Primary Key
- `string Email`: Unique, indexed, normalized on write (`Trim().ToLowerInvariant()`).
- `string PasswordHash`: Stored as a hashed string (PBKDF2 via `IPasswordHasher<Users>`). Never exposed via API.
- `string Role`: Defaults to `"User"`. Hardcoded for standard registrations; admin roles are managed directly in the DB.
- `string? Theme`: Nullable string. `null` indicates "follow system preference"; otherwise `"Light"` or `"Dark"`.
- `DateTime CreatedAt`: UTC timestamp set at creation.
- `ICollection<Tasks> Tasks`: Initialized to empty `new List<Tasks>()` (non-nullable in memory).
- `ICollection<Projects> Projects`: Initialized to empty `new List<Projects>()` (non-nullable in memory).

### Relationship Rules
- **Projects:** Belongs to a single User (`UserId`, required, cascade delete on user removal).
- **Tasks:** Belongs to a User (`UserId`, required).
- **Tasks & Projects:** A task can optionally belong to a project (`int? ProjectId` is nullable). Delete behavior on Project -> Task is set to `DeleteBehavior.SetNull` (or restrict) to avoid multiple cascade deletion paths in EF Core.

---

## 3. DTO Schema (`namespace ObliSem4Api.DTO.Users;`)

All User-related DTOs are consolidated under the `ObliSem4Api.DTO.Users` namespace:

- **`UserRegisterDto`:**
  - `string Email`
  - `string Password` *(Note: Inbound raw plain-text password from client; renamed from PasswordHash)*
- **`UserLoginDto`:**
  - `string Email`
  - `string Password` *(Note: Inbound raw plain-text password from client; renamed from PasswordHash)*
- **`UserReadDto`:**
  - `int Id`
  - `string Email`
  - `string Role`
  - `string? Theme`
  - `DateTime CreatedAt`
- **`UserSummaryDto`:**
  - `int Id`
  - `string Email`
- **`UserThemeDto`:**
  - `string? Theme` *(Nullable: null, "Light", or "Dark")*
- **`AuthResponseDto`:**
  - `string Token`
  - `DateTime ExpiresAt`
  - `UserReadDto User`

---

## 4. Repository Design & Decision Log

### `IUsersRepository.cs` Contract
```csharp
namespace ObliSem4Api.Interfaces;

public interface IUsersRepository
{
    Task<List<Users>> GetAll(); // Or GetAllAsync()
    Task<Users?> GetByEmailAsync(string email);
    Task<Users?> GetByIdAsync(int id);
    Task<bool> UpdateThemeAsync(int id, string? theme);
    Task AddAsync(Users user);
}
```

### Key Decisions
- **`AddAsync` signature:** Returns `Task` (void command). Respects Command-Query Separation (CQS). Because EF Core tracks entity references, calling `_context.SaveChangesAsync()` inside `AddAsync` mutates the entity in memory and populates `user.Id` for the caller.
- **`GetByEmailAsync`:** Uses `SingleOrDefaultAsync(u => u.Email == normalizedEmail)` to leverage the unique MySQL index (avoiding SQL function calls inside the LINQ expression like `u.Email.ToLower()`).
- **`UpdateThemeAsync`:** Uses `FindAsync(id)`, updates `user.Theme = theme;`, calls `SaveChangesAsync()`, and returns `true` (or `false` if not found).
- **No Direct Navigation Includes:** Standard user queries do not `.Include()` collections (`Tasks`, `Projects`) to avoid heavy payloads and circular reference serialization bugs.

---

## 5. Security & Authentication Architecture

- **Password Hashing:** Native `Microsoft.AspNetCore.Identity.IPasswordHasher<Users>` via DI (`PasswordHasher<Users>`). Zero third-party packages required.
  - Hashing: `_hasher.HashPassword(user, plainTextPassword)`
  - Verification: `_hasher.VerifyHashedPassword(user, user.PasswordHash, plainTextPassword)`
- **JWT Authentication:** Package `Microsoft.AspNetCore.Authentication.JwtBearer` (version matching .NET 10).
- **Claims Included in Token:**
  - `ClaimTypes.NameIdentifier` -> `user.Id.ToString()`
  - `ClaimTypes.Email` -> `user.Email`
  - `ClaimTypes.Role` -> `user.Role`
- **Identity Extraction:** Protected endpoints (`/me`, `/theme`) extract `userId` directly from `User.FindFirstValue(ClaimTypes.NameIdentifier)` to prevent unauthorized ID manipulation.

---

## 6. Immediate Next Steps for the Incoming Agent

1. **`UsersService` Implementation:**
   - Inject `IUsersRepository`, `IPasswordHasher<Users>`, and `IConfiguration` (for JWT settings).
   - Implement `RegisterAsync(UserRegisterDto)` with email normalization, duplicate check, hashing, role defaults (`"User"`), persistence, and token issuance.
   - Implement `LoginAsync(UserLoginDto)` with email lookup, hash verification, and token issuance.
   - Implement `UpdateThemeAsync(int userId, UserThemeDto)` with validation (`null`, `"Light"`, `"Dark"`).
2. **`UsersController` Implementation:**
   - Endpoints: `POST /api/users/register`, `POST /api/users/login`, `[Authorize] GET /api/users/me`, `[Authorize] PATCH /api/users/theme`.
3. **`Program.cs` Configuration:**
   - Configure JWT Bearer options with `TokenValidationParameters` (`ValidIssuer`, `ValidAudience`, `IssuerSigningKey`).
   - Register DI services: `AddScoped<IUsersRepository, UsersRepository>()`, `AddScoped<IUsersService, UsersService>()`, `AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>()`.
   - Ensure middleware order: `app.UseAuthentication();` before `app.UseAuthorization();`.