using Microsoft.Maui.Controls;
using WilliamApp.Helpers;

namespace WilliamApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Si hay token guardado, iniciar directamente la app principal
            if (!string.IsNullOrEmpty(Settings.Token))
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new Views.LoginPage());
            }
        }
    }

}