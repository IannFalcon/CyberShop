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
                Mensaje = "El nombre del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del Cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del Cliente no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                obj.Clave = CN_Recursos.EncriptarClave(obj.Clave);
                    return objCapaDatos.RegistrarUsuario(obj, out Mensaje);

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
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente </h3> </br><p>Su nueva contraseña para acceder ahora es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreoClave(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "Error al enviar ael correo";
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
