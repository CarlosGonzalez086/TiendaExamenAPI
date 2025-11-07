using Newtonsoft.Json;
using System.Reflection;
using TiendaExamenAPI.DbData.DtoModels.articulos;
using TiendaExamenAPI.DbData.DtoModels.Response;
using TiendaExamenAPI.DbData.DtoModels.Tienda;
using TiendaExamenAPI.DbData.Repository.Articulos;
using TiendaExamenAPI.Services.FuncionesGenerales;

namespace TiendaExamenAPI.Services.Articulos
{
    public class ArticulosServicios
    {
        ArticulosRepositorio articulos = new();
        ImageStorageService storageService = new();
        public async Task<Response> guardarArticulo(dtoArticulos articuloInfo)
        {
            if (string.IsNullOrEmpty(articuloInfo.codigo))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo del codigo es requerido",
                    respuesta = ""
                };
            }
            if (string.IsNullOrEmpty(articuloInfo.descripcion))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de la descripcion es requerido",
                    respuesta = ""
                };
            }
            if (!string.IsNullOrEmpty(articuloInfo.imagen))
            {
                Response result = await storageService.SaveImageAsync(null, articuloInfo.imagen);

                if (result.codigo == "000")
                {
                    articuloInfo.imagen = result.respuesta.ToString();
                }
            }



            if (articuloInfo.precio == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo del precio es requerido",
                    respuesta = ""
                };
            }
            if (articuloInfo.stock == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo del stock es requerido",
                    respuesta = ""
                };
            }

            var insertOk = await articulos.insertado(articuloInfo);

            if (insertOk)
            {
                return new Response
                {
                    codigo = "200",
                    mensaje = "Articulo registrado correctamente",
                };
            }
            return new Response
            {
                codigo = "900",
                mensaje = "Ha ocurrido un error al registrarse, intente más tarde",
            };

        }

        public async Task<Response> actualizarArticulo(dtoArticulos articuloInfo)
        {

            long Id = articuloInfo.id;

            if (string.IsNullOrEmpty(articuloInfo.codigo))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo del codigo es requerido",
                    respuesta = ""
                };
            }
            if (string.IsNullOrEmpty(articuloInfo.descripcion))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de la descripcion es requerido",
                    respuesta = ""
                };
            }
            if (string.IsNullOrEmpty(articuloInfo.imagen))
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo de la imagen es requerido",
                    respuesta = ""
                };
            }
            if (articuloInfo.precio == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo del precio es requerido",
                    respuesta = ""
                };
            }
            if (articuloInfo.stock == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "El campo del stock es requerido",
                    respuesta = ""
                };
            }


            bool insertOk = articulos.actualizacion(articuloInfo, Id);

            if (!insertOk)
            {
                return new Response
                {
                    codigo = "900",
                    mensaje = "Ha ocurrido un error al actualizar, intente más tarde",
                };
            }

            return new Response
            {
                codigo = "200",
                mensaje = "Guardado correctamente",
                respuesta = ""
            };

        }
        public async Task<Response> obtenerArticulo(long id)
        {

            dtoArticulos infoArticulo = articulos.obternerPorId(id);


            if (infoArticulo != null)
            {
                return new Response
                {
                    codigo = "200",
                    mensaje = "Informacion del articulo",
                    respuesta = infoArticulo
                };
            }

            return new Response
            {
                codigo = "900",
                mensaje = "Ocurrio un error",
            };

        }
        public async Task<Response> EliminarArticulo(long id)
        {
            if (id == null || id == 0)
            {
                return new Response
                {
                    codigo = "901",
                    mensaje = "Parametros incorrectos",
                    respuesta = ""
                };
            }

            bool eliminado = articulos.eliminacion(id);

            if (!eliminado)
            {
                return new Response
                {
                    codigo = "900",
                    mensaje = "Ha ocurrido un error al eliminar, intente más tarde",
                    respuesta = ""
                };
            }
            return new Response
            {
                codigo = "200",
                mensaje = "Eliminado correctamente",
                respuesta = ""
            };
        }
        public async Task<Response> lista(int iTake, int iSkip)
        {

            iTake = iTake == 0 ? 99999 : iTake;
            var dtInfo = articulos.lista(iTake, iSkip);
            int totalPages = dtInfo.filas == 0 ? 0 : (int)Math.Ceiling((double)dtInfo.filas / iTake);
            return new Response
            {
                codigo = "200",
                mensaje = "",
                respuesta = new
                {
                    data = JsonConvert.SerializeObject(dtInfo),
                    totalRows = dtInfo.filas,
                    totalPages = totalPages
                }

            };
        }
    }
}
