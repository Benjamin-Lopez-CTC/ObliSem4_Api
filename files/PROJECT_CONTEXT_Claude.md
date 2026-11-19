# Project context for the AI agent

Read this before suggesting or changing anything. Fill in the lines marked `TODO`.

## What this is

A task-keeping app for a university assignment, built by two students. It has three parts:

- **API:** .NET 10 Web API, Entity Framework Core, MySQL (Pomelo). Project name: `ObliSem4Api`.
- **Website:** Blazor, for desktop only. Hosting model: `TODO` (Server or WebAssembly). Styled with Bootstrap.
- **Mobile app:** React Native with Expo SDK 58, tested in Expo Go on Android. Styled with NativeWind.

Main features: projects that group tasks (optional), tasks with priority and due date, subtasks as checkbox items, trash and archive, filters, sorting, a dashboard, light and dark theme, and a standout feature: **tasks can be locked to a geographic zone**, so they can only be completed from inside it (mobile only).

## Rules for the agent (important)

- **AI is allowed only as assistance for the user interface and its implementation.** The API is written entirely by hand by the students. Do not create, edit or refactor API code unless explicitly asked.
- Make small, focused changes. Do not rewrite whole files. Explain what you changed in plain words so the students can defend it.
- Do not add new packages without asking first.
- Never read, print or write real secrets (database password, JWT key, SMTP password, Cloudinary secret). Never create files that contain them.
- Do not use raw color values in components. Use the palette tokens (see below).
- UI text is in **Spanish**. Code, identifiers and comments are in English.

## Priorities

**Imprescindible (must work):** register and login (username/email and password), projects, tasks, subtasks with progress, priority (Normal or High), trash and archive, search and filters, dashboard, light and dark theme, the zone feature on mobile.

**Deseable (only if time allows):** attachments (one file per task), month calendar, statistics, reminders, admin role (see all tasks, filter by user, hard delete).

**Dropped (do not build):** recurring tasks, task dependencies, location reminders, a map view of nearby tasks, Google login, biometric unlock.

## Data model (summary)

Tables: `Users`, `Projects`, `Tasks`, `Subtasks`. The API's read DTOs are the source of truth for what the screens receive. Ask to see the DTO files before assuming a field.

- A task belongs to one user and optionally to one project.
- Subtasks are created together with the task. They are not edited afterwards.
- Priority is `Normal` or `High`. Project status is `Pending`, `InProgress`, `Finished` or `Archived`.
- A task has an optional **zone** (latitude, longitude, radius in meters, address text) and an optional **attachment** (one file).
- Deleted items are soft-deleted (shown only in the trash view).

## Behavior the screens must follow

- **Color:** a task in a project shows its project's color. A task without a project can have its own optional color, and if it has none it shows a neutral outline circle. The color picker is hidden when the task has a project.
- **Subtask quick-create:** a "list" toggle on the new-task form shows a text field. The text is split on the `-T` marker, and each piece becomes a subtask. Show a live preview of the parsed list while typing. Send the list to the API in the same request that creates the task.
- **Zone:** the user sets it by dropping a pin on a map. The map starts at the user's current location, after asking for permission. A read-only field shows the street and number. The radius comes from a dropdown (100 m, 500 m, 1 km, 5 km, 10 km). A task whose zone the user is not inside cannot be completed from the app.
- **Filters and sorting:** each column header or button sets one field of the filter object that is sent to the API. The API does the filtering with `IQueryable`.
- **Session:** log in once. The token lasts three months and is stored securely (SecureStore on mobile, browser storage on web). An expired or rejected token sends the user to the login screen. Logging out clears the token and cancels scheduled notifications on the phone.
- **Notifications:** on the phone, local scheduled notifications derived from the due date. On the website, toast messages (toastr through a small service) while the site is open and the user is logged in.
- **Website on narrow screens:** hide the content and show a notice asking the user to use the mobile app or a desktop browser.

## Platform limits to respect

- Expo Go on Android: remote push notifications and background location are **not** available. Use local notifications and foreground location only.
- The API address must be configurable in one place in each client. The phone cannot use `localhost` (the Android emulator uses its own special address for the host machine).
- Navigation on mobile is a drawer opened from a button in the top-left corner, with the same visual style as the website.

## Design

- Layout reference: a simple, clean task list with a collapsible sidebar on the website (search, views, projects) and the same sections in the mobile drawer.
- Light and dark themes are required. Define the palette once as semantic names (background, surface, text, muted text, border, primary, danger) with a light and a dark value each, and use only those names in components.
  - Website: Bootstrap CSS variables, switching with `data-bs-theme` on the root element.
  - Mobile: the palette in the NativeWind config, `dark:` variants, and a manual toggle that defaults to the device setting.
- Check contrast of text against its background in both themes.

## Current state

- API: models, DbContext, DTOs, the first migration and the database are done. Auth and the routes are in progress.
- Website and mobile app: not started beyond setup. `TODO`: update this line as it changes.
