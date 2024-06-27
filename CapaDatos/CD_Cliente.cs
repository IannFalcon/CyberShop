using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Cliente
    {
        public List<Cliente> ListarClientes()
        {
            List<Cliente> listado = new List<Cliente>();
            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    string query = "SELECT IdCliente, Nombres, Apellidos, Correo, Clave, Reestablecer FROM CLIENTE WHERE ELIMINADO = 'No'";
                    SqlCommand cmd = new SqlCommand(query, cnx);
                    cnx.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listado.Add(new Cliente()
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                            });
                        }
                    }
                }
            }
            catch
            {
                listado = new List<Cliente>();
            }

            return listado;

        }

        public int RegistrarUsuario(Cliente obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCliente", cnx);
                    cmd.Parameters.AddWithValue("Nombres", obj.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnx.Open();
                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }

            return idautogenerado;

        }

        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    SqlCommand cmd = new SqlCommand("update cliente set clave = @nuevaclave, reestablecer = 0 where idcliente = @id", cnx);
                    cmd.Parameters.AddWithValue("@id", idcliente);
                    cmd.Parameters.AddWithValue("@nuevaclave", nuevaclave);
                    cmd.CommandType = CommandType.Text;
                    cnx.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex) { resultado = false; Mensaje = ex.Message; }
            return resultado;
        }

        public bool ReestablecerClave(int idcliente, string clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    SqlCommand cmd = new SqlCommand("update cliente set clave = @clave, reestablecer = 1 where idcliente = @id", cnx);
                    cmd.Parameters.AddWithValue("@id", idcliente);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;
                    cnx.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex) { resultado = false; Mensaje = ex.Message; }
            return resultado;
        }
    }
}
