using CapaEntidades;
using CapaEntidades.PayPal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Venta
    {
        public List<Reporte> ListarVentas(string fechaini, string fechafin, string idtransaccion)
        {
            List<Reporte> listado = new List<Reporte>();

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    SqlCommand cmd = new SqlCommand("LISTAR_VENTAS", cnx);
                    cmd.Parameters.AddWithValue("FechaInicio", fechaini);
                    cmd.Parameters.AddWithValue("FechaFin", fechafin);
                    cmd.Parameters.AddWithValue("IdTransaccion", idtransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnx.Open();
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listado.Add(new Reporte()
                            {
                                FechaVenta = dr[0].ToString(),
                                NombreCliente = dr[1].ToString(),
                                Nombre = dr[2].ToString(),
                                Precio = Convert.ToDecimal(dr[3], new CultureInfo("es-PE")),
                                Cantidad = Convert.ToInt32(dr[4]),
                                Total = Convert.ToDecimal(dr[5], new CultureInfo("es-PE")),
                                IdTransaccion = dr[6].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex}");
                listado = new List<Reporte>();
            }

            return listado;

        }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarVenta", cnx);
                    cmd.Parameters.AddWithValue("IdCliente", obj.IdCliente);
                    cmd.Parameters.AddWithValue("TotalProducto", obj.TotalProducto);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("Contacto", obj.Contacto);
                    cmd.Parameters.AddWithValue("IdDistrito", obj.IdDistrito);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("IdTransaccion", obj.IdTransaccion);
                    cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);

                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnx.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;

        }

        public List<DetalleVenta> ListarCompras(int idcliente)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    string query = "SELECT * FROM fn_ListarCompra(@idcliente)";

                    SqlCommand cmd = new SqlCommand(query, cnx);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;

                    cnx.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleVenta()
                            {
                                objProducto = new Producto()
                                {                                
                                    Nombre = dr["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-PE")),
                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString()
                                },
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-PE")),
                                IdTransaccion = dr["IdTransaccion"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<DetalleVenta>();
            }

            return lista;
        }
    }
}
