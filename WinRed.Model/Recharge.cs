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
    /// 充值记录
    /// </summary>
    [Table("Recharge")]
    public class Recharge : BaseEntity
    {
        /// <summary>
        /// 用户id
        /// </summary>
        // User_ID
        [InverseProperty("UserRecharges")]
        public virtual User User { get; set; }

        [NotMapped]
        public string UserName { get; set; }

        /// <summary>
        /// ip
        /// </summary>
        [Required]
        public int Count { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        // CreaterUser_ID
        [InverseProperty("CreateRecharges")]
        public virtual User CreaterUser { get; set; }
        [NotMapped]
        public string CreaterUserName { get; set; }

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
}
