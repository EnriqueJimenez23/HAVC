using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaDominio.ComponentesNegocio;
using Microsoft.Practices.Unity;
using CsWeb.App_Start;
using CsWeb.Filters;

namespace CsWeb.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Retorna el correo electrónico del usuario
        /// </summary>
        public string Usuario
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return string.Empty;

                FormsIdentity formsIdentity = User.Identity as FormsIdentity;
                if (formsIdentity != null)
                {
                    return formsIdentity.Ticket.UserData.Split('|')[1];
                }

                return string.Empty;
            }
        }

       public string NombreCompletoUsuario
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return string.Empty;

                return User.Identity.Name;
            }
        }

        ///// <summary>
        ///// Retorna el Id del usuario
        ///// </summary>
        public string UsuarioId
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return string.Empty;

                FormsIdentity formsIdentity = User.Identity as FormsIdentity;
                if (formsIdentity != null)
                {
                    return formsIdentity.Ticket.UserData.Split('|')[0];
                }

                return string.Empty;
            }
        }

        public string NombrePerfil
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                    return string.Empty;

                FormsIdentity formsIdentity = User.Identity as FormsIdentity;
                if (formsIdentity != null)
                {
                    return formsIdentity.Ticket.UserData.Split('|')[2];
                }

                return string.Empty;
            }
        }
     
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IUsuariosServicio usuariosServicio = UnityConfig.GetConfiguredContainer().Resolve<IUsuariosServicio>("UsuariosServicio");

            if ((filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "Home" && filterContext.ActionDescriptor.ActionName == "Index") || !filterContext.ActionDescriptor.IsDefined(typeof(ExcluirAutorizacionAttribute), true))
            {
                var url = Request.Url != null ? Request.Url.AbsolutePath : string.Empty;
                ViewBag.Menu = usuariosServicio.ObtenerMenu(UsuarioId, url);
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            response.Cache.SetValidUntilExpires(false);
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
        }
    }
}