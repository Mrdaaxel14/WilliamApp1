using System;
using System.Text.Json;
using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.ViewModels;

namespace WilliamApp.Views
{
    [QueryProperty(nameof(MetodoPagoJson), "metodoPagoJson")]
    public partial class AgregarMetodoPagoPage : ContentPage
    {
        private AgregarMetodoPagoViewModel viewModel;
        
        public string MetodoPagoJson
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        var metodoPago = JsonSerializer.Deserialize<MetodoPago>(Uri.UnescapeDataString(value));
                        viewModel?.CargarMetodoPago(metodoPago);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al deserializar método de pago: {ex.Message}");
                    }
                }
            }
        }

        public AgregarMetodoPagoPage()
        {
            InitializeComponent();
            viewModel = new AgregarMetodoPagoViewModel();
            BindingContext = viewModel;
        }
    }
}