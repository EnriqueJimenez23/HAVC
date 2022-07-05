using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CapaDatos.Repositorio.Repositories;
using CapaDatos.Repositorio.UnitOfWork;
using CapaDominio.ComponentesNegocio;
using CapaDominio.ComponentesNegocio.Filtros;
using CapaDominio.EntidadesNegocio;

namespace CapaDominio.ComponentesNegocio
{
    public interface  ICatalogoServicio 
    {
        Catalogo Obtener(Catalogo filtro);
        Catalogo ObtenerZonaGeografica(string codigo);
        Catalogo ObtenerCodigoDepartamento(string departamento);
        Catalogo ObtenerCodigoMunicipio(string departamento, string municipio);
        IEnumerable<Catalogo> ListarPorCategoria(Catalogo filtro);
        IEnumerable<Catalogo> ListarPorCategoriaOrdenId(Catalogo filtro);
        IEnumerable<Catalogo> ListarDepartamentos();
        IEnumerable<Catalogo> ListarMunicipios(string departamentoCodigo);
        IEnumerable<Catalogo> ListarMunicipiosTodos();
        IEnumerable<Catalogo> Listar(int pagina, int tamanoPagina, out int totalRegistros, Catalogo filtro = null, CategoriaCatalogo? categoriaCatalogo = null);
      
        IEnumerable<Catalogo> ListarPorCategoriaHijos(Catalogo filtro);
        IEnumerable<Catalogo> ListarPorCategoriaPadre(Catalogo filtro);
        void Crear(Catalogo catalogo);
        void Actualizar(Catalogo catalogo);
        void Eliminar(Catalogo catalogo);
       
        Catalogo ObtenerDepartamentoPorCodigo(string codigo);
        Catalogo ObtenerMunicipioPorCodigo(string codigo);
    
        IEnumerable<Municipios> ObtenerSubregion();
        IEnumerable<Municipios> ObtenerDepartamentos(string filtro);
        IEnumerable<Municipios> ObtenerDepartamentosAll();
        IEnumerable<Municipios> ObtenerMunicipios(string filtro,string subregion);
        IEnumerable<Municipios> ListarMunicipiosAll();
        IEnumerable<Municipios> ObtenerMunicipiosD(string filtro);
        IEnumerable<Municipios> ObtenerMunicipiosDe(string filtro);
        Municipios ObtenerNombreDepartamento(string codigo);
        Municipios ObtenerNombreMunicipio(string codigo);
    }

    public class CatalogoServicio :  ICatalogoServicio
    {
        private readonly IRepositoryAsync<Catalogo> _repository;
        private readonly IRepositoryAsync<CatalogoT> _repositoryT;
        private readonly IRepositoryAsync<Municipios> _repositoryMunicipio;
        private readonly IRepositoryAsync<ContenidoSitio> _repositorioContenido;
        private readonly IUnitOfWorkAsync _unitOfWork;
        #region Constructor

        public CatalogoServicio(IRepositoryAsync<Catalogo> repository, IRepositoryAsync<CatalogoT> repositoryT, IRepositoryAsync<Municipios> repositoryMunicipio,
           IRepositoryAsync<ContenidoSitio> repositorioContenido, IUnitOfWorkAsync unitOfWork) 
        {
            _repository = repository;
            _repositoryMunicipio = repositoryMunicipio;
            _repositorioContenido = repositorioContenido;
            _unitOfWork = unitOfWork;
        }


        #endregion

        #region Metodos

        public Catalogo Obtener(Catalogo filtro)
        {
            return _repository.GetSingle(x => x.CatalogoId == filtro.CatalogoId);
        }

        public Catalogo ObtenerZonaGeografica(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
                return new Catalogo();

            return _repository.GetSingle(x => x.Categoria == CategoriaCatalogo.ZonasGeograficas && x.Valor == codigo);
        }

        public IEnumerable<Catalogo> ListarPorCategoria(Catalogo filtro)
        {
            return _repository.Query(x => x.Categoria == filtro.Categoria).OrderBy(x => x.OrderBy(y => y.Etiqueta)).Select().ToList();
        }
        public IEnumerable<Catalogo> ListarPorCategoriaOrdenId(Catalogo filtro)
        {
            return _repository.Query(x => x.Categoria == filtro.Categoria).OrderBy(x => x.OrderBy(y => y.CatalogoId)).Select().ToList();
        }
        public IEnumerable<Catalogo> ListarPorCategoriaPadre(Catalogo filtro)
        {
            return _repository.Query(x => x.Categoria == filtro.Categoria && x.CatalogoPadreId == -1).OrderBy(x => x.OrderBy(y => y.Etiqueta)).Select().ToList();
        }
        public IEnumerable<Catalogo> ListarPorCategoriaHijos(Catalogo filtro)
        {
            return _repository.Query(x => x.CatalogoPadreId == filtro.CatalogoId).OrderBy(x => x.OrderBy(y => y.Etiqueta)).Select().ToList();
        }
        public IEnumerable<Catalogo> ListarDepartamentos()
        {
            return _repository.Query(x => x.Categoria == CategoriaCatalogo.ZonasGeograficas && x.CatalogoPadreId == -1).OrderBy(x => x.OrderBy(y => y.Etiqueta)).Select().ToList();
        }

        public IEnumerable<Catalogo> ListarMunicipios(string departamentoCodigo)
        {
            var entidad = _repository.Query(x => x.Categoria == CategoriaCatalogo.ZonasGeograficas && x.Valor == departamentoCodigo).Select().FirstOrDefault();
            return _repository.Query(x => x.Categoria == CategoriaCatalogo.ZonasGeograficas && x.CatalogoPadreId == entidad.CatalogoId).OrderBy(x => x.OrderBy(y => y.Etiqueta)).Select().ToList();
        }
        public IEnumerable<Catalogo> ListarMunicipiosTodos()
        {
            return _repository.Query(x => x.Categoria == CategoriaCatalogo.ZonasGeograficas && x.CatalogoPadreId != -1).OrderBy(x => x.OrderBy(y => y.Etiqueta)).Select().ToList();
        }
        public IEnumerable<Municipios> ListarMunicipiosAll()
        {
            return _repositoryMunicipio.Query(x => x.Subregion!="").OrderBy(x => x.OrderBy(y => y.Municipio)).Select().ToList();
        }
        public Catalogo ObtenerDepartamentoPorCodigo(string codigo)
        {
            return _repository.Query(x => x.Categoria == CategoriaCatalogo.ZonasGeograficas && x.Valor == codigo).OrderBy(x => x.OrderBy(y => y.Etiqueta)).Select().FirstOrDefault();
        }

        public Catalogo ObtenerMunicipioPorCodigo(string codigo )
        {
            return _repository.Query(x => x.Categoria == CategoriaCatalogo.ZonasGeograficas && x.Valor == codigo).OrderBy(x => x.OrderBy(y => y.Etiqueta)).Select().FirstOrDefault();
        }

        public Catalogo ObtenerCodigoDepartamento(string departamento)
        {
            return _repository.Query(x => x.Categoria == CategoriaCatalogo.ZonasGeograficas && x.CatalogoPadreId==-1 && x.Etiqueta == departamento).Select().FirstOrDefault();
        }

        public Catalogo ObtenerCodigoMunicipio(string departamento,string municipio)
        {
            return _repository.Query(x => x.Categoria == CategoriaCatalogo.ZonasGeograficas &&  x.Etiqueta == municipio && x.Valor.StartsWith(departamento)).Select().FirstOrDefault();
        }

        public Municipios ObtenerNombreDepartamento(string codigo)
        {
            return _repositoryMunicipio.Query(x => x.CodDepartamento == codigo).Select().FirstOrDefault();
        }

        public Municipios ObtenerNombreMunicipio(string codigo)
        {
            return _repositoryMunicipio.Query(x => x.CodMunicipio == codigo).Select().FirstOrDefault();
        }

        public IEnumerable<Municipios> ObtenerSubregion()
        {
            DataTable dt = ConexionReportes.ObtenerSubregion();
            return dt.AsEnumerable().Select(dataRow => new Municipios { Subregion = dataRow.Field<string>("subregion")}).ToList();
        }
        public IEnumerable<Municipios> ObtenerDepartamentos(string filtro)
        {
            DataTable dt = ConexionReportes.ObtenerDepartamentos(filtro);
            return dt.AsEnumerable().Select(dataRow => new Municipios { CodDepartamento = dataRow.Field<string>("CodDepartamento"), Departamento = dataRow.Field<string>("Departamento") }).ToList();
        }
        public IEnumerable<Municipios> ObtenerDepartamentosAll()
        {
            DataTable dt = ConexionReportes.ObtenerDepartamentosAll();
            return dt.AsEnumerable().Select(dataRow => new Municipios { CodDepartamento = dataRow.Field<string>("CodDepartamento"), Departamento = dataRow.Field<string>("Departamento") }).ToList();
        }

        public IEnumerable<Municipios> ObtenerMunicipios(string filtro, string subregion)
        {
            DataTable dt = ConexionReportes.ObtenerMunicipios(filtro,subregion);
            return dt.AsEnumerable().Select(dataRow => new Municipios { CodMunicipio = dataRow.Field<string>("CodMunicipio"), Municipio = dataRow.Field<string>("Municipio") }).ToList();
        }
        public IEnumerable<Municipios> ObtenerMunicipiosD(string filtro)
        {
            DataTable dt = ConexionReportes.ObtenerMunicipiosAx();
            return dt.AsEnumerable().Select(dataRow => new Municipios { CodMunicipio = dataRow.Field<string>("CodMunicipio"), Municipio = dataRow.Field<string>("Municipio") }).ToList();
        }
        public IEnumerable<Municipios> ObtenerMunicipiosDe(string filtro)
        {
            DataTable dt = ConexionReportes.ObtenerMunicipiosD(filtro);
            return dt.AsEnumerable().Select(dataRow => new Municipios { CodMunicipio = dataRow.Field<string>("CodMunicipio"), Municipio = dataRow.Field<string>("Municipio") }).ToList();
        }
        public Catalogo ObtenerItem(CategoriaCatalogo categoria, string item)
        {
            return _repository.Query(x => x.Categoria == categoria && x.Etiqueta == item).Select().FirstOrDefault();
        }
    

        public IEnumerable<Catalogo> Listar(int pagina, int tamanoPagina, out int totalRegistros, Catalogo filtro = null, CategoriaCatalogo? categoriaCatalogo = null)
        {
            if (filtro == null)
                return _repository.Query().OrderBy(x => x.OrderBy(y => y.Categoria).ThenBy(y => y.Etiqueta)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();

            var catalogoFiltro = new CatalogoFiltro()
                .FiltrarPorCategoria(categoriaCatalogo).FiltrarPorEtiqueta(filtro.Etiqueta).FiltrarPorDescripcion(filtro.Descripcion);

            return _repository.Query(catalogoFiltro).OrderBy(x => x.OrderBy(y => y.Categoria).ThenBy(y => y.Etiqueta)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();
        }

        public void Crear(Catalogo catalogo)
        {
            _repository.Insert(catalogo);
            _unitOfWork.SaveChanges();
        }

        public void Actualizar(Catalogo catalogo)
        {
            var entidad = _repository.GetSingle(x => x.CatalogoId == catalogo.CatalogoId);

            entidad.CatalogoPadreId = catalogo.CatalogoPadreId;
            entidad.Categoria = catalogo.Categoria;
            entidad.Etiqueta = catalogo.Etiqueta;
            entidad.Valor = catalogo.Valor;
            entidad.Descripcion = catalogo.Descripcion;

            _repository.Update(entidad);
            _unitOfWork.SaveChanges();
        }

        public void Eliminar(Catalogo catalogo)
        {
            // Válida que no tenga hijos

            var lista = _repository.Query(x => x.CatalogoPadreId == catalogo.CatalogoId).Select().ToList();
            if (lista.Count > 0)
            {
                throw new Exception("El catálogo no puede ser eliminado porque tiene varios catálogos hijos.");
            }

            _repository.Delete(catalogo.CatalogoId);
            _unitOfWork.SaveChanges();
        }
        #endregion
    }
}
