using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

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

        public int CantidadCarrito(int idcliente)
        {
            return objCapaDatos.CantidadCarrito(idcliente);
        }

        public List<Carrito> ListarCarrito(int idcliente)
        {
            return objCapaDatos.ListarCarrito(idcliente);
        }

        public bool ElimninarCarrito(int idcliente, int idproducto)
        {
            return objCapaDatos.ElimninarCarrito(idcliente, idproducto);
        }
    }
}
