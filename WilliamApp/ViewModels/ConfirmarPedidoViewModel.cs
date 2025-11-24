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
    public class ConfirmarPedidoViewModel : INotifyPropertyChanged
    {
        private readonly CarritoService carritoService;
        private readonly PedidoService pedidoService;
        private readonly ClienteService clienteService;

        private decimal total;
        private MetodoPago metodoPagoSeleccionado;
        private Direccion direccionSeleccionada;

        public ObservableCollection<CarritoItem> Items { get; }
        public ObservableCollection<MetodoPago> MetodosPago { get; }
        public ObservableCollection<Direccion> Direcciones { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal Total
        {
            get => total;
            set { total = value; OnPropertyChanged(); }
        }

        public MetodoPago MetodoPagoSeleccionado
        {
            get => metodoPagoSeleccionado;
            set { metodoPagoSeleccionado = value; OnPropertyChanged(); }
        }

        public Direccion DireccionSeleccionada
        {
            get => direccionSeleccionada;
            set { direccionSeleccionada = value; OnPropertyChanged(); }
        }

        public ICommand ConfirmarCommand { get; }
        public ICommand AgregarMetodoPagoCommand { get; }
        public ICommand AgregarDireccionCommand { get; }

        public ConfirmarPedidoViewModel()
        {
            carritoService = new CarritoService();
            pedidoService = new PedidoService();
            clienteService = new ClienteService();

            Items = new ObservableCollection<CarritoItem>();
            MetodosPago = new ObservableCollection<MetodoPago>();
            Direcciones = new ObservableCollection<Direccion>();

            ConfirmarCommand = new Command(async () => await Confirmar());
            AgregarMetodoPagoCommand = new Command(async () => await CrearMetodoPago());
            AgregarDireccionCommand = new Command(async () => await CrearDireccion());

            _ = CargarDatos();
        }

        private async Task CargarDatos()
        {
            var items = await carritoService.ObtenerCarrito();
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);

            Total = Items.Sum(i => i.Cantidad * i.Producto.Precio);

            var metodos = await clienteService.ObtenerMetodosPago();
            MetodosPago.Clear();
            foreach (var m in metodos) MetodosPago.Add(m);
            MetodoPagoSeleccionado = MetodosPago.FirstOrDefault();

            var direcciones = await clienteService.ObtenerDirecciones();
            Direcciones.Clear();
            foreach (var d in direcciones) Direcciones.Add(d);
            DireccionSeleccionada = Direcciones.FirstOrDefault();
        }

        private async Task Confirmar()
        {
            if (MetodoPagoSeleccionado == null || DireccionSeleccionada == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Faltan datos",
                    "Selecciona un método de pago y una dirección",
                    "OK");
                return;
            }

            bool ok = await pedidoService.ConfirmarPedido(
                MetodoPagoSeleccionado.IdMetodoPago,
                DireccionSeleccionada.IdDireccion,
                "Pago simulado");

            if (ok)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Pedido confirmado",
                    "Tu pedido fue confirmado con el pago simulado.",
                    "OK");

                await Shell.Current.GoToAsync("//Mis Pedidos");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se pudo confirmar el pedido",
                    "OK");
            }
        }

        private async Task CrearMetodoPago()
        {
            string alias = await Application.Current.MainPage.DisplayPromptAsync(
                "Nuevo método de pago", "Alias o nombre del medio");
            string titular = await Application.Current.MainPage.DisplayPromptAsync(
                "Titular", "Nombre del titular");
            string numero = await Application.Current.MainPage.DisplayPromptAsync(
                "Número", "Número de tarjeta o cuenta");
            string vencimiento = await Application.Current.MainPage.DisplayPromptAsync(
                "Vencimiento", "MM/AA");

            if (string.IsNullOrWhiteSpace(alias) || string.IsNullOrWhiteSpace(numero))
                return;

            var metodo = new MetodoPago
            {
                Alias = alias,
                Titular = titular,
                NumeroEnmascarado = numero,
                Vencimiento = vencimiento,
                Marca = "Personalizado"
            };

            bool ok = await clienteService.GuardarMetodoPago(metodo);
            if (ok)
            {
                MetodosPago.Add(metodo);
                MetodoPagoSeleccionado = metodo;
            }
        }

        private async Task CrearDireccion()
        {
            string calle = await Application.Current.MainPage.DisplayPromptAsync(
                "Dirección", "Calle");
            string numero = await Application.Current.MainPage.DisplayPromptAsync(
                "Dirección", "Número");
            string ciudad = await Application.Current.MainPage.DisplayPromptAsync(
                "Dirección", "Ciudad");
            string provincia = await Application.Current.MainPage.DisplayPromptAsync(
                "Dirección", "Provincia");
            string cp = await Application.Current.MainPage.DisplayPromptAsync(
                "Dirección", "Código Postal");

            if (string.IsNullOrWhiteSpace(calle) || string.IsNullOrWhiteSpace(ciudad))
                return;

            var direccion = new Direccion
            {
                Calle = calle,
                Numero = numero,
                Ciudad = ciudad,
                Provincia = provincia,
                CodigoPostal = cp
            };

            bool ok = await clienteService.GuardarDireccion(direccion);
            if (ok)
            {
                Direcciones.Add(direccion);
                DireccionSeleccionada = direccion;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}