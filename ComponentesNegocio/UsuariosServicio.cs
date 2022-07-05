using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDatos.Repositorio.Repositories;
using CapaDatos.Repositorio.UnitOfWork;
using CapaDominio.ComponentesNegocio.Filtros;
using CapaDominio.EntidadesNegocio;
using CapaServicios.Servicios;
using Elmah;

namespace CapaDominio.ComponentesNegocio
{
    public interface IUsuariosServicio
    {
        Perfil ObtenerPerfil(Perfil filtro);
        IEnumerable<Perfil> ListarPerfiles(Perfil filtro = null);
        IEnumerable<Perfil> ListarPerfiles(int pagina, int tamanoPagina, out int totalRegistros, Perfil filtro = null);
        void CrearPerfil(Perfil perfil);
        void ActualizarPerfil(Perfil perfil);
        void EliminarPerfil(Perfil perfil);

        Usuario ObtenerUsuario(Usuario filtro);
        Usuario ObtenerUsuarioPorNombreUsuario(Usuario filtro);
        IEnumerable<Usuario> ListarUsuarios(int pagina, int tamanoPagina, out int totalRegistros, Usuario filtro = null);
        IEnumerable<Usuario> ListarUsuarios(Usuario filtro = null);
        DataTable ExportarUsuarios(Usuario filtro);
        string CrearUsuario(Usuario usuario);
        void ActualizarUsuario(Usuario usuario);
        void ActualizarContrasena(Usuario usuario);

        IEnumerable<Permiso> ListarPermisos();
        void ActualizarPermisos(Perfil perfil);

        bool AutenticarUsuario(Usuario usuario, out string nombreCompleto, out string nombrePerfil, out int usuarioId,
            out string mensajeError);

        bool RecuperarContrasena(Usuario usuario, out string mensajeError);
        List<Menu> ObtenerMenu(string id, string urlActual);
        bool AccesoPermitido(string id, string controlador, string accion);
    }

    public class UsuariosServicio : IUsuariosServicio
    {
        private readonly IRepositoryAsync<Perfil> _repositorioPerfil;
        private readonly IRepositoryAsync<Usuario> _repositorioUsuario;
        private readonly IRepositoryAsync<Permiso> _repositorioPermiso;
        private readonly IVariableConfiguracionServicio _variableConfiguracionServicio;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly UrlHelper _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

        #region Constructor

        public UsuariosServicio(IRepositoryAsync<Perfil> repositorioPerfil, IRepositoryAsync<Usuario> repositorioUsuario,
            IRepositoryAsync<Permiso> repositorioPermiso, IUnitOfWorkAsync unitOfWork,
            IVariableConfiguracionServicio variableConfiguracionServicio)
        {
            _repositorioPerfil = repositorioPerfil;
            _repositorioUsuario = repositorioUsuario;
            _repositorioPermiso = repositorioPermiso;
            _variableConfiguracionServicio = variableConfiguracionServicio;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Metodos

        #region Perfiles

        public Perfil ObtenerPerfil(Perfil filtro)
        {
            return _repositorioPerfil.Query(x => x.NombrePerfil == filtro.NombrePerfil)
                .Include(x => x.Permisos)
                .Select()
                .FirstOrDefault();
        }

        public IEnumerable<Perfil> ListarPerfiles(Perfil filtro = null)
        {
            if (filtro == null)
                return _repositorioPerfil.Query()
                    .Include(x => x.Permisos)
                    .OrderBy(x => x.OrderBy(y => y.NombrePerfil))
                    .Select()
                    .ToList();

            var perfilFiltro = new PerfilFiltro().FiltrarPorNombre(filtro.NombrePerfil);

            return _repositorioPerfil.Query(perfilFiltro)
                .Include(x => x.Permisos)
                .OrderBy(x => x.OrderBy(y => y.NombrePerfil))
                .Select()
                .ToList();
        }

        public IEnumerable<Perfil> ListarPerfiles(int pagina, int tamanoPagina, out int totalRegistros,
            Perfil filtro = null)
        {
            if (filtro == null)
                return _repositorioPerfil.Query()
                    .Include(x => x.Permisos)
                    .OrderBy(x => x.OrderBy(y => y.NombrePerfil))
                    .SelectPage(pagina, tamanoPagina, out totalRegistros)
                    .ToList();

            var perfilFiltro = new PerfilFiltro().FiltrarPorNombre(filtro.NombrePerfil);

            return _repositorioPerfil.Query(perfilFiltro)
                .Include(x => x.Permisos)
                .OrderBy(x => x.OrderBy(y => y.NombrePerfil))
                .SelectPage(pagina, tamanoPagina, out totalRegistros)
                .ToList();
        }

        public void CrearPerfil(Perfil perfil)
        {
            perfil.NombrePerfil = perfil.NombrePerfil.Replace(" ", "");

            // Valida que no exista otro perfil con el mismo nombre

            var entidad = ObtenerPerfil(perfil);
            if (entidad != null)
            {
                throw new Exception(string.Format("El perfil de usuario {0} ya se encuentra registrado.",
                    perfil.NombrePerfil));
            }

            perfil.Activo = true;

            _repositorioPerfil.Insert(perfil);
            _unitOfWork.SaveChanges();
        }

        public void ActualizarPerfil(Perfil perfil)
        {
            var entidad = ObtenerPerfil(perfil);

            entidad.Activo = perfil.Activo;
            entidad.Descripcion = perfil.Descripcion;

            _repositorioPerfil.Update(entidad);
            _unitOfWork.SaveChanges();
        }

        public void EliminarPerfil(Perfil perfil)
        {
            // Valida que no existan cuentas de usuarios asociadas al perfil

            var usuarios =
                _repositorioUsuario.Query(x => x.Perfil.NombrePerfil == perfil.NombrePerfil)
                    .Include(x => x.Perfil)
                    .Select()
                    .ToList();

            if (usuarios != null && usuarios.Any())
            {
                throw new Exception(
                    string.Format(
                        "El perfil de usuario {0} no puede ser eliminado, actualmente existen cuentas de usuario asociadas.",
                        perfil.NombrePerfil));
            }

            var entidad = _repositorioPerfil.Query(x => x.NombrePerfil == perfil.NombrePerfil)
                .Include(x => x.Permisos)
                .Select()
                .FirstOrDefault();

            if (entidad != null)
            {
                entidad.Permisos.Clear();
                _repositorioPerfil.Delete(entidad);

                _unitOfWork.SaveChanges();
            }
        }

        #endregion

        #region Usuarios

        public Usuario ObtenerUsuario(Usuario filtro)
        {
            return _repositorioUsuario.GetSingle(x => x.UsuarioId == filtro.UsuarioId);
        }

        public Usuario ObtenerUsuarioPorNombreUsuario(Usuario filtro)
        {
            return _repositorioUsuario.Query(x => x.NombreUsuario == filtro.NombreUsuario).Include(y => y.Perfil).Select().FirstOrDefault();
        }

        public IEnumerable<Usuario> ListarUsuarios(int pagina, int tamanoPagina, out int totalRegistros,
            Usuario filtro = null)
        {
            if (filtro == null)
                return
                    _repositorioUsuario.Query()
                        .OrderBy(x => x.OrderBy(y => y.NombreCompleto))
                        .Include(y => y.Perfil)
                        .SelectPage(pagina, tamanoPagina, out totalRegistros)
                        .ToList();

            var usuarioFiltro = new UsuarioFiltro()
                .FiltrarPorSubRegion(filtro.Subregion)
                .FiltrarPorDepartamento(filtro.Departamento)
                .FiltrarPorMunicipio(filtro.Municipio)
                .FiltrarPorNombre(filtro.NombreCompleto)
                .FiltrarPorPerfil(filtro.Perfil.NombrePerfil);

            return
                _repositorioUsuario.Query(usuarioFiltro)
                    .OrderBy(x => x.OrderBy(y => y.NombreCompleto))
                    .Include(y => y.Perfil)
                    .SelectPage(pagina, tamanoPagina, out totalRegistros)
                    .ToList();
        }

        public IEnumerable<Usuario> ListarUsuarios(Usuario filtro = null)
        {
            if (filtro == null)
                return
                    _repositorioUsuario.Query()
                        .OrderBy(x => x.OrderBy(y => y.NombreCompleto))
                        .Include(y => y.Perfil)
                        .Select()
                        .ToList();

            var usuarioFiltro = new UsuarioFiltro()
                .FiltrarPorSubRegion(filtro.Subregion)
                .FiltrarPorDepartamento(filtro.Departamento)
                .FiltrarPorMunicipio(filtro.Municipio)
                .FiltrarPorNombreUsuario(filtro.NombreUsuario)
                .FiltrarPorNombre(filtro.NombreCompleto)
                .FiltrarPorPerfil(filtro.Perfil.NombrePerfil);

            return
                _repositorioUsuario.Query(usuarioFiltro)
                    .OrderBy(x => x.OrderBy(y => y.NombreCompleto))
                    .Include(y => y.Perfil)
                    .Select()
                    .ToList();
        }

       

        public DataTable ExportarUsuarios(Usuario filtro)
        {
            var lista = ListarUsuarios(filtro);

            DataTable dataTable = Mapeo.CrearDataTable<Usuario>(null, new[] {"Contrasena", "Perfil"});
            Mapeo.CrearDataTable<Perfil>(dataTable);

            foreach (Usuario usuario in lista)
            {
                DataRow dataRow = dataTable.NewRow();
                dataRow[Mapeo.ObtenerNombreColumna<Usuario>(x => x.Subregion)] = usuario.Subregion;
                dataRow[Mapeo.ObtenerNombreColumna<Usuario>(x => x.Departamento)] = usuario.Departamento;
                dataRow[Mapeo.ObtenerNombreColumna<Usuario>(x => x.Municipio)] = usuario.Municipio;
                dataRow[Mapeo.ObtenerNombreColumna<Usuario>(x => x.UsuarioId)] = usuario.UsuarioId;
                dataRow[Mapeo.ObtenerNombreColumna<Usuario>(x => x.NombreUsuario)] = usuario.NombreUsuario;
                dataRow[Mapeo.ObtenerNombreColumna<Usuario>(x => x.Activo)] = usuario.Activo;
                dataRow[Mapeo.ObtenerNombreColumna<Usuario>(x => x.NombreCompleto)] = usuario.NombreCompleto;
                dataRow[Mapeo.ObtenerNombreColumna<Usuario>(x => x.FechaRegistro)] =
                    usuario.FechaRegistro.ToString(new CultureInfo("es-CO"));
                if (usuario.FechaUltimoAcceso != null)
                    dataRow[Mapeo.ObtenerNombreColumna<Usuario>(x => x.FechaUltimoAcceso)] =
                        usuario.FechaUltimoAcceso.Value.ToString(new CultureInfo("es-CO"));

                if (usuario.Perfil != null)
                {
                    dataRow[Mapeo.ObtenerNombreColumna<Perfil>(x => x.NombrePerfil)] = usuario.Perfil.NombrePerfil;
                }

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        public string CrearUsuario(Usuario usuario)
        {
            // Valida que no exista otro usuario con el mismo nombre de usuario

            //   var perfilx = usuario.Perfil.NombrePerfil;
            Perfil perfil = new Perfil();
            var entidad = _repositorioUsuario.GetSingle(x => x.NombreUsuario == usuario.NombreUsuario);
            if (entidad != null)
            {
                throw new Exception(
                    string.Format("El correo electrónico {0} ya se encuentra registrado por otro usuario.",
                        usuario.NombreUsuario));
            }

            var contrasena = usuario.Contrasena;
            usuario.Contrasena = Seguridad.Encriptar(usuario.Contrasena);
            usuario.Activo = true;
            usuario.FechaRegistro = DateTime.Now;
            usuario.FechaUltimoAcceso = usuario.FechaRegistro;
            perfil.NombrePerfil = usuario.Subregion != " Nivel central RIAV"?"Veedor": "AdminSitio";
            usuario.Perfil = perfil;

            _repositorioUsuario.Insert(usuario);
            _unitOfWork.SaveChanges();

            var mensajeAsunto = "USUARIO CREADO";

            var temp = "Estimado / a {0}, Su usuario ha sido creado. A continuación le enviamos sus credenciales de usuario.  Usuario: {1} Contraseña: {2}  Cordialmente,   Herramienta de apoyo a veedurias ciudadanas";

            var mensajeCuerpo = string.Format(temp, usuario.NombreCompleto, usuario.NombreUsuario, contrasena);

            Dictionary<string, string> configuracion = _variableConfiguracionServicio.ObtenerConfiguracionCorreoElectronico();

            string error = null;
            try
            {
                CorreoElectronico.Enviar(configuracion, usuario.NombreUsuario, mensajeAsunto, mensajeCuerpo);
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            return error;
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            // Valida que no exista otro usuario con el mismo nombre de usuario

            var entidad = _repositorioUsuario.GetSingle(x => x.NombreUsuario == usuario.NombreUsuario && x.UsuarioId != usuario.UsuarioId);
            if (entidad != null)
            {
                throw new Exception($"El correo electrónico {usuario.NombreUsuario} ya se encuentra registrado por otro usuario.");
            }

            entidad = ObtenerUsuario(usuario);
            entidad.Activo = usuario.Activo;
            entidad.Perfil = ObtenerPerfil(usuario.Perfil);
            entidad.NombreCompleto = usuario.NombreCompleto;
            entidad.TipoIdentificacion = usuario.TipoIdentificacion;
            entidad.Identificacion = usuario.Identificacion;
            entidad.Telefono = usuario.Telefono;
            entidad.NombreUsuario= usuario.NombreUsuario;
            entidad.Subregion = usuario.Subregion;
            entidad.Departamento = usuario.Departamento;
            entidad.Municipio = usuario.Municipio;
            entidad.CodDepartamento=usuario.CodDepartamento;
            entidad.CodMunicipio=usuario.CodMunicipio;
            _repositorioUsuario.Update(entidad);
            _unitOfWork.SaveChanges();
        }

        public void ActualizarContrasena(Usuario usuario)
        {
            var entidad =
                _repositorioUsuario.Query(x => x.UsuarioId == usuario.UsuarioId)
                    .Include(x => x.Perfil)
                    .Select()
                    .FirstOrDefault();

            if (entidad != null)
            {
                entidad.Contrasena = Seguridad.Encriptar(usuario.Contrasena);

                _repositorioUsuario.Update(entidad);
                _unitOfWork.SaveChanges();
            }
        }

        #endregion

        #region Permisos

        public IEnumerable<Permiso> ListarPermisos()
        {
            return _repositorioPermiso.Query(x => x.Activo).OrderBy(x => x.OrderBy(y => y.Index)).Select().ToList();
        }

        public void ActualizarPermisos(Perfil perfil)
        {
            var entidad =
                _repositorioPerfil.Query(x => x.NombrePerfil == perfil.NombrePerfil)
                    .Include(x => x.Permisos)
                    .Select()
                    .FirstOrDefault();

            if (entidad != null)
            {
                List<Permiso> listaPermisos = ListarPermisos().ToList();

                entidad.Permisos.Clear();
                foreach (var item in perfil.Permisos)
                {
                    entidad.Permisos.Add(listaPermisos.FirstOrDefault(x => x.PermisoId == item.PermisoId));
                }

                _repositorioPerfil.Update(entidad);
                _unitOfWork.SaveChanges();
            }
        }

        #endregion

        #region Autenticación

        public bool AutenticarUsuario(Usuario usuario, out string nombreCompleto, out string nombrePerfil, out int usuarioId, out string mensajeError)
        {
            nombreCompleto = string.Empty;
            nombrePerfil = string.Empty;
            usuarioId = -1;
            mensajeError = string.Empty;

            var contrasenaEncriptada =Seguridad.Encriptar(usuario.Contrasena);

            // Valida la contraseña del usuario

            Usuario entidadUsuario =
                _repositorioUsuario.Query(
                    x => x.NombreUsuario == usuario.NombreUsuario && x.Contrasena.Equals(contrasenaEncriptada))
                    .Include(x => x.Perfil)
                    .Select()
                    .FirstOrDefault();

            if (entidadUsuario == null)
            {
                mensajeError = ArchivoDeRecursos.Mensaje_ErrorAutenticacion;
                return false;
            }

            // Válida que el Usuario no se encuentre Inactivo 

            if (!entidadUsuario.Activo )
            {
                mensajeError = ArchivoDeRecursos.Mensaje_ErrorUsuarioInactivo;
                return false;
            }

            // Válida que el Perfil del Usuario se encuentre activo 

            if (!entidadUsuario.Perfil.Activo)
            {
                mensajeError = ArchivoDeRecursos.Mensaje_ErrorPerfilInactivo;
                return false;
            }

            // Carga la información del Usuario

            nombreCompleto = entidadUsuario.NombreCompleto;
            nombrePerfil = entidadUsuario.Perfil.NombrePerfil;
            usuarioId = entidadUsuario.UsuarioId;

            // Actualiza la fecha del último acceso

            entidadUsuario.FechaUltimoAcceso = DateTime.Now;

            _repositorioUsuario.Update(entidadUsuario);
            _unitOfWork.SaveChanges();

            return true;
        }

        public bool RecuperarContrasena(Usuario usuario, out string mensajeError)
        {
            mensajeError = string.Empty;

            try
            {
                // Valida el correo electrónico

                Usuario entidadUsuario = _repositorioUsuario.GetSingle(x => x.NombreUsuario == usuario.NombreUsuario);

                if (entidadUsuario == null)
                {
                    mensajeError = string.Format(ArchivoDeRecursos.Mensaje_ErrorCorreoNoRegistrado, usuario.NombreUsuario);
                    return false;
                }

                var contrasenaPlana = Seguridad.Desencriptar(entidadUsuario.Contrasena);

                var mensajeAsunto = "RECUPERACIÓN DE CONTRASEÑA";

                var temp = "Estimado / a {0}, Recibimos su solicitud de recuperación de contraseña.A continuación le enviamos sus credenciales de usuario.  Usuario: {1} Contraseña: {2}  Cordialmente,   Herramienta de apoyo a veedurias ciudadanas";

                var mensajeCuerpo = string.Format(temp, entidadUsuario.NombreCompleto, entidadUsuario.NombreUsuario, contrasenaPlana);

                Dictionary<string, string> configuracion = _variableConfiguracionServicio.ObtenerConfiguracionCorreoElectronico();
                CorreoElectronico.Enviar(configuracion, usuario.NombreUsuario, mensajeAsunto, mensajeCuerpo);

                return true;
            }
            catch (Exception ex)
            {
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return false;
        }

        public List<Menu> ObtenerMenu(string id, string urlActual)
        {
            if (string.IsNullOrEmpty(id))
                return new List<Menu>();

            List<Permiso> permisos =
                ObtenerPermisosPorUsuario(id).Where(x => x.EsElementoMenu).OrderBy(x => x.PosicionMenu).ToList();

            return
                (from action in permisos
                    where action.PermisoPadreId == -1
                    select ObtenerMenuItem(action.PermisoId, urlActual, permisos)).ToList();
        }

        public bool AccesoPermitido(string id, string controlador, string accion)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(controlador) || string.IsNullOrEmpty(accion))
                return false;

            List<Permiso> listaPermiso = ObtenerPermisosPorUsuario(id).OrderBy(x => x.Accion).ToList();
            return
                listaPermiso.Any(
                    item =>
                        !string.IsNullOrEmpty(item.Controlador) && item.Controlador.Equals(controlador) &&
                        !string.IsNullOrEmpty(item.Accion) && item.Accion.Equals(accion));
        }

        public IEnumerable<Permiso> ObtenerPermisosPorUsuario(string usuarioId)
        {
                Usuario entidadUsuario = _repositorioUsuario.Query(x => x.UsuarioId.ToString().Equals(usuarioId))
                    .Include(x => x.Perfil)
                    .Include(x => x.Perfil.Permisos)
                    .Select()
                    .FirstOrDefault();

                if (entidadUsuario == null)
                {
                    return new List<Permiso>();
                }
            return entidadUsuario.Perfil.Permisos.Where(action => action.Activo).ToList();
        }

        public Menu ObtenerMenuItem(int id, string urlActual, List<Permiso> listaPermisos)
        {
            Permiso permiso = listaPermisos.FirstOrDefault(item => item.PermisoId == id);
            
            if (permiso == null)
                return null;

            string etiqueta = string.Empty;

            if (!string.IsNullOrEmpty(permiso.Etiqueta))
            {
                etiqueta = permiso.Etiqueta;
                if (string.IsNullOrEmpty(etiqueta))
                {
                    etiqueta = permiso.Etiqueta;
                    if (string.IsNullOrEmpty(etiqueta))
                    {
                        etiqueta = permiso.Etiqueta;
                    }
                }
            }

            Menu menuModel = new Menu
            {
                Etiqueta = etiqueta,
                Url = !string.IsNullOrEmpty(permiso.Accion) && !string.IsNullOrEmpty(permiso.Controlador)
                        ? _urlHelper.Action(permiso.Accion, permiso.Controlador)
                        : string.Empty,
                Icono = permiso.Icono
            };

            foreach (Permiso hijo in listaPermisos.Where(q => q.PermisoPadreId == id).OrderBy(x => x.PosicionMenu).ToList())
            {
                menuModel.SubMenus.Add(ObtenerMenuItem(hijo.PermisoId, urlActual, listaPermisos));
            }

            if (menuModel.Url.Equals(urlActual))
            {
                menuModel.EstaSeleccionado = true;
            }
            else
            {
                foreach (Menu subMenu in menuModel.SubMenus)
                {
                    if (subMenu.EstaSeleccionado)
                    {
                        menuModel.EstaSeleccionado = true;
                        break;
                    }
                }
            }

            return menuModel;
        }

        #endregion
        
        #endregion
    }
}
