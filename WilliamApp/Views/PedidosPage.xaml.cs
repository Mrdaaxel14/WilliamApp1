using WilliamApp.ViewModels;
namespace WilliamApp.Views
{
    public partial class PedidosPage : ContentPage
    {
        public PedidosPage()
        {
            InitializeComponent();
            BindingContext = new PedidosViewModel();
        }
    }
}
