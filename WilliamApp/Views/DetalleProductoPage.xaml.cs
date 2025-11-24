using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    public partial class DetalleProductoPage : ContentPage
    {
        public DetalleProductoPage(Producto producto)
        {
            InitializeComponent();

            var vm = new ProductoDetalleViewModel
            {
                Producto = producto
            };

            BindingContext = vm;
        }
    }
}