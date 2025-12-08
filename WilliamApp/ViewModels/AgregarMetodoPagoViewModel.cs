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
    public class AgregarMetodoPagoViewModel : INotifyPropertyChanged
    {
        private readonly ClienteService clienteService;

        private string metodo;
        private string titular;
        private string ultimos4;
        private string expiracion;
        private bool isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Metodo
        {
            get => metodo;
            set { metodo = value; OnPropertyChanged(); }
        }

        public string Titular
        {
            get => titular;
            set { titular = value; OnPropertyChanged(); }
        }

        public string Ultimos4
        {
            get => ultimos4;
            set { ultimos4 = value; OnPropertyChanged(); }
        }

        public string Expiracion
        {
            get => expiracion;
            set { expiracion = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => isBusy;
            set { isBusy = value; OnPropertyChanged(); }
        }

        public ICommand GuardarCommand { get; }
        public ICommand CancelarCommand { get; }

        public AgregarMetodoPagoViewModel()
        {
            clienteService = new ClienteService();
            GuardarCommand = new Command(async () => await Guardar());
            CancelarCommand = new Command(async () => await Cancelar());
        }

        private async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(Metodo))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Debes seleccionar el tipo de método de pago",
                    "OK");
                return;
            }

            IsBusy = true;

            try
            {
                var metodoPago = new MetodoPago
                {
                    Metodo = Metodo,
                    Titular = Titular,
                    Ultimos4 = Ultimos4,
                    Expiracion = Expiracion
                };

                bool ok = await clienteService.GuardarMetodoPago(metodoPago);

                if (ok)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Éxito",
                        "Método de pago guardado correctamente",
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