using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CapaDatos.Repositorio.Repositories;
using CapaDatos.Repositorio.UnitOfWork;
using CapaDominio.EntidadesNegocio;
using CapaServicios.Servicios;

namespace CapaDominio.ComponentesNegocio
{
    public interface IDocumentoServicio
    {
        Documento Obtener(Documento filtro);
        Documento ObtenerTipo_Reg(int registroId, TipoOrigenDocumento tipoOrigenDocumento);
        Documento ObtenerPorToken(string token);
        Documento ObtenerPorNombre(string nombreArchivo);
        IEnumerable<Documento> Listar(int pagina, int tamanoPagina, out int totalRegistros, TipoOrigenDocumento tipoOrigenDocumento);       
        IEnumerable<Documento> ListarPorRegistroId(int registroId, TipoOrigenDocumento tipoOrigenDocumento);
        IEnumerable<Documento> ListarPorNcaso(string ncaso);
        IEnumerable<Documento> ListarPorRegistroIdSinArchivo(int registroId, TipoOrigenDocumento tipoOrigenDocumento);       
        IEnumerable<Documento> ListarPorRegistroIdSinArchivo(int registroId, List<TipoOrigenDocumento> tipoOrigenDocumento);
        IEnumerable<Documento> ListarPorTipoSinArchivo(TipoOrigenDocumento tipoOrigenDocumento);
        IEnumerable<Documento> ListarPorTipoSinArchivoPorUsuario(TipoOrigenDocumento tipoOrigenDocumento, string usuario);
        void Crear(Documento documento);
        void Crear(Documento documento, bool eliminar);
        void Actualizar(Documento documento);
        void ActualizarRadicado(int registroId);
        void EliminarDocumentos(Documento filtro);
        void Eliminar(string token, string usuario = null, string nombreUsuario = null);
        List<Documento> ListarPorRegistro(int registroId);
        MemoryStream AbrirFileMS(string token);
        string AbrirFile1(string token);
    }

    public class DocumentoServicio : IDocumentoServicio
    {
        private readonly IRepositoryAsync<Documento> _repositorioDocumento;
        private readonly IRegistroOperacionServicio _registroOperacionServicio;
        private readonly IUnitOfWorkAsync _unitOfWork;

        #region Constructor

        public DocumentoServicio(IRepositoryAsync<Documento> repositorioDocumento, IRegistroOperacionServicio registroOperacionServicio, IUnitOfWorkAsync unitOfWork)
        {
            _repositorioDocumento = repositorioDocumento;
            _registroOperacionServicio = registroOperacionServicio;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Metodos

        public Documento Obtener(Documento filtro)
        {
            return _repositorioDocumento.GetSingle(x => x.DocumentoId == filtro.DocumentoId);
        }
        public Documento ObtenerTipo_Reg(int registroId, TipoOrigenDocumento tipoOrigenDocumento)
        {
            return _repositorioDocumento.Query(x => x.RegistroId == registroId && x.TipoOrigenDocumento == tipoOrigenDocumento).OrderBy(x => x.OrderByDescending(y => y.FechaRegistro)).Select().FirstOrDefault();
        }
        public Documento ObtenerPorToken(string token)
        {
            return _repositorioDocumento.GetSingle(x => x.Token == token);
        }
        public Documento ObtenerPorNombre(string nombreArchivo)
        {
            return _repositorioDocumento.GetSingle(x => x.NombreArchivo == nombreArchivo);
        }
        public IEnumerable<Documento> Listar(int pagina, int tamanoPagina, out int totalRegistros, TipoOrigenDocumento tipoOrigenDocumento)
        {
            return _repositorioDocumento.Query(x => x.TipoOrigenDocumento == tipoOrigenDocumento).OrderBy(x => x.OrderByDescending(y => y.FechaRegistro).ThenBy(y => y.NombreArchivo)).SelectPage(pagina, tamanoPagina, out totalRegistros).ToList();
        } 
        
        public IEnumerable<Documento> ListarPorRegistroId(int registroId, TipoOrigenDocumento tipoOrigenDocumento)
        {
            return _repositorioDocumento.Query(x => x.RegistroId == registroId && x.TipoOrigenDocumento == tipoOrigenDocumento).OrderBy(x => x.OrderBy(y => y.NombreArchivo).ThenByDescending(y => y.FechaRegistro)).Select().ToList();
        }
        public IEnumerable<Documento> ListarPorNcaso(string ncaso)
        {
            return _repositorioDocumento.Query(x => x.Ncaso == ncaso && 
            (x.TipoOrigenDocumento == TipoOrigenDocumento.Resultado || x.TipoOrigenDocumento == TipoOrigenDocumento.GraficaCH)).OrderBy(x => x.OrderBy(y => y.NombreArchivo).ThenByDescending(y => y.FechaRegistro)).Select().ToList();
        }
        public IEnumerable<Documento> ListarPorRegistroIdSinArchivo(int registroId, TipoOrigenDocumento tipoOrigenDocumento)
        {
            return _repositorioDocumento.SelectQuery($"SELECT [DocumentoId],[TipoOrigenDocumento],[RegistroId],[Token],[NombreArchivo],[ExtensionArchivo],NULL AS [BytesArchivo],[TamanoArchivo],[TipoContenido],[FechaRegistro],[Usuario],[NombreUsuario] FROM [Documento] WHERE RegistroId = {registroId} AND TipoOrigenDocumento = '{(int) tipoOrigenDocumento}'");
        }

        public IEnumerable<Documento> ListarPorRegistroIdSinArchivo(int registroId, List<TipoOrigenDocumento> tipoOrigenDocumento)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"SELECT [DocumentoId],[TipoOrigenDocumento],[RegistroId],[Token],[NombreArchivo],[ExtensionArchivo],NULL AS [BytesArchivo],[TamanoArchivo],[TipoContenido],[FechaRegistro],[Usuario],[NombreUsuario],[Radicado],[FechaRadicacion],[AzDirectorioId],[AzDocumentoId] FROM [Documento] WHERE RegistroId = {registroId} AND (");

            foreach (var item in tipoOrigenDocumento)
            {
                stringBuilder.Append($"TipoOrigenDocumento = '{(int) item}' OR ");
            }

            stringBuilder.Remove(stringBuilder.Length - 4, 4);
            stringBuilder.Append(")");

            return _repositorioDocumento.SelectQuery(stringBuilder.ToString());
        }

        public IEnumerable<Documento> ListarPorTipoSinArchivo(TipoOrigenDocumento tipoOrigenDocumento)
        {
            return _repositorioDocumento.SelectQuery($"SELECT [DocumentoId],[TipoOrigenDocumento],[RegistroId],[Token],[NombreArchivo],[ExtensionArchivo],NULL AS [BytesArchivo],[TamanoArchivo],[TipoContenido],[FechaRegistro],[Usuario],[NombreUsuario],[Radicado],[FechaRadicacion],[AzDirectorioId],[AzDocumentoId] FROM [Documento] WHERE TipoOrigenDocumento = '{(int) tipoOrigenDocumento}' ORDER BY [FechaRegistro] DESC");
        }

        public IEnumerable<Documento> ListarPorTipoSinArchivoPorUsuario(TipoOrigenDocumento tipoOrigenDocumento,string usuario)
        {
            return _repositorioDocumento.SelectQuery($"SELECT [DocumentoId],[TipoOrigenDocumento],[RegistroId],[Token],[NombreArchivo],[ExtensionArchivo],NULL AS [BytesArchivo],[TamanoArchivo],[TipoContenido],[FechaRegistro],[Usuario],[NombreUsuario],[Radicado],[FechaRadicacion],[AzDirectorioId],[AzDocumentoId] FROM [Documento] WHERE TipoOrigenDocumento = '{(int)tipoOrigenDocumento}'  AND Usuario= '{usuario}' ORDER BY [FechaRegistro] DESC");
        }

        public void Crear(Documento documento)
        {
            _repositorioDocumento.Insert(documento);
            _unitOfWork.SaveChanges();

            documento.Token = Seguridad.CrearToken("Documento", documento.DocumentoId);

            _repositorioDocumento.Update(documento);
            _unitOfWork.SaveChanges();
        }
        public void Crear(Documento documento,bool eliminar)
        {
            if (eliminar)
            {
                EliminarDocumentos(new Documento { RegistroId = documento.RegistroId, TipoOrigenDocumento = documento.TipoOrigenDocumento });
                EliminarDocumentosNombre(new Documento { NombreArchivo = documento.NombreArchivo });
            }
            _repositorioDocumento.Insert(documento);
            _unitOfWork.SaveChanges();

            documento.Token = Seguridad.CrearToken("Documento", documento.DocumentoId);

            _repositorioDocumento.Update(documento);
            _unitOfWork.SaveChanges();
        }
        public void Actualizar(Documento documento)
        {
            _repositorioDocumento.Update(documento);
            _unitOfWork.SaveChanges();
        }
        public void ActualizarRadicado(int RegistroId)
        {
            var documentos = _repositorioDocumento.Query(x => x.RegistroId == RegistroId && x.TipoOrigenDocumento == 0).Select();
            if (documentos != null)
            {
                foreach (Documento item in documentos)
                {
                    item.Radicado = "SI";
                    _repositorioDocumento.Update(item);
                }
                _unitOfWork.SaveChanges();
            }
        }
        public void EliminarDocumentos(Documento filtro)
        {
            var documentos = _repositorioDocumento.Query(x => x.RegistroId == filtro.RegistroId && x.TipoOrigenDocumento == filtro.TipoOrigenDocumento).Select();

            foreach (Documento item in documentos)
            {
                _repositorioDocumento.Delete(item.DocumentoId);
            }

            _unitOfWork.SaveChanges();
        }
        public void EliminarDocumentosNombre(Documento filtro)
        {
            var documentos = _repositorioDocumento.Query(x => x.NombreArchivo == filtro.NombreArchivo).Select();

            foreach (Documento item in documentos)
            {
                _repositorioDocumento.Delete(item.DocumentoId);
            }

            _unitOfWork.SaveChanges();
        }
        public void Eliminar(string token, string usuario = null, string nombreUsuario = null)
        {
            Documento documento = ObtenerPorToken(token);

            if (documento == null)
                return;
            
            _repositorioDocumento.Delete(documento.DocumentoId);
            _unitOfWork.SaveChanges();

        }
        public List<Documento> ListarPorRegistro(int registroId)
        {
            return _repositorioDocumento.Query(x => x.RegistroId == registroId).OrderBy(x => x.OrderBy(y => y.NombreArchivo).ThenByDescending(y => y.FechaRegistro)).Select().ToList();
        }

        public string AbrirFile1(string token)
        {
            Documento documento = ObtenerPorToken(token);

            string path1 = System.Web.HttpContext.Current.Server.MapPath("~/Content/PDF/");
            string path = string.Empty;

            path = path1 + documento.NombreArchivo;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = File.Create(path))
            {
                Byte[] info = documento.BytesArchivo;
                fs.Write(info, 0, info.Length);
            }
         //   MemoryStream ms = new MemoryStream(File.ReadAllBytes(path));

            return path;
        }

        public MemoryStream AbrirFileMS(string token)
        {
            Documento documento = ObtenerPorToken(token);

            string path1 = System.Web.HttpContext.Current.Server.MapPath("~/Content/PDF/");
            string path = string.Empty;

            path = path1 + documento.NombreArchivo;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = File.Create(path))
            {
                Byte[] info = documento.BytesArchivo;
                fs.Write(info, 0, info.Length);
            }
            MemoryStream ms = new MemoryStream(File.ReadAllBytes(path));
            File.Delete(path);
            return ms;
        }


        #endregion
    }
}
