using System.Web.Mvc;
using CsWeb.Filters;

namespace CsWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new FiltroSeguridad());
        }
    }
}
