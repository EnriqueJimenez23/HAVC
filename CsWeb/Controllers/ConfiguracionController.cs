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

namespace CsWeb.Controllers
{
   // [HandleError]
    [ValidateInput(false)]
    [Authorize]
    public class ConfiguracionController : BaseController
    {
        private readonly ICatalogoServicio _catalogoServicio;
        private readonly IVariableConfiguracionServicio _variableConfiguracionServicio;
        private readonly IRegistroOperacionServicio _registroOperacionServicio;

        public ConfiguracionController(ICatalogoServicio catalogoServicio, IVariableConfiguracionServicio variableConfiguracionServicio, IRegistroOperacionServicio registroOperacionServicio)
        {
            _catalogoServicio = catalogoServicio;
            _variableConfiguracionServicio = variableConfiguracionServicio;
            _registroOperacionServicio = registroOperacionServicio;
        }

        #region Variables de configuracion

        public ActionResult Variables()
        {
            List<SelectListItem> categorias = Enum.GetValues(typeof(CategoriaVariableConfiguracion)).Cast<CategoriaVariableConfiguracion>().Select(x => new SelectListItem
            {
                Text = x.GetDisplayName(),
                Value = ((int)x).ToString()
            }).ToList();
            categorias.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });

            ViewBag.Categorias = categorias;
            ViewBag.FiltroCategoria = Request.QueryString["FiltroCategoria"];
            ViewBag.FiltroNombre = Request.QueryString["FiltroNombre"];
            ViewBag.FiltroDescripcion = Request.QueryString["FiltroDescripcion"];

            return View();
        }

        public ActionResult Variable(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VariableConfiguracion variableConfiguracion = _variableConfiguracionServicio.Obtener(new VariableConfiguracion {VariableConfiguracionId = (int) id});
            if (variableConfiguracion == null)
            {
                return HttpNotFound();
            }

            return View(variableConfiguracion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Variable([Bind(Include = "VariableConfiguracionId,Categoria,Nombre,Valor,Descripcion")] VariableConfiguracion variableConfiguracion)
        {
            if (ModelState.IsValid)
            {
                _variableConfiguracionServicio.Actualizar(variableConfiguracion);

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
                return RedirectToAction("Variables");
            }
            return View(variableConfiguracion);
        }

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerVariables(string sidx, string sord, int page, int rows)
        {
            VariableConfiguracion filtro = null;
            CategoriaVariableConfiguracion? categoriaVariableConfiguracion = null;
            if (Request.UrlReferrer != null)
            {
                var filtroCategoria = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroCategoria"];
                int valorCategoria;
                if (int.TryParse(filtroCategoria, out valorCategoria))
                {
                    categoriaVariableConfiguracion = (CategoriaVariableConfiguracion) valorCategoria;
                }

                filtro = new VariableConfiguracion
                {
                    Nombre = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroNombre"],
                    Descripcion = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDescripcion"]
                };
            }

            int cantidad;
            var lista = _variableConfiguracionServicio.Listar(page, rows, out cantidad, filtro, categoriaVariableConfiguracion);

            var jsonData = new
            {
                total = (int) Math.Ceiling((float) cantidad/rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.VariableConfiguracionId,
                        Categoria = item.Categoria.GetDisplayName(),
                        item.Nombre,
                        item.Valor
                    }).ToArray()
            };

            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }
     
        #endregion

        #region Catalogo

        public ActionResult Catalogos()
        {
            List<SelectListItem> categorias = Enum.GetValues(typeof(CategoriaCatalogo)).Cast<CategoriaCatalogo>().Select(x => new SelectListItem
            {
                Text = x.GetDisplayName(),
                Value = ((int)x).ToString()
            }).ToList();
            categorias.Insert(0, new SelectListItem{ Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = ""});

            ViewBag.Categorias = categorias;
            ViewBag.FiltroCategoria = Request.QueryString["FiltroCategoria"];
            ViewBag.FiltroEtiqueta = Request.QueryString["FiltroEtiqueta"];
            ViewBag.FiltroDescripcion = Request.QueryString["FiltroDescripcion"];

            return View();
        }

        public ActionResult NuevoCatalogo()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoCatalogo([Bind(Include = "CatalogoPadreId,Categoria,Etiqueta,Valor,Descripcion")] Catalogo catalogo)
        {
            if (ModelState.IsValid)
            {
                _catalogoServicio.Crear(catalogo);

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
                return RedirectToAction("Catalogos");
            }
            
            return View(catalogo);
        }

        public ActionResult Catalogo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Catalogo catalogo = _catalogoServicio.Obtener(new Catalogo { CatalogoId = (int)id });
            if (catalogo == null)
            {
                return HttpNotFound();
            }

            return View(catalogo);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Catalogo([Bind(Include = "CatalogoId,CatalogoPadreId,Categoria,Etiqueta,Valor,Descripcion")] Catalogo catalogo)
        {
            if (ModelState.IsValid)
            {
                _catalogoServicio.Actualizar(catalogo);

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
                return RedirectToAction("Catalogos");
            }
            return View(catalogo);
        }
        
        public ActionResult EliminarCatalogo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Catalogo catalogo = _catalogoServicio.Obtener(new Catalogo { CatalogoId = (int)id });
            if (catalogo == null)
            {
                return HttpNotFound();
            }

            return View(catalogo);
        }

        [HttpPost, ActionName("EliminarCatalogo")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarCatalogo(int id)
        {
            _catalogoServicio.Eliminar(new Catalogo {CatalogoId = id});

            this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_RegistroEliminadoExitosamente);
            return RedirectToAction("Catalogos");
        }
        
        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerCatalogosHijos(CategoriaCatalogo categoriaCatalogo)
        {
            var temp = _catalogoServicio.ListarPorCategoria(new Catalogo {Categoria = categoriaCatalogo});
            List<SelectListItem> lista = temp.Select(item => new SelectListItem {Text = item.Etiqueta, Value = item.CatalogoId.ToString()}).ToList();
            lista.Insert(0, new SelectListItem{ Text = "Sin categoría padre", Value = "-1"});

            return new ContentResult {Content = JsonConvert.SerializeObject(lista), ContentType = "application/json"};
        }

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerCatalogos(string sidx, string sord, int page, int rows)
        {
            Catalogo filtro = null;
            CategoriaCatalogo? categoriaCatalogo = null;

            if (Request.UrlReferrer != null)
            {
                var filtroCategoria = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroCategoria"];
                int valorCategoria;
                if (int.TryParse(filtroCategoria, out valorCategoria))
                {
                    categoriaCatalogo = (CategoriaCatalogo) valorCategoria;
                }

                filtro = new Catalogo
                {
                    Etiqueta = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroEtiqueta"],
                    Descripcion = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDescripcion"]
                };
            }

            int cantidad;
            var lista = _catalogoServicio.Listar(page, rows, out cantidad, filtro, categoriaCatalogo);

            var jsonData = new
            {
                total = (int) Math.Ceiling((float) cantidad/rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.CatalogoId,
                        item.CatalogoPadreId,
                        Categoria = item.Categoria.GetDisplayName(),
                        item.Etiqueta,
                        item.Valor,
                        item.Descripcion
                    }).ToArray()
            };

            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        #endregion

        #region Registro de operaciones

        public ActionResult RegistrosOperaciones()
        {
            List<SelectListItem> categorias = Enum.GetValues(typeof(CategoriaRegistroOperacion)).Cast<CategoriaRegistroOperacion>().Select(x => new SelectListItem
            {
                Text = x.GetDisplayName(),
                Value = ((int)x).ToString()
            }).ToList();
            categorias.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });

            ViewBag.Categorias = categorias;
            ViewBag.FiltroCategoria = Request.QueryString["FiltroCategoria"];
            ViewBag.FiltroRegistroId = Request.QueryString["FiltroRegistroId"];
            ViewBag.FiltroNombreUsuario = Request.QueryString["FiltroNombreUsuario"];
            ViewBag.FiltroDescripcion = Request.QueryString["FiltroDescripcion"];
            ViewBag.FiltroFechaInicial = Request.QueryString["FiltroFechaInicial"];
            ViewBag.FiltroFechaFinal = Request.QueryString["FiltroFechaFinal"];

            return View();
        }

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerRegistrosOperaciones(string sidx, string sord, int page, int rows)
        {
            RegistroOperacion filtro = null;
            CategoriaRegistroOperacion? categoriaRegistroOperacion = null;
            DateTime? filtroFechaInicial = null;
            DateTime? filtroFechaFinal = null;
            if (Request.UrlReferrer != null)
            {
                var filtroCategoria = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroCategoria"];
                int valorCategoria;
                if (int.TryParse(filtroCategoria, out valorCategoria))
                {
                    categoriaRegistroOperacion = (CategoriaRegistroOperacion)valorCategoria;
                }

                int registroId;
                int.TryParse(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroRegistroId"], out registroId);

                filtro = new RegistroOperacion
                {
                    RegistroId = registroId,
                    NombreUsuario = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroNombreUsuario"],
                    DescripcionOperacion = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDescripcion"]
                };

                if (!string.IsNullOrEmpty(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroFechaInicial"]))
                    filtroFechaInicial = Convert.ToDateTime(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroFechaInicial"]);

                if (!string.IsNullOrEmpty(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroFechaFinal"]))
                    filtroFechaFinal = Convert.ToDateTime(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroFechaFinal"]);
            }

            int cantidad;
            var lista = _registroOperacionServicio.Listar(page, rows, out cantidad, filtro, categoriaRegistroOperacion, filtroFechaInicial, filtroFechaFinal);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Resumen =
                        $"Fecha operación: {item.FechaOperacion}<br />Categoría: {item.Categoria.GetDisplayName()}<br />Usuario: {item.NombreUsuario} ({item.Usuario})<br />Id del registro: {item.RegistroId}",
                        DescripcionOperacion = item.DescripcionOperacion.Replace("\n", "<br />")
                    }).ToArray()
            };

            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        #endregion


        #region Métodos generales

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerMunicipios(string departamentoCodigo)
        {
            List<SelectListItem> municipios = _catalogoServicio.ListarMunicipios(departamentoCodigo.Trim()).Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
            }).ToList();

            municipios.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });

            return new ContentResult { Content = JsonConvert.SerializeObject(municipios), ContentType = "application/json" };
        }

        private void CargarViewBag()
        {
            List<SelectListItem> departamentos = _catalogoServicio.ListarDepartamentos().Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
            }).ToList();

            departamentos.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.Departamentos = departamentos;
          
        }
     
        #endregion
    }
}