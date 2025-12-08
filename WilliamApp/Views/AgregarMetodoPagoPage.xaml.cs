using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    public partial class AgregarMetodoPagoPage : ContentPage
    {
        public AgregarMetodoPagoPage()
        {
            InitializeComponent();
            BindingContext = new AgregarMetodoPagoViewModel();
        }
    }
}