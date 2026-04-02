# AGENTS.md - BlazorApp1 Development Guide

> Guidelines for agents working on this .NET 10.0 Blazor Server application.

## Build & Run Commands

```bash
# Build the solution
dotnet build

# Build specific project
dotnet build BlazorApp1/BlazorApp1.csproj

# Run in development (http://localhost:5000)
dotnet run --project BlazorApp1

# Run with HTTPS (https://localhost:5001)
dotnet run --project BlazorApp1 --launch-profile https

# Watch mode (auto-reload on changes)
dotnet watch --project BlazorApp1

# Release build
dotnet build --configuration Release
```

## Testing

> **Note:** No test project exists yet. Create one using:
> `dotnet new xunit -n BlazorApp1.Tests`

```bash
# Run all tests
dotnet test

# Run a single test project
dotnet test BlazorApp1.Tests

# Run specific test method
dotnet test --filter "FullyQualifiedName~TestMethodName"

# Run tests without building
dotnet test --no-build
```

## Code Style

### Naming Conventions
| Element | Convention | Example |
|---------|-----------|---------|
| Razor Components | kebab-case | `NuevaTarima.razor` |
| C# Files | PascalCase | `TarimaService.cs` |
| Routes/URLs | kebab-case | `/tarimas/nueva_tarima` |
| Properties/Methods | PascalCase | `CantidadCajas`, `HandleSubmit()` |
| Private fields | camelCase with `_` prefix | `_errorMessage`, `_isLoading` |

### C# Conventions
- **Implicit usings**: Enabled
- **Nullable**: Enabled (`string?` syntax preferred)
- **Accessibility**: Always specify (private, public, etc.)
- **File-scoped namespaces**: Required

```csharp
namespace BlazorApp1.Components.Tarimas;

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

<PageTitle>Page Title</PageTitle>

<!-- Markup -->

@code {
    private string? errorMessage;

    [Parameter] public string? ItemId { get; set; }

    [Inject] private IMyService? MyService { get; set; }

    protected override async Task OnInitializedAsync() { }

    private void HandleClick() { }

    public class MyModel { }
}
```

### Razor Guidelines
- **PascalCase** for component names and attributes (`<PageTitle>`, `@bind-Value`)
- **camelCase** for event handlers (`@onclick`, `@oninput`)
- **kebab-case** for CSS classes (`class="form-control"`)
- Prefer `@bind-Value` for two-way binding
- Use `@Assets[]` for static asset references in .NET 10

```razor
@* Static asset reference *@
<link rel="stylesheet" href="@Assets["lib/bootstrap/dist/css/bootstrap.min.css"]" />

@* Event handler binding *@
<button @onclick="HandleClick">Click me</button>

@* Two-way binding *@
<InputText @bind-Value="model.CodigoBarras" class="form-control" />
```

### Global Imports (`Components/_Imports.razor`)
```razor
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using static Microsoft.AspNetCore.Components.Web.RenderMode
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.JSInterop
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

    [Range(1, 999, ErrorMessage = "Debe ser entre 1 y 999")]
    public int CantidadCajas { get; set; }
}
```

### Forms
```razor
<EditForm Model="model" OnValidSubmit="HandleSubmit">
    <DataAnnotationsValidator />
    <div class="mb-3">
        <label class="form-label">Código de Barras</label>
        <InputText @bind-Value="model.CodigoBarras" class="form-control" />
        <ValidationMessage For="@(() => model.CodigoBarras)" />
    </div>
    <button type="submit" class="btn btn-primary" disabled="@isLoading">
        @(isLoading ? "Guardando..." : "Guardar")
    </button>
</EditForm>
```

### Error Handling
```csharp
private async Task HandleSubmit()
{
    try
    {
        isLoading = true;
        // await service.CallAsync();
    }
    catch (Exception ex)
    {
        errorMessage = "Error al procesar la solicitud";
        Console.Error.WriteLine(ex);
    }
    finally
    {
        isLoading = false;
    }
}
```

### Navigation
```csharp
Navigation.NavigateTo("/path", forceLoad: true);  // Full page reload
Navigation.NavigateTo("/path");                   // SPA navigation
```

## Project Structure

```
BlazorApp1/
├── BlazorApp1.sln
├── BlazorApp1/
│   ├── BlazorApp1.csproj
│   ├── Program.cs
│   ├── Components/
│   │   ├── _Imports.razor
│   │   ├── App.razor
│   │   ├── Layout/
│   │   │   └── NavMenu.razor
│   │   ├── Pages/
│   │   │   ├── Home.razor
│   │   │   └── Inicio.razor
│   │   └── Shared/
│   │       └── ResourcePreloader.razor
│   ├── wwwroot/
│   │   ├── css/
│   │   │   └── tarimas.css
│   │   └── lib/
│   └── appsettings.json
└── AGENTS.md
```

## Important Notes

1. **No code-behind files** - Use `@code` blocks within .razor files
2. **Nested models** - Simple models can be defined within components
3. **Spanish UI** - All user-facing text in Spanish
4. **Spanish date formats** - dd/MM/yyyy, `,` as decimal separator
5. **No authentication** - This is a basic Blazor Server app without auth
6. **No tests** - Test project needs to be created

## External References
- **Bootstrap 5**: https://getbootstrap.com/docs/5.3/
- **FontAwesome 6**: https://fontawesome.com/docs
