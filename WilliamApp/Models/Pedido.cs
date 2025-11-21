using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WilliamApp.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public List<PedidoDetalle> Detalles { get; set; }
    }
}

