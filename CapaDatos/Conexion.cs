using System.Configuration;

namespace CapaDatos
{
    public class Conexion
    {
        public static string con = ConfigurationManager.ConnectionStrings["cn1"].ConnectionString;
    }
}