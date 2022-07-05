using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using CapaDominio.ComponentesNegocio;
using Newtonsoft.Json;
using System.Web;
using CapaDominio.EntidadesNegocio;
using CapaServicios.Servicios;
using CsWeb.Filters;
using CsWeb.Infrastructure.Notification;
using System.IO;
using System.Data;
using System.Text;

namespace CsWeb.Controllers
{
    [HandleError]
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly ICatalogoServicio _catalogoServicio;
        private readonly IAdminServicio _adminServicio;
        private readonly IVariableConfiguracionServicio _variableConfiguracionServicio;
        public AdminController(ICatalogoServicio catalogoServicio, IAdminServicio adminServicio, IVariableConfiguracionServicio variableConfiguracionServicio)
        {
            _catalogoServicio = catalogoServicio;
            _adminServicio = adminServicio;
            _variableConfiguracionServicio = variableConfiguracionServicio;
        }

        public ActionResult ContenidosSitio(string seccion)
        {
            ContenidoSitio contenidoSitio = new ContenidoSitio();
            contenidoSitio.Seccion = seccion;
            return View(contenidoSitio);
        }

        public ActionResult ContenidoSitio(int? id, string seccion)
        {
            ContenidoSitio contenido = new ContenidoSitio();
            contenido.Seccion = seccion;
            if (id > 0)
            {
                contenido = _adminServicio.ObtenerContenido(new ContenidoSitio { ContenidoSitioId = (int)id });
            }
            return View(contenido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContenidoSitio(ContenidoSitio contenidoSitio)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                contenidoSitio.FechaCreado = DateTime.Now;
                if (contenidoSitio.ContenidoSitioId == 0)
                    _adminServicio.CrearContenido(contenidoSitio);
                else
                    _adminServicio.ActualizarContenido(contenidoSitio);
                return RedirectToActionPermanent("ContenidosSitio", new { Seccion = contenidoSitio.Seccion });
            }
            return View();
        }

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerContenidoSitio(string sidx, string sord, int page, int rows, string seccion)
        {
            ContenidoSitio filtro = new ContenidoSitio();
            filtro.Seccion = seccion;
            int cantidad;
            var lista = _adminServicio.ListarContenidoSitio(page, rows, out cantidad, filtro);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.ContenidoSitioId,
                        item.Seccion,
                        item.Titulo,
                        item.Contenido,
                        item.Enlace,
                        item.Publicar,
                    }).ToArray()
            };

            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        public ActionResult Proyectos()
        {
            CargarViewBag();
            ViewBag.FiltroRegion = Request.QueryString["FiltroRegion"];
            ViewBag.FiltroDepartamento = Request.QueryString["FiltroDepartamento"];
            ViewBag.FiltroMunicipio = Request.QueryString["FiltroMunicipio"];
            ViewBag.FiltroNombre = Request.QueryString["FiltroNombre"];
            ViewBag.FiltroPunto = Request.QueryString["FiltroPunto"];
            ViewBag.FiltroPilar = Request.QueryString["FiltroPilar"];
            ViewBag.FiltroSector = Request.QueryString["FiltroSector"];
            ViewBag.FiltroEntidad = Request.QueryString["FiltroEntidad"];
            ViewBag.FiltroEstado = Request.QueryString["FiltroEstado"];
            ViewBag.Perfil = NombrePerfil;
                return View();
        }
        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerProyectos(string sidx, string sord, int page, int rows)
        {
            Proyecto filtro = null;

            if (Request.UrlReferrer != null)
            {
                filtro = new Proyecto
                {
                    CodDepartamento = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDepartamento"],
                    CodMunicipio = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroMunicipio"],
                    NombreProyecto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroNombre"],
                    Punto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroPunto"],
                    Pilar = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroPilar"],
                    Sector = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroSector"],
                    EntidadResponsable = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroEntidad"],
                    EstadoProyecto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroEstado"],
                };
            };
            //if (NombrePerfil == "Veedor")
            //{
            //   Veedor veedor= _adminServicio.ObtenerVeedor(new Veedor { UsuarioCreacion = Usuario });
            //    filtro.CodMunicipio = veedor.CodMunicipio;
            //}
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

        public ActionResult Proyecto(int? id)
        {
            CargarViewBag();
            Proyecto proyecto = new Proyecto();
            if (id > 0)
            {
                proyecto = _adminServicio.ObtenerProyecto(new Proyecto { ProyectoId = (int)id });

            }
            ViewBag.Perfil = NombrePerfil;
            return View(proyecto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Proyecto(Proyecto proyecto)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                proyecto.Departamento = _catalogoServicio.ObtenerNombreDepartamento(proyecto.CodDepartamento).Departamento;
                proyecto.Municipio = _catalogoServicio.ObtenerNombreMunicipio(proyecto.CodMunicipio).Municipio;
                proyecto.FechaCreado = DateTime.Now;
                if (proyecto.ProyectoId == 0)
                    _adminServicio.CrearProyecto(proyecto);
                else
                    _adminServicio.ActualizarProyecto(proyecto);
                return RedirectToActionPermanent("Proyectos");
            }
            ViewBag.Perfil = NombrePerfil;
            CargarViewBag();
            return View();
        }

        public ActionResult ExportarProyectos()
        {
            CargarViewBag();
            ViewBag.FiltroRegion = Request.QueryString["FiltroRegion"];
            ViewBag.FiltroDepartamento = Request.QueryString["FiltroDepartamento"];
            ViewBag.FiltroMunicipio = Request.QueryString["FiltroMunicipio"];
            ViewBag.FiltroNombre = Request.QueryString["FiltroNombre"];
            ViewBag.FiltroPunto = Request.QueryString["FiltroPunto"];
            ViewBag.FiltroPilar = Request.QueryString["FiltroPilar"];
            ViewBag.FiltroSector = Request.QueryString["FiltroSector"];
            ViewBag.FiltroEntidad = Request.QueryString["FiltroEntidad"];
            ViewBag.FiltroEstado = Request.QueryString["FiltroEstado"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public ActionResult ProyectoAvance(int? id, bool nuevo)
        {
            ProyectoAvance avance = new ProyectoAvance();
            Proyecto proyecto = new Proyecto();
            if (!nuevo)
            {
                avance = _adminServicio.ObtenerProyectoAvance(new ProyectoAvance { ProyectoAvanceId = (int)id });
                avance.NombreProyecto= _adminServicio.ObtenerProyecto(new Proyecto { ProyectoId = avance.ProyectoId }).NombreProyecto;
            }
            else if (id > 0)
            {
                proyecto = _adminServicio.ObtenerProyecto(new Proyecto { ProyectoId = (int)id });
                avance.ProyectoId = proyecto.ProyectoId;
                avance.NombreProyecto = proyecto.NombreProyecto;
            }
            return View(avance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProyectoAvance(ProyectoAvance avancex)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                avancex.FechaCreado = DateTime.Now;
                avancex.FechaAvance = DateTime.Now;

                if(avancex.ProyectoAvanceId==0)
                _adminServicio.CrearProyectoAvance(avancex);
                else
                _adminServicio.ActualizarProyectoAvance(avancex);
                return RedirectToActionPermanent("Proyecto", new { id = avancex.ProyectoId});
            }
            CargarViewBag();
            return View();
        }

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerAvances(string sidx, string sord, int page, int rows, int id)
        {
            ProyectoAvance filtro = new ProyectoAvance();
            filtro.ProyectoId = id;
            int cantidad;
            var lista = _adminServicio.ListarProyectoAvance(page, rows, out cantidad, filtro);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.ProyectoAvanceId,
                        FechaAvance = Convert.ToDateTime(item.FechaAvance).ToString("dd/MM/yyyy"),
                        item.Indicador,
                        item.Avance,
                        item.Meta,
                        FechaMeta = Convert.ToDateTime(item.FechaMeta).ToString("dd/MM/yyyy"),
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }


        public ActionResult ProyectoSeleccionado(int? id)
        {
            CargarViewBag();
            Proyecto proyecto = new Proyecto();
            Pasos paso = new Pasos();
            if (id > 0)
            {
                proyecto = _adminServicio.ObtenerProyecto(new Proyecto { ProyectoId = (int)id });
               Veedor veedor = _adminServicio.ObtenerVeedor(new Veedor { UsuarioCreacion = Usuario });
                paso.Subregion = proyecto.Subregion;
                paso.Departamento = proyecto.Departamento;
                paso.Municipio = proyecto.Municipio;
                paso.CodDepartamento = proyecto.CodDepartamento;
                paso.CodMunicipio = proyecto.CodMunicipio;
                paso.CodigoProyecto = proyecto.CodigoProyecto;
                paso.NombreVeedor = veedor.Nombres+" "+veedor.Apellidos;
                paso.ProyectoId = proyecto.ProyectoId;
                paso.NombreObjeto = proyecto.NombreProyecto;
                paso.EntidadResponsable = proyecto.EntidadResponsable;
                paso.Usuario=Usuario;
                paso.VeedorId=veedor.VeedorId;
                paso.FechaCreado = DateTime.Now;
                _adminServicio.CrearPasos(paso);
                return RedirectToActionPermanent("ProyectosVeedor","Veedor");
            }
            return View(proyecto);
        }

        #region Métodos generales

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerDepartamentos(string subregion)
        {
            List<SelectListItem> deptos = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(subregion))
            {
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
                    municipios = _catalogoServicio.ObtenerMunicipios(codDepartamento.Trim(),subregion).Select(x => new SelectListItem
                {
                    Text = x.Municipio,
                    Value = x.CodMunicipio
                }).ToList();

                municipios.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            }
            return new ContentResult { Content = JsonConvert.SerializeObject(municipios), ContentType = "application/json" };
        }


        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ListarMunicipios(string codDepartamento, string subregion)
        {
            List<SelectListItem> municipios = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(codDepartamento))
            {
                municipios = _catalogoServicio.ListarMunicipios(codDepartamento.Trim()).Select(x => new SelectListItem
                {
                    Text = x.Etiqueta,
                    Value = x.Valor
                }).ToList();

                municipios.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            }
            return new ContentResult { Content = JsonConvert.SerializeObject(municipios), ContentType = "application/json" };
        }

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ListarMunicipiosD(string codDepartamento)
        {
            List<SelectListItem> municipios = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(codDepartamento))
            {
                municipios = _catalogoServicio.ObtenerMunicipiosD(codDepartamento.Trim()).Select(x => new SelectListItem
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

            List<SelectListItem> listaSubregion = _catalogoServicio.ListarPorCategoria(new Catalogo { Categoria = CategoriaCatalogo.Subregion }).Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
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

        public ActionResult ExportarProyectosExcel()
        {
            Proyecto filtro = new Proyecto();
            filtro = new Proyecto
            {
                CodDepartamento = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDepartamento"],
                CodMunicipio = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroMunicipio"],
                NombreProyecto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroNombre"],
                Punto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroPunto"],
                Pilar = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroPilar"],
                Sector = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroSector"],
                EntidadResponsable = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroEntidad"],
                EstadoProyecto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroEstado"],
            };
            var lista = _adminServicio.ListarProyectosRep(filtro).ToList();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn { ColumnName = "Subregión PDET" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Departamento" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Municipio" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Punto Acuerdo de Paz" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Pilar PDET" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Estrategia" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Entidad responsable" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Sector" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Código del proyecto" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Título" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Descripción" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Nombre del proyecto" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Estado" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Monto inversión" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Fuente recursos" });

            StringBuilder stringBuilder = new StringBuilder();
            foreach (Proyecto item in lista)
            {
                stringBuilder.Clear();
                var registro = _adminServicio.ObtenerProyecto(item);

                DataRow dataRow = dataTable.NewRow();
                dataRow["Subregión PDET"] = item.Subregion;
                dataRow["Departamento"] = item.Departamento;
                dataRow["Municipio"] = item.Municipio;
                dataRow["Punto Acuerdo de Paz"] = item.Punto;
                dataRow["Pilar PDET"] = item.Pilar;
                dataRow["Estrategia"] = item.Estrategia;
                dataRow["Entidad responsable"] = item.EntidadResponsable;
                dataRow["Sector"] = item.Sector;
                dataRow["Código del proyecto"] = item.CodigoProyecto;
                dataRow["Título"] = item.TItulo;
                dataRow["Descripción"] = item.Descripcion;
                dataRow["Nombre del proyecto"] = item.NombreProyecto;
                dataRow["Estado"] = item.EstadoProyecto;
                dataRow["Monto inversión"] = item.MontoInversión;
                dataRow["Fuente recursos"] = item.FuenteRecursos;
                dataTable.Rows.Add(dataRow);
            }

            stringBuilder.Clear();
            string nombreArchivo;
            var stream = Excel.CrearReporteGeneral("Listado Proyectos", stringBuilder.ToString(), dataTable, out nombreArchivo);
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            stream.Position = 0;
            return File(stream, contentType, nombreArchivo);
        }

        #endregion

        #region Contenido Sitio
        public ActionResult Documentos()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "Documentos" });
        }
        public ActionResult DocumentosOtros()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "DocumentosOtros" });
        }
        public ActionResult Sistemas()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "Sistemas" });
        }
        public ActionResult Formacion()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "Formacion" });
        }
        public ActionResult EntidadesRed()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "Entidades" });
        }
        public ActionResult Preguntas()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "Preguntas" });
        }
        public ActionResult Cifras()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "Cifras" });
        }
        public ActionResult Paginas()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "PaginasInteres" });
        }
        public ActionResult Capacitacion()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "Capacitacion" });
        }
        #endregion
    }
}