using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinRed.IService;
using WinRed.Model;
using WinRed.Web.Controllers;
using WinRed.Web.Filters;

namespace WinRed.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseAdminController
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
        // GET: 

        #region 用户/管理员

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <param name="no">编号 - 搜索项</param>
        /// <returns></returns>
        public ActionResult GetPageList(int pageIndex, int pageSize, string name)
        {
            return JResult(IUserService.GetPageList(pageIndex, pageSize, name, UserType.Admin));
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <param name="no">编号 - 搜索项</param>
        /// <returns></returns>
        public ActionResult GetUserPageList(int pageIndex, int pageSize, string name)
        {
            return JResult(IUserService.GetPageList(pageIndex, pageSize, name, UserType.User));
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Delete(string ids)
        {
            return JResult(IUserService.Delete(ids));
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string id)
        {
            return JResult(IUserService.Find(id));
        }

        [HttpPost]
        public ActionResult Add(User model)
        {
            model.Type = UserType.Admin;
            var result = IUserService.Add(model);
            return JResult(result);
        }
        [HttpPost]
        public ActionResult Update(User model)
        {
            var result = IUserService.Update(model);
            return JResult(result);
        }

        public ActionResult ChangePassword(string oldPassword, string newPassword, string cfmPassword, string id)
        {
            return JResult(IUserService.ChangePassword(oldPassword, newPassword, cfmPassword, id));
        }

        #endregion

        #region 充值记录

        public ActionResult Recharge()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <param name="no">编号 - 搜索项</param>
        /// <returns></returns>
        public ActionResult GetRechargePageList(int pageIndex, int pageSize, string userId,DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            return JResult(IRechargeService.GetPageList(pageIndex, pageSize, userId, createdTimeStart, createdTimeEnd));
        }

        #endregion


        #region 提现记录

        public ActionResult Withdrawals()
        {
            return View();
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="name">名称 - 搜索项</param>
        /// <param name="no">编号 - 搜索项</param>
        /// <returns></returns>
        public ActionResult GetWithdrawalsPageList(int pageIndex, int pageSize, string userId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            return JResult(IWithdrawalsService.GetPageList(pageIndex, pageSize, userId, createdTimeStart, createdTimeEnd));
        }

        [HttpPost]
        public ActionResult Audit(Withdrawals model)
        {
            var result = IWithdrawalsService.Update(model);
            return JResult(result);
        }

        #endregion
    }
}