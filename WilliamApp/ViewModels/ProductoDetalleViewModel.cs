using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.Services;

namespace WilliamApp.ViewModels
{
    public class ProductoDetalleViewModel : INotifyPropertyChanged
    {
        private Producto producto;
        private int cantidad = 1;
        private readonly CarritoService carritoService;
        private readonly ProductoService productoService;

        public event PropertyChangedEventHandler PropertyChanged;

        public Producto Producto
        {
            get => producto;
            set
            {
                producto = value;
                OnPropertyChanged();
                ActualizarGaleria();
            }
        }

        public int Cantidad
        {
            get => cantidad;
            set
            {
                if (value < 1) value = 1;
                if (value > 99) value = 99;
                cantidad = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ImagenesGaleria { get; set; } = new ObservableCollection<string>();

        public ICommand AgregarAlCarritoCommand { get; }
        public ICommand AumentarCantidadCommand { get; }
        public ICommand DisminuirCantidadCommand { get; }

        public ProductoDetalleViewModel()
        {
            carritoService = new CarritoService();
            productoService = new ProductoService();

            AgregarAlCarritoCommand = new Command(async () => await AgregarAlCarrito());
            AumentarCantidadCommand = new Command(() => Cantidad++);
            DisminuirCantidadCommand = new Command(() => { if (Cantidad > 1) Cantidad--; });
        }

        private void ActualizarGaleria()
        {
            ImagenesGaleria.Clear();

            if (Producto?.Galeria != null && Producto.Galeria.Any())
            {
                // Si hay galería, usar todas las imágenes
                foreach (var img in Producto.Galeria)
                {
                    ImagenesGaleria.Add(img);
                }
            }
            else if (!string.IsNullOrEmpty(Producto?.ImagenPrincipal))
            {
                // Si solo hay imagen principal
                ImagenesGaleria.Add(Producto.ImagenPrincipal);
            }
            else
            {
                // Imagen placeholder
                ImagenesGaleria.Add("https://via.placeholder.com/400x400.png?text=Sin+Imagen");
            }

            OnPropertyChanged(nameof(ImagenesGaleria));
        }

        public async Task CargarDetalleCompleto(int idProducto)
        {
            var productoCompleto = await productoService.ObtenerProductoDetalle(idProducto);
            if (productoCompleto != null)
            {
                Producto = productoCompleto;
            }
        }

        private async Task AgregarAlCarrito()
        {
            if (Producto == null) return;

            try
            {
                bool ok = await carritoService.Agregar(Producto.IdProducto, Cantidad);

                if (ok)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "¡Agregado!",
                        $"{Cantidad} unidad(es) de {Producto.Descripcion} agregadas al carrito",
                        "OK");

                    await Shell.Current.GoToAsync("//carrito");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "No se pudo agregar el producto al carrito",
                        "OK");
                }
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Ocurrió un error: {ex.Message}",
                    "OK");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}