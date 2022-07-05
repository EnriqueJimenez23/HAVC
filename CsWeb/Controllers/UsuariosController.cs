
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CapaDominio.ComponentesNegocio;
using CapaDominio.EntidadesNegocio;
using CapaServicios.Servicios;
using Newtonsoft.Json;
using CsWeb.Filters;
using CsWeb.Infrastructure.Notification;
using CsWeb.Models;

namespace CsWeb.Controllers
{
    [HandleError]
    [Authorize]
    public class UsuariosController : BaseController
    {
        private readonly ICatalogoServicio _catalogoServicio;
        private readonly IUsuariosServicio _usuariosServicio;
        private readonly IRegistroOperacionServicio _registroOperacionServicio;

        public UsuariosController(ICatalogoServicio catalogoServicio, IUsuariosServicio usuariosServicio, IRegistroOperacionServicio registroOperacionServicio)
        {
            _catalogoServicio = catalogoServicio;
            _usuariosServicio = usuariosServicio;
            _registroOperacionServicio = registroOperacionServicio;
        }

        #region Perfiles

        public ActionResult Perfiles()
        {
            ViewBag.FiltroNombrePerfil = Request.QueryString["FiltroNombrePerfil"];
            return View();
        }

        public ActionResult NuevoPerfil()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoPerfil([Bind(Include = "NombrePerfil,Activo,Descripcion")] Perfil perfil)
        {
            perfil.Activo = false;
            var entidad = _usuariosServicio.ObtenerPerfil(new Perfil { NombrePerfil = perfil.NombrePerfil.Replace(" ", "") });
            if (entidad != null)
            {
                ModelState.AddModelError("NombrePerfil", string.Format("El perfil de usuario {0} ya se encuentra registrado.", perfil.NombrePerfil));
            }

            if (ModelState.IsValid)
            {
                _usuariosServicio.CrearPerfil(perfil);

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
                return RedirectToAction("Perfiles");
            }

            return View(perfil);
        }
        
        public ActionResult Perfil(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Perfil perfil = _usuariosServicio.ObtenerPerfil(new Perfil { NombrePerfil = id });
            if (perfil == null)
            {
                return HttpNotFound();
            }

            return View(perfil);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Perfil([Bind(Include = "NombrePerfil,Activo,Descripcion")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                _usuariosServicio.ActualizarPerfil(perfil);

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
                return RedirectToAction("Perfiles");
            }

            return View(perfil);
        }

        public ActionResult EliminarPerfil(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Perfil perfil = _usuariosServicio.ObtenerPerfil(new Perfil { NombrePerfil = id });
            if (perfil == null)
            {
                return HttpNotFound();
            }

            return View(perfil);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarPerfil([Bind(Include = "NombrePerfil")] Perfil perfil)
        {
            if (ModelState.IsValid)
            {
                _usuariosServicio.EliminarPerfil(perfil);

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_RegistroEliminadoExitosamente);
                return RedirectToAction("Perfiles");
            }

            return View(perfil);
        }

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerPerfiles(string sidx, string sord, int page, int rows)
        {
            Perfil filtro = null;
            if (Request.UrlReferrer != null)
            {
                filtro = new Perfil
                {
                    NombrePerfil = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroNombrePerfil"],
                };
            }

            int cantidad;
            var lista = _usuariosServicio.ListarPerfiles(page, rows, out cantidad, filtro);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.NombrePerfil,
                        item.Descripcion,
                        Activo = item.Activo ? "Sí" : "No"
                    }).ToArray()
            };

            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        #endregion

        #region Usuarios

        public ActionResult Usuarios()
        {
            // Carga la lista de perfiles

            List<SelectListItem> perfiles = _usuariosServicio.ListarPerfiles().Select(x => new SelectListItem
            {
                Text = x.NombrePerfil,
                Value = x.NombrePerfil
            }).ToList();

            perfiles.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            
            ViewBag.Perfiles = perfiles;
            ViewBag.FiltroNombreUsuario = Request.QueryString["FiltroNombreUsuario"];
            ViewBag.FiltroNombreCompleto = Request.QueryString["FiltroNombreCompleto"];
            ViewBag.FiltroPerfil = Request.QueryString["FiltroPerfil"];

            return View();
        }
        public ActionResult ExportarUsuarios()
        {
            // Carga la lista de perfiles

            List<SelectListItem> perfiles = _usuariosServicio.ListarPerfiles().Select(x => new SelectListItem
            {
                Text = x.NombrePerfil,
                Value = x.NombrePerfil
            }).ToList();

            perfiles.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });

            ViewBag.Perfiles = perfiles;
            ViewBag.FiltroRegion = Request.QueryString["FiltroRegion"];
            ViewBag.FiltroDepartamento = Request.QueryString["FiltroDepartamento"];
            ViewBag.FiltroMunicipio = Request.QueryString["FiltroMunicipio"];
            ViewBag.FiltroPerfil = Request.QueryString["FiltroPerfil"];
            CargarViewBag();
            return View();
        }
     
        [Authorize]
        public ActionResult NuevoUsuario()
        {
            CargarViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoUsuario(Usuario usuario)
        {
            var entidad = _usuariosServicio.ObtenerUsuarioPorNombreUsuario(usuario);
            if (entidad != null)
            {
                ModelState.AddModelError("NombreUsuario", string.Format("El usuario {0} ya se encuentra registrado.", usuario.NombreUsuario));
            }

            ModelState.Remove("Departamento");
            ModelState.Remove("Municipio");
            ModelState.Remove("FechaRegistro");
            ModelState.Remove("FechaUltimoAcceso");
            usuario.Departamento = _catalogoServicio.ObtenerNombreDepartamento(usuario.CodDepartamento).Departamento;
            usuario.Municipio = _catalogoServicio.ObtenerNombreMunicipio(usuario.CodMunicipio).Municipio;
            if (ModelState.IsValid)
            {
                _usuariosServicio.CrearUsuario(usuario);
                RegistrarOperacion(usuario.UsuarioId, "creó", usuario.ToString());
                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
                return RedirectToAction("Usuarios");
            }

            CargarViewBag();
            return View(usuario);
        }

        public ActionResult Usuario(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Usuario usuario = _usuariosServicio.ObtenerUsuario(new Usuario { UsuarioId = (int) id });
            if (usuario == null)
            {
                return HttpNotFound();
            }

            CargarViewBag();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usuario(Usuario usuario)
        {
            var entidad = _usuariosServicio.ObtenerUsuarioPorNombreUsuario(usuario);
            if (entidad != null && entidad.UsuarioId != usuario.UsuarioId)
            {
                ModelState.AddModelError("NombreUsuario", string.Format("El usuario {0} ya se encuentra registrado.", usuario.NombreUsuario));
            }

            ModelState.Remove("Contrasena");
            ModelState.Remove("FechaRegistro");
            ModelState.Remove("FechaUltimoAcceso");
            usuario.Departamento = _catalogoServicio.ObtenerNombreDepartamento(usuario.CodDepartamento).Departamento;
            usuario.Municipio = _catalogoServicio.ObtenerNombreMunicipio(usuario.CodMunicipio).Municipio;
            if (ModelState.IsValid)
            {
                _usuariosServicio.ActualizarUsuario(usuario);

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
                return RedirectToAction("Usuarios");
            }

            CargarViewBag();
            return View(usuario);
        }


        public ActionResult Usuarioa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Usuario usuario = _usuariosServicio.ObtenerUsuario(new Usuario { UsuarioId = (int)id });
            if (usuario == null)
            {
                return HttpNotFound();
            }

            CargarViewBag();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Usuarioa(Usuario usuario)
        {
            var entidad = _usuariosServicio.ObtenerUsuarioPorNombreUsuario(usuario);
            if (entidad != null && entidad.UsuarioId != usuario.UsuarioId)
            {
                ModelState.AddModelError("NombreUsuario", string.Format("El usuario {0} ya se encuentra registrado.", usuario.NombreUsuario));
            }

            ModelState.Remove("Contrasena");
            ModelState.Remove("FechaRegistro");
            ModelState.Remove("FechaUltimoAcceso");

            if (ModelState.IsValid)
            {
                _usuariosServicio.ActualizarUsuario(usuario);

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
                return RedirectToAction("Usuarios");
            }

            CargarViewBag();
            return View(usuario);
        }



        public ActionResult CambiarContrasena(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             if (id==0)
                id = Convert.ToInt32(UsuarioId);
            
            Usuario usuario = _usuariosServicio.ObtenerUsuario(new Usuario { UsuarioId = (int)id });
            
            if (usuario == null)
            {
                return HttpNotFound();
            }

            return View(new CambiarContrasenaViewModel {UsuarioId = (int) id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarContrasena(CambiarContrasenaViewModel cambiarContrasenaViewModel)
        {
            if (ModelState.IsValid)
            {
                _usuariosServicio.ActualizarContrasena(new Usuario { UsuarioId = cambiarContrasenaViewModel.UsuarioId, Contrasena = cambiarContrasenaViewModel.NuevaContrasena });

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
              //  return RedirectToAction("Usuarios");
            }

          //  return View(cambiarContrasenaViewModel);
            return View();
        }

   

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerUsuarios(string sidx, string sord, int page, int rows)
        {
            Usuario filtro = null;
            if (Request.UrlReferrer != null)
            {
                //filtro = new Usuario
                //{
                //    Subregion = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroRegion"],
                //    CodDepartamento = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDepartamento"],
                //    CodMunicipio = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroMunicipio"],
                //    Perfil = new Perfil { NombrePerfil = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroPerfil"] }
                //};
            }

            int cantidad;
            var lista = _usuariosServicio.ListarUsuarios(page, rows, out cantidad, filtro);

            var jsonData = new
            {
                total = (int)Math.Ceiling((float)cantidad / rows),
                page,
                records = cantidad,
                rows = (
                    from item in lista
                    select new
                    {
                        Id = item.UsuarioId,
                        item.Subregion,
                        item.Departamento,
                        item.Municipio,
                        item.NombreUsuario,
                        item.NombreCompleto,
                       // item.Perfil.NombrePerfil,
                        Activo = item.Activo ? "Sí" : "No",
                    }).ToArray()
            };

            return new ContentResult { Content = JsonConvert.SerializeObject(jsonData), ContentType = "application/json" };
        }

        [ExcluirAutorizacion]
        public ActionResult ExportarUsuariosExcel()
        {
            Usuario filtro = null;
            if (Request.UrlReferrer != null)
            {
                filtro = new Usuario
                {
                    Subregion = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroRegion"],
                    CodDepartamento = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroDepartamento"],
                    CodMunicipio = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroMunicipio"],
                    Perfil = new Perfil { NombrePerfil = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["FiltroPerfil"] }
                };
            }

            DataTable dataTable = _usuariosServicio.ExportarUsuarios(filtro);

            StringBuilder stringBuilder = new StringBuilder();
            if (filtro != null && !string.IsNullOrEmpty(filtro.NombreUsuario))
                stringBuilder.Append(string.Format("Nombre usuario: {0} ", filtro.NombreUsuario));
            if (filtro != null && !string.IsNullOrEmpty(filtro.NombreCompleto))
                stringBuilder.Append(string.Format("Nombre completo: {0} ", filtro.NombreCompleto));
            if (filtro != null && filtro.Perfil != null && !string.IsNullOrEmpty(filtro.Perfil.NombrePerfil))
                stringBuilder.Append(string.Format("Perfil: {0} ", filtro.Perfil.NombrePerfil));

            string nombreArchivo;
            var stream = Excel.CrearReporteGeneral("Reporte usuarios", stringBuilder.ToString(), dataTable, out nombreArchivo);
            const string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            stream.Position = 0;
            return File(stream, contentType, nombreArchivo);
        }
        
        #endregion

        #region Permisos

        public ActionResult Permisos(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Perfil perfil = _usuariosServicio.ObtenerPerfil(new Perfil { NombrePerfil = id });
            if (perfil == null)
            {
                return HttpNotFound();
            }

            ViewBag.Permisos = _usuariosServicio.ListarPermisos();
            return View(perfil);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Permisos(Perfil perfil, IEnumerable<string> acciones)
        {
            perfil.Permisos = new List<Permiso>();

            if (acciones == null)
                acciones = new List<string>(); 

            foreach (var item in acciones)
            {
                perfil.Permisos.Add(new Permiso{ PermisoId = Convert.ToInt32(item)});
            }

            if (ModelState.IsValid)
            {
                _usuariosServicio.ActualizarPermisos(perfil);

                this.ShowMessage(MessageType.Success, ArchivoDeRecursos.Mensaje_CambiosGuardadosExitosamente);
                return RedirectToAction("Perfiles");
            }

            ViewBag.Permisos = _usuariosServicio.ListarPermisos();
            return View(perfil);
        }

        #endregion

        #region Método generales

        private void CargarViewBag()
        {
            // Carga la lista de perfiles

            List<SelectListItem> perfiles = _usuariosServicio.ListarPerfiles().Select(x => new SelectListItem
            {
                Text = x.NombrePerfil,
                Value = x.NombrePerfil
            }).ToList();

            perfiles.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.Perfiles = perfiles;


            List<SelectListItem> listaSubregion = _catalogoServicio.ListarPorCategoria(new Catalogo { Categoria = CategoriaCatalogo.Subregion }).Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
            }).ToList();

            listaSubregion.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaSubregion = listaSubregion;



            List<SelectListItem> listaDeptos = new List<SelectListItem>();
            listaDeptos = _catalogoServicio.ObtenerDepartamentosAll().Select(x => new SelectListItem
            {
                Text = x.Departamento,
                Value = x.CodDepartamento
            }).ToList();

            listaDeptos.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaDepartamentos = listaDeptos;

            List<SelectListItem> listaMunicipio = _catalogoServicio.ListarMunicipiosAll().Select(x => new SelectListItem
            {
                Text = x.Municipio,
                Value = x.CodMunicipio
            }).ToList();

            listaMunicipio.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            ViewBag.ListaMunicipio = listaMunicipio;

            List<SelectListItem> tiposIdentificacion = _catalogoServicio.ListarPorCategoria(new Catalogo { Categoria = CategoriaCatalogo.TiposIdentificacion }).Select(x => new SelectListItem
                {
                    Text = x.Etiqueta,
                    Value = x.Valor
                }).ToList();
                //   tiposIdentificacion.RemoveAt(2);
                tiposIdentificacion.Insert(0, new SelectListItem { Text = "Tipo de identificación", Value = "" });
                ViewBag.TiposIdentificacion = tiposIdentificacion;

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
        public ActionResult ObtenerMunicipios1(string codDepartamento, string subregion)
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
        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerMunicipios(string codDepartamento, string subregion)
        {
            List<SelectListItem> municipios = new List<SelectListItem>();
            municipios = _catalogoServicio.ObtenerMunicipiosDe(codDepartamento.Trim()).Select(x => new SelectListItem
            {
                Text = x.Municipio,
                Value = x.CodMunicipio
            }).ToList();
            municipios.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            return new ContentResult { Content = JsonConvert.SerializeObject(municipios), ContentType = "application/json" };
        }
        #endregion
        public void RegistrarOperacion(int registroId, string accion, string informacion)
        {
            string xtemp = string.Format("El usuario {0} ({1}) {2} el {3} usuario:{4}",
                                  UsuarioId, NombreCompletoUsuario, accion, DateTime.Now, informacion);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(xtemp);
            RegistroOperacion registroOperacion = new RegistroOperacion
            {
                Categoria = CategoriaRegistroOperacion.Usuario,
                RegistroId = registroId,
                Usuario = UsuarioId,
                NombreUsuario = NombreCompletoUsuario,
                DescripcionOperacion = stringBuilder.ToString(),
                FechaOperacion = DateTime.Now
            };
            _registroOperacionServicio.Crear(registroOperacion);
        }
    }
}
