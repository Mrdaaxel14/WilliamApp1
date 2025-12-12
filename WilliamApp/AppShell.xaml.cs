using Microsoft.Maui.Controls;
using WilliamApp.Helpers;

namespace WilliamApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registro de rutas (si necesitás navegación interna)
            Routing.RegisterRoute(nameof(Views.RegisterPage), typeof(Views.RegisterPage));
            Routing.RegisterRoute(nameof(Views.DetalleProductoPage), typeof(Views.DetalleProductoPage));
            Routing.RegisterRoute(nameof(Views.ConfirmarPedidoPage), typeof(Views.ConfirmarPedidoPage));
            Routing.RegisterRoute(nameof(Views.CuentaUsuarioPage), typeof(Views.CuentaUsuarioPage));
            Routing.RegisterRoute(nameof(Views.AgregarDireccionPage), typeof(Views.AgregarDireccionPage));
            Routing.RegisterRoute(nameof(Views.AgregarMetodoPagoPage), typeof(Views.AgregarMetodoPagoPage));
        }

        private async void OnCerrarSesionClicked(object sender, System.EventArgs e)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Cerrar Sesión",
                "¿Estás seguro de que deseas cerrar sesión?",
                "Sí",
                "No");

            if (!confirm) return;

            // Limpiar token y volver al login
            Settings.Token = string.Empty;

            // Opcional: limpiar estado relacionado si existe (ej: caches, servicios)

            Application.Current.MainPage = new NavigationPage(new Views.LoginPage());
        }
    }
}