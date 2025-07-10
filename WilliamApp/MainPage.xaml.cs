using System.Runtime.Versioning;
using Microsoft.Maui.Controls;

namespace WilliamApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        [SupportedOSPlatform("windows10.0.17763.0")]
        private async void OnVerReportesDeVentasClicked(object sender, EventArgs e)
        {
            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763))
            {
                // Navegar a la página SalesReportPage
                await Navigation.PushAsync(new SalesReportPage());
            }
            else
            {
                await DisplayAlert("Error", "Esta funcionalidad solo está disponible en Windows 10.0.17763.0 o versiones posteriores.", "OK");
            }
        }
    }
}