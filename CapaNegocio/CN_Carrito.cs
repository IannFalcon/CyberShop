using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Carrito
    {
        private CD_Carrito objCapaDatos = new CD_Carrito();

        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            return objCapaDatos.ExisteCarrito(idcliente, idproducto);
        }

        public bool OperacionCarrito(int idcliente, int idproducto, bool sumar, out string Mensaje)
        {
            return objCapaDatos.OperacionCarrito(idcliente, idproducto, sumar, out Mensaje);
        }

        public int CantidadEnCarrito(int idcliente)
        {
            return objCapaDatos.CantidadEnCarrito(idcliente);
        }
    }
}
