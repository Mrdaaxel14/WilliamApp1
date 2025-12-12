using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
                Console.WriteLine($"Error al cargar pedidos: {ex.Message}");
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
                Console.WriteLine($"Error al refrescar pedidos: {ex.Message}");
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

            // Aquí se puede navegar a una página de detalle del pedido
            // Por ahora, solo mostramos un mensaje
            await Application.Current.MainPage.DisplayAlert(
                "Detalle del pedido",
                $"Pedido #{pedido.IdPedido}\nFecha: {pedido.FechaFormateada}\nTotal: ${pedido.Total:F2}\nEstado: {pedido.EstadoPedido}",
                "OK");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

