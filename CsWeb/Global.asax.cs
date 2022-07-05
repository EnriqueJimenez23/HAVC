using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CsWeb.Infrastructure.Binders;

namespace CsWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        const int TimedOutExceptionCode = -2147467259;

        protected void Application_Start()
        {
           
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalNullableModelBinder());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (IsMaxRequestExceededException(Server.GetLastError()))
            {
                Server.ClearError();
                HttpContext.Current.ClearError();
                /*  NOTE: Because of the special context we're in, Server.Execute and Server.Transfer
                    *  won't work on the next line. */
                Response.Redirect(@"~/Home/ErrorCargaArchivo");
                //Response.Redirect("Views/Shared/ErrorArchivoGrande.cshtml");
            }
        }

        private static bool IsMaxRequestExceededException(Exception e)
        {
            if (e == null)
                return false;

            // unhandled errors = caught at global.ascx level
            // http exception = caught at page level

            Exception main;
            var unhandled = e as HttpUnhandledException;

            if (unhandled != null && unhandled.ErrorCode == TimedOutExceptionCode)
            {
                main = unhandled.InnerException;
            }
            else
            {
                main = e;
            }


            var http = main as HttpException;

            if (http != null && http.ErrorCode == TimedOutExceptionCode)
            {
                // hack: no real method of identifying if the error is max request exceeded as 
                // it is treated as a timeout exception
                if (http.StackTrace.Contains("GetEntireRawContent"))
                {
                    // MAX REQUEST HAS BEEN EXCEEDED
                    return true;
                }
            }

            return false;
        }
    }
}
