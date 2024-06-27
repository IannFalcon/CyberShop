using CapaEntidades;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppProyectoEFSRTCyberShop.Controllers
{
    [Authorize]
    public class MantenimientoController : Controller
    {
        public ActionResult Categoria()
        {
            return View();
        }

        public ActionResult Marca()
        {
            return View();
        }

        public ActionResult Producto()
        {
            return View();
        }

        #region CATEGORIAS

        [HttpGet]
        public JsonResult ListarCategoria()
        {
            var listado = new List<Categoria>();
            listado = new CN_Categoria().Listar();

            return Json(new { data = listado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categoria obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdCategoria == 0)
            {
                resultado = new CN_Categoria().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Categoria().Editar(obj, out mensaje);
            }

            return Json(new { _resultado = resultado, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Categoria().Eliminar(id, out mensaje);

            return Json(new { _resultado = resultado, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MARCAS

        [HttpGet]
        public JsonResult ListarMarca()
        {
            var listado = new List<Marca>();
            listado = new CN_Marca().Listar();

            return Json(new { data = listado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMarca(Marca obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.IdMarca == 0)
            {
                resultado = new CN_Marca().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new CN_Marca().Editar(obj, out mensaje);
            }

            return Json(new { _resultado = resultado, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Marca().Eliminar(id, out mensaje);

            return Json(new { _resultado = resultado, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PRODUCTO

        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> listado = new List<Producto>();
            listado = new CN_Productos().Listar();

            return Json(new { data = listado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(string obj, HttpPostedFileBase imagen)
        {
            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool guardar_imagen_exito = true;

            Producto objProd = new Producto();
            objProd = JsonConvert.DeserializeObject<Producto>(obj);

            decimal precio;

            if (decimal.TryParse(objProd.PrecioTexto, System.Globalization.NumberStyles.AllowDecimalPoint, new CultureInfo("es-PE"), out precio))
            {
                objProd.Precio = precio;
            }
            else
            {
                return Json(new { _operacion_exitosa = false, _mensaje = "El formato del precio debe ser 0.00" }, JsonRequestBehavior.AllowGet);
            }

            if(objProd.IdProducto == 0)
            {
                int idProductoGenerado = new CN_Productos().Registrar(objProd, out mensaje);

                if (idProductoGenerado != 0)
                {
                    objProd.IdProducto = idProductoGenerado;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Productos().Editar(objProd, out mensaje);
            }

            if (operacion_exitosa)
            {
                if(imagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(imagen.FileName);
                    string nombre_imagen = string.Concat(objProd.IdProducto.ToString(), extension);

                    try
                    {
                        imagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        string msj = ex.Message;
                        guardar_imagen_exito = false;
                    }

                    if (guardar_imagen_exito)
                    {
                        objProd.RutaImagen = ruta_guardar;
                        objProd.NombreImagen = nombre_imagen;
                        bool rspta = new CN_Productos().GuardarDatosImagen(objProd, out mensaje);
                    }
                    else
                    {
                        mensaje = "Se guardo el producto pero hubo problemas con la imagen";
                    }
                }
            }

            return Json(new { _operacion_exitosa = operacion_exitosa, idGenerado = objProd, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;
            Producto objProd = new CN_Productos().Listar().Where(p => p.IdProducto == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(objProd.RutaImagen, objProd.NombreImagen), out conversion);

            return Json(new { _conversion = conversion, _textobase64 = textoBase64, extension = Path.GetExtension(objProd.NombreImagen) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            bool resultado = false;
            string mensaje = string.Empty;

            resultado = new CN_Productos().Eliminar(id, out mensaje);

            return Json(new { _resultado = resultado, _mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}