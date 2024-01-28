using System.Web;
using System.Web.Mvc;

namespace Sample.AspNet.SwaggerUI.Swashbuckle
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
