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
        private bool isLoading;

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

        public bool IsLoading
        {
            get => isLoading;
            set { isLoading = value; OnPropertyChanged(); }
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
            // Validación de campos obligatorios
            if (string.IsNullOrWhiteSpace(Calle))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Campo requerido",
                    "La calle es obligatoria",
                    "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Ciudad))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Campo requerido",
                    "La ciudad es obligatoria",
                    "OK");
                return;
            }

            IsLoading = true;

            var direccion = new Direccion
            {
                Calle = Calle,
                Numero = Numero,
                Ciudad = Ciudad,
                Provincia = Provincia,
                CodigoPostal = CodigoPostal
            };

            bool ok = await clienteService.GuardarDireccion(direccion);

            IsLoading = false;

            if (ok)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Dirección guardada",
                    "La dirección se guardó correctamente",
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

        private async Task Cancelar()
        {
            await Shell.Current.GoToAsync("..");
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
