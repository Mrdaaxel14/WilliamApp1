using System.Text.Json;
using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    [QueryProperty(nameof(DireccionJson), "direccionJson")]
    public partial class AgregarDireccionPage : ContentPage
    {
        private AgregarDireccionViewModel viewModel;
        
        public string DireccionJson
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var direccion = JsonSerializer.Deserialize<Direccion>(Uri.UnescapeDataString(value));
                    viewModel?.CargarDireccion(direccion);
                }
            }
        }

        public AgregarDireccionPage()
        {
            InitializeComponent();
            viewModel = new AgregarDireccionViewModel();
            BindingContext = viewModel;
        }
    }
}
