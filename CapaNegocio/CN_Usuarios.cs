using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Usuarios
    {

        private CD_Usuarios objCapaDatos = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDatos.ListarUsuarios();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = CN_Recursos.GenerarClave();

                string asunto = "Verificación de cuenta - CyberShop";
                string mensaje_correo = "<h3>Su cuenta ha sido creada con éxito<h3>" +
                                        "<br>" +
                                        $"<p>Su contraseña para acceder a CyberShop es: {clave} </p>";

                bool respuesta = CN_Recursos.EnviarCorreoClave(obj.Correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    obj.Clave = CN_Recursos.EncriptarClave(clave);
                    return objCapaDatos.RegistrarUsuario(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puedo enviar el correo";
                    return 0;
                }
            } 
            else
            {
                return 0;
            }

        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombres) || string.IsNullOrWhiteSpace(obj.Nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellidos) || string.IsNullOrWhiteSpace(obj.Apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.EditarUsuario(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarUsuario(id, out Mensaje);
        }

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje) 
        {
            return objCapaDatos.CambiarClave(idusuario, nuevaclave, out Mensaje);
        }

        public bool ReestablecerClave(int idusuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();

            bool resultado = objCapaDatos.ReestablecerClave(idusuario, CN_Recursos.EncriptarClave(nuevaclave), out Mensaje);

            if (resultado)
            {
                string asunto = "Reestablecer Contraseña - CyberShop";
                string mensaje_correo = "<h3>Su cuenta fue reestablecida correctamente</h3>" +
                                        "</br>" +
                                        $"<p>Su nueva contraseña para acceder es: {nuevaclave} </p>";

                bool respuesta = CN_Recursos.EnviarCorreoClave(correo, asunto, mensaje_correo); 

                if (respuesta)
                {
                    return true;
                } 
                else
                {
                    Mensaje = "Error al enviar el correo de recuperación";
                    return false; 
                }
            }
            else
            {
                Mensaje = "Error al reestablecer contraseña, intentelo nuevamente";
                return false ;
            }
        }

    }
}
