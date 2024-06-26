﻿using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Resumen
    {
        public Resumen VerResumen()
        {
            Resumen obj = new Resumen();

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    SqlCommand cmd = new SqlCommand("RESUMEN_INDEX", cnx);

                    cnx.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Resumen()
                            {
                                TotalCliente = Convert.ToInt32(dr[0]),
                                TotalVenta = Convert.ToInt32(dr[1]),
                                TotalProducto = Convert.ToInt32(dr[2])
                            };
                        }
                    }
                }
            }
            catch
            {
                obj = new Resumen();
            }

            return obj;

        }
    }
}
