using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Security;
using WinRed.Core.Helper;

namespace WinRed.Core
{
    public class Params
    {

        /// <summary>
        /// 网站地址
        /// </summary>
        public static readonly string DomianUrl = CustomHelper.GetValue("Domain_Url");

        /// <summary>
        /// 网站标题
        /// </summary>
        public static readonly string DomianTitle =CustomHelper.GetValue("Domain_Title");
        /// <summary>
        /// 网站关键字
        /// </summary>
        public static readonly string DomianKeyWord = CustomHelper.GetValue("Domain_Title");
        /// <summary>
        /// 网站描述
        /// </summary>
        public static readonly string DomianDescription = CustomHelper.GetValue("Domain_Title");

        /// <summary>
        /// 时间戳有效时间c
        /// </summary>
        public const int TimspanExpiredMinutes = 60;
        /// <summary>
        /// token失效时间
        /// </summary>
        public const int ExpiredDays = 7;

        public static readonly string SecretKey = CustomHelper.GetValue("Data_SecretKey");

        /// <summary>
        /// 跟平台通信密钥
        /// </summary>
        public static readonly string WeixinAppSecret = CustomHelper.GetValue("WeChat_AppSecret");

        /// <summary>
        /// 平台地址
        /// </summary>
        public static readonly string WeixinAppId = CustomHelper.GetValue("WeChat_AppId");

        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string UserCookieName = "wechat_user";
        /// <summary>
        /// 登陆cookie
        /// </summary>
        public static readonly string AdminCookieName = "admin_cookie";

        /// <summary>
        /// 跟平台通信密钥
        /// </summary>
        public static readonly string AlipayKey = CustomHelper.GetValue("Alipay_Key");

        /// <summary>
        /// 平台地址
        /// </summary>
        public static readonly string AlipayPid = CustomHelper.GetValue("Alipay_Pid");
    }
}
