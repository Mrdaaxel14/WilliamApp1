using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilliamApp.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string CodigoBarra { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public int IdCategoria { get; set; }
        public decimal Precio { get; set; }
        public Categoria oCategoria { get; set; }
    }

    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Descripcion { get; set; }
    }
}
