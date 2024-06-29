using System;
using System.Web;
using System.Web.Mvc;

namespace AppProyectoEFSRTCapaPresentacion.Filter
{
    public class ValidarSesionAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.Session["Cliente"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/LoginCliente");
            }
            else
            {
                base.OnActionExecuted(filterContext);
            }
        }
    }
}