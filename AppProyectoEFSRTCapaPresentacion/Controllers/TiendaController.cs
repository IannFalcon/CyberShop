using CapaEntidades;
using CapaEntidades.PayPal;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

using AppProyectoEFSRTCapaPresentacion.Filter;
using System.Diagnostics;

namespace AppProyectoEFSRTCapaPresentacion.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }

        [ValidarSesion]
        [Authorize]
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

        #region FILTROS

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
                    NombreImagen = c.objProducto.NombreImagen,
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

            respuesta = new CN_Carrito().OperacionCarrito(idcliente, idproducto, sumar, out mensaje);

            return Json(new { _respuesta = respuesta, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
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
            ResponsePaypal<ResponseCheckOut> response_paypal = null;

            try
            {
                decimal total = 0;

                DataTable detalle_venta = new DataTable();
                detalle_venta.Locale = new CultureInfo("es-PE");
                detalle_venta.Columns.Add("IdProducto", typeof(string));
                detalle_venta.Columns.Add("Cantidad", typeof(int));
                detalle_venta.Columns.Add("Total", typeof(decimal));

                List<Item> oListaItem = new List<Item>();

                foreach (Carrito carrito in oListaCarrito)
                {
                    decimal subTotal = Convert.ToDecimal(carrito.Cantidad.ToString()) * carrito.objProducto.Precio;
                    total += subTotal;

                    oListaItem.Add(new Item()
                    {
                        name = carrito.objProducto.Nombre,
                        quantity = carrito.Cantidad.ToString(),
                        unit_amount = new UnitAmount()
                        {
                            currency_code = "USD",
                            value = carrito.objProducto.Precio.ToString("G", new CultureInfo("es-PE"))
                        }

                    });

                    detalle_venta.Rows.Add(new object[]
                    {
                        carrito.objProducto.IdProducto,
                        carrito.Cantidad,
                        subTotal
                    });
                }

                PurchaseUnit purchaseUnit = new PurchaseUnit()
                {
                    amount = new Amount()
                    {
                        currency_code = "USD",
                        value = total.ToString("G", new CultureInfo("es-PE")),
                        breakdown = new Breakdown()
                        {
                            item_total = new ItemTotal()
                            {
                                currency_code = "USD",
                                value = total.ToString("G", new CultureInfo("es-PE")),
                            }
                        }
                    },
                    description = "Compra de producto en CyberShop",
                    items = oListaItem
                };

                CheckOutOrder oCheckoutOrder = new CheckOutOrder()
                {
                    intent = "CAPTURE",
                    purchase_units = new List<PurchaseUnit> { purchaseUnit },
                    application_context = new ApplicationContext()
                    {
                        brand_name = "CyberShop",
                        landing_page = "NO_PREFERENCE",
                        user_action = "PAY_NOW",
                        return_url = "https://localhost:44314/Tienda/PagoEfectuado",
                        cancel_url = "https://localhost:44314/Tienda/Carrito"
                    }
                };

                oVenta.MontoTotal = total;
                oVenta.IdCliente = ((Cliente)Session["Cliente"]).IdCliente;

                TempData["Venta"] = oVenta;
                TempData["DetalleVenta"] = detalle_venta;

                CN_Paypal cN_Paypal = new CN_Paypal();

                response_paypal = await cN_Paypal.CrearSolicitud(oCheckoutOrder);
                Debug.WriteLine($"ResponsePaypal: {response_paypal}");
            }
            catch (Exception ex) 
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
            
            return Json(response_paypal, JsonRequestBehavior.AllowGet);
        }

        [ValidarSesion]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];

            CN_Paypal oPaypal = new CN_Paypal();
            ResponsePaypal<ResponseCapture> response_paypal = new ResponsePaypal<ResponseCapture>();
            response_paypal = await oPaypal.AprobarPago(token);

            ViewData["Status"] = response_paypal.status;
            if (response_paypal.status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                oVenta.IdTransaccion = response_paypal.response.purchase_units[0].payments.captures[0].id;
                
                string mensaje = string.Empty;
                bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

                ViewData["IdTransaccion"] = oVenta.IdTransaccion;
            }

            return View();

        }


        #endregion

        #region MIS COMPRAS
        [ValidarSesion]
        [Authorize]
        public ActionResult MisCompras()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;

            List<DetalleVenta> listado = new List<DetalleVenta>();

            bool conversion;

            listado = new CN_Venta().ListarCompras(idcliente).Select(c => new DetalleVenta()
            {
                objProducto = new Producto()
                {
                    Nombre = c.objProducto.Nombre,
                    Precio = c.objProducto.Precio,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(c.objProducto.RutaImagen, c.objProducto.NombreImagen), out conversion),
                    Extension = Path.GetExtension(c.objProducto.NombreImagen)
                },
                Cantidad = c.Cantidad,
                Total = c.Total,
                IdTransaccion = c.IdTransaccion,
            }).ToList();

            return View(listado);
        }
        #endregion

    }

}