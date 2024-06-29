using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

using CapaNegocio;
using System.Data;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objCapaDatos = new CD_Venta();

        public List<Reporte> Ventas(string fechaini, string fechafin, string idtransaccion)
        {
            return objCapaDatos.ListarVentas(fechaini, fechafin, idtransaccion);
        }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            return objCapaDatos.Registrar(obj, DetalleVenta, out Mensaje);
        }

        public List<DetalleVenta> ListarCompras(int idcliente)
        {
            return objCapaDatos.ListarCompras(idcliente);
        }

    }

}