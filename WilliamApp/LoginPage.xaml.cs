using Microsoft.Maui.Controls;
using System.Runtime.Versioning;

namespace WilliamApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        [SupportedOSPlatform("windows10.0.17763.0")]
        private void OnLoginClicked(object sender, EventArgs e)
        {
            // Validación para evitar desreferencia de una referencia posiblemente NULL
            if (Application.Current != null)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
        }
    }
}