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
    public class UserController : BaseController
    {
        public IUserService IUserService;
        public IRechargeService IRechargeService;
        public IWithdrawalsService IWithdrawalsService;

        public UserController(IUserService _IUserService, IRechargeService _IRechargeService, IWithdrawalsService _IWithdrawalsService)
        {
            this.IUserService = _IUserService;
            this.IRechargeService = _IRechargeService;
            this.IWithdrawalsService = _IWithdrawalsService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Recharge()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Save(User model)
        {

            ModelState.Remove("ID");
            ModelState.Remove("CreatedTime");
            ModelState.Remove("IsDelete");
            ModelState.Remove("Password");
            ModelState.Remove("NewPassword");
            ModelState.Remove("ConfirmPassword");
            if (ModelState.IsValid)
            {
                var result = IUserService.Update_User(model);
                return JResult(result);
            }
            else
            {
                return ParamsErrorJResult(ModelState);
            }
        }
        


        public ActionResult ChangePassword(string oldPassword, string newPassword, string cfmPassword,string id)
        {
            return JResult(IUserService.ChangePassword(oldPassword, newPassword, cfmPassword, id));
        }
    }
}