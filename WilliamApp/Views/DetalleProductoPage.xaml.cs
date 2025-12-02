
using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    [QueryProperty(nameof(Producto), "producto")]
    public partial class DetalleProductoPage : ContentPage
    {
        private ProductoDetalleViewModel viewModel;

        public Producto Producto
        {
            get => viewModel?.Producto;
            set
            {
                if (viewModel != null && value != null)
                {
                    viewModel.Producto = value;
                    Title = value.Descripcion;

                    // Cargar detalle completo con todas las imágenes
                    _ = viewModel.CargarDetalleCompleto(value.IdProducto);
                }
            }
        }

        public DetalleProductoPage()
        {
            InitializeComponent();
            viewModel = new ProductoDetalleViewModel();
            BindingContext = viewModel;
        }
    }
}