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
        public string DescripcionCategoria => Categoria?.Descripcion ?? "Sin descripción";
    }

    public class CatalogoViewModel : INotifyPropertyChanged
    {
        private readonly ProductoService productoService;
        private readonly CategoriaService categoriaService;
        private bool isBusy;
        private string mensajeEstado = string.Empty;

        // Caché para evitar recargas innecesarias
        private static List<Producto> productosCache;
        private static List<Categoria> categoriasCache;
        private static DateTime ultimaCarga = DateTime.MinValue;
        private static readonly TimeSpan TiempoCache = TimeSpan.FromMinutes(5);

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
                if (mensajeEstado == value) return;
                mensajeEstado = value;
                OnPropertyChanged(nameof(MensajeEstado));
            }
        }

        public bool TieneProductos => Categorias.Any();

        public CatalogoViewModel()
        {
            productoService = new ProductoService();
            categoriaService = new CategoriaService();
            RecargarCommand = new Command(async () => await CargarCatalogo(forzarRecarga: true));

            // Cargar inmediatamente si hay caché, sino cargar del servidor
            if (EsCacheValido())
            {
                ConstruirCategorias(productosCache, categoriasCache);
                OnPropertyChanged(nameof(TieneProductos));
            }
            else
            {
                _ = CargarCatalogo();
            }
        }

        private bool EsCacheValido()
        {
            return productosCache != null &&
                   categoriasCache != null &&
                   (DateTime.Now - ultimaCarga) < TiempoCache;
        }

        private async Task CargarCatalogo(bool forzarRecarga = false)
        {
            if (IsBusy) return;

            // Si el caché es válido y no es una recarga forzada, usar caché
            if (!forzarRecarga && EsCacheValido())
            {
                ConstruirCategorias(productosCache, categoriasCache);
                return;
            }

            IsBusy = true;
            MensajeEstado = "Cargando productos...";

            try
            {
                // Cargar en paralelo para mayor velocidad
                var productosTask = productoService.ObtenerProductos();
                var categoriasTask = categoriaService.ObtenerCategorias();

                await Task.WhenAll(productosTask, categoriasTask);

                var productos = await productosTask ?? new List<Producto>();
                var categorias = await categoriasTask ?? new List<Categoria>();

                // Actualizar caché
                productosCache = productos;
                categoriasCache = categorias;
                ultimaCarga = DateTime.Now;

                MensajeEstado = productos.Any()
                    ? string.Empty
                    : "No hay productos disponibles en este momento.";

                ConstruirCategorias(productos, categorias);
            }
            catch (Exception ex)
            {
                MensajeEstado = "No pudimos conectarnos al servidor. Desliza hacia abajo para reintentar.";
                Categorias.Clear();

                // Log para debugging (sin bloquear UI)
                Console.WriteLine($"Error al cargar catálogo: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
                OnPropertyChanged(nameof(TieneProductos));
            }
        }

        private void ConstruirCategorias(IEnumerable<Producto> productos, IEnumerable<Categoria> categorias)
        {
            Categorias.Clear();

            var grupos = categorias
                .Select(cat => new CategoriaConProductos
                {
                    Categoria = cat,
                    Productos = new ObservableCollection<Producto>(
                        productos
                            .Where(p => p.IdCategoria == cat.IdCategoria)
                            .Select(p =>
                            {
                                p.oCategoria ??= cat;
                                return p;
                            })
                    )
                })
                .Where(g => g.Productos.Any())
                .OrderBy(g => g.Categoria.Descripcion);

            foreach (var grupo in grupos)
                Categorias.Add(grupo);

            OnPropertyChanged(nameof(TieneProductos));
        }

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}