using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    public partial class ConfirmarPedidoPage : ContentPage
    {
        private ConfirmarPedidoViewModel viewModel;

        public ConfirmarPedidoPage()
        {
            InitializeComponent();
            viewModel = new ConfirmarPedidoViewModel();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.RecargarDatos();
        }
    }
}