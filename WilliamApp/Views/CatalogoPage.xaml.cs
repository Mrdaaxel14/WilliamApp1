using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.ViewModels;
using System.Collections.Generic;

namespace WilliamApp.Views
{
    public partial class CatalogoPage : ContentPage
    {
        public CatalogoPage()
        {
            InitializeComponent();
            BindingContext = new CatalogoViewModel();
        }

        // Ya no usamos SelectionChanged, ahora Tap
        private async void OnProductoTapped(object sender, TappedEventArgs e)
        {
            if (sender is Frame frame && frame.BindingContext is Producto producto)
            {
                await Shell.Current.GoToAsync(nameof(DetalleProductoPage), new Dictionary<string, object>
                {
                    { "producto", producto }
                });
            }
        }
    }
}