using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppProyectoEFSRTCapaPresentacion.Filter;

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
            base.OnActionExecuted(filterContext);
        }
    }
}