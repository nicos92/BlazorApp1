# BlazorApp1 Project Overview

## Project Type
**Code Project** - ASP.NET Core Blazor Web Application

## Project Overview

This is a **Blazor Interactive Server** web application built with **.NET 10.0**. The project uses the latest Blazor features with interactive server-side rendering mode.

### Main Technologies
- **Framework**: ASP.NET Core 10.0
- **UI Framework**: Blazor (Interactive Server render mode)
- **Language**: C# with nullable reference types enabled
- **IDE**: Visual Studio (solution file: `BlazorApp1.slnx`)

### Architecture

The application follows standard Blazor project structure:

```
BlazorApp1/
├── Components/
│   ├── Auth/          # Authentication-related components
│   ├── Dashboard/     # Dashboard components
│   ├── Layout/        # Layout components (MainLayout, NavMenu)
│   ├── Pages/         # Razor page components
│   ├── Shared/        # Shared components
│   ├── Tarimas/       # Custom feature module
│   ├── App.razor      # Root component
│   └── Routes.razor   # Routing configuration
├── Properties/
│   └── launchSettings.json
├── wwwroot/           # Static assets (implied)
├── Program.cs         # Application entry point
└── appsettings.json   # Configuration
```

### Key Features (inferred from structure)
- Interactive server-side Blazor rendering
- Custom layout with navigation menu
- Error handling and status code pages (404 handling)
- Multiple pages: Home, Counter, Dashboard, Weather, Inicio
- Reconnection modal support for server disconnects
- Antiforgery protection enabled

## Building and Running

### Prerequisites
- .NET 10.0 SDK or later
- Visual Studio 2022+ or VS Code with C# extension

### Commands

| Action | Command |
|--------|---------|
| **Restore dependencies** | `dotnet restore` |
| **Build** | `dotnet build` |
| **Run (development)** | `dotnet run` |
| **Run with specific profile** | `dotnet run --launch-profile http` or `dotnet run --launch-profile https` |
| **Watch for changes** | `dotnet watch run` |

### Development URLs
- **HTTP**: `http://blazorapp1.dev.localhost:5196`
- **HTTPS**: `https://blazorapp1.dev.localhost:7093`

### Environment
- Default environment: `Development`
- Logging: Information level (default), Warning for Microsoft.AspNetCore

## Development Conventions

### Code Style
- **Nullable reference types**: Enabled
- **Implicit usings**: Enabled (reduces boilerplate imports)
- **File-scoped namespaces**: Likely used (modern C# convention)

### Component Organization
- **Pages**: Components with `@page` directive for routing go in `Components/Pages/`
- **Layouts**: Layout components inherit from `LayoutComponentBase`
- **Shared components**: Reusable UI components in `Components/Shared/`
- **Feature modules**: Organized in subdirectories (e.g., `Dashboard/`, `Tarimas/`, `Auth/`)

### Razor Imports
Global imports are defined in `Components/_Imports.razor`:
- Common ASP.NET Core namespaces
- `BlazorApp1` and `BlazorApp1.Components`
- `BlazorApp1.Components.Layout`

### Testing
- No test project currently in solution
- Consider adding xUnit or NUnit test project for production code

### Configuration
- `appsettings.json`: Base configuration
- `appsettings.Development.json`: Development-specific settings
- `Properties/launchSettings.json`: Development server profiles

## Notes
- `BlazorDisableThrowNavigationException` is set to `true` in the project file, which disables throwing exceptions on navigation failures (useful for development)
- The app uses HTTPS redirection and HSTS in non-development environments
- Static assets are mapped via `MapStaticAssets()`
- Antiforgery tokens are enabled for form submissions
