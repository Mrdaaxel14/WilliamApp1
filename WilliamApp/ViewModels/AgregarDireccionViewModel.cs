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

        private string provincia;
        private string ciudad;
        private string calle;
        private string numero;
        private string codigoPostal;
        private bool isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

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
                    Provincia = Provincia,
                    Ciudad = Ciudad,
                    Calle = Calle,
                    Numero = Numero,
                    CodigoPostal = CodigoPostal
                };

                bool ok = await clienteService.GuardarDireccion(direccion);

                if (ok)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Éxito",
                        "Dirección guardada correctamente",
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