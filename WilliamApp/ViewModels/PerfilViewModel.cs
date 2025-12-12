using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.Services;
using WilliamApp.Helpers;

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
        public ICommand EditarMetodoPagoCommand { get; }
        public ICommand EliminarMetodoPagoCommand { get; }
        public ICommand EditarDireccionCommand { get; }
        public ICommand EliminarDireccionCommand { get; }

        // Nuevo comando para cerrar sesión
        public ICommand CerrarSesionCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PerfilViewModel()
        {
            clienteService = new ClienteService();
            MetodosPago = new ObservableCollection<MetodoPago>();
            Direcciones = new ObservableCollection<Direccion>();

            GuardarDatosCommand = new Command(async () => await GuardarDatos());
            AgregarMetodoPagoCommand = new Command(async () => await AgregarMetodoPago());
            AgregarDireccionCommand = new Command(async () => await AgregarDireccion());
            EditarMetodoPagoCommand = new Command<MetodoPago>(async (metodo) => await EditarMetodoPago(metodo));
            EliminarMetodoPagoCommand = new Command<MetodoPago>(async (metodo) => await EliminarMetodoPago(metodo));
            EditarDireccionCommand = new Command<Direccion>(async (direccion) => await EditarDireccion(direccion));
            EliminarDireccionCommand = new Command<Direccion>(async (direccion) => await EliminarDireccion(direccion));

            CerrarSesionCommand = new Command(async () => await CerrarSesion());

            _ = CargarPerfil();
        }

        public async Task RecargarDatos()
        {
            await CargarPerfil();
        }

        private async Task CargarPerfil()
        {
            try
            {
                // Cargar datos del usuario
                var usuario = await clienteService.ObtenerPerfil();
                if (usuario != null)
                {
                    Nombre = usuario.Nombre;
                    Email = usuario.Email;
                    Telefono = usuario.Telefono;
                }

                // Cargar métodos de pago
                var metodos = await clienteService.ObtenerMetodosPago();
                MetodosPago.Clear();
                foreach (var mp in metodos)
                    MetodosPago.Add(mp);

                // Cargar direcciones
                var direcciones = await clienteService.ObtenerDirecciones();
                Direcciones.Clear();
                foreach (var dir in direcciones)
                    Direcciones.Add(dir);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar perfil: {ex.Message}");
            }
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
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se pudieron guardar los datos",
                    "OK");
            }
        }

        private async Task AgregarMetodoPago()
        {
            await Shell.Current.GoToAsync(nameof(Views.AgregarMetodoPagoPage));
        }

        private async Task AgregarDireccion()
        {
            await Shell.Current.GoToAsync(nameof(Views.AgregarDireccionPage));
        }

        private async Task EditarMetodoPago(MetodoPago metodo)
        {
            if (metodo == null) return;

            try
            {
                var metodoPagoJson = JsonSerializer.Serialize(metodo);
                await Shell.Current.GoToAsync($"{nameof(Views.AgregarMetodoPagoPage)}?metodoPagoJson={Uri.EscapeDataString(metodoPagoJson)}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"No se pudo abrir la página de edición: {ex.Message}",
                    "OK");
            }
        }

        private async Task EliminarMetodoPago(MetodoPago metodo)
        {
            if (metodo == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar eliminación",
                $"¿Estás seguro de que quieres eliminar el método de pago {metodo.Metodo}?",
                "Sí",
                "No");

            if (!confirm) return;

            bool ok = await clienteService.EliminarMetodoPago(metodo.IdMetodoPago);
            if (ok)
            {
                MetodosPago.Remove(metodo);
                await Application.Current.MainPage.DisplayAlert(
                    "Éxito",
                    "Método de pago eliminado correctamente",
                    "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se pudo eliminar el método de pago",
                    "OK");
            }
        }

        private async Task EditarDireccion(Direccion direccion)
        {
            if (direccion == null) return;

            try
            {
                var direccionJson = JsonSerializer.Serialize(direccion);
                await Shell.Current.GoToAsync($"{nameof(Views.AgregarDireccionPage)}?direccionJson={Uri.EscapeDataString(direccionJson)}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"No se pudo abrir la página de edición: {ex.Message}",
                    "OK");
            }
        }

        private async Task EliminarDireccion(Direccion direccion)
        {
            if (direccion == null) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Confirmar eliminación",
                $"¿Estás seguro de que quieres eliminar la dirección {direccion.Etiqueta}?",
                "Sí",
                "No");

            if (!confirm) return;

            bool ok = await clienteService.EliminarDireccion(direccion.IdDireccion);
            if (ok)
            {
                Direcciones.Remove(direccion);
                await Application.Current.MainPage.DisplayAlert(
                    "Éxito",
                    "Dirección eliminada correctamente",
                    "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se pudo eliminar la dirección",
                    "OK");
            }
        }

        // Nuevo método: cerrar sesión
        private async Task CerrarSesion()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Cerrar Sesión",
                "¿Estás seguro de que deseas cerrar sesión?",
                "Sí",
                "No");

            if (!confirm) return;

            // Limpiar token
            Settings.Token = string.Empty;

            // Volver al login
            Application.Current.MainPage = new NavigationPage(new Views.LoginPage());
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}