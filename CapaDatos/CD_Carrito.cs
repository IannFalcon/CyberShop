﻿using CapaEntidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Carrito
    {
        public bool ExisteCarrito(int idcliente, int idproducto)
        {
            bool resultado = true;
        

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    SqlCommand cmd = new SqlCommand("EXISTE_CARRITO", cnx);

                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                 
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnx.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                   
                }
            }
            catch (Exception ex)
            {
                resultado = false;

            }

            return resultado;

        }

        public bool OperacionCarrito(int idcliente,int idproducto, bool sumar,out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    SqlCommand cmd = new SqlCommand("OPERACION_CARRITO", cnx);

                    cmd.Parameters.AddWithValue("IdCliente", idcliente);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);

                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnx.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;

        }

        //SELECT COUNT(*) FROM CARRITO WHERE idcliente = 1

        public int CantidadEnCarrito(int idcliente)
        {
            int  resultado = 0;


            try
            {
                using (SqlConnection cnx = new SqlConnection(Conexion.con))
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM CARRITO WHERE idcliente = @idcliente", cnx);
                    cmd.Parameters.AddWithValue("@idcliente", idcliente);
                    cmd.CommandType = CommandType.Text;
                    cnx.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;

            }

            return resultado;

        }




    }
}
