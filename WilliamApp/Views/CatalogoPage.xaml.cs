using WilliamApp.ViewModels;
namespace WilliamApp.Views
{
    public partial class CatalogoPage : ContentPage
    {
        public CatalogoPage()
        {
            InitializeComponent();
            BindingContext = new CatalogoViewModel();
        }
    }
}
