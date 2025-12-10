using Microsoft.Maui.Controls;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    public partial class AgregarDireccionPage : ContentPage
    {
        public AgregarDireccionPage()
        {
            InitializeComponent();
            BindingContext = new AgregarDireccionViewModel();
        }
    }
}
