using GameManual.Components;
using GameManual.Services;

namespace GameManual
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddHttpClient("PokeApi", c => c.BaseAddress = new Uri("https://pokeapi.co/api/v2/"));
            builder.Services.AddScoped<ApiClient>();
            builder.Services.AddScoped<CharacterService>();
            builder.Services.AddScoped<ItemService>();
            builder.Services.AddSingleton<NoteService>();

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
