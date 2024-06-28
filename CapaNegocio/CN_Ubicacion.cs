using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDatos = new CD_Ubicacion();

        public List<Departamento> ObtenerDepartamentos()
        {
            return objCapaDatos.ListarDepartamentos();
        }

        public List<Provincia> ObtenerProvincias(string iddepartamento)
        {
            return objCapaDatos.ListarProvincias(iddepartamento);
        }

        public List<Distrito> ObtenerDistritos(string idprovincia, string iddepartamento)
        {
            return objCapaDatos.ListarDistritos(idprovincia, iddepartamento);
        }

    }
}
