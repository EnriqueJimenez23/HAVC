using System.IO;
using System.Net;
using System.Web.Mvc;
using CapaDominio.ComponentesNegocio;
using CapaDominio.EntidadesNegocio;
using CsWeb.Filters;

namespace CsWeb.Controllers
{
    [HandleError]
    [Authorize]
    public class DocumentosController : BaseController
    {
        private readonly IDocumentoServicio _documentoServicio;

        public DocumentosController(IDocumentoServicio documentoServicio)
        {
            _documentoServicio = documentoServicio;
        }

        [ExcluirAutorizacion]
        public ActionResult Descargar(string id) 
        {                       
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
            Documento documento = _documentoServicio.ObtenerPorToken(id);

            if (documento == null)
            {
                return HttpNotFound();
            }

            return File(documento.BytesArchivo, documento.TipoContenido, documento.NombreArchivo);
          //  return new FileStreamResult(new MemoryStream(data.Fisier.ToArray()), "application/pdf");
        }


        [ExcluirAutorizacion]
        public ActionResult AbrirFile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemoryStream ms = _documentoServicio.AbrirFileMS(id);
            return new FileStreamResult(ms, "application/pdf");
        }





        [HttpPost]
        [ExcluirAutorizacion]
        public ActionResult Eliminar(string id) 
        {                       
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _documentoServicio.Eliminar(id, Usuario, NombreCompletoUsuario);

            return Json("Registro eliminado");
        }
    }
}
