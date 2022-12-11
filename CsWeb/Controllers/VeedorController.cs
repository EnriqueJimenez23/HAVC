using System;
using System.Collections.Generic;
using System.Linq;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;

namespace CsWeb.Controllers
{
    [HandleError]
    [Authorize]
    public class VeedorController : BaseController
    {
        private readonly ICatalogoServicio _catalogoServicio;
        private readonly IAdminServicio _adminServicio;
        private readonly IVariableConfiguracionServicio _variableConfiguracionServicio;
        private readonly IRegistroOperacionServicio _registroOperacionServicio;

        public VeedorController(ICatalogoServicio catalogoServicio, IAdminServicio adminServicio, IVariableConfiguracionServicio variableConfiguracionServicio, IRegistroOperacionServicio registroOperacionServicio)
        {
            _catalogoServicio = catalogoServicio;
            _adminServicio = adminServicio;
            _variableConfiguracionServicio = variableConfiguracionServicio;
            _registroOperacionServicio = registroOperacionServicio;
        }

        public ActionResult Veedores()
        {
            CargarViewBag();
            return View();
        }

        public ActionResult Veedor()
        {
            CargarViewBagV();
            Veedor veedor = new Veedor();
                veedor = _adminServicio.ObtenerVeedor(new Veedor { UsuarioCreacion =Usuario });
            return View(veedor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult veedor(Veedor veedor)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                _adminServicio.ActualizarVeedor(veedor);
            }
            CargarViewBagV();
            return View();
        }
   #region Pasos
        public ActionResult Paso1(int? id)
        {
            if (id == null)
            {
                return RedirectToActionPermanent("Index","Home");
            }
            CargarViewBag();

            Pasos pasos = new Pasos();
            if (id > 0)
            {
                pasos = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            }
            pasos.NumPaso = 1;
            return View(pasos);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paso1(Pasos pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                pasos.Usuario = UsuarioId;
                pasos.FechaCreado = DateTime.Now;
                if (pasos.PasosId == 0)
                    _adminServicio.CrearPasos(pasos);
                else
                    _adminServicio.ActualizarPasos(pasos);

                RegistroOperacion registroOperacion = new RegistroOperacion { Categoria = CategoriaRegistroOperacion.Veedor, RegistroId = pasos.PasosId, Usuario = UsuarioId, NombreUsuario = Usuario, DescripcionOperacion = "Creo/actualizó paso1", FechaOperacion = DateTime.Now };
                _registroOperacionServicio.Crear(registroOperacion);
                this.ShowMessage(MessageType.Success, "La información ha sido guardada.");
            }
            pasos.NumPaso = 1;
            return View(pasos);
        }

        public ActionResult Paso2(int? id)
        {
            if (id == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            CargarViewBag();
            Pasos pasos = new Pasos();
            if (id > 0)
            {
                pasos = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            }
            pasos.NumPaso = 2;
            return View(pasos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paso2(Pasos pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                pasos.Usuario = UsuarioId;
                _adminServicio.ActualizarPasos(pasos);
                RegistroOperacion registroOperacion = new RegistroOperacion { Categoria = CategoriaRegistroOperacion.Veedor, RegistroId = pasos.PasosId, Usuario = UsuarioId, NombreUsuario = Usuario, DescripcionOperacion = "Creo/actualizó paso2", FechaOperacion = DateTime.Now };
                _registroOperacionServicio.Crear(registroOperacion);
                this.ShowMessage(MessageType.Success, "La información ha sido guardada.");
            }
            CargarViewBag();
            pasos.NumPaso = 2;
            return View(pasos);
        }

        public ActionResult Paso3(int? id)
        {
            if (id == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            CargarViewBag();

            Pasos pasos = new Pasos();
            if (id > 0)
            {
                pasos = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            }
            pasos.NumPaso = 3;
            return View(pasos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paso3(Pasos pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");
            CargarViewBag();
            if (ModelState.IsValid)
            {
                if(!string.IsNullOrEmpty(pasos.CodDepartamentoActa))
                pasos.DepartamentoActa = _catalogoServicio.ObtenerNombreDepartamento(pasos.CodDepartamentoActa).Departamento;
                if (!string.IsNullOrEmpty(pasos.CodMunicipioActa))
                    pasos.MunicipioActa = _catalogoServicio.ObtenerNombreMunicipio(pasos.CodMunicipioActa).Municipio;
                pasos.Usuario = UsuarioId;
                _adminServicio.ActualizarPasos(pasos);
                this.ShowMessage(MessageType.Success, "La información ha sido guardada.");
            }
           
            pasos.NumPaso = 3;
            return View(pasos);
        }

        public ActionResult Paso4(int? id)
        {
            if (id == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            CargarViewBag();
            Pasos4 pasos = new Pasos4();
            pasos.PasosId = (int)id;

            Pasos pasosx = new Pasos();
            if (id > 0)
            {
                pasosx = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            }
            pasos.NumPaso = 4;
            pasos.NombreObjeto = pasosx.NombreObjeto;
            return View(pasos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paso4(Pasos4 pasos)
        {
            this.ShowMessage(MessageType.Success, "La información ha sido guardada.");
            CargarViewBag();
            pasos.NumPaso = 4;
            return View(pasos);
        }

        public ActionResult Paso5(int? id)
        {
            if (id == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            CargarViewBag();

            Pasos pasos = new Pasos();
            if (id > 0)
            {
                pasos = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            }
            pasos.NumPaso = 5;
            return View(pasos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paso5(Pasos pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                pasos.Usuario = UsuarioId;
                _adminServicio.ActualizarPasos(pasos);
                this.ShowMessage(MessageType.Success, "La información ha sido guardada.");
            }
            CargarViewBag();
            pasos.NumPaso = 5;
            return View(pasos);
        }

        public ActionResult Paso6(int? id)
        {
            if (id == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            CargarViewBag();

            Pasos6 pasos = new Pasos6();
            pasos.PasosId = (int)id;
            Pasos pasosx = new Pasos();
            if (id > 0)
            {
                pasosx = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            }
            ViewBag.ObjetoProyecto = pasosx.NombreObjeto;
            pasos.NombreObjeto = pasosx.NombreObjeto;
            pasosx.NumPaso = 6;
            return View(pasos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paso6(Pasos6 pasos)
        {
            this.ShowMessage(MessageType.Success, "La información ha sido guardada.");
            CargarViewBag();
            pasos.NumPaso = 6;
            return View(pasos);
        }

        public ActionResult Paso7(int? id)
        {
            if (id == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            CargarViewBag();

            Pasos pasos = new Pasos();
            if (id > 0)
            {
                pasos = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            }
            pasos.NumPaso = 7;
            return View(pasos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paso7(Pasos pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                pasos.Usuario = UsuarioId;
                _adminServicio.ActualizarPasos(pasos);
                this.ShowMessage(MessageType.Success, "La información ha sido guardada.");
            }
            pasos.NumPaso = 7;
            return View(pasos);
        }

        public ActionResult Paso8(int? id)
        {
            if (id == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            CargarViewBag();
            Pasos8 pasos = new Pasos8();
            pasos.PasosId = (int)id;
            Pasos pasosx = new Pasos();
            if (id > 0)
            {
                pasosx = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            }
            pasos.NombreObjeto = pasosx.NombreObjeto;
            return View(pasos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paso8(Pasos8 pasos)
        {
            this.ShowMessage(MessageType.Success, "La información ha sido guardada.");
            CargarViewBag();
            pasos.NumPaso = 8;
            return View(pasos);
        }

        public ActionResult Paso9(int? id)
        {
            if (id == null)
            {
                return RedirectToActionPermanent("Index", "Home");
            }
            CargarViewBag();
            Pasos9 pasos = new Pasos9();
            pasos.PasosId = (int)id;
            Pasos pasosx = new Pasos();
            if (id > 0)
            {
                pasosx = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            }
            pasos.NombreObjeto = pasosx.NombreObjeto;
            pasosx.NumPaso = 9;
            return View(pasos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Paso9(Pasos9 pasos)
        {
            this.ShowMessage(MessageType.Success, "La información ha sido guardada.");
            CargarViewBag();
            pasos.NumPaso = 9;
            return View(pasos);
        }

        [HttpPost]
        public ActionResult CrearPaso3(Pasos3 pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                pasos.FechaCreado = DateTime.Now;
                _adminServicio.CrearPaso3(pasos);
            }
            var jsonData = new
            {
                ok = 1
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }
        [HttpPost]
        public ActionResult CrearPaso4(Pasos4 pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(pasos.Otra))
                {
                    pasos.PasoCronograma = pasos.Otra;
                }
                  
                 pasos.FechaCreado = DateTime.Now;
                 pasos.Usuario = UsuarioId;
                _adminServicio.CrearPaso4(pasos);
            }
            var jsonData = new
            {
                ok = 1
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [HttpPost]
        public ActionResult CrearPaso5(Pasos5 pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                pasos.Usuario = UsuarioId;
                pasos.FechaCreado = DateTime.Now;
                pasos.FechaEvaluacion = DateTime.Today;
                _adminServicio.CrearPaso5(pasos);
            }
            var jsonData = new
            {
                ok = 1
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [HttpPost]
        public ActionResult CrearPaso6(Pasos6 pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                pasos.Usuario = UsuarioId;
                pasos.FechaCreado = DateTime.Now;
                _adminServicio.CrearPaso6(pasos);
            }
            var jsonData = new
            {
                ok = 1
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [HttpPost]
        public ActionResult CrearPaso8(Pasos8 pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                pasos.Usuario = UsuarioId;
                pasos.FechaCreado = DateTime.Now;
                _adminServicio.CrearPaso8(pasos);
            }
            var jsonData = new
            {
                ok = 1
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }
        [HttpPost]
        public ActionResult CrearPaso9(Pasos9 pasos)
        {
            ModelState.Remove("Usuario");
            ModelState.Remove("NombreUsuario");
            ModelState.Remove("FechaCreado");

            if (ModelState.IsValid)
            {
                pasos.Usuario = UsuarioId;
                pasos.FechaCreado = DateTime.Now;
                _adminServicio.CrearPaso9(pasos);
            }
            var jsonData = new
            {
                ok = 1
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

      
        #endregion




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
            List<SelectListItem> deptos = new List<SelectListItem>();
            deptos = _catalogoServicio.ObtenerDepartamentosAll().Select(x => new SelectListItem
            {
                Text = x.Departamento,
                Value = x.CodDepartamento
            }).ToList();

            deptos.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.Departamentos = deptos;

            List<SelectListItem> listaMunicipio = _catalogoServicio.ListarMunicipiosAll().Select(x => new SelectListItem
            {
                Text = x.Municipio,
                Value = x.CodMunicipio
            }).ToList();

            listaMunicipio.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaMunicipio = listaMunicipio;

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

            List<SelectListItem> listaCronograma = _catalogoServicio.ListarPorCategoriaOrdenId(new Catalogo { Categoria = CategoriaCatalogo.ActividadCronograma }).Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
            }).ToList();
            listaCronograma.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaCronograma = listaCronograma;
            List<SelectListItem> SiNo = new List<SelectListItem>();
            SiNo.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            SiNo.Insert(1, new SelectListItem { Text = "Si", Value = "Si" });
            SiNo.Insert(2, new SelectListItem { Text = "No", Value = "No" });
            ViewBag.listaSiNo = SiNo;

            List<SelectListItem> nivel = new List<SelectListItem>();
            nivel.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            nivel.Insert(1, new SelectListItem { Text = "Nacional", Value = "Nacional" });
            nivel.Insert(2, new SelectListItem { Text = "Departamental", Value = "Departamental" });
            nivel.Insert(3, new SelectListItem { Text = "Distrital", Value = "Distrital" });
            nivel.Insert(4, new SelectListItem { Text = "Municipal", Value = "Municipal" });
            ViewBag.listaNivel= nivel;
        }

        private void CargarViewBagV()
        {
           
            List<SelectListItem> listaSubregion = _catalogoServicio.ListarPorCategoria(new Catalogo { Categoria = CategoriaCatalogo.Subregion}).Select(x => new SelectListItem
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
            // Carga la lista de años

            List<SelectListItem> tiposIdentificacion = _catalogoServicio.ListarPorCategoria(new Catalogo { Categoria = CategoriaCatalogo.TiposIdentificacion }).Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
            }).ToList();
            tiposIdentificacion.Insert(0, new SelectListItem { Text = "Tipo de identificación", Value = "" });
            ViewBag.TiposIdentificacion = tiposIdentificacion;

        }

        private void CargarViewBagPasos()
        {
           

            List<SelectListItem> listaCronograma = _catalogoServicio.ListarPorCategoriaOrdenId(new Catalogo { Categoria = CategoriaCatalogo.ActividadCronograma }).Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
            }).ToList();
            listaCronograma.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaCronograma = listaCronograma;
            List<SelectListItem> SiNo = new List<SelectListItem>();
            SiNo.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            SiNo.Insert(1, new SelectListItem { Text = "Si", Value = "Si" });
            SiNo.Insert(2, new SelectListItem { Text = "No", Value = "No" });
            ViewBag.listaSiNo = SiNo;

            List<SelectListItem> nivel = new List<SelectListItem>();
            nivel.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            nivel.Insert(1, new SelectListItem { Text = "Nacional", Value = "Nacional" });
            nivel.Insert(2, new SelectListItem { Text = "Departamental", Value = "Departamental" });
            nivel.Insert(3, new SelectListItem { Text = "Distrital", Value = "Distrital" });
            nivel.Insert(4, new SelectListItem { Text = "Municipal", Value = "Municipal" });
            ViewBag.listaNivel = nivel;
        }

        public ActionResult ProyectosVeedor()
        {
            CargarViewBag();
            ViewBag.FiltroNombre = Request.QueryString["FiltroNombre"];
            ViewBag.FiltroEntidad = Request.QueryString["FiltroEntidad"];

            return View();
        }
        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerProyectosVeedor(string sidx, string sord, int page, int rows)
        {
            Pasos filtro = null;

            if (Request.UrlReferrer != null)
            {
                filtro = new Pasos
                {
                    NombreObjeto = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroNombre"],
                    EntidadResponsable = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroEntidad"],
                    Usuario=Usuario,
                };
            };

            int cantidad;
            var lista = _adminServicio.ListarProyectosVeedor(page, rows, out cantidad, filtro);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.PasosId,
                        item.NombreObjeto,
                        item.EntidadResponsable,
                        item.EtapaCronograma,
                        Informe=" ",
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }
        [HttpPost]
        public ActionResult ObtenerPaso3(int? id)
        {
            var lista = _adminServicio.ListarPaso3((int)id);

            var jsonData = new
            {
                total = 30,
                page = 1,
                records = 30,
                rows = (
                    from item in lista
                    select new
                    {
                        item.Pasos3Id,
                        item.Nombres,
                        item.Identificacion,
                        item.LugarResidencia,
                        item.Direccion,
                        item.Telefono,
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [HttpPost]
        public ActionResult ObtenerPaso4(int? id)
        {
            var lista = _adminServicio.ListarPaso4((int)id);

            var jsonData = new
            {
                total = 30,
                page = 1,
                records = 30,
                rows = (
                    from item in lista
                    select new
                    {
                        item.Pasos4Id,
                        item.PasoCronograma,
                        item.QueHacer,
                        item.Responsables,
                        item.Recursos,
                        Fecha=item.Fecha.HasValue
                                   ? item.Fecha.Value.ToString("dd/MM/yyyy") : string.Empty,
                        
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [HttpPost]
        public ActionResult ObtenerPaso5(int? id)
        {
            var lista = _adminServicio.ListarPaso5((int)id);
            var jsonData = new
            {
                total = 30,
                page = 1,
                records = 30,
                rows = (
                    from item in lista
                    select new
                    {
                        item.Pasos5Id,
                        item.ObjetivoProyecto,
                        item.EvaluacionCualitativa,
                        item.Cumple,
                        FechaEvaluacion = item.FechaEvaluacion.HasValue
                                   ? item.FechaEvaluacion.Value.ToString("dd/MM/yyyy") : string.Empty,
                       
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [HttpPost]
        public ActionResult ObtenerPaso6(int? id)
        {
            var lista = _adminServicio.ListarPaso6((int)id);
            var jsonData = new
            {
                total = 30,
                page = 1,
                records = 30,
                rows = (
                    from item in lista
                    select new
                    {
                        item.Pasos6Id,
                        item.InformacionRequerida,
                        item.FuenteInformacion,
                        item.MecanismoAccesoInformacion,
                        item.GrupoInformacion,
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [HttpPost]
        public ActionResult ObtenerPaso8(int? id)
        {
            var lista = _adminServicio.ListarPaso8((int)id);
            var jsonData = new
            {
                total = 30,
                page = 1,
                records = 30,
                rows = (
                    from item in lista
                    select new
                    {
                        item.Pasos8Id,
                        item.NumeroInforme,
                        item.Actividad,
                        item.PlanSeguimiento,
                        item.AccionesDespuesSeguimiento,
                        FechaProximoSeguimiento = item.FechaProximoSeguimiento.HasValue
                                   ? item.FechaProximoSeguimiento.Value.ToString("dd/MM/yyyy") : string.Empty,
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [HttpPost]
        public ActionResult ObtenerPaso9(int? id)
        {
            var lista = _adminServicio.ListarPaso9((int)id);
            var jsonData = new
            {
                total = 30,
                page = 1,
                records = 30,
                rows = (
                    from item in lista
                    select new
                    {
                        item.Pasos9Id,
                        FechaDivulgacion = item.FechaDivulgacion.HasValue
                                   ? item.FechaDivulgacion.Value.ToString("dd/MM/yyyy") : string.Empty,
                        item.MedioDivulgacion,
                        item.InformacionPresentar,
                        item.Herramientas,
                        item.AquienInvitara,
                        item.Temas,
                    }).ToArray()
            };
            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }


        public ActionResult ExportarVeedores()
        {
            CargarViewBag();
            ViewBag.FiltroRegion = Request.QueryString["FiltroRegion"];
            ViewBag.FiltroDepartamento = Request.QueryString["FiltroDepartamento"];
            ViewBag.FiltroMunicipio = Request.QueryString["FiltroMunicipio"];
            return View();
        }

        public ActionResult ExportarVeedoresExcel()
        {
            Veedor filtro = new Veedor();
            filtro = new Veedor
            {
                Subregion = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroSubRegion"],
                CodDepartamento = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDepartamento"],
                CodMunicipio = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroMunicipio"],
            };
            var lista = _adminServicio.ListarVeedorRep(filtro).ToList();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn { ColumnName = "Subregión PDET" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Departamento" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Municipio" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Nombre completo" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Tipo de identificación" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Número de identificación" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Correo electrónico" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Teléfono de contacto" });
            dataTable.Columns.Add(new DataColumn { ColumnName = "Observaciones" });

            StringBuilder stringBuilder = new StringBuilder();
            foreach (Veedor item in lista)
            {
                stringBuilder.Clear();
                var registro = _adminServicio.ObtenerVeedor(item);

                DataRow dataRow = dataTable.NewRow();
                dataRow["Subregión PDET"] = item.Subregion;
                dataRow["Departamento"] = item.Departamento;
                dataRow["Municipio"] = item.Municipio;
                dataRow["Nombre completo"] = item.Nombres + " " + item.Apellidos;
                dataRow["Tipo de identificación"] = item.TipoIdentificacion;
                dataRow["Número de identificación"] = item.Identificacion;
                dataRow["Correo electrónico"] = item.Correo;
                dataRow["Teléfono de contacto"] = item.Telefono;
                dataRow["Observaciones"] = item.Observaciones;
                dataTable.Rows.Add(dataRow);
            }

            stringBuilder.Clear();
            string nombreArchivo;
            var stream = Excel.CrearReporteGeneral("Listado Veedores", stringBuilder.ToString(), dataTable, out nombreArchivo);
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            stream.Position = 0;
            return File(stream, contentType, nombreArchivo);
        }

        public ActionResult ExP(int id, int paso)
        {
            DataTable dt= new DataTable();
            switch (paso)
            {
                case 1:
                    dt = ConexionReportes.ExPaso1(id);
                    break;
                case 2:
                    dt = ConexionReportes.ExPaso2(id);
                    break;
                case 3:
                    dt = ConexionReportes.ExPaso3(id);
                    break;
                case 4:
                    dt = ConexionReportes.ExPaso4(id);
                    break;
                case 5:
                    dt = ConexionReportes.ExPaso5(id);
                    break;
                case 6:
                    dt = ConexionReportes.ExPaso6(id);
                    break;
                case 8:
                    dt = ConexionReportes.ExPaso8(id);
                    break;
                case 9:
                    dt = ConexionReportes.ExPaso9(id);
                    break;
                default:
                    break;
            }
            if (id > 0)
            {
                string nombre = "Paso " + paso;
                string nombreArchivo;
                var stream = Excel.CrearRPasos(nombre, "", dt, out nombreArchivo);
                const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                stream.Position = 0;
                return File(stream, contentType, nombreArchivo);
            }
            return View();
        }

        public ActionResult ExP3(int id, int paso)
        {
            DataTable dtP = ConexionReportes.ExPaso3(id);
            DataTable dt3 = ConexionReportes.ExPaso3A(id);
            if (id > 0)
            {
                string nombreArchivo;
                var stream = Excel.CrearActa( dtP, dt3, out nombreArchivo);
                const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                stream.Position = 0;
                return File(stream, contentType, nombreArchivo);
            }
            return View();
        }

        public ActionResult ExP5(int id)
        {
            DataTable dtP = ConexionReportes.ExPaso5(id);
            DataTable dt5 = ConexionReportes.ExPaso5A(id);
            if (id > 0)
            {
                string nombreArchivo;
                var stream = Excel.CrearRPaso5(dtP, dt5, out nombreArchivo);
                const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                stream.Position = 0;
                return File(stream, contentType, nombreArchivo);
            }
            return View();
        }

        public ActionResult ExP7(int id, int paso)
        {
            DataTable dtA = ConexionReportes.ExPasoAll(id);
            DataTable dtP = ConexionReportes.ExPaso7(id);
            DataTable dt4 = ConexionReportes.informepaso4(id);
            DataTable dt5 = ConexionReportes.infpaso(id,5);
            DataTable dt6 = ConexionReportes.infpaso(id, 6);
            try
            {
                if (id > 0)
                {
                    string nombreArchivo;
                    var stream = Excel.CrearInforme1("Informe", dtA, dtP, dt4, dt5, dt6, out nombreArchivo);
                    const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    stream.Position = 0;
                    return File(stream, contentType, nombreArchivo);
                }
            }
            catch 
            {
               
            }
            return View();
        }

        public ActionResult Paso3Pdf(int id)
        {
            DataTable dtP = ConexionReportes.ExPaso3(id);
            DataTable dt3 = ConexionReportes.ExPaso3A(id);
            List<byte[]> xpdf = new List<byte[]>();
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.LETTER, 35, 35, 35, 35);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                document.Add(Chunk.NEWLINE);
                Paragraph pg = new Paragraph();
                pg.Add(" ACTA DE CONSTITUCIÓN DE VEEDURÍA CIUDADANA\n\n");
                pg.SpacingBefore = 30f;
                pg.SpacingAfter = 30f;
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);
                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add("NOMBRE DE LA VEEDURÍA: " );
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add("ACTA No.");
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
                pg.Add("En la ciudad de ________________ a los ____días del mes de ____________, del año _____, se reunieron en Asamblea General las personas que se relacionan al final, con la intención de constituir la veeduría ciudadana, cuyo objeto de conformación es: ");
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);
                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add(dtP.Rows[0]["ObjetoConformacion"].ToString());
                document.Add(pg);

                pg = new Paragraph();
                pg.Add("Objeto de vigilancia: " + dtP.Rows[0]["ObjetoConformacion"].ToString());
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add("Nivel territorial: " + dtP.Rows[0]["NivelTerritorial"].ToString());
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
              
                pg.SpacingAfter = 10f;
                pg.Add("El domicilio de la veeduría es en el municipio de " + dtP.Rows[0]["MunicipioActa"].ToString()+ ", departamento de " + dtP.Rows[0]["DepartamentoActa"].ToString());
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add("La duración de la veeduría es: " + dtP.Rows[0]["DuracionActa"].ToString());
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add("Para presidir o coordinar La Asamblea fue elegido(a) el(la) señor(a): " + dtP.Rows[0]["Presidir"].ToString());
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add("Como Secretario(a) fue elegido(a) el(la) señor(a): " + dtP.Rows[0]["Secretario"].ToString());
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add("Como Coordinador de la veeduría fue elegido(a) el señor(a):  " + dtP.Rows[0]["Coordinador"].ToString());
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add("El lugar de funcionamiento de la veeduría es: " + dtP.Rows[0]["LugarFuncionamiento"].ToString());
                pg.Alignment = Element.ALIGN_JUSTIFIED;
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingAfter = 15f;
                pg.Add("La presente veeduría se rige según lo establecido en la Ley 850 de 2003 y su reglamento de funcionamiento hace parte de la presente acta de conformación." );
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingAfter = 15f;
                pg.Add("Instalada la asamblea los participantes por unanimidad decidieron elegir los miembros de la veeduría. Las personas elegidas fueron las siguientes:");
                document.Add(pg);

                PdfPTable table = new PdfPTable(5);
                PdfPCell cell = new PdfPCell();
                cell = new PdfPCell(new Phrase("Nombre"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Documento de identidad"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Lugar de residencia"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Dirección"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Teléfono"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                foreach (DataRow fuente in dt3.Rows)
                {
                    cell = new PdfPCell(new Phrase(fuente["Nombres"].ToString()));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente["Identificacion"].ToString()));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente["LugarResidencia"].ToString()));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente["Direccion"].ToString()));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente["Telefono"].ToString()));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }
                    document.Add(table);
                    document.Close();
                    writer.Close();
                    Response.ContentType = "pdf/application";
                    Response.AddHeader("content-disposition",
                    "attachment;filename=Paso3.pdf");
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            }
            return View();// pdfFinal;
        }
        public ActionResult Paso7Pdf(int id)
        {
           Pasos pasos = _adminServicio.ObtenerPasos(new Pasos { PasosId = (int)id });
            IEnumerable<Pasos4> paso4 = _adminServicio.ListarPaso4((int)id);
            IEnumerable<Pasos5> paso5 = _adminServicio.ListarPaso5((int)id);
            IEnumerable<Pasos6> paso6 = _adminServicio.ListarPaso6((int)id);
            DataTable dtP = ConexionReportes.ExPaso7(id);

            List<byte[]> xpdf = new List<byte[]>();
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.LETTER, 20, 20, 20, 20);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                document.Add(Chunk.NEWLINE);
                Paragraph pg = new Paragraph();
                pg.Add(" HERRAMIENTA DE ACOMPAÑAMIENTO A VEEDURIAS CIUDADANAS\n\n");
                pg.SpacingBefore = 20f;
                pg.SpacingAfter = 20f;
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);
                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add("INFORME DE VEEDURIA: ");
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);

                PdfPTable table = new PdfPTable(4);
                PdfPCell cell = new PdfPCell();
                cell = new PdfPCell(new Phrase("Nombre del proyecto:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.NombreObjeto)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Código proyecto:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.CodigoProyecto)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Entidad responsable:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.EntidadResponsable)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Departamento:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Departamento)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Municipio:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Municipio)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("Pilar:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(dtP.Rows[0]["Pilar"].ToString())); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Estado del proyecto:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(dtP.Rows[0]["EstadoProyecto"].ToString())); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                document.Add(table);

                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                pg.Add(" ");
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);

                table = new PdfPTable(4);
                cell = new PdfPCell(new Phrase("Objeto de veeduría:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.ObjetoVeeduria)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Introducción:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Introduccion)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("MetodologÍa:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Metodologia)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Resultados:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.ResultadosInf)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Recomendaciones:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Recomendaciones)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                document.Add(table);

              
                pg = new Paragraph();
                pg.SpacingBefore = 20f;
                pg.SpacingAfter = 20f;
                pg.Add("INFORMACIÓN DILIGENCIADA EN LOS PASOS DE CONTROL SOCIAL: ");
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);
              
                pg = new Paragraph();
                pg.SpacingAfter = 20f;
                pg.Add("PASO 1. Identificación del objeto de control social ");
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);
                table = new PdfPTable(4);
                cell = new PdfPCell();
                cell = new PdfPCell(new Phrase("Problema:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Problema)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Territorio:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Territorio)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Población:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Poblacion)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Qué es lo que se va a vigilar:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.ObjetoVigilar)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Cuál es el objeto de vigilancia:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.ObjetoVigilancia)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);


                cell = new PdfPCell(new Phrase("Cuál es la Entidad responsable del objeto de vigilancia:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.EntidadResponsable)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Describa Objeto de la veeduría:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.ObjetoVeeduria)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                document.Add(table);

                pg = new Paragraph();
                pg.SpacingBefore = 20f;
                pg.SpacingAfter = 20f;
                pg.Add("PASO 2. Convocar a la comunidad ");
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);
                table = new PdfPTable(4);
                cell = new PdfPCell();
                cell = new PdfPCell(new Phrase("A quiénes haría la invitación:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Invitacion)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Cómo motivaría la participación:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.ComoMotivar)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Qué canales va a utilizar para la convocatoria:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.CanalConvocatoria)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Cuál es el mensaje a partir del cual realizaría la convocatoria: "));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.MensajeConvocatoria)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("En qué fechas se va a realizar la convocatoria: "));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.FechasConvocatoria)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                document.Add(table);
                pg = new Paragraph();
                pg.SpacingAfter = 10f;
                document.Add(pg);

                pg = new Paragraph();
                pg.SpacingBefore = 20f;
                pg.SpacingAfter = 20f;
                pg.Add("PASO 4. Formulación del plan de trabajo ");
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);

                table = new PdfPTable(5);
                cell = new PdfPCell(new Phrase("Actividad"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Acciones"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Responsable"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Recursos"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Fecha"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                foreach (Pasos4 fuente in paso4)
                {
                    cell = new PdfPCell(new Phrase(fuente.PasoCronograma));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente.QueHacer));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente.Responsables));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente.Recursos));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(Convert.ToDateTime(fuente.Fecha).ToShortDateString()));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }
                document.Add(table);

                pg = new Paragraph();
                pg.SpacingBefore = 20f;
                pg.SpacingAfter = 20f;
                pg.Add("PASO 5. Establecer criterios de Evaluación ");
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);
                table = new PdfPTable(4);
                cell = new PdfPCell(new Phrase("Impactos:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Impactos)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
             
                cell = new PdfPCell(new Phrase("Resultados:"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Resultados)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Productor: "));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Productos)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Actividades: "));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Actividades)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Insumos: "));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(pasos.Insumos)); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                table = new PdfPTable(3);
                cell = new PdfPCell(new Phrase("Establecer criterios de Evaluación:")); cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Objetivo"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Evaluación cualitativa"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Cumple"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                document.Add(table);
                foreach (Pasos5 fuente in paso5)
                {
                    cell = new PdfPCell(new Phrase(fuente.ObjetivoProyecto));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente.EvaluacionCualitativa));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente.Cumple));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }
                document.Add(table);

                pg = new Paragraph();
                pg.SpacingAfter = 20f;
                pg.SpacingBefore = 20f;
                pg.Add("PASO 6. Recoger y analizar la información obtenida ");
                pg.Alignment = Element.ALIGN_CENTER;
                document.Add(pg);

                table = new PdfPTable(4);
                cell = new PdfPCell(new Phrase("Grupo de información"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Identificación de la información requerida"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Fuente de donde tomará la información"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Mecanismo a utilizar para acceder a la información"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                foreach (Pasos6 fuente in paso6)
                {
                    cell = new PdfPCell(new Phrase(fuente.GrupoInformacion));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente.InformacionRequerida));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);

                    cell = new PdfPCell(new Phrase(fuente.FuenteInformacion));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                    cell = new PdfPCell(new Phrase(fuente.MecanismoAccesoInformacion));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }
                document.Add(table);
                document.Close();
                writer.Close();
                Response.ContentType = "pdf/application";
                Response.AddHeader("content-disposition",
                "attachment;filename=Paso7.pdf");
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            }
            return View();// pdfFinal;
        }
        #endregion

        #region Contenido Sitio
        public ActionResult Documentos()
        {
            return RedirectToActionPermanent("ContenidosSitio", new { Seccion = "Documentos" });
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
        #endregion

       
    }
}