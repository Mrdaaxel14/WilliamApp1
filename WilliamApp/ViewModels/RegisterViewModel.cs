using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using WilliamApp.Services;

namespace WilliamApp.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly AuthService authService;

        private string nombre;
        private string email;
        private string telefono;
        private string password;
        private string confirmPassword;
        private bool isBusy;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); }
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set { confirmPassword = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => isBusy;
            set { isBusy = value; OnPropertyChanged(); }
        }

        public ICommand RegisterCommand { get; }
        public ICommand IrALoginCommand { get; }

        public RegisterViewModel()
        {
            authService = new AuthService();
            RegisterCommand = new Command(async () => await Register());
            IrALoginCommand = new Command(async () => await IrALogin());
        }

        private async Task Register()
        {
            // Validar nombre
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El nombre es requerido",
                    "OK");
                return;
            }

            // Validar email
            if (string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El email es requerido",
                    "OK");
                return;
            }

            // Validar formato del email
            if (!IsValidEmail(Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El formato del email no es válido",
                    "OK");
                return;
            }

            // Validar contraseña
            if (string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña es requerida",
                    "OK");
                return;
            }

            // Validar longitud de contraseña
            if (Password.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña debe tener al menos 6 caracteres",
                    "OK");
                return;
            }

            // Validar confirmación de contraseña
            if (Password != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Las contraseñas no coinciden",
                    "OK");
                return;
            }

            IsBusy = true;

            try
            {
                bool ok = await authService.Register(Nombre, Email, Password, Telefono);

                if (ok)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Éxito",
                        "¡Cuenta creada exitosamente! Ahora puedes iniciar sesión",
                        "OK");

                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        "Error al crear la cuenta. Intenta nuevamente",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                // Display the specific error message from the service
                var errorMessage = ex.Message == "El email ya está registrado" 
                    ? ex.Message 
                    : "Error al crear la cuenta. Intenta nuevamente";
                    
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    errorMessage,
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task IrALogin()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use a simple but more robust email validation pattern
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
