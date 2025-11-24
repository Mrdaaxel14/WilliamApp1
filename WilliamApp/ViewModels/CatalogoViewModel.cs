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
        public ObservableCollection<Categoria> Categorias { get; set; }
        public ObservableCollection<Producto> ProductosFiltrados { get; set; }

        private readonly ProductoService service;
        private Categoria categoriaSeleccionada;

        public event PropertyChangedEventHandler PropertyChanged;

        public CatalogoViewModel()
        {
            Productos = new ObservableCollection<Producto>();
            Categorias = new ObservableCollection<Categoria>();
            ProductosFiltrados = new ObservableCollection<Producto>();
            service = new ProductoService();
            Cargar();
        }

        private async Task Cargar()
        {
            var lista = await service.ObtenerProductos();
            Productos.Clear();
            foreach (var p in lista) Productos.Add(p);

            Categorias.Clear();
            foreach (var cat in lista.Select(p => p.oCategoria).DistinctBy(c => c.IdCategoria))
                Categorias.Add(cat);

            AplicarFiltro();
        }

        public Categoria CategoriaSeleccionada
        {
            get => categoriaSeleccionada;
            set
            {
                categoriaSeleccionada = value;
                OnPropertyChanged(nameof(CategoriaSeleccionada));
                AplicarFiltro();
            }
        }

        public void AplicarFiltro()
        {
            ProductosFiltrados.Clear();

            var filtrados = categoriaSeleccionada == null
                ? Productos
                : new ObservableCollection<Producto>(Productos.Where(p => p.IdCategoria == categoriaSeleccionada.IdCategoria));

            foreach (var p in filtrados)
                ProductosFiltrados.Add(p);
        }

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}