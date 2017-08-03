using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace WinRed.Model
{

    /// <summary>
    /// 微信用户
    /// </summary>
    [Table("User")]
    public partial class User : BaseEntity
    {

        [Display(Name = "账号"), MaxLength(32)]
        public string Account { get; set; }

        [Display(Name = "密码"), MaxLength(32)]
        public string Password { get; set; }

        /// <summary>
        /// 请输入密码
        /// </summary>
        [Display(Name = "请输入密码")]
        [MaxLength(12), MinLength(6)]
        [NotMapped]
        public string NewPassword { get; set; }

        /// <summary>
        /// 再次输入密码
        /// </summary>
        [MaxLength(12), MinLength(6), Compare("NewPassword", ErrorMessage = "两次密码输入不一致")]
        [NotMapped]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserType Type { get; set; } = UserType.User;

        [MaxLength(32)]
        public string OpenId { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Display(Name = "昵称"), MaxLength(32)]
        [Required]
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像"), MaxLength(128)]
        public string HeadImgUrl { get; set; }
     
        /// <summary>
        /// 性别
        /// </summary>
        [Display(Name = "性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        [Display(Name = "余额")]
        public int Balance { get; set; }

        /// <summary>
        /// 充值总额
        /// </summary>
        [Display(Name = "充值总额")]
        public int TotalRecharge { get; set; }
        [JsonIgnore]
        public virtual List<Recharge> UserRecharges { get; set; }
        [JsonIgnore]
        public virtual List<Recharge> CreateRecharges { get; set; }

        /// <summary>
        /// 提现总额
        /// </summary>
        [Display(Name = "提现总额")]
        public int TotalWithdrawals { get; set; }
        [JsonIgnore]
        public virtual List<Withdrawals> UserWithdrawals { get; set; }
        [JsonIgnore]
        public virtual List<Withdrawals> AuditWithdrawals { get; set; }



        /// <summary>
        /// 微信号
        /// </summary>
        [Display(Name = "提现总额")]
        public string WechatNum { get; set; }
    }

    public enum UserType
    {
        User = 1,

        Admin = 2,

        SuperAdmin = 3
    }
}

