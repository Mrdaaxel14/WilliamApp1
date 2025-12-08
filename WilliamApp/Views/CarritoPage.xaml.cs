using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;
using WilliamApp.Models;
using System.Collections.Generic;

namespace WilliamApp.Views
{
    public partial class CarritoPage : ContentPage
    {
        private CarritoViewModel _viewModel;

        public CarritoPage()
        {
            InitializeComponent();
            _viewModel = new CarritoViewModel();
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.Recargar();
        }

        // ✅ MODIFICADO: Evento con CommandParameter
        private async void OnCarritoItemTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is CarritoItem item && item.Producto != null)
            {
                await Shell.Current.GoToAsync(nameof(DetalleProductoPage), new Dictionary<string, object>
                {
                    { "producto", item.Producto }
                });
            }
        }
    }
}