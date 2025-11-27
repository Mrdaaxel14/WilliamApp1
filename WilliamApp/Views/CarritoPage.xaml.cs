using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;
using WilliamApp.Models;
using System.Collections.Generic;

namespace WilliamApp.Views
{
    public partial class CarritoPage : ContentPage
    {
        public CarritoPage()
        {
            InitializeComponent();
            BindingContext = new CarritoViewModel();
        }

        private async void OnCarritoItemSeleccionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is CarritoItem item && item.Producto != null)
            {
                await Shell.Current.GoToAsync(nameof(DetalleProductoPage), new Dictionary<string, object>
                {
                    { "producto", item.Producto }
                });
            }
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}