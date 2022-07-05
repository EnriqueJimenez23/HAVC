using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CapaDominio.ComponentesNegocio;
using Microsoft.Practices.Unity;
using CsWeb.App_Start;

namespace CsWeb.Filters
{
    public class FiltroSeguridad : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            bool skipCustom = filterContext.ActionDescriptor.IsDefined(typeof(ExcluirAutorizacionAttribute), true);

            if (skipAuthorization || skipCustom)
                return;

            FormsIdentity formsIdentity = filterContext.HttpContext.User.Identity as FormsIdentity;
            var usuarioId = string.Empty;
            if (formsIdentity != null)
            {
                usuarioId = formsIdentity.Ticket.UserData.Split('|')[0];
            }

            IUsuariosServicio usuariosServicio = UnityConfig.GetConfiguredContainer().Resolve<IUsuariosServicio>("UsuariosServicio");

            var isAccessAllowed = usuariosServicio.AccesoPermitido(usuarioId, filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, filterContext.ActionDescriptor.ActionName);
            if (!isAccessAllowed)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new {controller = "Cuenta", action = "IniciarSesion"}));
            }

        }
    }

    public class ExcluirAutorizacionAttribute : FilterAttribute
    {
               
    }
}