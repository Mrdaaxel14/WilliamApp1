using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    public partial class CuentaUsuarioPage : ContentPage
    {
        public CuentaUsuarioPage()
        {
            InitializeComponent();
            BindingContext = new PerfilViewModel();
        }
    }
}