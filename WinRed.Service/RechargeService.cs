
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
    /// 用户充值记录
    /// </summary>
    public class RechargeService : BaseService<Recharge>, IRechargeService
    {
        public PageList<Recharge> GetPageList(int pageIndex, int pageSize, string userId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository db = new DbRepository())
            {
                var list = new List<Recharge>();
                var returnList = new List<Recharge>();
                var count = 0;
                if (userId.IsNotNullOrEmpty())
                {
                    var user = db.User.Find(userId);
                    if (user != null&&user.UserRecharges!=null&&user.UserRecharges.Count>0)
                    {
                        var query = user.UserRecharges.AsQueryable().Include("User");
                        if (createdTimeStart != null)
                        {
                            query = query.Where(x => x.CreatedTime >= createdTimeStart);
                        }
                        if (createdTimeEnd != null)
                        {
                            createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                            query = query.Where(x => x.CreatedTime < createdTimeEnd);
                        }
                        count = query.Count();
                        list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                    }
                    
                }
                else
                {
                    var query = db.Recharge.AsQueryable().Include("CreaterUser");
                    if (createdTimeStart != null)
                    {
                        query = query.Where(x => x.CreatedTime >= createdTimeStart);
                    }
                    if (createdTimeEnd != null)
                    {
                        createdTimeEnd = createdTimeEnd.Value.AddDays(1);
                        query = query.Where(x => x.CreatedTime < createdTimeEnd);
                    }
                    count = query.Count();
                    list = query.OrderByDescending(x => x.CreatedTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                list.ForEach(x =>
                {
                    returnList.Add(new Recharge()
                    {
                        UserName=x.User.NickName,
                        CreaterUserName=x.CreaterUser.NickName,
                        CreatedTime=x.CreatedTime,
                        Count=x.Count,
                        ID=x.ID,
                        VoucherImg=x.VoucherImg,
                        VoucherNo=x.VoucherNo
                    });
                });
                return CreatePageList(returnList, pageIndex, pageSize, count);
            }
         }
    }
}
