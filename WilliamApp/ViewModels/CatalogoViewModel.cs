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
            categoriaService = new CategoriaService();
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
                var categorias = await categoriaService.ObtenerCategorias() ?? new List<Categoria>();

                MensajeEstado = productos.Any()
                    ? string.Empty
                    : "No hay productos disponibles en este momento.";

                ConstruirCategorias(productos, categorias);

                // DEBUG: Mostrar cuántos productos y categorías se están mostrando
                await Application.Current.MainPage.DisplayAlert(
                    "Debug",
                    $"CargarCatalogo:\nProductos={productos.Count}\nCategorias={Categorias.Count}",
                    "OK");
            }
            catch (Exception ex)
            {
                MensajeEstado = "No pudimos conectarnos al servidor. Desliza hacia abajo para reintentar.";
                Categorias.Clear();
                await Application.Current.MainPage.DisplayAlert(
                    "DebugError",
                    $"Ex: {ex.Message}",
                    "OK");
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