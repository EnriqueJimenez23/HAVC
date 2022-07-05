using System;
using System.Collections.Generic;
using System.Linq;
using CapaDatos.Repositorio.Repositories;
using CapaDatos.Repositorio.UnitOfWork;
using CapaDominio.ComponentesNegocio.Filtros;
using CapaDominio.EntidadesNegocio;

namespace CapaDominio.ComponentesNegocio
{
    public interface IRegistroOperacionServicio : IServicio<RegistroOperacion>
    {
        RegistroOperacion Obtener(RegistroOperacion filtro);
        IEnumerable<RegistroOperacion> Listar(CategoriaRegistroOperacion? categoriaRegistroOperacion, int? registroId, string usuario, DateTime? filtroFechaInicial = null, DateTime? filtroFechaFinal = null);
        IEnumerable<RegistroOperacion> Listar(CategoriaRegistroOperacion? categoriaRegistroOperacion, int? registroId, string usuario, int pagina, int tamanoPagina, out int totalRegistros, DateTime? filtroFechaInicial = null, DateTime? filtroFechaFinal = null);
        IEnumerable<RegistroOperacion> Listar(int pagina, int tamanoPagina, out int totalRegistros, RegistroOperacion filtro = null, CategoriaRegistroOperacion? categoriaRegistroOperacion = null, DateTime? filtroFechaInicial = null, DateTime? filtroFechaFinal = null);
        void Crear(RegistroOperacion registroOperacion);
    }

    public class RegistroOperacionServicio : Servicio<RegistroOperacion>, IRegistroOperacionServicio
    {
        #region Constructor

        public RegistroOperacionServicio(IRepositoryAsync<RegistroOperacion> repository, IUnitOfWorkAsync unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        #endregion

        #region Metodos

        public RegistroOperacion Obtener(RegistroOperacion filtro)
        {
            return GetSingle(x => x.RegistroOperacionId == filtro.RegistroOperacionId);
        }

        public IEnumerable<RegistroOperacion> Listar(CategoriaRegistroOperacion? categoriaRegistroOperacion, int? registroId, string usuario, DateTime? filtroFechaInicial = null, DateTime? filtroFechaFinal = null)
        {
            var registroOperacionFiltro = new RegistroOperacionFiltro()
                .FiltrarPorCategoria(categoriaRegistroOperacion).FiltrarPorRegistro(registroId).FiltrarPorUsuario(usuario).FiltrarPorFechaInicial(filtroFechaInicial).FiltrarPorFechaFinal(filtroFechaFinal);

            return Query(registroOperacionFiltro).OrderBy(x => x.OrderByDescending(y => y.FechaOperacion)).Select().ToList();
        }

        public IEnumerable<RegistroOperacion> Listar(CategoriaRegistroOperacion? categoriaRegistroOperacion, int? registroId, string usuario, int pagina, int tamanoPagina, out int totalRegistros, DateTime? filtroFechaInicial = null, DateTime? filtroFechaFinal = null)
        {
            var registroOperacionFiltro = new RegistroOperacionFiltro()
               .FiltrarPorCategoria(categoriaRegistroOperacion).FiltrarPorRegistro(registroId).FiltrarPorUsuario(usuario).FiltrarPorFechaInicial(filtroFechaInicial).FiltrarPorFechaFinal(filtroFechaFinal);

            return Query(registroOperacionFiltro).OrderBy(x => x.OrderByDescending(y => y.FechaOperacion)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();
        }

        public IEnumerable<RegistroOperacion> Listar(int pagina, int tamanoPagina, out int totalRegistros, RegistroOperacion filtro = null, CategoriaRegistroOperacion? categoriaRegistroOperacion = null, DateTime? filtroFechaInicial = null, DateTime? filtroFechaFinal = null)
        {
            if (filtro == null)
                return Query().OrderBy(x => x.OrderByDescending(y => y.FechaOperacion).ThenBy(y => y.Categoria).ThenBy(y => y.NombreUsuario)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();

            var variableConfiguracionFiltro = new RegistroOperacionFiltro()
                .FiltrarPorCategoria(categoriaRegistroOperacion).FiltrarPorRegistroId(filtro.RegistroId).FiltrarPorNombreUsuario(filtro.NombreUsuario).FiltrarPorDescripcion(filtro.DescripcionOperacion).FiltrarPorFechaInicial(filtroFechaInicial).FiltrarPorFechaFinal(filtroFechaFinal);

            return Query(variableConfiguracionFiltro).OrderBy(x => x.OrderByDescending(y => y.FechaOperacion).ThenBy(y => y.Categoria).ThenBy(y => y.NombreUsuario)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();
        }

        public void Crear(RegistroOperacion registroOperacion)
        {
            Insert(registroOperacion);
            SaveChanges();
        }

        #endregion
    }
}
