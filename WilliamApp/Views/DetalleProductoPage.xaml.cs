using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.ViewModels;
using System.Collections.Generic;

namespace WilliamApp.Views
{
    [QueryProperty(nameof(Producto), "producto")]
    public partial class DetalleProductoPage : ContentPage
    {
        private ProductoDetalleViewModel viewModel;

        public Producto Producto
        {
            set
            {
                if (value != null)
                {
                    viewModel.Producto = value;

                    // Cargar detalles completos en segundo plano
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

        // Liberar recursos cuando se sale de la página
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Limpiar galería para liberar memoria
            if (viewModel != null)
            {
                viewModel.ImagenesGaleria.Clear();
            }
        }
    }
}