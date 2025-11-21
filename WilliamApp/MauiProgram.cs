using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.Hosting;

namespace WilliamApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Si querés registrar services/viewmodels via DI, podés hacerlo aquí.
            // Ejemplo:
            // builder.Services.AddSingleton<WilliamApp.Services.AuthService>();
            // builder.Services.AddTransient<WilliamApp.Views.LoginPage>();
            // builder.Services.AddTransient<WilliamApp.ViewModels.LoginViewModel>();

            return builder.Build();
        }
    }
}
