using CapaDatos;
using CapaEntidades;

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
