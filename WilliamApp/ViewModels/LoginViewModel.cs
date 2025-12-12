using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WilliamApp.Services;
using WilliamApp.Views;
using WilliamApp.Helpers;
using Microsoft.Maui.Controls;

namespace WilliamApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string email;
        private string password;
        private bool isBusy;

        private readonly AuthService authService;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); }
        }

        public bool IsBusy
        {
            get => isBusy;
            set { isBusy = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }
        public ICommand IrARegistroCommand { get; }

        public LoginViewModel()
        {
            authService = new AuthService();
            LoginCommand = new Command(async () => await Login());
            IrARegistroCommand = new Command(async () => await IrARegistro());
        }

        private async Task Login()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var result = await authService.Login(Email?.Trim(), Password);

                if (result.Success)
                {
                    // Cambiar la MainPage para que la navegación use el Shell principal
                    // (el token ya fue guardado en Settings por AuthService)
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        result.Message,
                        "OK"
                    );
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Ocurrió un error: {ex.Message}",
                    "OK"
                );
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task IrARegistro()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}