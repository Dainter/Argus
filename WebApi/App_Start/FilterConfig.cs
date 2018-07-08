using System.Web;
using System.Web.Mvc;

namespace WebApi
{
    /// <summary/>
    public class FilterConfig
    {
        /// <summary/>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
