using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using WinRed.IService;
using WinRed.Model;
using WinRed.Web.Filters;

namespace WinRed.Web.Controllers
{
    [LoginFilter]
    public class HomeController : BaseController
    {
        public IUserService IUserService;
        public IRechargeService IRechargeService;
        public IWithdrawalsService IWithdrawalsService;

        public HomeController(IUserService _IUserService, IRechargeService _IRechargeService, IWithdrawalsService _IWithdrawalsService)
        {
            this.IUserService = _IUserService;
            this.IRechargeService = _IRechargeService;
            this.IWithdrawalsService = _IWithdrawalsService;
        }
        
        public ActionResult Index()
        {
            return View();
        }
    }
}