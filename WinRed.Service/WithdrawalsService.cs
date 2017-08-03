
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
    /// 用户提现
    /// </summary>
    public class WithdrawalsService : BaseService<Withdrawals>, IWithdrawalsService
    {



        public PageList<Withdrawals> GetPageList(int pageIndex, int pageSize, string userId, DateTime? createdTimeStart, DateTime? createdTimeEnd)
        {
            using (DbRepository db = new DbRepository())
            {
                var list = new List<Withdrawals>();
                var returnList = new List<Withdrawals>();
                var count = 0;
                if (userId.IsNotNullOrEmpty())
                {
                    var user = db.User.Find(userId);
                    if (user != null)
                    {
                        var query = user.UserWithdrawals.AsQueryable();
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
                    var query = db.Withdrawals.AsQueryable();
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
                    returnList.Add(new Withdrawals()
                    {
                        UserName = x.User.NickName,
                        AuditUserName = x.AuditUser?.NickName,
                        CreatedTime = x.CreatedTime,
                        Count = x.Count,
                        ID = x.ID,
                        VoucherImg = x.VoucherImg,
                        VoucherNo = x.VoucherNo,
                        StateStr=x.State.GetDescription(),
                        State=x.State
                    });
                });
                return CreatePageList(returnList, pageIndex, pageSize, count);
            }
        }
    }
}
