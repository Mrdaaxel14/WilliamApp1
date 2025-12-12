using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    public partial class CuentaUsuarioPage : ContentPage
    {
        private PerfilViewModel viewModel;

        public CuentaUsuarioPage()
        {
            InitializeComponent();
            viewModel = new PerfilViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.RecargarDatos();
        }
    }
}