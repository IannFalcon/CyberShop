using CapaDatos;
using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Resumen
    {
        private CD_Resumen objCapaDatos = new CD_Resumen();

        public Resumen ResumenData()
        {
            return objCapaDatos.VerResumen();
        }
    }
}
