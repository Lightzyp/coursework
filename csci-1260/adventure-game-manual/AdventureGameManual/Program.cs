using AdventureGameManual.Components;
using AdventureGameManual.Services;


namespace AdventureGameManual
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // Register app services
            builder.Services.AddSingleton<GameDataService>();
            builder.Services.AddSingleton<NoteService>(sp =>
            {
                var dataPath = Path.Combine(AppContext.BaseDirectory, "App_Data", "notes.json");
                return new NoteService(dataPath);
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
