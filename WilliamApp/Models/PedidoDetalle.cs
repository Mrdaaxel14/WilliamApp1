using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilliamApp.Models
{
    public class PedidoDetalle
    {
        public int IdPedidoDetalle { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}

