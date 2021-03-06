﻿
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WinRed.Core;
using WinRed.Core.Code;
using WinRed.Core.Extensions;
using WinRed.Core.Helper;
using WinRed.Core.Model;
using WinRed.DB;
using WinRed.IService;
using WinRed.Model;
using WinRed.WeChat;

namespace WinRed.Service
{
    /// <summary>
    /// 微信用户
    /// </summary>
    public class UserService : BaseService<User>, IUserService
    {


        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public WebResult<User> Login(string account, string password)
        {
            using (DbRepository entities = new DbRepository())
            {
                string md5Password = CryptoHelper.MD5_Encrypt(password);

                var user = Find(x => x.Account == account && x.Password == md5Password && !x.IsDelete);
                if (user == null)
                    return Result(new User());
                else 
                    return Result(user);
            }
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public User FindByOpenId(string openId)
        {
            if (string.IsNullOrEmpty(openId))
                return null;
            return Find(x => x.OpenId == openId);
        }


        /// <summary>
        /// 编辑管理用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Update(WXUser model)
        {
            var user = FindByOpenId(model.openid.Trim());
            if (user == null)
            {
                var addEntity = new User()
                {
                    OpenId = model.openid.Trim(),
                    NickName = model.nickname,
                    Sex = model.sex.GetInt(),
                    HeadImgUrl = model.headimgurl,
                    CreatedTime = DateTime.Now
                };

                Add(addEntity);
            }
            else
            {
                user.NickName = model.nickname;
                user.Sex = model.sex.GetInt();
                user.HeadImgUrl = model.headimgurl;
                Update(user);
            }
            return Result(true);

        }


        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        public PageList<User> GetPageList(int pageIndex, int pageSize, string name, UserType? type)
        {
            using (DbRepository db = new DbRepository())
            {
                var query = db.User.Where(x => !x.IsDelete).Include(x=>x.UserRecharges);
                if (name.IsNotNullOrEmpty())
                {
                    query = query.Where(x => x.NickName.Contains(name));
                }
                if (type != null)
                {
                    query = query.Where(x => x.Type == type);
                }
                var count = query.Count();
                var list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var returnList = new List<User>();
                list.ForEach(x =>
                {
                    returnList.Add(new User()
                    {
                        ID = x.ID,
                        Balance = x.Balance,
                        HeadImgUrl = x.HeadImgUrl,
                        NickName = x.NickName,
                        AuditWithdrawals = x.AuditWithdrawals,
                        TotalRecharge = x.TotalRecharge,
                        TotalWithdrawals = x.TotalWithdrawals,
                        Sex = x.Sex,
                        WechatNum=x.WechatNum,
                        CreatedTime = x.CreatedTime
                    });
                });
                return CreatePageList(returnList, pageIndex, pageSize, count);

            }
        }


        /// <summary>
        /// 获取用户所有的会议厅
        /// </summary>
        /// <returns></returns>
        public List<User> GetList(Expression<Func<User, bool>> predicate)
        {
            return GetList(predicate).ToList();
        }


        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Register(User model)
        {
            if (model.WechatNum.IsNullOrEmpty())
                return Result(false, ErrorCode.sys_param_format_error);
            model.Type = UserType.User;
            if (IsExits(x => x.Account == model.Account))
                return Result(false, ErrorCode.system_name_already_exist);
            if (model.HeadImgUrl.IsNullOrEmpty())
                model.HeadImgUrl = "/Images/avtar.png";
            model.Password = CryptoHelper.MD5_Encrypt(model.NewPassword);
            if (base.Add(model) > 0)
            {
                LoginHelper.CreateUser(model);
                return Result(true);
            }
            else
                return Result(false);
        }


        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public WebResult<bool> Update_User(User model)
        {
            if (model.WechatNum.IsNullOrEmpty())
                return Result(false, ErrorCode.sys_param_format_error);
            if (IsExits(x => x.Account == model.Account && x.ID != model.ID))
                return Result(false, ErrorCode.user_phone_already_exist);
            var user = Find(model.ID);
            if(user==null)
                return Result(false, ErrorCode.system_name_already_exist);
            user.NickName = model.NickName;
            if (base.Update(user) > 0)
            {
                LoginHelper.CreateUser(model);
                return Result(true);
            }
            else
                return Result(false);

        }


        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns> 
        public WebResult<bool> ChangePassword(string oldPassword, string newPassword, string cfmPassword, string id)
        {
            if (newPassword.IsNullOrEmpty() || cfmPassword.IsNullOrEmpty() || id.IsNullOrEmpty())
            {
                return Result(false, ErrorCode.sys_param_format_error);
            }
            if (!cfmPassword.Equals(newPassword))
            {
                return Result(false, ErrorCode.user_password_notequal);

            }
            var user = Find(id);
            if (user == null)
                return Result(false, ErrorCode.user_not_exit);
            if (oldPassword == "")
            {
                oldPassword = CryptoHelper.MD5_Encrypt(oldPassword);
                if (!user.Password.Equals(oldPassword))
                    return Result(false, ErrorCode.user_password_nottrue);
            }
            newPassword = CryptoHelper.MD5_Encrypt(newPassword);
            user.Password = newPassword;
            Update(user);
            return Result(true);
        }


    }
}
