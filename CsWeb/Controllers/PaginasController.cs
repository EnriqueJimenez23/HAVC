using System.Web.Mvc;
using CsWeb.Filters;
using System.Collections.Generic;
using CapaDominio.ComponentesNegocio;
using CapaServicios.Servicios;
using System;
using System.Linq;
using Newtonsoft.Json;
using CsWeb.Models;
using CapaDominio.EntidadesNegocio;
using System.Web;
using System.Data;

namespace CsWeb.Controllers
{
    [HandleError]
    public class PaginasController : BaseController
    {
        private readonly IAdminServicio _adminServicio;
        private readonly ICatalogoServicio _catalogoServicio;
        public PaginasController(IAdminServicio adminServicio, ICatalogoServicio catalogoServicio)
        {
            _adminServicio = adminServicio;
            _catalogoServicio = catalogoServicio;
        }
        [ExcluirAutorizacion]
        public ActionResult index()
        {
            Proyecto proy = new Proyecto();
            return View(proy);
        }
        [ExcluirAutorizacion]
        public ActionResult Portal()
        {
            return View();
        }
        [ExcluirAutorizacion]
        public ActionResult Formacion()
        {
            return View();
        }
        [ExcluirAutorizacion]
        public ActionResult Documentos()
        {
           
            var lista = new List<ContenidoSitio>();
            ContenidoSitio Item = new ContenidoSitio();
            lista = _adminServicio.ListarContenidoSitio("Documentos").ToList();
            var arrayLista = lista.ToArray();
            ViewBag.Lista=arrayLista;

            var listaD = new List<ContenidoSitio>();
            ContenidoSitio ItemD= new ContenidoSitio();
            listaD = _adminServicio.ListarContenidoSitio("DocumentosOtros").ToList();
            var arrayListaD = listaD.ToArray();
            ViewBag.ListaD = arrayListaD;
            return View();
        }
        [ExcluirAutorizacion]
        public ActionResult PaginasInteres()
        {
            return View();
        }

        [ExcluirAutorizacion]
        public ActionResult Cifras()
        {
            var listac = new List<ContenidoSitio>();
            ContenidoSitio Itemc = new ContenidoSitio();
            listac = _adminServicio.ListarContenidoSitio("Cifras").ToList();
            var arrayListac = listac.ToArray();
            ViewBag.ListaC = arrayListac;
            return View();
        }

        [ExcluirAutorizacion]
        public ActionResult Sistemas()
        {
            var listas = new List<ContenidoSitio>();
            ContenidoSitio Items = new ContenidoSitio();
            listas = _adminServicio.ListarContenidoSitio("Sistemas").ToList();
            var arrayListas = listas.ToArray();
            ViewBag.ListaS = arrayListas;
            return View();
        }

        [ExcluirAutorizacion]
        public ActionResult Preguntas()
        {
            ViewBag.Lista = new List<ContenidoSitio>();
            var lista = _adminServicio.ListarContenidoSitio("Preguntas").ToList();
            var arrayLista = lista.ToArray();
            ViewBag.Lista = arrayLista;
            return View();
        }

        [ExcluirAutorizacion]
        public ActionResult Ficha(int? id)
        {
            Proyecto proyecto = new Proyecto();
            if (id > 0)
            {
                proyecto = _adminServicio.ObtenerProyecto(new Proyecto { ProyectoId = (int)id });

            }
            ViewBag.Perfil = NombrePerfil;
            return View(proyecto);
        }
        [ExcluirAutorizacion]
        public ActionResult Localizacion()
        {
            return View();
        }
       
        [ExcluirAutorizacion]
        public ActionResult Entidades()
        {
            var lista = new List<ContenidoSitio>();
            ContenidoSitio Item = new ContenidoSitio();
            lista = _adminServicio.ListarContenidoSitio("Entidades").ToList();
            var arrayLista = lista.ToArray();
            ViewBag.Lista = arrayLista;
            return View();
        }
        [ExcluirAutorizacion]
        public ActionResult Pasos()
        {
            return View();
        }
        [ExcluirAutorizacion]
        public ActionResult herramienta()
        {
            return View();
        }
        [ExcluirAutorizacion]
        public ActionResult Modulos()
        {
            var lista = _adminServicio.ListarContenidoSitio("Formacion").ToList();
            var arrayLista = lista.ToArray();
            ViewBag.Lista = arrayLista;
            return View();
        }
        [ExcluirAutorizacion]
        public ActionResult Capacitacion()
        {
            var lista = _adminServicio.ListarContenidoSitio("Capacitacion").ToList();
            var arrayLista = lista.ToArray();
            ViewBag.Lista = arrayLista;
            return View();
        }
        [ExcluirAutorizacion]
        public ActionResult Proyectosw(int? id,string Palabra)
        {
            Proyecto proyecto = new Proyecto();
            CargarViewBag();
            ViewBag.FiltroRegion = Request.QueryString["FiltroRegion"];
            ViewBag.FiltroDepartamento = Request.QueryString["FiltroDepartamento"];
            ViewBag.FiltroMunicipio = Request.QueryString["FiltroMunicipio"];
            ViewBag.FiltroNombre = Request.QueryString["FiltroNombre"];
            ViewBag.FiltroPilar = Request.QueryString["FiltroPilar"];
            proyecto.Descripcion=Palabra;
            proyecto.ProyectoId =Convert.ToInt16(id);

            //ViewBag.FiltroEntidad = Request.QueryString["FiltroEntidad"];
            return View(proyecto);
        }

        [ExcluirAutorizacion]
        public ActionResult Proyectosgeo(string cod)
        {
            DatosGeo datos = _adminServicio.ObtenerDatosGeom(cod);
            return View(datos);
        }
        [ExcluirAutorizacion]
        public ActionResult ObtenerProyectos(string sidx, string sord, int page, int rows,int? id,string Descripcion)
        {
            Proyecto filtro = null;

            if (Request.UrlReferrer != null)
            {
                filtro = new Proyecto
                {
                    Subregion = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroRegion"],
                    CodDepartamento = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDepartamento"],
                    CodMunicipio = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroMunicipio"],
                    NombreProyecto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroNombre"],
                    Pilar = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroPilar"],
                };
                if (!string.IsNullOrEmpty(Descripcion))
                {
                   filtro.NombreProyecto = Descripcion;
                }
                if (id > 0)
                {
                    DatosGeo datos = _adminServicio.ObtenerDatosGeo(Convert.ToInt16(id));
                    filtro.CodMunicipio = datos.CodMunicipio;
                    filtro.NombreProyecto = string.Empty;
                }
            };
            int cantidad;
            var lista = _adminServicio.ListarProyectos(page, rows, out cantidad, filtro);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.ProyectoId,
                        item.Subregion,
                        item.EntidadResponsable,
                        item.Departamento,
                        item.Municipio,
                        item.NombreProyecto,
                        item.Punto,
                        item.Pilar,
                        item.EstadoProyecto,
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }


        [ExcluirAutorizacion]
        public ActionResult ObtenerProyectos1(string sidx, string sord, int page, int rows, int? id, string Descripcion)
        {
            Proyecto filtro = null;

            if (Request.UrlReferrer != null)
            {
                filtro = new Proyecto
                {
                    Subregion = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroRegion"],
                    CodDepartamento = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDepartamento"],
                    CodMunicipio = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroMunicipio"],
                    NombreProyecto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroNombre"],
                    Pilar = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroPilar"],
                };
                if (!string.IsNullOrEmpty(Descripcion))
                {
                    filtro.NombreProyecto = Descripcion;
                }
                //if (id>0)
                //{
                //   DatosGeo datos= _adminServicio.ObtenerDatosGeo(Convert.ToInt16(id));
                //    filtro.CodMunicipio = datos.CodMunicipio;
                //    filtro.NombreProyecto = string.Empty;
                //}
            };
            int cantidad;
            var lista = _adminServicio.ListarProyectos(page, rows, out cantidad, filtro);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.ProyectoId,
                        item.Subregion,
                        item.Departamento,
                        item.Municipio,
                        item.NombreProyecto,
                        item.Punto,
                        item.Pilar,
                        item.EstadoProyecto,
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [ExcluirAutorizacion]
        public ActionResult ObtenerProyectosgeo(string sidx, string sord, int page, int rows, int? id, string cod)
        {
            Proyecto filtro = null;
                filtro = new Proyecto
                {
                    CodMunicipio = cod,
                };
            int cantidad;
            var lista = _adminServicio.ListarProyectos(page, rows, out cantidad, filtro);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.ProyectoId,
                        item.Subregion,
                        item.Departamento,
                        item.Municipio,
                        item.NombreProyecto,
                        item.EntidadResponsable,
                        item.Punto,
                        item.Pilar,
                        item.EstadoProyecto,
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }


        [ExcluirAutorizacion]
        public ActionResult DeptoRegion(string subregion)
        {
            //List<SelectListItem> lista = new List<SelectListItem>();
            //if (!string.IsNullOrEmpty(subregion))
            //{
            //    lista = _catalogoServicio.ObtenerDepartamentos(subregion).Select(x => new SelectListItem
            //    {
            //        Text = x.Departamento,
            //        Value = x.CodDepartamento
            //    }).ToList();
            //}
            //var arrayLista = lista.ToArray();
            //ViewBag.Lista = arrayLista;
            //ViewBag.Subregion= subregion;
            //return PartialView("DeptoRegion");
            return View();
        }

        [ExcluirAutorizacion]
        public ActionResult ObtenerDatosGeo(string sidx, string sord, int page, int rows, string subregion)
        {

            int cantidad = 70;
            var lista = _adminServicio.ListarDatosGeo(subregion);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.DatosGeoId,
                        item.Subregion,
                        item.Departamento,
                        item.Municipio,
                        item.Extension,
                        item.PobTotal,
                        item.PobRural,
                        item.PobUrbana,
                        item.PobHombres,
                        item.PobMujeres,
                        item.PobIndigena,
                        item.PobAfro,
                        item.PobPalenque,
                        item.PobRaizal,
                        item.PobRom,
                        item.VeeduriasPaz,
                        item.TotalVeedurías,
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }





        [ExcluirAutorizacion]
        public ActionResult DatosGeo(int? id,string subregion)
        {
            DatosGeo datos = new DatosGeo();
            if (id > 0)
                datos.DatosGeoId = Convert.ToInt16(id);
            datos.Subregion = subregion;
            return View(datos);
        }
        [ExcluirAutorizacion]
        public ActionResult DatosGeom(int? id, string cod)
        {
            DatosGeo datos = new DatosGeo();
            if (id > 0)
                datos.DatosGeoId = Convert.ToInt16(id);
            datos = _adminServicio.ObtenerDatosGeom(cod);
            //datos.CodMunicipio = cod;
            return View(datos);
        }

        [ExcluirAutorizacion]
        public ActionResult ErrorCargaArchivo()
        {
            return View();
        }
        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerDepartamentos(string subregion)
        {
            List<SelectListItem> deptos = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(subregion))
            {
                //List<SelectListItem> 
                deptos = _catalogoServicio.ObtenerDepartamentos(subregion).Select(x => new SelectListItem
                {
                    Text = x.Departamento,
                    Value = x.CodDepartamento
                }).ToList();

                deptos.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            }
            return new ContentResult { Content = JsonConvert.SerializeObject(deptos), ContentType = "application/json" };
        }

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerMunicipios(string codDepartamento, string subregion)
        {
            List<SelectListItem> municipios = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(codDepartamento))
            {
                //List<SelectListItem>
                municipios = _catalogoServicio.ObtenerMunicipios(codDepartamento.Trim(), subregion).Select(x => new SelectListItem
                {
                    Text = x.Municipio,
                    Value = x.CodMunicipio
                }).ToList();

                municipios.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            }
            return new ContentResult { Content = JsonConvert.SerializeObject(municipios), ContentType = "application/json" };
        }

        private void CargarViewBag()
        {
            List<SelectListItem> listaPuntos = _catalogoServicio.ListarPorCategoria(new Catalogo { Categoria = CategoriaCatalogo.Punto }).Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
            }).ToList();

            listaPuntos.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaPuntos = listaPuntos;

            List<SelectListItem> listaPilar = _catalogoServicio.ListarPorCategoria(new Catalogo { Categoria = CategoriaCatalogo.PilarPDET }).Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
            }).ToList();

            listaPilar.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaPilar = listaPilar;

            List<SelectListItem> listaSubregion = _catalogoServicio.ObtenerSubregion().Select(x => new SelectListItem
            {
                Text = x.Subregion,
                Value = x.Subregion
            }).ToList();

            listaSubregion.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaSubregion = listaSubregion;

            List<SelectListItem> listaMunicipio = _catalogoServicio.ListarMunicipiosAll().Select(x => new SelectListItem
            {
                Text = x.Municipio,
                Value = x.CodMunicipio
            }).ToList();

            listaMunicipio.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaMunicipio = listaMunicipio;
        }
    }
}