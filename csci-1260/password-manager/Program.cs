using Project_4.Components;
using Project_4.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<PasswordManagerService>();
builder.Services.AddHttpClient<PwnedPasswordService>(client =>
{
    client.BaseAddress = new Uri("https://api.pwnedpasswords.com/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("Project4PasswordManager/1.0");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
