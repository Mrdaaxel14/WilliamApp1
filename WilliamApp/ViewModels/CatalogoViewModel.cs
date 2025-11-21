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
    public class CatalogoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Producto> Productos { get; set; }
        private readonly ProductoService service;

        public event PropertyChangedEventHandler PropertyChanged;

        public CatalogoViewModel()
        {
            Productos = new ObservableCollection<Producto>();
            service = new ProductoService();
            Cargar();
        }

        private async Task Cargar()
        {
            var lista = await service.ObtenerProductos();
            Productos.Clear();
            foreach (var p in lista) Productos.Add(p);
        }
    }
}
