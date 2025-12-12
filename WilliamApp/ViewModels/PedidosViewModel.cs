using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using WilliamApp.Services;
using WilliamApp.Models;

namespace WilliamApp.ViewModels
{
    public class PedidosViewModel : INotifyPropertyChanged
    {
        private PedidoService service;
        private bool isLoading;
        private bool isRefreshing;

        public ObservableCollection<Pedido> Pedidos { get; set; }

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand { get; }
        public ICommand VerDetalleCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PedidosViewModel()
        {
            service = new PedidoService();
            Pedidos = new ObservableCollection<Pedido>();
            RefreshCommand = new Command(async () => await RefreshPedidos());
            VerDetalleCommand = new Command<Pedido>(async (pedido) => await VerDetallePedido(pedido));
            Cargar();
        }

        private async void Cargar()
        {
            IsLoading = true;
            try
            {
                var lista = await service.MisPedidos();
                Pedidos.Clear();
                foreach (var p in lista.OrderByDescending(x => x.Fecha))
                    Pedidos.Add(p);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se pudieron cargar los pedidos. Por favor, intenta nuevamente.",
                    "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task RefreshPedidos()
        {
            IsRefreshing = true;
            try
            {
                var lista = await service.MisPedidos();
                Pedidos.Clear();
                foreach (var p in lista.OrderByDescending(x => x.Fecha))
                    Pedidos.Add(p);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se pudieron actualizar los pedidos. Por favor, intenta nuevamente.",
                    "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async Task VerDetallePedido(Pedido pedido)
        {
            if (pedido == null)
                return;

            // Display order details in a formatted alert
            var detalles = $"Pedido #{pedido.IdPedido}\n" +
                          $"Fecha: {pedido.FechaFormateada}\n" +
                          $"Estado: {pedido.EstadoPedido}\n" +
                          $"{pedido.CantidadItemsTexto}\n" +
                          $"Total: ${pedido.Total:F2}";
            
            await Application.Current.MainPage.DisplayAlert(
                "Detalle del pedido",
                detalles,
                "OK");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

