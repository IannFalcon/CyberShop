using CapaEntidades;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Ubicacion
    {
        public List<Departamento> ListarDepartamentos()
        {
            List<Departamento> listado = new List<Departamento>();

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    string query = "SELECT * FROM DEPARTAMENTO";

                    SqlCommand cmd = new SqlCommand(query, cnx);

                    cnx.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listado.Add(new Departamento()
                            {
                                IdDepartamento = dr[0].ToString(),
                                Descripcion = dr[1].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                listado = new List<Departamento>();
            }
            return listado;
        }

        public List<Provincia> ListarProvincias(string iddepartamento)
        {
            List<Provincia> listado = new List<Provincia>();

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    string query = "SELECT * FROM PROVINCIA WHERE IdDepartamento = @IdDepartamento";

                    SqlCommand cmd = new SqlCommand(query, cnx);
                    cmd.Parameters.AddWithValue("@IdDepartamento", iddepartamento);
                    cmd.CommandType = CommandType.Text;

                    cnx.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listado.Add(new Provincia()
                            {
                                IdProvincia = dr[0].ToString(),
                                Descripcion = dr[1].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                listado = new List<Provincia>();
            }
            return listado;
        }

        public List<Distrito> ListarDistritos(string idprovincia, string iddepartamento)
        {
            List<Distrito> listado = new List<Distrito>();

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    string query = "SELECT * FROM DISTRITO WHERE IdDepartamento = @iddepartamento AND IdProvincia = @IdProvincia";

                    SqlCommand cmd = new SqlCommand(query, cnx);
                    cmd.Parameters.AddWithValue("@IdProvincia", idprovincia);
                    cmd.Parameters.AddWithValue("@IdDepartamento", iddepartamento);
                    cmd.CommandType = CommandType.Text;

                    cnx.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listado.Add(new Distrito()
                            {
                                IdDistrito = dr[0].ToString(),
                                Descripcion = dr[1].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                listado = new List<Distrito>();
            }
            return listado;
        }
    }
}
