using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CaptchaMvc.Infrastructure;
using CaptchaMvc.Interface;
using CaptchaMvc.Models;
using CsWeb.Infrastructure.Binders;


namespace CsWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        const int TimedOutExceptionCode = -2147467259;
        private static Random random = new Random();
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalNullableModelBinder());

            //Captcha 
            var captchaManager = (DefaultCaptchaManager)CaptchaUtils.CaptchaManager;
            captchaManager.CharactersFactory = () => " my characters ";
            captchaManager.PlainCaptchaPairFactory = length =>
            {
                string randomText = RandomText.Generate("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length);
                bool ignoreCase = false;
                return new KeyValuePair<string, ICaptchaValue>(Guid.NewGuid().ToString("N"),
                                      new StringCaptchaValue(randomText, randomText, ignoreCase));

            };

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
