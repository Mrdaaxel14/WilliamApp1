using System.ComponentModel;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public Producto Producto
        {
            get => producto;
            set
            {
                producto = value;
                OnPropertyChanged();
            }
        }

        public int Cantidad
        {
            get => cantidad;
            set
            {
                if (value < 1) value = 1;
                cantidad = value;
                OnPropertyChanged();
            }
        }

        public ICommand AgregarAlCarritoCommand { get; }

        public ProductoDetalleViewModel()
        {
            carritoService = new CarritoService();
            AgregarAlCarritoCommand = new Command(async () => await AgregarAlCarrito());
        }

        private async Task AgregarAlCarrito()
        {
            if (Producto == null) return;

            bool ok = await carritoService.Agregar(Producto.IdProducto, Cantidad);

            if (ok)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Agregado",
                    "Producto agregado al carrito",
                    "OK");

                await Shell.Current.GoToAsync("//Carrito");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se pudo agregar el producto",
                    "OK");
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}