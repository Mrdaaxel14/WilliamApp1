// WilliamApp/ViewModels/CarritoViewModel.cs

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
        private readonly ProductoService productoService;   // ✅ NUEVO

        private decimal total;
        private bool isLoading;
        private bool carritoVacio;

        public ObservableCollection<CarritoItem> Items { get; set; }
        public Command IrAConfirmarCommand { get; }

        // ✅ Comando para eliminar
        public Command<CarritoItem> EliminarItemCommand { get; }

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
            productoService = new ProductoService();   // ✅ inicializamos

            Items = new ObservableCollection<CarritoItem>();
            IrAConfirmarCommand = new Command(async () => await IrAConfirmar(), () => Items.Any());

            EliminarItemCommand = new Command<CarritoItem>(async (item) => await EliminarItem(item));

            // Suscribirse a mensajes del detalle de producto
            MessagingCenter.Subscribe<ProductoDetalleViewModel>(this, "CarritoActualizado", async (sender) =>
            {
                await CargarCarrito();
            });

            _ = CargarCarrito();
        }

        private async Task CargarCarrito()
        {
            if (IsLoading) return;

            IsLoading = true;

            try
            {
                // 1) Obtener items del carrito
                var lista = await carritoService.ObtenerCarrito();
                Items.Clear();

                if (lista == null || !lista.Any())
                {
                    CarritoVacio = true;
                    Total = 0;
                    return;
                }

                CarritoVacio = false;

                // 2) Obtener productos completos (con imagen) desde el API
                List<Producto> productosCompletos = null;
                try
                {
                    productosCompletos = await productoService.ObtenerProductos();
                }
                catch
                {
                    // Si falla esta llamada, mostramos igual el carrito (pero sin imagen)
                    productosCompletos = null;
                }

                // 3) Para cada item del carrito, reemplazar el Producto por el completo
                foreach (var item in lista)
                {
                    if (productosCompletos != null && item.Producto != null)
                    {
                        var prodCompleto = productosCompletos
                            .FirstOrDefault(p => p.IdProducto == item.Producto.IdProducto);

                        if (prodCompleto != null)
                        {
                            // ⬅️ IMPORTANTE: reemplazamos el objeto entero
                            // así viene con ImagenMostrar ya calculado
                            item.Producto = prodCompleto;
                        }
                    }

                    Items.Add(item);
                }

                // 4) Calcular total
                Total = Items.Sum(i => i.Cantidad * i.Producto.Precio);

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

        // ✅ Eliminar item del carrito
        private async Task EliminarItem(CarritoItem item)
        {
            if (item == null) return;

            var confirmar = await Application.Current.MainPage.DisplayAlert(
                "Confirmar",
                $"¿Eliminar {item.Producto?.Descripcion} del carrito?",
                "Sí",
                "No");

            if (!confirmar) return;

            IsLoading = true;

            try
            {
                var success = await carritoService.Eliminar(item.IdCarritoDetalle);

                if (success)
                {
                    Items.Remove(item);
                    Total = Items.Sum(i => i.Cantidad * i.Producto.Precio);
                    CarritoVacio = !Items.Any();

                    ((Command)IrAConfirmarCommand).ChangeCanExecute();

                    await Application.Current.MainPage.DisplayAlert(
                        "Eliminado",
                        "Producto eliminado del carrito",
                        "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "No se pudo eliminar el producto",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Error al eliminar: {ex.Message}",
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

        public async Task Recargar()
        {
            await CargarCarrito();
        }

        ~CarritoViewModel()
        {
            MessagingCenter.Unsubscribe<ProductoDetalleViewModel>(this, "CarritoActualizado");
        }
    }
}
