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

            try
            {
                var productos = await productoService.ObtenerProductos();
                Categorias.Clear();

                var grupos = productos
                    .Where(p => p.oCategoria != null)
                    .GroupBy(p => p.oCategoria.IdCategoria)
                    .Select(g => new CategoriaConProductos
                    {
                        Categoria = g.First().oCategoria,
                        Productos = new ObservableCollection<Producto>(g.OrderBy(p => p.Descripcion))
                    })
                    .OrderBy(c => c.Categoria.Descripcion);

                foreach (var grupo in grupos)
                    Categorias.Add(grupo);
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}