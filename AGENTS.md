# AGENTS.md - BlazorApp1 Development Guide

> Guidelines for agents working on this .NET 10.0 Blazor Server application.

## Build & Run Commands

```bash
# Build
dotnet build

# Run in development (http)
dotnet run --project BlazorApp1

# Run with HTTPS
dotnet run --project BlazorApp1 --launch-profile https

# Watch mode
dotnet watch --project BlazorApp1

# Release build
dotnet build --configuration Release
```

## Testing

```bash
# Run all tests
dotnet test

# Run a single test project
dotnet test BlazorApp1.Tests

# Run specific test method
dotnet test --filter "FullyQualifiedName~TestMethodName"

# Run without building
dotnet test --no-build
```

## Code Style

### Naming Conventions
| Element | Convention | Example |
|---------|-----------|---------|
| Razor Components | kebab-case | `NuevaTarima.razor` |
| C# Files | PascalCase | `TarimaService.cs` |
| Routes/URLs | kebab-case | `/tarimas/nueva_tarima` |

### C# Conventions
- **Implicit usings**: Enabled
- **Nullable**: Enabled (`string?` syntax preferred)
- **Properties**: PascalCase
- **Fields**: camelCase with `private` modifier
- **Accessibility**: Always specify

```csharp
public class TarimaModel
{
    public string CodigoBarras { get; set; } = "";
    private string? _error;

    [Required(ErrorMessage = "El campo es requerido")]
    public int CantidadCajas { get; set; }
}
```

### Component Structure (.razor files)

```razor
@page "/route-path"
@using BlazorApp1.Components.Shared
@inject NavigationManager Navigation
@inject AuthenticationStateProvider AuthenticationStateProvider

<PageTitle>Page Title</PageTitle>

<!-- Markup -->

@code {
    // Private fields first
    private string? errorMessage;

    // Parameters
    [Parameter] public string? ItemId { get; set; }

    // Inject dependencies
    [Inject] private IMyService? MyService { get; set; }

    // Lifecycle
    protected override async Task OnInitializedAsync() { }

    // Private methods
    private void HandleClick() { }

    // Nested classes
    public class MyModel { }
}
```

### Razor Guidelines
- **PascalCase** for component names and attributes
- **camelCase** for event handlers (`@onclick`, `@bind-Value`)
- **kebab-case** for CSS classes
- Prefer `@bind-Value` for two-way binding

### Global Imports (`Components/_Imports.razor`)
```razor
@using System.Net.Http
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using BlazorApp1
@using BlazorApp1.Components
@using BlazorApp1.Components.Layout
```

### Validation Patterns
```csharp
public class TarimaModel
{
    [Required(ErrorMessage = "El código de barras es requerido")]
    [StringLength(30)]
    public string CodigoBarras { get; set; } = "";

    [Range(1, 999)]
    public int CantidadCajas { get; set; }
}
```

### Forms
```razor
<EditForm Model="model" OnValidSubmit="HandleSubmit">
    <DataAnnotationsValidator />
    <InputText @bind-Value="model.CodigoBarras" class="form-control" />
    <ValidationMessage For="@(() => model.CodigoBarras)" />
</EditForm>
```

### Error Handling
```csharp
private async Task HandleSubmit()
{
    try
    {
        isLoading = true;
    }
    catch (Exception ex)
    {
        errorMessage = "Error al procesar";
    }
    finally
    {
        isLoading = false;
    }
}
```

### Navigation
```csharp
// With page reload
Navigation.NavigateTo("/path", forceLoad: true);

// SPA style
Navigation.NavigateTo("/path");
```

### Authentication
```csharp
var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
var user = authState.User;
bool isAdmin = user.IsInRole("administrador") || user.IsInRole("jefe_produccion");
```

## Project Structure

```
BlazorApp1/BlazorApp1/
├── Components/
│   ├── _Imports.razor         # Global usings
│   ├── App.razor              # HTML shell
│   ├── Layout/                # Layout components
│   ├── Pages/                 # Route pages
│   ├── Shared/                # Reusable components
│   ├── Tarimas/               # Feature: Tarimas
│   ├── Dashboard/             # Feature: Dashboard
│   └── Auth/                  # Feature: Auth
├── Program.cs
└── wwwroot/
```

## Important Notes

1. **No code-behind files** - Use `@code` blocks within .razor files
2. **ViewModels as nested classes** - Simple models defined within components
3. **Spanish UI** - All user-facing text in Spanish
4. **Spanish date formats** - dd/MM/yyyy, `,` as decimal separator
5. **TODO comments** - Many services are stubs

## Skills & References
- **ABP Patterns**: `.agents/skills/abp-blazor/SKILL.md`
- **Bootstrap 5**: https://getbootstrap.com/docs/5.3/
- **FontAwesome 6**: https://fontawesome.com/docs
