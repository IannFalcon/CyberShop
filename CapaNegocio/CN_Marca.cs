using CapaDatos;
using CapaEntidades;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CN_Marca
    {
        private CD_Marca objCapaDatos = new CD_Marca();

        public List<Marca> Listar()
        {
            return objCapaDatos.ListarMarca();
        }


        public int Registrar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripcion de la marca no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.RegistrarMarca(obj, out Mensaje);
            }
            else
            {
                return 0;
            }

        }

        public bool Editar(Marca obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El descripcion de la marca no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.EditarMarca(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.EliminarMarca(id, out Mensaje);
        }

        public List<Marca> ListarMarcaporCategoria(int idcategoria)
        {
            return objCapaDatos.ListarMarcaporCategoria(idcategoria);
        }
    }
}
