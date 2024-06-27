using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private CD_Cliente objCapaDatos = new CD_Cliente();

        public List<Cliente> ListarCliente()
        {
            return objCapaDatos.ListarClientes();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "Los nombres no pueden ser vacios";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "Los apellidos no pueden ser vacios";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = CN_Recursos.EncriptarClave(obj.Clave);

                string asunto = "Bienvenido a CyberShop";
                string mensaje_correo = $"<h3>Cuenta registrada con exito</h3></br><p>¡Bienvenido a CyberShop {obj.Nombres} {obj.Apellidos}!</p>";

                bool respuesta = CN_Recursos.EnviarCorreoClave(obj.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return objCapaDatos.RegistrarUsuario(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "Error al enviar el correo";
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }

        public bool CambiarClave(int idcliente, string nuevaclave, out string Mensaje)
        {
            return objCapaDatos.CambiarClave(idcliente, nuevaclave, out Mensaje);
        }
        
        public bool ReestablecerClave(int idcliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDatos.ReestablecerClave(idcliente, CN_Recursos.EncriptarClave(nuevaclave), out Mensaje);
            if (resultado)
            {
                string asunto = "Reestablecer Contraseña";
                string mensaje_correo = $"<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su nueva contraseña para acceder ahora es: {nuevaclave}</p>";

                bool respuesta = CN_Recursos.EnviarCorreoClave(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "Error al enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "Error al reestablecer contraseña, intentelo nuevamente";
                return false;
            }
        }

    }
}
