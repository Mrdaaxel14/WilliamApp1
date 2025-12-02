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
using WilliamApp.Views;

namespace WilliamApp.ViewModels
{
    public class CarritoViewModel : INotifyPropertyChanged
    {
        private readonly CarritoService carritoService;
        private decimal total;
        private bool isLoading;
        private bool carritoVacio;

        public ObservableCollection<CarritoItem> Items { get; set; }
        public Command IrAConfirmarCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal Total
        {
            get => total;
            set
            {
                total = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Total)));
            }
        }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
            }
        }

        public bool CarritoVacio
        {
            get => carritoVacio;
            set
            {
                carritoVacio = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CarritoVacio)));
            }
        }

        public CarritoViewModel()
        {
            carritoService = new CarritoService();
            Items = new ObservableCollection<CarritoItem>();
            IrAConfirmarCommand = new Command(async () => await IrAConfirmar(), () => Items.Any());
            _ = CargarCarrito();
        }

        private async Task CargarCarrito()
        {
            if (IsLoading) return;

            IsLoading = true;

            try
            {
                var lista = await carritoService.ObtenerCarrito();
                Items.Clear();

                if (lista == null || !lista.Any())
                {
                    CarritoVacio = true;
                    Total = 0;
                    return;
                }

                CarritoVacio = false;
                foreach (var item in lista)
                    Items.Add(item);

                Total = Items.Sum(i => i.Cantidad * i.Producto.Precio);

                // Notificar que el comando puede ejecutarse
                ((Command)IrAConfirmarCommand).ChangeCanExecute();
            }
            catch (HttpRequestException)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Sesión requerida",
                    "Inicia sesión nuevamente para ver tu carrito.",
                    "OK");

                Application.Current.MainPage = new NavigationPage(new Views.LoginPage());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"No se pudo cargar el carrito: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task IrAConfirmar()
        {
            if (!Items.Any())
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Carrito vacío",
                    "Agrega productos antes de confirmar",
                    "OK");
                return;
            }

            await Shell.Current.GoToAsync(nameof(ConfirmarPedidoPage));
        }

        // Método público para recargar (si se necesita)
        public async Task Recargar()
        {
            await CargarCarrito();
        }
    }
}