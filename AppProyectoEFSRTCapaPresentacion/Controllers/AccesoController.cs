using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaEntidades;
using CapaNegocio;

namespace AppProyectoEFSRTCapaPresentacion.Controllers
{
    public class AccesoController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        


        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente oCliente = null;
            oCliente = new CN_Cliente().ListarCliente().Where(item => item.Correo == correo && item.Clave == CN_Recursos.EncriptarClave(clave)).FirstOrDefault();
            if (oCliente == null) {
                ViewBag.Error = "Correo o Contraseña invalidos";
                return View();
            }
            else{
                if (oCliente.Reestablecer) {
                TempData["IdCliente"] = oCliente.IdCliente;
                return RedirectToAction("CambiarClave", "Acceso");
                       
                }else
                {
                    FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
                    Session["Cliente"] = oCliente;
                    ViewBag.Error = null;

                    return RedirectToAction("Index", "Tienda");
                }
            }
        }

        [HttpPost]
        public ActionResult Registrar(Cliente obj)
        {
            int resultado;
            string Mensaje = String.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(obj.Nombres) ? "" : obj.Nombres;
            ViewData["Apellidos"] = string.IsNullOrEmpty(obj.Apellidos) ? "" : obj.Apellidos;
            ViewData["Correo"] = string.IsNullOrEmpty(obj.Correo) ? "" : obj.Correo;

            if(obj.Clave !=  obj.ConfirmarClave) {

                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new CN_Cliente().Registrar(obj, out Mensaje);
            if (resultado > 0) {
                ViewBag.Error = null; 
                return RedirectToAction("Index", "Acceso");
            }else{
                ViewBag.Error = Mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Cliente cliente = new Cliente();
            cliente = new CN_Cliente().ListarCliente().Where(u => u.Correo == correo).FirstOrDefault();
            
            if (cliente == null)
            {
                ViewBag.Error = "No existe un cliente relacionado a este correo";
                return View();
            }
            string mensaje = string.Empty;
            bool respuesta = new CN_Cliente().ReestablecerClave(cliente.IdCliente, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idcliente, string claveactual, string nuevaclave, string confirmarclave)
        {
            Cliente cliente = new Cliente();
            cliente = new CN_Cliente().ListarCliente().Where(u => u.IdCliente == int.Parse(idcliente)).FirstOrDefault();
            if (cliente.Clave != CN_Recursos.EncriptarClave(claveactual))
            {
                TempData["IdCliente"] = idcliente;

                ViewBag.Error = "Contraseña Incorrecta";
                return View();

            }
            else if (nuevaclave != confirmarclave)
            {
                TempData["IdCliente"] = idcliente;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();

            }
            ViewData["vclave"] = "";
            nuevaclave = CN_Recursos.EncriptarClave(nuevaclave);
            string mensaje = string.Empty;
            bool respuesta = new CN_Usuarios().CambiarClave(int.Parse(idcliente), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["IdCliente"] = idcliente;
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }

    }
}