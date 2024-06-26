using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
