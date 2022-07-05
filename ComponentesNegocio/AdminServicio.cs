using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CapaDatos.Repositorio.Repositories;
using CapaDatos.Repositorio.UnitOfWork;
using CapaDominio.ComponentesNegocio.Filtros;
using CapaDominio.EntidadesNegocio;
using CapaServicios.Servicios;

namespace CapaDominio.ComponentesNegocio
{
    public interface IAdminServicio 
    {
        IEnumerable<ContenidoSitio> ListarContenidoSitio(string filtro);
        IEnumerable<ContenidoSitio> ListarContenidoSitio(int pagina, int tamanoPagina, out int totalRegistros, ContenidoSitio filtro = null);
        void CrearContenido(ContenidoSitio reg);
        void ActualizarContenido(ContenidoSitio reg);
        ContenidoSitio ObtenerContenido(ContenidoSitio filtro);
        IEnumerable<Proyecto> ListarProyectos(int pagina, int tamanoPagina, out int totalRegistros, Proyecto filtro = null);
        IEnumerable<Proyecto> ListarProyectosRep(Proyecto filtro = null);
        void CrearProyecto(Proyecto reg);
        void ActualizarProyecto(Proyecto reg);
        Proyecto ObtenerProyecto(Proyecto filtro);
        void CrearProyectoAvance(ProyectoAvance reg);
        void ActualizarProyectoAvance(ProyectoAvance reg);
        ProyectoAvance ObtenerProyectoAvance(ProyectoAvance filtro);
        IEnumerable<ProyectoAvance> ListarProyectoAvance(int pagina, int tamanoPagina, out int totalRegistros, ProyectoAvance filtro = null);

        IEnumerable<Pasos> ListarProyectosVeedor(int pagina, int tamanoPagina, out int totalRegistros, Pasos filtro = null);
        IEnumerable<Pasos3> ListarPaso3(int id);
        IEnumerable<Pasos4> ListarPaso4(int id);
        IEnumerable<Pasos5> ListarPaso5(int id);
        IEnumerable<Pasos6> ListarPaso6(int id);
        IEnumerable<Pasos8> ListarPaso8(int id);
        IEnumerable<Pasos9> ListarPaso9(int id);
        void CrearPasos(Pasos reg);
        void ActualizarPasos(Pasos reg);
        Pasos ObtenerPasos(Pasos filtro);
        void CrearPaso3(Pasos3 reg);
        void CrearPaso4(Pasos4 reg);
        void CrearPaso5(Pasos5 reg);
        void CrearPaso6(Pasos6 reg);
        void CrearPaso8(Pasos8 reg);
        void CrearPaso9(Pasos9 reg);
        Veedor ObtenerVeedor(Veedor reg);
        void CrearVeedor(Veedor reg);
        void ActualizarVeedor(Veedor reg);
        IEnumerable<Veedor> ListarVeedorRep(Veedor filtro = null);
        IEnumerable<DatosGeo> ListarDatosGeo(string subregion);
        DatosGeo ObtenerDatosGeo(int id);
        DatosGeo ObtenerDatosGeom(string codd);
    }

    public class AdminServicio : IAdminServicio
    {
        private readonly IRepositoryAsync<Catalogo> _repository;
        private readonly IRepositoryAsync<CatalogoT> _repositoryT;
        private readonly IRepositoryAsync<Municipios> _repositoryMunicipio;
        private readonly IRepositoryAsync<Proyecto> _repositorioProyecto;
        private readonly IRepositoryAsync<ProyectoAvance> _repositorioAvance;
        private readonly IRepositoryAsync<Pasos> _repositorioPasos;
        private readonly IRepositoryAsync<Pasos3> _repositorioPaso3;
        private readonly IRepositoryAsync<Pasos4> _repositorioPaso4;
        private readonly IRepositoryAsync<Pasos5> _repositorioPaso5;
        private readonly IRepositoryAsync<Pasos6> _repositorioPaso6;
        private readonly IRepositoryAsync<Pasos8> _repositorioPaso8;
        private readonly IRepositoryAsync<Pasos9> _repositorioPaso9;
        private readonly IRepositoryAsync<ContenidoSitio> _repositorioContenido;
        private readonly IRepositoryAsync<Veedor> _repositorioVeedor;
        private readonly IRepositoryAsync<DatosGeo> _repositorioGeo;
        private readonly IUnitOfWorkAsync _unitOfWork;
        #region Constructor

        public AdminServicio(IRepositoryAsync<Catalogo> repository, IRepositoryAsync<CatalogoT> repositoryT, IRepositoryAsync<Municipios> repositoryMunicipio, IRepositoryAsync<Proyecto> repositorioProyecto, IRepositoryAsync<ProyectoAvance> repositorioAvance, IRepositoryAsync<Pasos> repositorioPasos, IRepositoryAsync<Pasos3> repositorioPaso3,IRepositoryAsync<Pasos4> repositorioPaso4, IRepositoryAsync<Pasos5> repositorioPaso5, IRepositoryAsync<Pasos6> repositorioPaso6, IRepositoryAsync<Pasos8> repositorioPaso8, IRepositoryAsync<Pasos9> repositorioPaso9,
            IRepositoryAsync<ContenidoSitio> repositorioContenido, IRepositoryAsync<Veedor> repositorioVeedor, IRepositoryAsync<DatosGeo> repositorioGeo, IUnitOfWorkAsync unitOfWork) 
        {
            _repository = repository;
            _repositoryT = repositoryT;
            _repositoryMunicipio = repositoryMunicipio;
            _repositorioContenido = repositorioContenido;
            _repositorioProyecto = repositorioProyecto;
            _repositorioAvance = repositorioAvance;
            _repositorioPasos = repositorioPasos;
            _repositorioPaso3 = repositorioPaso3;
            _repositorioPaso4 = repositorioPaso4;
            _repositorioPaso5 = repositorioPaso5;
            _repositorioPaso6 = repositorioPaso6;
            _repositorioPaso8 = repositorioPaso8;
            _repositorioPaso9 = repositorioPaso9;
            _repositorioVeedor = repositorioVeedor;
            _repositorioGeo = repositorioGeo;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Metodos
        public IEnumerable<ContenidoSitio> ListarContenidoSitio(string filtro)
        {
          return _repositorioContenido.Query(x => x.Seccion == filtro && x.Publicar=="SI").OrderBy(x => x.OrderBy(y => y.ContenidoSitioId)).Select().ToList();
        }
        public void CrearContenido(ContenidoSitio contenido)
        {
            _repositorioContenido.Insert(contenido);
            _unitOfWork.SaveChanges();
        }
        public void ActualizarContenido(ContenidoSitio reg)
        {
            var entidad = _repositorioContenido.GetSingle(x => x.ContenidoSitioId == reg.ContenidoSitioId);

            entidad.Seccion = reg.Seccion;
            entidad.Titulo = reg.Titulo;
            entidad.Enlace = reg.Enlace;
            entidad.Contenido = reg.Contenido;
            entidad.PalabraClave = reg.PalabraClave;
            entidad.Publicar = reg.Publicar;
            entidad.Imagen = reg.Imagen;
            entidad.Descripcion = reg.Descripcion;
            entidad.FechaCreado = reg.FechaCreado;

            _repositorioContenido.Update(entidad);
            _unitOfWork.SaveChanges();
        }
        public IEnumerable<ContenidoSitio> ListarContenidoSitio(int pagina, int tamanoPagina, out int totalRegistros, ContenidoSitio filtro = null)
        {
                return _repositorioContenido.Query(x => x.Seccion == filtro.Seccion).OrderBy(x => x.OrderBy(y => y.Seccion).ThenBy(y => y.ContenidoSitioId)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();
        }
        public ContenidoSitio ObtenerContenido(ContenidoSitio filtro)
        {
            return _repositorioContenido.GetSingle(x => x.ContenidoSitioId == filtro.ContenidoSitioId);
        }
        public DatosGeo ObtenerDatosGeo(int id)
        {
            return _repositorioGeo.GetSingle(x => x.DatosGeoId ==id);
        }

        public DatosGeo ObtenerDatosGeom(string cod)
        {
            return _repositorioGeo.GetSingle(x => x.CodMunicipio ==cod);
        }
        public IEnumerable<Proyecto> ListarProyectos(int pagina, int tamanoPagina, out int totalRegistros, Proyecto filtro = null)
        {
            if (filtro == null)
                return _repositorioProyecto.Query(x => x.Subregion == "1").OrderBy(x => x.OrderBy(y => y.Municipio).ThenBy(y => y.NombreProyecto)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();

            var proyectoFiltro = new ProyectoFiltro()
                .FiltrarPorSubRegion(filtro.Subregion)
                .FiltrarPorDepartamento(filtro.CodDepartamento)
                .FiltrarPorMunicipio(filtro.CodMunicipio)
                .FiltrarPorNombre(filtro.NombreProyecto)
                .FiltrarPorPilar(filtro.Pilar)
                .FiltrarPorEntidad(filtro.EntidadResponsable);

            return _repositorioProyecto.Query(proyectoFiltro).OrderBy(x => x.OrderBy(y => y.Municipio).ThenBy(y => y.NombreProyecto)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();
        }
        public IEnumerable<Proyecto> ListarProyectosRep(Proyecto filtro = null)
        {
            if (filtro == null)
                return _repositorioProyecto.Query().OrderBy(x => x.OrderBy(y => y.Municipio).ThenBy(y => y.NombreProyecto)).Select().ToList();

            var proyectoFiltro = new ProyectoFiltro()
                .FiltrarPorSubRegion(filtro.Subregion)
                .FiltrarPorDepartamento(filtro.CodDepartamento)
                .FiltrarPorMunicipio(filtro.CodMunicipio)
                .FiltrarPorNombre(filtro.NombreProyecto)
                .FiltrarPorPunto(filtro.Punto)
                .FiltrarPorPilar(filtro.Pilar)
                .FiltrarPorSector(filtro.Sector)
                .FiltrarPorEntidad(filtro.EntidadResponsable)
                .FiltrarPorEstado(filtro.EstadoProyecto);

            return _repositorioProyecto.Query(proyectoFiltro).OrderBy(x => x.OrderBy(y => y.Municipio).ThenBy(y => y.NombreProyecto)).Select().ToList();
        }
        public IEnumerable<Veedor> ListarVeedorRep(Veedor filtro = null)
        {
                return _repositorioVeedor.Query().OrderBy(x => x.OrderBy(y => y.Subregion).ThenBy(y => y.Departamento)).Select().ToList();
        }


        public Proyecto ObtenerProyecto(Proyecto filtro)
        {
            return _repositorioProyecto.GetSingle(x => x.ProyectoId == filtro.ProyectoId);
        }
        public void CrearProyecto(Proyecto proyecto)
        {
            _repositorioProyecto.Insert(proyecto);
            _unitOfWork.SaveChanges();
        }
        public void ActualizarProyecto(Proyecto reg)
        {
            var entidad = _repositorioProyecto.GetSingle(x => x.ProyectoId == reg.ProyectoId);

            entidad.Subregion = reg.Subregion;
            entidad.CodDepartamento = reg.CodDepartamento;
            entidad.Departamento = reg.Departamento;
            entidad.CodMunicipio = reg.CodMunicipio;
            entidad.Municipio = reg.Municipio;
            entidad.NombreProyecto = reg.NombreProyecto;
            entidad.Punto = reg.Punto;
            entidad.Pilar = reg.Pilar;
            entidad.Sector = reg.Sector;
            entidad.EntidadResponsable = reg.EntidadResponsable;
            entidad.FechaCreado = reg.FechaCreado;
            _repositorioProyecto.Update(entidad);
            _unitOfWork.SaveChanges();
        }

        public void CrearProyectoAvance(ProyectoAvance avance)
        {
            _repositorioAvance.Insert(avance);
            _unitOfWork.SaveChanges();
        }
        public void ActualizarProyectoAvance(ProyectoAvance reg)
        {
            var entidad = _repositorioAvance.GetSingle(x => x.ProyectoAvanceId == reg.ProyectoAvanceId);

            entidad.Indicador = reg.Indicador;
            entidad.Avance = reg.Avance;
            entidad.Meta = reg.Meta;
            entidad.Estado = reg.Estado;
            entidad.FechaAvance = reg.FechaAvance;
            entidad.FechaMeta = reg.FechaMeta;
            _repositorioAvance.Update(entidad);
            _unitOfWork.SaveChanges();
        }
        public ProyectoAvance ObtenerProyectoAvance(ProyectoAvance filtro)
        {
            return _repositorioAvance.GetSingle(x => x.ProyectoAvanceId == filtro.ProyectoAvanceId);
        }
        public IEnumerable<ProyectoAvance> ListarProyectoAvance(int pagina, int tamanoPagina, out int totalRegistros, ProyectoAvance filtro = null)
        {
            return _repositorioAvance.Query(x => x.ProyectoId == filtro.ProyectoId).OrderBy(x => x.OrderByDescending(y => y.FechaAvance)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList(); 
        }

        public IEnumerable<Pasos> ListarProyectosVeedor(int pagina, int tamanoPagina, out int totalRegistros, Pasos filtro = null)
        {
            if (filtro == null)
                return _repositorioPasos.Query().OrderBy(x => x.OrderBy(y => y.NombreObjeto).ThenBy(y => y.EntidadResponsable)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();

            var pasosFiltro = new PasosFiltro()
                .FiltrarPorNombre(filtro.NombreObjeto)
                .FiltrarPorUsuario(filtro.Usuario)
                .FiltrarPorEntidad(filtro.EntidadResponsable);
                

            return _repositorioPasos.Query(pasosFiltro).OrderBy(x => x.OrderBy(y => y.NombreObjeto).ThenBy(y => y.EntidadResponsable)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();
        }
        public IEnumerable<DatosGeo> ListarDatosGeo(string subregion)
        {
             return _repositorioGeo.Query(x => x.Subregion == subregion).OrderBy(x => x.OrderBy(y => y.Departamento)).Select().ToList();
        }
  
        public IEnumerable<Pasos3> ListarPaso3(int Id)
        {
            return _repositorioPaso3.Query(x => x.PasosId == Id).OrderBy(x => x.OrderBy(y => y.Pasos3Id)).Select().ToList();
        }
        public IEnumerable<Pasos4> ListarPaso4(int Id)
        {
            return _repositorioPaso4.Query(x => x.PasosId == Id).OrderBy(x => x.OrderBy(y => y.Pasos4Id)).Select().ToList();
        }
        public IEnumerable<Pasos5> ListarPaso5(int Id)
        {
            return _repositorioPaso5.Query(x => x.PasosId == Id).OrderBy(x => x.OrderBy(y => y.Pasos5Id)).Select().ToList();
        }
        public IEnumerable<Pasos6> ListarPaso6(int Id)
        {
            return _repositorioPaso6.Query(x => x.PasosId == Id).OrderBy(x => x.OrderBy(y => y.Pasos6Id)).Select().ToList();
        }
        public IEnumerable<Pasos8> ListarPaso8(int Id)
        {
            return _repositorioPaso8.Query(x => x.PasosId == Id).OrderBy(x => x.OrderBy(y => y.Pasos8Id)).Select().ToList();
        }
        public IEnumerable<Pasos9> ListarPaso9(int Id)
        {
            return _repositorioPaso9.Query(x => x.PasosId == Id).OrderBy(x => x.OrderBy(y => y.Pasos9Id)).Select().ToList();
        }
        public void CrearPasos(Pasos pasos)
        {
            _repositorioPasos.Insert(pasos);
            _unitOfWork.SaveChanges();
        }
        public void ActualizarPasos(Pasos reg)
        {
            var entidad = _repositorioPasos.GetSingle(x => x.PasosId == reg.PasosId);
            if (reg.NumPaso == 1)
            {
                entidad.Problema = reg.Problema;
                entidad.Territorio = reg.Territorio;
                entidad.Poblacion = reg.Poblacion;
                entidad.ObjetoVigilar = reg.ObjetoVigilar;
                entidad.NombreObjeto = reg.NombreObjeto;
                entidad.EntidadResponsable = reg.EntidadResponsable;
                entidad.ObjetoVeeduria = reg.ObjetoVeeduria;
            }
            else if (reg.NumPaso == 2)
            {
                entidad.Invitacion = reg.Invitacion;
                entidad.ComoMotivar = reg.ComoMotivar;
                entidad.CanalConvocatoria = reg.CanalConvocatoria;
                entidad.FechasConvocatoria = reg.FechasConvocatoria;
                entidad.FechaCreado = reg.FechaCreado;
            }
            else if (reg.NumPaso == 3)
            {
                entidad.Acta = reg.Acta;
                entidad.Ciudad = reg.Ciudad;
                entidad.ObjetoConformacion = reg.ObjetoConformacion;
                entidad.ObjetoVigilancia = reg.ObjetoVigilancia;
                entidad.NivelTerritorial = reg.NivelTerritorial;
                entidad.DepartamentoActa = reg.DepartamentoActa;
                entidad.CodDepartamentoActa = reg.CodDepartamentoActa;
                entidad.MunicipioActa = reg.MunicipioActa;
                entidad.CodMunicipioActa = reg.CodMunicipioActa;
                entidad.DuracionActa = reg.DuracionActa;
                entidad.Presidir = reg.Presidir;
                entidad.Secretario = reg.Secretario;
                entidad.Coordinador = reg.Coordinador;
                entidad.LugarFuncionamiento = reg.LugarFuncionamiento;
            }
            
            else if (reg.NumPaso == 5)
            {
                entidad.Impactos = reg.Impactos;
                entidad.Resultados = reg.Resultados;
                entidad.Productos = reg.Productos;
                entidad.Actividades = reg.Actividades;
                entidad.Insumos = reg.Insumos;
                entidad.FechaCreado = reg.FechaCreado;
            }
            else if (reg.NumPaso == 7)
            {
                entidad.ObjetoVeeduria = reg.Impactos;
                entidad.Introduccion = reg.Introduccion;
                entidad.Metodologia = reg.Metodologia;
                entidad.ResultadosInf = reg.ResultadosInf;
                entidad.Recomendaciones = reg.Recomendaciones;
                entidad.AquienEntregaraInforme = reg.AquienEntregaraInforme;
                entidad.CadaCuanto = reg.CadaCuanto;
            }
            _repositorioPasos.Update(entidad);
            _unitOfWork.SaveChanges();
        }

        public Pasos ObtenerPasos(Pasos filtro)
        {
            return _repositorioPasos.GetSingle(x => x.PasosId == filtro.PasosId);
        }
        public void CrearPaso3(Pasos3 pasos)
        {
            _repositorioPaso3.Insert(pasos);
            _unitOfWork.SaveChanges();
        }
        public void CrearPaso4(Pasos4 pasos)
        {
            _repositorioPaso4.Insert(pasos);
            _unitOfWork.SaveChanges();
        }
        public void CrearPaso5(Pasos5 pasos)
        {
            _repositorioPaso5.Insert(pasos);
            _unitOfWork.SaveChanges();
        }
        public void CrearPaso6(Pasos6 pasos)
        {
            _repositorioPaso6.Insert(pasos);
            _unitOfWork.SaveChanges();
        }
        public void CrearPaso8(Pasos8 pasos)
        {
            _repositorioPaso8.Insert(pasos);
            _unitOfWork.SaveChanges();
        }
        public void CrearPaso9(Pasos9 pasos)
        {
            _repositorioPaso9.Insert(pasos);
            _unitOfWork.SaveChanges();
        }

        public Veedor ObtenerVeedor(Veedor reg)
        {
            return _repositorioVeedor.GetSingle(x => x.UsuarioCreacion == reg.UsuarioCreacion);
        }


        public void CrearVeedor(Veedor reg)
        {
            _repositorioVeedor.Insert(reg);
            _unitOfWork.SaveChanges();

        }

        public void ActualizarVeedor(Veedor reg)
        {
            Veedor entidad = _repositorioVeedor.GetSingle(x => x.VeedorId == reg.VeedorId);
            entidad.Nombres=reg.Nombres;
            entidad.Apellidos=reg.Apellidos;
        	entidad.Subregion=reg.Subregion;
            entidad.Departamento=reg.Departamento;
            entidad.CodDepartamento=reg.CodDepartamento;
            entidad.Municipio=reg.Municipio;
            entidad.CodMunicipio=reg.CodMunicipio;
            entidad.Telefono=reg.Telefono;
            entidad.Observaciones=reg.Observaciones;
            _repositorioVeedor.Update(entidad);
            _unitOfWork.SaveChanges();
        }
        #endregion

      
    }
}
