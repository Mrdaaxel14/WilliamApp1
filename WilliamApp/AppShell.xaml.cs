using Microsoft.Maui.Controls;

namespace WilliamApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registro de rutas (si necesitás navegación interna)
            Routing.RegisterRoute("catalogo", typeof(Views.CatalogoPage));
            Routing.RegisterRoute("carrito", typeof(Views.CarritoPage));
            Routing.RegisterRoute("pedidos", typeof(Views.PedidosPage));
            //Routing.RegisterRoute("perfil", typeof(Views.PerfilPage));
            Routing.RegisterRoute("home", typeof(Views.HomePage));
        }
    }
}

