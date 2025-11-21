using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WilliamApp.Services;
using WilliamApp.Models;

namespace WilliamApp.ViewModels
{
    public class CarritoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CarritoItem> Items { get; set; }
        private CarritoService carritoService;

        public event PropertyChangedEventHandler PropertyChanged;

        public CarritoViewModel()
        {
            carritoService = new CarritoService();
            Items = new ObservableCollection<CarritoItem>();
            CargarCarrito();
        }

        private async void CargarCarrito()
        {
            var lista = await carritoService.ObtenerCarrito();
            Items.Clear();
            foreach (var item in lista)
                Items.Add(item);
        }
    }
}
