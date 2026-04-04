using BlazorApp1.Application.Interfaces;
using BlazorApp1.Application.Services;
using BlazorApp1.Components;
using BlazorApp1.Infrastructure.Persistence;
using BlazorApp1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Infrastructure - Repository (MySQL implementation)
builder.Services.AddScoped<ITarimaRepository, MySqlTarimaRepository>();

// Application - Services
builder.Services.AddScoped<ITarimaService, TarimaService>();

// Presentation - Validators and Parsers
builder.Services.AddScoped<IBarcodeParserService, BarcodeParserService>();
builder.Services.AddScoped<ITarimaValidatorService, TarimaValidatorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
