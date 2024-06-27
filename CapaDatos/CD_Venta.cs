using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            catch
            {
                listado = new List<Reporte>();
            }

            return listado;

        }
    }
}
