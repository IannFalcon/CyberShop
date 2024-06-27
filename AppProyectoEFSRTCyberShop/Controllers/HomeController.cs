using CapaEntidades;
using CapaNegocio;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

namespace AppProyectoEFSRTCyberShop.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public ActionResult Resumen()
        {
            return View();
        }

        public ActionResult Ventas()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        #region USUARIOS

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            var listado = new List<Usuario>();
            listado = new CN_Usuarios().Listar();

            return Json(new { data = listado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarUsuario(Usuario obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdUsuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(obj, out mensaje);
            }

            return Json(new { _resultado = resultado, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Usuarios().Eliminar(id, out mensaje);

            return Json(new { _resultado = resultado, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region VENTAS

        [HttpGet]
        public JsonResult ListarVentas(string fechaini, string fechafin, string idtransaccion)
        {
            List<Reporte> listado = new CN_Venta().Ventas(fechaini, fechafin, idtransaccion);

            return Json(new { data = listado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVenta(string fechaini, string fechafin, string idtransaccion)
        {
            List<Reporte> listado = new CN_Venta().Ventas(fechaini, fechafin, idtransaccion);

            DataTable dt = new DataTable();

            dt.Locale = new CultureInfo("es-PE");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("IdTransaccion", typeof(string));

            foreach ( Reporte rp in listado )
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.NombreCliente,
                    rp.Nombre,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.IdTransaccion
                });
            }

            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var desde = Convert.ToDateTime(fechaini);
                    var hasta = Convert.ToDateTime(fechafin);
                    DateTime _desde = desde.Date;
                    DateTime _hasta = hasta.Date;
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                                                  $"CYBERSHOP-REPORTE-VENTAS-DEL-{_desde.ToString("d")}-AL-{_hasta.ToString("d")}.xlsx");
                }
            }
        }

        #endregion

        [HttpGet]
        public JsonResult VistaResumen()
        {
            Resumen obj = new CN_Resumen().ResumenData();

            return Json(new { _resultado = obj }, JsonRequestBehavior.AllowGet);
        }

    }

}