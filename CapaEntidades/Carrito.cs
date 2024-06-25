using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidades
{
    public class Carrito
    {
        public int IdCarrito { get; set; }
        public Cliente objCliente { get; set; }
        public Producto objProducto { get; set; }
        public int Cantidad { get; set; }
    }
}
