using Project_3.Services;

var builder = WebApplication.CreateBuilder(args);

// Blazor Web App services (Server interactivity)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DI for your game engine
builder.Services.AddScoped<GameEngine>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// ?? New .NET 8 mapping (NO MapFallbackToPage("/_Host"))
app.MapRazorComponents<Project_3.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
