using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objCapaDatos = new CD_Venta();

        public List<Reporte> Ventas(string fechaini, string fechafin, string idtransaccion)
        {
            return objCapaDatos.ListarVentas(fechaini, fechafin, idtransaccion);
        }
    }
}
