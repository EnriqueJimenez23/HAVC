using System.Web.Mvc;
using CsWeb.Filters;

namespace CsWeb.Controllers
{
    [HandleError]
    [Authorize]
    public class HomeController : BaseController
    {
        [ExcluirAutorizacion]
        public ActionResult Index()
        {
            ViewBag.Perfil = NombrePerfil;
            return View();
        }
        
        [ExcluirAutorizacion]
        public ActionResult ErrorCargaArchivo()
        {
            return View();
        }
    }
}