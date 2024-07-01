using CapaEntidades;
using CapaNegocio;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;

namespace AppProyectoEFSRTCyberShop.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        #region GESTION DE SESIÓN

        [HttpPost]
        public ActionResult Login(string correo, string clave)
        {
            Usuario usuario = new Usuario();

            usuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.EncriptarClave(clave)).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Correo y/o contraseña invalidos";
                return View();
            }
            else
            {
                if (usuario.Reestablecer)
                {
                    TempData["IdUsuario"] = usuario.IdUsuario;
                    return RedirectToAction("CambiarClave");
                }

                FormsAuthentication.SetAuthCookie(usuario.Correo, false);

                ViewBag.Error = null;
                return RedirectToAction("Resumen", "Home");
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmarclave)
        {
            Usuario usuario = new Usuario();

            usuario = new CN_Usuarios().Listar().Where(u => u.IdUsuario == int.Parse(idusuario)).FirstOrDefault();

            if (usuario.Clave != CN_Recursos.EncriptarClave(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewBag.Error = "Contraseña Incorrecta";
                return View();
            }
            else if (string.IsNullOrEmpty(nuevaclave) || string.IsNullOrWhiteSpace(nuevaclave))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Su nueva contraseña no puede ser vacio";
                return View();
            }
            else if (string.IsNullOrEmpty(confirmarclave) || string.IsNullOrWhiteSpace(confirmarclave) || nuevaclave != confirmarclave)
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();

            }

            ViewData["vclave"] = ""; 

            nuevaclave = CN_Recursos.EncriptarClave(nuevaclave);
            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Login");
            } 
            else
            {
                TempData["IdUsuario"] = idusuario;
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario usuario = new Usuario();
            usuario = new CN_Usuarios().Listar().Where(u => u.Correo == correo).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "No existe un usuario relacionado a este correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().ReestablecerClave(usuario.IdUsuario, correo, out mensaje);

            if (respuesta )
            {
                ViewBag.Error = null;
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Acceso");
        }

        #endregion

    }
}