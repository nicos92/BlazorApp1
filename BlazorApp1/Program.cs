using BlazorApp1.Application.Interfaces;
using BlazorApp1.Application.Services;
using BlazorApp1.Components;
using BlazorApp1.Infrastructure.Persistence;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Authentication
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthenticationStateProvider>());

// Infrastructure - Repository (MySQL implementation)
builder.Services.AddScoped<ITarimaRepository, MySqlTarimaRepository>();
builder.Services.AddScoped<IUsuarioRepository, MySqlUsuarioRepository>();

// Application - Services
builder.Services.AddScoped<ITarimaService, TarimaService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
