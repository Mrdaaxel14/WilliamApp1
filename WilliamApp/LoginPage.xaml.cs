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
            // Placeholder for authentication logic
            string usuario = UsuarioEntry.Text;
            string contrase�a = Contrase�aEntry.Text;

            // Example validation (replace with actual authentication logic)
            if (!string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(contrase�a))
            {
                // Navigate to MainPage using Shell navigation
                Shell.Current.GoToAsync("//MainPage");
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            }
            else
            {
                DisplayAlert("Error", "Por favor, ingrese usuario y contrase�a", "OK");
            }
        }

    }
}