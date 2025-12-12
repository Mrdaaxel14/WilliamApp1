using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.Services;

namespace WilliamApp.ViewModels
{
    public class AgregarDireccionViewModel : INotifyPropertyChanged
    {
        private readonly ClienteService clienteService;

        private int idDireccion;
        private string provincia;
        private string ciudad;
        private string calle;
        private string numero;
        private string codigoPostal;
        private bool isBusy;
        private string titulo;

        public event PropertyChangedEventHandler PropertyChanged;

        public int IdDireccion
        {
            get => idDireccion;
            set { idDireccion = value; OnPropertyChanged(); }
        }

        public string Titulo
        {
            get => titulo;
            set { titulo = value; OnPropertyChanged(); }
        }

        public string Provincia
        {
            get => provincia;
            set { provincia = value; OnPropertyChanged(); }
        }

        public string Ciudad
        {
            get => ciudad;
            set { ciudad = value; OnPropertyChanged(); }
        }

        public string Calle
        {
            get => calle;
            set { calle = value; OnPropertyChanged(); }
        }

        public string Numero
        {
            get => numero;
            set { numero = value; OnPropertyChanged(); }
        }

        public string CodigoPostal
        {
            get => codigoPostal;
            set { codigoPostal = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => isBusy;
            set { isBusy = value; OnPropertyChanged(); }
        }

        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }

        public AgregarDireccionViewModel()
        {
            clienteService = new ClienteService();
            GuardarCommand = new Command(async () => await Guardar());
            CancelarCommand = new Command(async () => await Cancelar());
            Titulo = "Agregar dirección";
        }

        public void CargarDireccion(Direccion direccion)
        {
            if (direccion != null)
            {
                IdDireccion = direccion.IdDireccion;
                Provincia = direccion.Provincia;
                Ciudad = direccion.Ciudad;
                Calle = direccion.Calle;
                Numero = direccion.Numero;
                CodigoPostal = direccion.CodigoPostal;
                Titulo = "Editar dirección";
            }
        }

        private async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(Calle) || string.IsNullOrWhiteSpace(Ciudad))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes ingresar al menos la calle y la ciudad",
                    "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var direccion = new Direccion
                {
                    IdDireccion = IdDireccion,
                    Provincia = Provincia,
                    Ciudad = Ciudad,
                    Calle = Calle,
                    Numero = Numero,
                    CodigoPostal = CodigoPostal
                };

                bool ok = await clienteService.GuardarDireccion(direccion);

                if (ok)
                {
                    string mensaje = IdDireccion > 0 
                        ? "Dirección actualizada correctamente"
                        : "Dirección guardada correctamente";
                    
                    await Application.Current.MainPage.DisplayAlert(
                        "Éxito",
                        mensaje,
                        "OK");

                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "No se pudo guardar la dirección",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Error: {ex.Message}",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task Cancelar()
        {
            await Shell.Current.GoToAsync("..");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}