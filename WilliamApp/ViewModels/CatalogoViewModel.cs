using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using WilliamApp.Models;
using WilliamApp.Services;

namespace WilliamApp.ViewModels
{
    public class CategoriaConProductos
    {
        public Categoria Categoria { get; set; }
        public ObservableCollection<Producto> Productos { get; set; } = new();
        public string NombreCategoria => Categoria?.Descripcion ?? "Categoría";
    }

    public class CatalogoViewModel : INotifyPropertyChanged
    {
        private readonly ProductoService productoService;
        private bool isBusy;
        private string mensajeEstado = string.Empty;

        public ObservableCollection<CategoriaConProductos> Categorias { get; } = new();

        public ICommand RecargarCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value) return;
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public string MensajeEstado
        {
            get => mensajeEstado;
            set
            {
                if (mensajeEstado == value)
                    return;

                mensajeEstado = value;
                OnPropertyChanged(nameof(MensajeEstado));
            }
        }

        public bool TieneProductos => Categorias.Any();

        public CatalogoViewModel()
        {
            productoService = new ProductoService();
            RecargarCommand = new Command(async () => await CargarCatalogo());
            _ = CargarCatalogo();
        }

        private async Task CargarCatalogo()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            MensajeEstado = "Cargando productos...";

            try
            {
                var productos = await productoService.ObtenerProductos() ?? new List<Producto>();

                MensajeEstado = productos.Any()
                    ? string.Empty
                    : "No hay productos disponibles en este momento.";

                ConstruirCategorias(productos);
            }
            catch (Exception)
            {
                MensajeEstado = "No pudimos conectarnos al servidor. Desliza hacia abajo para reintentar.";
                Categorias.Clear();
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(TieneProductos));
            }
        }

        private void ConstruirCategorias(IEnumerable<Producto> productos)
        {
            Categorias.Clear();

            var grupos = productos
                .Where(p => p != null)
                .GroupBy(p => p.oCategoria?.IdCategoria ?? p.IdCategoria)
                .Select(g => new CategoriaConProductos
                {
                    Categoria = g.First().oCategoria ?? new Categoria
                    {
                        IdCategoria = g.Key,
                        Descripcion = g.First().oCategoria?.Descripcion ?? "Sin categoría",
                    },
                    Productos = new ObservableCollection<Producto>(g.OrderBy(p => p.Descripcion))
                })
                .OrderBy(c => c.Categoria.Descripcion);

            foreach (var grupo in grupos)
                Categorias.Add(grupo);

            OnPropertyChanged(nameof(TieneProductos));
        }

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}