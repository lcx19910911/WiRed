using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinRed.Web.Controllers;
using WinRed.Web.Filters;

namespace WinRed.Web.Areas.Admin.Controllers
{
    [LoginFilter]
    public class BaseAdminController : BaseController
    {

    }
}