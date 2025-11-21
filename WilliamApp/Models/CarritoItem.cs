using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilliamApp.Models
{
    public class CarritoItem
    {
        public int IdCarritoDetalle { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
