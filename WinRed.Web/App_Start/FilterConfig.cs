using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WinRed.Web.Filters;

namespace WinRed.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {         
            filters.Add(new ExceptionFilterAttribute());     
        }
    }
}
