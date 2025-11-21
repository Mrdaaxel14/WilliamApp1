using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;
namespace WilliamApp.Views
{
    public partial class CarritoPage : ContentPage
    {
        public CarritoPage()
        {
            InitializeComponent();
            BindingContext = new CarritoViewModel();
        }
    }
}
