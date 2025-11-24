using Microsoft.Maui.Controls;
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

        private async void OnProductoSeleccionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Models.Producto producto)
            {
                await Navigation.PushAsync(new DetalleProductoPage(producto));
                ((CollectionView)sender).SelectedItem = null;
            }
        }
    }
}