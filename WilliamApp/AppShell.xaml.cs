using Microsoft.Maui.Controls;

namespace WilliamApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registro de rutas (si necesitás navegación interna)
            Routing.RegisterRoute(nameof(Views.DetalleProductoPage), typeof(Views.DetalleProductoPage));
            Routing.RegisterRoute(nameof(Views.ConfirmarPedidoPage), typeof(Views.ConfirmarPedidoPage));
            Routing.RegisterRoute(nameof(Views.CuentaUsuarioPage), typeof(Views.CuentaUsuarioPage));
            Routing.RegisterRoute(nameof(Views.AgregarDireccionPage), typeof(Views.AgregarDireccionPage));
            Routing.RegisterRoute(nameof(Views.AgregarMetodoPagoPage), typeof(Views.AgregarMetodoPagoPage));
        }
    }
}