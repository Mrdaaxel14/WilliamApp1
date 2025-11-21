using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}
