
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WinRed.Core.Model;
using WinRed.Model;

namespace WinRed.IService
{
    public interface IWithdrawalsService : IBaseService<Withdrawals>
    {

        //                           提现记录
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="title">标题 - 搜索项</param>
        /// <returns></returns>
        PageList<Withdrawals> GetPageList(int pageIndex, int pageSize, string userId, DateTime? createdTimeStart, DateTime? createdTimeEnd);

    }
}
