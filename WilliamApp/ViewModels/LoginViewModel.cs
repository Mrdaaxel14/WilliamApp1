using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WilliamApp.Services;
using WilliamApp.Views;
using WilliamApp;

namespace WilliamApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string email;
        private string password;

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
            var result = await authService.Login(Email, Password);

            if (result.Success)
            {
                // Cambiar la MainPage para que la navegación use el Shell principal
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

        private async Task IrARegistro()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.RegisterPage());
        }

        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
