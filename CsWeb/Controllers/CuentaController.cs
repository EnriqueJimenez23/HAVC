using System.Web.Security;
using CsWeb.Models;
//using Recaptcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDominio.ComponentesNegocio;
using CapaDominio.EntidadesNegocio;
using Newtonsoft.Json;
using CsWeb.Infrastructure.Notification;
using CapaServicios.Servicios;
using CsWeb.Filters;

namespace CsWeb.Controllers
{
    [HandleError]
    [AllowAnonymous]
    public class CuentaController : BaseController
    {
        #region Members
        private readonly ICatalogoServicio _catalogoServicio;
        private readonly IUsuariosServicio _usuariosServicio;
        private readonly IAdminServicio _adminServicio;
        private readonly IVariableConfiguracionServicio _variableConfiguracionServicio;
        
        public CuentaController(ICatalogoServicio catalogoServicio, IUsuariosServicio usuariosServicio, IAdminServicio adminServicio,  IVariableConfiguracionServicio variableConfiguracionServicio)
        {
            _catalogoServicio = catalogoServicio;
            _usuariosServicio = usuariosServicio;
            _adminServicio = adminServicio;
            _variableConfiguracionServicio = variableConfiguracionServicio;
        }

        #endregion

        //
        // GET: /Account/
        public ActionResult IniciarSesion()
        {
            InicioSesionViewModel inicioSesionModel = new InicioSesionViewModel();

            HttpCookie httpCookie = Request.Cookies["DatosRecordarme"];
            if (httpCookie != null && httpCookie["NombreUsuario"] != null)
            {
                inicioSesionModel.NombreUsuario = httpCookie["NombreUsuario"];
                inicioSesionModel.Recordarme = true;
            }

            return View(inicioSesionModel);
        }

        //
        // POST: /Account/

        [HttpPost]
        public ActionResult IniciarSesion(InicioSesionViewModel inicioSesionModel)
        {
                string nombreCompletoOut;
                string nombrePerfilOut;
                int usuarioIdOut;

                Usuario usuario = new Usuario();
                string mensajeError;

                usuario.NombreUsuario = inicioSesionModel.NombreUsuario;
                usuario.Contrasena = inicioSesionModel.Contrasena;

              
                    bool autenticado = _usuariosServicio.AutenticarUsuario(usuario, out nombreCompletoOut, out nombrePerfilOut, out usuarioIdOut, out mensajeError);
                    var nombreCompleto = nombreCompletoOut;
                    var nombrePerfil = nombrePerfilOut;
                    var usuarioId = usuarioIdOut;
                //}

                Session["usuarioId"] = usuarioId;
                Session["nombrePerfil"] = nombrePerfil;
                Session["NombreUsuario"] = usuario.NombreUsuario;
                
                if (!autenticado)
                {
                    //ModelState.AddModelError("Contrasena", "El nombre de usuario o la contraseña introducidos no son correctos.");
                    ModelState.AddModelError("Contrasena", mensajeError);
                    return View(inicioSesionModel);
                }

                var authTicket = new FormsAuthenticationTicket(1, nombreCompleto, DateTime.Now, DateTime.Now.AddMinutes(30), true, usuarioId.ToString() + "|" + inicioSesionModel.NombreUsuario + "|" + nombrePerfil);

                string cookieContents = FormsAuthentication.Encrypt(authTicket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
                {
                    Expires = authTicket.Expiration,
                    Path = FormsAuthentication.FormsCookiePath,
                    Secure = false,
                    HttpOnly = true
                };

                Response.Cookies.Add(cookie);

                if (inicioSesionModel.Recordarme)
                {
                    HttpCookie httpCookie = new HttpCookie("DatosRecordarme");
                    httpCookie["NombreUsuario"] = inicioSesionModel.NombreUsuario;
                    httpCookie.Expires = DateTime.Now.AddDays(30d);
                    httpCookie.HttpOnly = true;
                    Response.Cookies.Add(httpCookie);
                }
                else
                {
                    if (Request.Cookies["DatosRecordarme"] != null)
                    {
                        HttpCookie httpCookie = new HttpCookie("DatosRecordarme")
                        {
                            Expires = DateTime.Now.AddDays(-1d)
                        };
                        Response.Cookies.Add(httpCookie);
                    }
                }
                return RedirectToActionPermanent("Index", "Home");
        }

        //
        // GET: /Account/Recovery

        public ActionResult RecuperarContrasena()
        {
            return View();
        }

        //
        // POST: /Account/Recovery

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Recuperarcontrasena(RecuperarContrasenaViewModel recuperarContrasenaViewModel)
        {
            string mensajeError;
            Usuario usuario = new Usuario { NombreUsuario = recuperarContrasenaViewModel.CorreoElectronico };

            if (_usuariosServicio.RecuperarContrasena(usuario, out mensajeError))
            {
                this.ShowMessage(MessageType.Success, string.Format("Contraseña enviada al correo {0}.", recuperarContrasenaViewModel.CorreoElectronico));
                return RedirectToActionPermanent("IniciarSesion");
            }

            return View();
        }

        //
        // GET: /Account/logout
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            Request.Cookies.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Paginas");
        }

        public ActionResult Registro()
        {
            CargarViewBag();
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registro(Veedor model)
        {
            if (string.IsNullOrEmpty(model.Correo))
            {
                ModelState.AddModelError("Correo", "* Obligatorio.");
            }
            else
            {
                var entidadx = _usuariosServicio.ObtenerUsuarioPorNombreUsuario(new Usuario { NombreUsuario = model.Correo });
                if (entidadx != null)
                {
                    ModelState.AddModelError("Correo", $"El correo electrónico {model.Correo} ya se encuentra registrado.");
                }
            }
            if (string.IsNullOrEmpty(model.Subregion))
            {
                ModelState.AddModelError("Subregion", "* Obligatorio.");
            }
            if (string.IsNullOrEmpty(model.CodDepartamento))
            {
                ModelState.AddModelError("CodDepartamento", "* Obligatorio.");
            }
            if (string.IsNullOrEmpty(model.CodMunicipio))
            {
                ModelState.AddModelError("CodMunicipio", "* Obligatorio.");
            }
            if (string.IsNullOrEmpty(model.Nombres))
            {
                ModelState.AddModelError("Nombres", "* Obligatorio.");
            }
            if (string.IsNullOrEmpty(model.Apellidos))
            {
                ModelState.AddModelError("Apellidos", "* Obligatorio.");
            }
            
            if (!model.Autorizo)
            {
                ModelState.AddModelError("Autorizo", "* Obligatorio para el registro.");
            }
            ModelState.Remove("VeedorId");
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario
                {
                    NombreUsuario = model.Correo,
                    Contrasena = "Riav2021.",
                    NombreCompleto = model.Nombres + " " + model.Apellidos,
                    // Perfil = "Veedor",
                    TipoIdentificacion = model.TipoIdentificacion,
                    Identificacion = model.Identificacion,
                    Telefono = model.Telefono,
                    Subregion = model.Subregion,
                    CodDepartamento = model.CodDepartamento,
                    Departamento = _catalogoServicio.ObtenerNombreDepartamento(model.CodDepartamento).Departamento,
                    CodMunicipio = model.CodMunicipio,
                    Municipio = _catalogoServicio.ObtenerNombreMunicipio(model.CodMunicipio).Municipio,
                    FechaRegistro = DateTime.Now,
                };
                string error = _usuariosServicio.CrearUsuario(usuario);

                if (model.Subregion != " Nivel central RIAV")
                {
                    Veedor veedor = new Veedor
                    {
                        Nombres = model.Nombres,
                        Apellidos = model.Apellidos,
                        TipoIdentificacion = model.TipoIdentificacion,
                        Identificacion = model.Identificacion,
                        Telefono = model.Telefono,
                        Subregion = model.Subregion,
                        CodDepartamento = model.CodDepartamento,
                        Departamento = _catalogoServicio.ObtenerNombreDepartamento(model.CodDepartamento).Departamento,
                        CodMunicipio = model.CodMunicipio,
                        Municipio = _catalogoServicio.ObtenerNombreMunicipio(model.CodMunicipio).Municipio,
                        UsuarioCreacion = usuario.NombreUsuario,
                        FechaCreado = DateTime.Now,
                    };
                    _adminServicio.CrearVeedor(veedor);
                }
            
            ViewBag.Mensaje = "Su solicitud ha sido enviada correctamente y será revisada y validada por la RIAV. En el correo electrónico registrado recibirá los datos e instrucciones de acceso. Por favor consúltelo con frecuencia. ";
                if (error != null)
                {
                    ViewBag.Mensaje += "Ocurrió un error al enviar el correo electrónico.";
                }
            }
            CargarViewBag();
            return View();
        }

        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult ObtenerDepartamentos(string subregion)
        {
        List<SelectListItem> deptos = new List<SelectListItem>();
            if (subregion != " Nivel central RIAV")
            {
                deptos = _catalogoServicio.ObtenerDepartamentos(subregion).Select(x => new SelectListItem
                {
                    Text = x.Departamento,
                    Value = x.CodDepartamento
                }).ToList();
                deptos.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            }
            else
            {
                deptos = _catalogoServicio.ObtenerDepartamentosAll().Select(x => new SelectListItem
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
                //if (subregion == " Nivel central RIAV")
                //{
                //    municipios = _catalogoServicio.ObtenerMunicipiosD(codDepartamento.Trim()).Select(x => new SelectListItem
                //    {
                //        Text = x.Municipio,
                //        Value = x.CodMunicipio
                //    }).ToList();
                //}
                //else
                //{

                municipios = _catalogoServicio.ObtenerMunicipiosDe(codDepartamento.Trim()).Select(x => new SelectListItem
                {
                    Text = x.Municipio,
                    Value = x.CodMunicipio
                }).ToList();
              municipios.Insert(0, new SelectListItem { Text = ArchivoDeRecursos.Mensaje_SeleccioneUnaOpcion, Value = "" });
            //}
            return new ContentResult { Content = JsonConvert.SerializeObject(municipios), ContentType = "application/json" };
        }

        private void CargarViewBag()
        {
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


            // Carga la lista de años

            List<SelectListItem> tiposIdentificacion = _catalogoServicio.ListarPorCategoria(new Catalogo { Categoria = CategoriaCatalogo.TiposIdentificacion }).Select(x => new SelectListItem
            {
                Text = x.Etiqueta,
                Value = x.Valor
            }).ToList();
            tiposIdentificacion.Insert(0, new SelectListItem { Text = "Tipo de identificación", Value = "" });
            ViewBag.TiposIdentificacion = tiposIdentificacion;
            
            
        }
    }
}

