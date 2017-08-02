using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRed.Model
{
    /// <summary>
    /// 提现记录
    /// </summary>
    [Table("Withdrawals")]
    public class Withdrawals : BaseEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        //User_ID
        [InverseProperty("UserWithdrawals")]
        public  User User { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        [Required]
        public int Count { get; set; }

        /// <summary>
        /// 审核用户id
        /// </summary>
        //AuditUser_ID
        [InverseProperty("AuditWithdrawals")]
        public User AuditUser { get; set; }

        public AuditState State { get; set; } = AuditState.Wait;

        [NotMapped]
        public string StateStr { get; set; }

        /// <summary>
        /// 转账 微信订单号
        /// </summary>
        [MaxLength(32)]
        public string VoucherNo { get; set; }

        /// <summary>
        /// 转账截图
        /// </summary>
        [MaxLength(128)]
        public string VoucherImg { get; set; }
    }
    public enum AuditState
    {
        Wait=0,
        /// <summary>
        /// 转账成功
        /// </summary>
        Success=1,
        /// <summary>
        /// 驳回
        /// </summary>
        Reject=2
    }
}
