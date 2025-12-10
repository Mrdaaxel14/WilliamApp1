using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.Services;

namespace WilliamApp.ViewModels
{
    public class AgregarMetodoPagoViewModel : INotifyPropertyChanged
    {
        private readonly ClienteService clienteService;

        private string tipoSeleccionado;
        private string alias;
        private string titular;
        private string ultimos4Digitos;
        private string vencimiento;
        private bool isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        public string[] TiposMetodoPago { get; } = new[]
        {
            "Tarjeta de Crédito",
            "Tarjeta de Débito",
            "MercadoPago",
            "Efectivo"
        };

        public string TipoSeleccionado
        {
            get => tipoSeleccionado;
            set
            {
                tipoSeleccionado = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EsTarjeta));
            }
        }

        public string Alias
        {
            get => alias;
            set { alias = value; OnPropertyChanged(); }
        }

        public string Titular
        {
            get => titular;
            set { titular = value; OnPropertyChanged(); }
        }

        public string Ultimos4Digitos
        {
            get => ultimos4Digitos;
            set { ultimos4Digitos = value; OnPropertyChanged(); }
        }

        public string Vencimiento
        {
            get => vencimiento;
            set { vencimiento = value; OnPropertyChanged(); }
        }

        public bool IsLoading
        {
            get => isLoading;
            set { isLoading = value; OnPropertyChanged(); }
        }

        public bool EsTarjeta => TipoSeleccionado == "Tarjeta de Crédito" || TipoSeleccionado == "Tarjeta de Débito";

        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }

        public AgregarMetodoPagoViewModel()
        {
            clienteService = new ClienteService();
            GuardarCommand = new Command(async () => await Guardar());
            CancelarCommand = new Command(async () => await Cancelar());
            TipoSeleccionado = TiposMetodoPago[0];
        }

        private async Task Guardar()
        {
            // Validación de campos obligatorios
            if (string.IsNullOrWhiteSpace(Titular))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Campo requerido",
                    "El nombre del titular es obligatorio",
                    "OK");
                return;
            }

            if (EsTarjeta)
            {
                if (string.IsNullOrWhiteSpace(Ultimos4Digitos) || Ultimos4Digitos.Length != 4)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo requerido",
                        "Ingrese los últimos 4 dígitos de la tarjeta",
                        "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Vencimiento))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Campo requerido",
                        "La fecha de vencimiento es obligatoria",
                        "OK");
                    return;
                }

                // Validar formato MM/YY
                if (!System.Text.RegularExpressions.Regex.IsMatch(Vencimiento, @"^\d{2}/\d{2}$"))
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Formato inválido",
                        "El vencimiento debe tener formato MM/YY",
                        "OK");
                    return;
                }
            }

            IsLoading = true;

            var metodo = new MetodoPago
            {
                Alias = string.IsNullOrWhiteSpace(Alias) ? TipoSeleccionado : Alias,
                Titular = Titular,
                NumeroEnmascarado = EsTarjeta ? $"****{Ultimos4Digitos}" : "",
                Vencimiento = EsTarjeta ? Vencimiento : "",
                Marca = TipoSeleccionado
            };

            bool ok = await clienteService.GuardarMetodoPago(metodo);

            IsLoading = false;

            if (ok)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Método guardado",
                    "El método de pago se guardó correctamente",
                    "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "No se pudo guardar el método de pago",
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
