using CapaEntidades;
using CapaNegocio;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public JsonResult CantidadEnCarrito()
        {
            int idcliente = ((Cliente)Session["Cliente"]).IdCliente;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idcliente);

            return Json(new {_cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

}