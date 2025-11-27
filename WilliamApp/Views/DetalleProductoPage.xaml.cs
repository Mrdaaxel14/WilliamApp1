using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    [QueryProperty(nameof(Producto), "producto")]
    public partial class DetalleProductoPage : ContentPage
    {
        public Producto Producto
        {
            get => ((ProductoDetalleViewModel)BindingContext).Producto;
            set
            {
                ((ProductoDetalleViewModel)BindingContext).Producto = value;
                Title = value?.Descripcion;
            }
        }

        public DetalleProductoPage()
        {
            InitializeComponent();
            BindingContext = new ProductoDetalleViewModel();
        }
    }
}