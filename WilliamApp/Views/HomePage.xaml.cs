using Microsoft.Maui.Controls;

namespace WilliamApp.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }
        private async void OnPerfilClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//perfil");
        }
    }
}