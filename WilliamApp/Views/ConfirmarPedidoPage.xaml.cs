using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    public partial class ConfirmarPedidoPage : ContentPage
    {
        public ConfirmarPedidoPage()
        {
            InitializeComponent();
            BindingContext = new ConfirmarPedidoViewModel();
        }
    }
}