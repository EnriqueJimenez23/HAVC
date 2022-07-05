using System.Collections.Generic;
using System.Linq;
using CapaDatos.Repositorio.Repositories;
using CapaDatos.Repositorio.UnitOfWork;
using CapaDominio.ComponentesNegocio.Filtros;
using CapaDominio.EntidadesNegocio;

namespace CapaDominio.ComponentesNegocio
{
    public interface IVariableConfiguracionServicio : IServicio<VariableConfiguracion>
    {
        VariableConfiguracion Obtener(VariableConfiguracion filtro);
        VariableConfiguracion ObtenerPorCategoriaNombre(VariableConfiguracion filtro);
        Dictionary<string, string> ObtenerConfiguracionCorreoElectronico();
          IEnumerable<VariableConfiguracion> Listar(int pagina, int tamanoPagina, out int totalRegistros, VariableConfiguracion filtro = null, CategoriaVariableConfiguracion? categoriaVariableConfiguracion = null);
        void Actualizar(VariableConfiguracion variableConfiguracion);
    }

    public class VariableConfiguracionServicio : Servicio<VariableConfiguracion>, IVariableConfiguracionServicio
    {
        #region Constructor

        public VariableConfiguracionServicio(IRepositoryAsync<VariableConfiguracion> repository, IUnitOfWorkAsync unitOfWork) : base(repository, unitOfWork)
        {

        }

        #endregion

        #region Metodos

        public VariableConfiguracion Obtener(VariableConfiguracion filtro)
        {
            return GetSingle(x => x.VariableConfiguracionId == filtro.VariableConfiguracionId);
        }

        public VariableConfiguracion ObtenerPorCategoriaNombre(VariableConfiguracion filtro)
        {
            return Query(x => x.Categoria == filtro.Categoria && x.Nombre == filtro.Nombre).Select().FirstOrDefault();
        }

        public Dictionary<string, string> ObtenerConfiguracionCorreoElectronico()
        {
            return Query(x => x.Categoria == CategoriaVariableConfiguracion.ConfiguracionCorreoElectronico).Select().ToList().ToDictionary(item => item.Nombre, item => item.Valor);
        }

        public IEnumerable<VariableConfiguracion> Listar(int pagina, int tamanoPagina, out int totalRegistros, VariableConfiguracion filtro = null, CategoriaVariableConfiguracion? categoriaVariableConfiguracion = null)
        {
            if (filtro == null)
                return Query().OrderBy(x => x.OrderBy(y => y.Categoria).ThenBy(y => y.Nombre)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();

            var variableConfiguracionFiltro = new VariableConfiguracionFiltro()
                .FiltrarPorCategoria(categoriaVariableConfiguracion).FiltrarPorNombre(filtro.Nombre).FiltrarPorDescripcion(filtro.Descripcion);

            return Query(variableConfiguracionFiltro).OrderBy(x => x.OrderBy(y => y.Categoria).ThenBy(y => y.Nombre)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();
        }

        public void Actualizar(VariableConfiguracion variableConfiguracion)
        {
            var entidad = GetSingle(x => x.VariableConfiguracionId == variableConfiguracion.VariableConfiguracionId);

            entidad.Valor = variableConfiguracion.Valor;
            entidad.Descripcion = variableConfiguracion.Descripcion;

            Update(entidad);
            SaveChanges();
        }
        
        #endregion
    }
}
