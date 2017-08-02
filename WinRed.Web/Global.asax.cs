using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WinRed.Core.Helper;
using WinRed.Core.Util;
using WinRed.DB;

namespace WinRed.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //脚本资源注册
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ///预加载
            using (var dbcontext = new DbRepository())
            {
                var objectContext = ((IObjectContextAdapter)dbcontext).ObjectContext;
                var mappingCollection = (StorageMappingItemCollection)objectContext.MetadataWorkspace.GetItemCollection(DataSpace.CSSpace);
                mappingCollection.GenerateViews(new List<EdmSchemaError>());
                if (!dbcontext.User.Any())
                {
                    var user = new Model.User()
                    {
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now,
                        Account = "Admin",
                        NickName = "超级管理员",
                        HeadImgUrl = "/Images/avtar.png",
                        Type = Model.UserType.SuperAdmin,
                        Password = CryptoHelper.MD5_Encrypt("123456"),
                    };
                    dbcontext.User.Add(user);

                    var lcxUser = new Model.User()
                    {
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now,
                        Account = "lcx",
                        NickName = "超级管理员",
                        HeadImgUrl = "/Images/avtar.png",
                        Type = Model.UserType.User
                    };
                    lcxUser.UserRecharges = new List<Model.Recharge>() {
                        new Model.Recharge() {
                            User=lcxUser,
                            CreaterUser=user,
                            Count=10,
                            VoucherImg="/Images/avtar.png",
                            VoucherNo="11",                          
                        },
                         new Model.Recharge() {
                            User=lcxUser,
                            CreaterUser=user,
                            Count=20,
                            VoucherImg="/Images/avtar.png",
                            VoucherNo="22",
                        }
                    };
                    lcxUser.UserWithdrawals = new List<Model.Withdrawals>() {
                        new Model.Withdrawals() {
                            User=lcxUser,
                            AuditUser=user,
                            Count=10,
                            VoucherImg="/Images/avtar.png",
                            VoucherNo="11",
                        },
                         new Model.Withdrawals() {
                            User=lcxUser,
                            AuditUser=user,
                            Count=20,
                            VoucherImg="/Images/avtar.png",
                            VoucherNo="22",
                        }
                    };
                    dbcontext.User.Add(lcxUser);
                    dbcontext.SaveChanges();
                }
            }
        }
        protected void Application_End(object sender, EventArgs e)
        {
            var runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);
            if (runtime != null)
            {
                string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                ApplicationShutdownReason shutDownReason = (ApplicationShutdownReason)runtime.GetType().InvokeMember("_shutdownReason", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                LogHelper.WriteCustom(string.Format("Application_End:shutDownMessage:\r\n{0}\r\nshutDownStack:\r\n{1}\r\nshutDownReason:\r\n{2}\r\n", shutDownMessage, shutDownStack, shutDownReason.ToString()), @"Application\", false);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            if (exception is ThreadAbortException)
            {
                Thread.ResetAbort();
                HttpContext.Current.ClearError();
                return;
            }
            var httpException = exception as HttpException;

            if (httpException != null && httpException.GetHttpCode() == 404)
            {
                LogHelper.WriteCustom(httpException.ToString(), "404Error\\");
                Server.ClearError();
                Response.Clear();
                Response.Redirect("/base/_404");
            }
            else
            {
                LogHelper.WriteException("Application Error.", Server.GetLastError());
                Server.ClearError();
                Response.Clear();
                Response.Redirect("/base/_505");
            }

        }

        /// <summary>
        /// 附加cookie解决 flash上传时不带cookie的错
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /* we guess at this point session is not already retrieved by application so we recreate cookie with the session id... */
            try
            {
                string session_param_name = "ASPSESSID";
                string session_cookie_name = "ASP.NET_SessionId";

                if (HttpContext.Current.Request.Form[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
                }
            }
            catch
            {
            }

            try
            {
                string auth_param_name = "AUTHID";
                string auth_cookie_name = FormsAuthentication.FormsCookieName;

                if (HttpContext.Current.Request.Form[auth_param_name] != null)
                {
                    Core.Util.LogHelper.WriteError("1");
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[auth_param_name] != null)
                {
                    Core.Util.LogHelper.WriteError("2");
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);
                }

            }
            catch
            {
            }
        }

        private void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (null == cookie)
            {
                cookie = new HttpCookie(cookie_name);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
    }
}
