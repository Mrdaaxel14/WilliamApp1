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
    public class PerfilViewModel : INotifyPropertyChanged
    {
        private readonly ClienteService clienteService;

        private string nombre;
        private string email;
        private string telefono;

        public ObservableCollection<MetodoPago> MetodosPago { get; }
        public ObservableCollection<Direccion> Direcciones { get; }

        public string Nombre
        {
            get => nombre;
            set { nombre = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        public string Telefono
        {
            get => telefono;
            set { telefono = value; OnPropertyChanged(); }
        }

        public ICommand GuardarDatosCommand { get; }
        public ICommand AgregarMetodoPagoCommand { get; }
        public ICommand AgregarDireccionCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PerfilViewModel()
        {
            clienteService = new ClienteService();
            MetodosPago = new ObservableCollection<MetodoPago>();
            Direcciones = new ObservableCollection<Direccion>();

            GuardarDatosCommand = new Command(async () => await GuardarDatos());
            AgregarMetodoPagoCommand = new Command(async () => await AgregarMetodoPago());
            AgregarDireccionCommand = new Command(async () => await AgregarDireccion());

            _ = CargarPerfil();
        }

        private async Task CargarPerfil()
        {
            var perfil = await clienteService.ObtenerPerfil();
            if (perfil?.Usuario != null)
            {
                Nombre = perfil.Usuario.Nombre;
                Email = perfil.Usuario.Email;
                Telefono = perfil.Usuario.Telefono;
            }

            MetodosPago.Clear();
            foreach (var mp in perfil?.MetodosPago ?? Enumerable.Empty<MetodoPago>())
                MetodosPago.Add(mp);

            Direcciones.Clear();
            foreach (var dir in perfil?.Direcciones ?? Enumerable.Empty<Direccion>())
                Direcciones.Add(dir);
        }

        private async Task GuardarDatos()
        {
            var usuario = new Usuario
            {
                Nombre = Nombre,
                Email = Email,
                Telefono = Telefono
            };

            bool ok = await clienteService.ActualizarPerfil(usuario);
            if (ok)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Perfil actualizado",
                    "Guardamos tus datos personales",
                    "OK");
            }
        }

        private async Task AgregarMetodoPago()
        {
            string alias = await Application.Current.MainPage.DisplayPromptAsync(
                "Nuevo método", "Alias de la tarjeta o cuenta");
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
            }
        }

        private async Task AgregarDireccion()
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
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}