using Microsoft.Maui.Controls;
namespace WilliamApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
        string usuario = CorreoEntry?.Text ?? string.Empty;
        string contrase�a = Contrase�aEntry?.Text ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(usuario) && !string.IsNullOrWhiteSpace(contrase�a))
            {
            Application.Current.MainPage = new AppShell();
            }
        else
            {
            DisplayAlert("Error", "Por favor, ingrese usuario y contrase�a", "OK");
            }
        }

    }
}