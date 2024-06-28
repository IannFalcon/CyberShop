using CapaEntidades;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AppProyectoEFSRTCapaPresentacion.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Carrito()
        {
            return View();
        }

        #region DETALLE DEL PRODUCTO

        public ActionResult DetalleProducto(int idproducto = 0)
        {
            Producto objProducto = new Producto();
            bool conversion;

            objProducto = new CN_Productos().Listar().Where(p => p.IdProducto == idproducto).FirstOrDefault();

            if (objProducto != null)
            {
                objProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(objProducto.RutaImagen, objProducto.NombreImagen), out conversion);
                objProducto.Extension = Path.GetExtension(objProducto.NombreImagen);
            }
            return View(objProducto);
        }

        #endregion

        #region LISTADO DE PRODUCTOS

        [HttpGet]
        public JsonResult ListaCategoria()
        {
            List<Categoria> lista = new List<Categoria>();
            lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ListaMarcaporCategoria(int idcategoria)
        {
            List<Marca> lista = new List<Marca>();
            lista = new CN_Marca().ListarMarcaporCategoria(idcategoria);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProducto(int idcategoria, int idmarca)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion = false;

            lista = new CN_Productos().Listar().Select(p => new Producto()
            {

                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                objMarca = p.objMarca,
                objCategoria = p.objCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Activo = p.Activo

            }).Where(p =>

                p.objCategoria.IdCategoria == (idcategoria == 0 ? p.objCategoria.IdCategoria : idcategoria) &&
                p.objMarca.IdMarca == (idmarca == 0 ? p.objMarca.IdMarca : idmarca) &&
                p.Stock > 0 && p.Activo == true

            ).ToList();

            var jsonresult = Json( new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }

        #endregion

        #region INDEX_CLIENTE

        [HttpGet]
        public JsonResult ListaCategorias()
        {
            List<Categoria> lista = new CN_Categoria().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListaMarcas() 
        {
            List<Marca> lista = new CN_Marca().Listar();
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductos(int idcategoria, int idmarca) 
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            lista = new CN_Productos().Listar().Select(p => new Producto()
            {

                IdProducto = p.IdProducto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                objCategoria = p.objCategoria,
                objMarca = p.objMarca,
                Precio = p.Precio,
                Stock = p.Stock,

                RutaImagen = p.RutaImagen,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),

                Activo = p.Activo

            }).Where(p => 

                p.objCategoria.IdCategoria == (idcategoria == 0 ? p.objCategoria.IdCategoria : idcategoria) &&
                p.objMarca.IdMarca == (idmarca == 0 ? p.objMarca.IdMarca : idmarca) &&
                p.Stock > 0 && p.Activo == true

            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;
        }

        #endregion

        #region CARRITO

        [HttpPost]
        public JsonResult AgregarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool existe = new CN_Carrito().ExisteCarrito(idcliente, idproducto);

            bool respuesta = false;

            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito ";
            }
            else
            {
                respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);
            }

            return Json(new { _respuesta = respuesta, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CantidadCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadCarrito(idcliente);

            return Json(new { _cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarProductosCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            List<Carrito> listado = new List<Carrito>();

            bool conversion;

            listado = new CN_Carrito().ListarCarrito(idcliente).Select(c => new Carrito()
            {
                objProducto = new Producto()
                {
                    IdProducto = c.objProducto.IdProducto,
                    Nombre = c.objProducto.Nombre,
                    objMarca = c.objProducto.objMarca,
                    Precio = c.objProducto.Precio,
                    RutaImagen = c.objProducto.RutaImagen,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(c.objProducto.RutaImagen, c.objProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(c.objProducto.NombreImagen)
                },
                Cantidad = c.Cantidad
            }).ToList();

            return Json(new { data = listado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OperacionCarrito(int idproducto, bool sumar)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, true, out mensaje);

            return Json(new { data = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(int idproducto)
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CN_Carrito().ElimninarCarrito(idcliente, idproducto);

            return Json(new { _respuesta = respuesta, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region UBICACION

        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> lista = new CN_Ubicacion().ObtenerDepartamentos();

            return Json(new { _lista = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerProvincia(string iddepartamento)
        {
            List<Provincia> lista = new CN_Ubicacion().ObtenerProvincias(iddepartamento);

            return Json(new { _lista = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDistrito(string idprovincia, string iddepartamento)
        {
            List<Distrito> lista = new CN_Ubicacion().ObtenerDistritos( idprovincia, iddepartamento);

            return Json(new { _lista = lista }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PROCESAR PAGO
        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, Venta oVenta)
        {
            decimal total = 0;

            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("en-PE");
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            foreach(Carrito carrito in oListaCarrito)
            {
                decimal subTotal = Convert.ToDecimal(carrito.Cantidad.ToString()) * carrito.objProducto.Precio;
                total += subTotal;

                detalle_venta.Rows.Add(new object[]
                {
                    carrito.objProducto.IdProducto,
                    carrito.Cantidad,
                    subTotal
                });
            }
            oVenta.MontoTotal = total;
            oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

            TempData ["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_venta;

            return Json(new {Status = true, Link = "/Tienda/PagoEfectuado?idTransaccion=code0001&status=true"}, JsonRequestBehavior.AllowGet);
            
        }

        public async Task<ActionResult> PagoEfectuado()
        {
            string idTransaccion = Request.QueryString["idTransaccion"];
            bool status = Convert.ToBoolean(Request.QueryString["idTransaccion"]);

            ViewData["Status"] = status;
            if (status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                oVenta.IdTransaccion = idTransaccion;
                string mensaje = string.Empty;
                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;

            }
            return View();

        }


        #endregion

    }

}