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

        public LoginViewModel()
        {
            authService = new AuthService();
            LoginCommand = new Command(async () => await Login());
        }

        private async Task Login()
        {
            bool ok = await authService.Login(Email, Password);

            if (ok)
            {
                // 🔥 Navegación correcta usando Shell
                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Credenciales inválidas",
                    "OK"
                );
            }
        }

        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
