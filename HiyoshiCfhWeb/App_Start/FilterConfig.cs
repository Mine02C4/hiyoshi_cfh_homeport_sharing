using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace HiyoshiCfhWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
