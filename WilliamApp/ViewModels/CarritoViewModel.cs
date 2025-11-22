using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using WilliamApp.Services;
using WilliamApp.Models;

namespace WilliamApp.ViewModels
{
    public class CarritoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CarritoItem> Items { get; set; }
        private CarritoService carritoService;

        public event PropertyChangedEventHandler PropertyChanged;

        public CarritoViewModel()
        {
            carritoService = new CarritoService();
            Items = new ObservableCollection<CarritoItem>();
            CargarCarrito();
        }

        private async void CargarCarrito()
        {
            try
            {
                var lista = await carritoService.ObtenerCarrito();
                Items.Clear();

                if (lista == null || !lista.Any())
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Carrito vacío",
                        "Aún no tienes productos en tu carrito.",
                        "OK");

                    return;
                }

                foreach (var item in lista)
                    Items.Add(item);
            }
            catch (HttpRequestException)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Sesión requerida",
                    "Inicia sesión nuevamente para ver tu carrito.",
                    "OK");

                Application.Current.MainPage = new NavigationPage(new Views.LoginPage());
            }
        }
    }
}