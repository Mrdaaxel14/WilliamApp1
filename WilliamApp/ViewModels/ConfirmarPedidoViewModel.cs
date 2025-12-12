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

        public async Task RecargarDatos()
        {
            await RecargarMetodosPago();
            await RecargarDirecciones();
        }

        private async Task RecargarMetodosPago()
        {
            var metodos = await clienteService.ObtenerMetodosPago();
            MetodosPago.Clear();
            foreach (var m in metodos) MetodosPago.Add(m);

            // Seleccionar el último agregado si no hay selección válida
            if (MetodoPagoSeleccionado == null || !MetodosPago.Any(mp => mp.IdMetodoPago == MetodoPagoSeleccionado.IdMetodoPago))
                MetodoPagoSeleccionado = MetodosPago.LastOrDefault();
        }

        private async Task RecargarDirecciones()
        {
            var direcciones = await clienteService.ObtenerDirecciones();
            Direcciones.Clear();
            foreach (var d in direcciones) Direcciones.Add(d);

            // Seleccionar la última agregada si no hay selección válida
            if (DireccionSeleccionada == null || !Direcciones.Any(dir => dir.IdDireccion == DireccionSeleccionada.IdDireccion))
                DireccionSeleccionada = Direcciones.LastOrDefault();
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

            // Pasar los IDs seleccionados al crear el pedido
            bool ok = await pedidoService.CrearPedido(
                idDireccion: DireccionSeleccionada.IdDireccion,
                idMetodoPagoUsuario: MetodoPagoSeleccionado.IdMetodoPago
            );

            if (ok)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Pedido confirmado",
                    "Tu pedido fue creado exitosamente.",
                    "OK");

                await Shell.Current.GoToAsync("//pedidos");
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
            await Shell.Current.GoToAsync(nameof(Views.AgregarMetodoPagoPage));
        }

        private async Task CrearDireccion()
        {
            await Shell.Current.GoToAsync(nameof(Views.AgregarDireccionPage));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}