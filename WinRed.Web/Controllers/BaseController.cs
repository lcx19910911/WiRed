﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using WinRed.Core;
using Microsoft.AspNet.SignalR;
using WinRed.Core.Helper;
using WinRed.Core.Model;
using WinRed.Core.Code;
using WinRed.Model;
using WinRed.Core.Extensions;

namespace WinRed.Web.Controllers
{
    //当前 hub  
    public class CurHub : Hub
    {
        public void SetRecGroup(string id)//设置接收组  
        {
            this.Groups.Add(this.Context.ConnectionId, id);
            Send(id);
            for (int i = 0; i < 100; i++)
            {
                IHubContext chat = GlobalHost.ConnectionManager.GetHubContext<CurHub>();
                chat.Clients.Group("clientId").notify(i);//向指定组发送  
                System.Threading.Thread.Sleep(100);
            }
        }


        public void Send(string id)
        {
            IHubContext chat = GlobalHost.ConnectionManager.GetHubContext<CurHub>();
            chat.Clients.Group(id).NotifySendMessage(new
            {
                ID = 1,
                RelationID = 2,
                From = 3,
                Message = "3234"
            });//向指定组发送  
        }
    }
    public class BaseController : Controller
    {

        public ActionResult UploadImage(string mark)
        {
            HttpPostedFileBase file = Request.Files[0];
            if (file != null)
            {
                string path = UploadHelper.Save(file, mark);
                return Content(path);
            }
            else
                return Content("");
        }

        // GET: Upload
        //public ActionResult UploadFile(string mark)
        //{
        //    var fileList = Request.Files;
        //    string path = UploadHelper.Save(Request.Files[0], mark);
        //    return Content(new { Name = name, Path = path }.ToJson());

        //}
        public ActionResult Select()
        {
            return View();
        }

        public ActionResult _404()
        {
            return View();
        }

        public ActionResult Broadcast()
        {
            return View();
        }

        public ActionResult _500()
        {
            return View();
        }

        public ActionResult Forbidden()
        {
            return View("Forbidden");
        }




        /// <summary>
        /// 返回部分视图的错误页
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PartialError()
        {
            return null;
        }


        #region Json返回

        /// <summary>
        /// 返回异常编号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected internal JsonResult JResult(ErrorCode code)
        {
            return Json(new
            {
                Code = code,
                ErrorDesc = code.GetDescription()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 返回异常编号附带自定义消息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="appendMsg"></param>
        /// <returns></returns>
        protected internal JsonResult JResult(ErrorCode code, string appendMsg)
        {
            return Json(new
            {
                Code = code,
                ErrorDesc = code.GetDescription(),
                Append = appendMsg
            }, JsonRequestBehavior.AllowGet);
        }


        protected internal JsonResult JResult<T>(T model)
        {
            return Json(new
            {
                Code = ErrorCode.sys_success,
                Result = model
            }, JsonRequestBehavior.AllowGet);
        }


        protected internal JsonResult ParamsErrorJResult(ModelStateDictionary type)
        {
            return Json(new
            {
                Code = ErrorCode.sys_param_format_error,
                ErrorDesc = type.Where(x => x.Value.Errors.Count != 0).FirstOrDefault().Value.Errors.FirstOrDefault()?.ErrorMessage
            }, JsonRequestBehavior.AllowGet);
        }


        protected internal JsonResult JResult<T>(WebResult<T> model)
        {
            if (model.OccurError)
            {
                return JResult(model.Code, model.Append);
            }
            return JResult(model.Result);
        }

        protected internal JsonResult JResult(string model)
        {
            return Json(new
            {
                Code = ErrorCode.sys_success,
                Result = model
            }, JsonRequestBehavior.AllowGet);
        }


        protected internal JsonResult JResult<T>(WebResult<PageList<T>> model, Func<T, object> selector)
        {
            if (model.OccurError)
            {
                return JResult(model.Code);
            }
            return Json(new
            {
                Code = model.Code,
                Result = new
                {
                    RecordCount = model.Result.RecordCount,
                    PageCount = model.Result.PageCount,
                    IsLastPage = model.Result.IsLastPage,
                    List = model.Result.List.Select(selector).ToList()
                }
            }, JsonRequestBehavior.AllowGet);
        }

        protected internal JsonResult JResult<T>(WebResult<List<T>> model, Func<T, object> selector)
        {
            if (model.OccurError)
            {
                return JResult(model.Code);
            }
            return Json(new
            {
                Code = model.Code,
                Result = model.Result != null ? model.Result.Select(selector).ToList() : null
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        protected internal ViewResult View<T>(WebResult<T> model)
        {
            if (model.OccurError)
            {
                return View("Error");
            }
            return View(model.Result);
        }

        protected internal ActionResult ReLogin()
        {
            return RedirectToAction("Index", "Login");
        }


        private User _loginUser = null;

        public User LoginUser
        {
            get
            {
                if (_loginUser == null)
                {
                    return LoginHelper.GetCurrentUser();
                }
                else
                {
                    return _loginUser;
                }            
            }
            set
            {
                if (value == null)
                    LoginHelper.ClearUser();
                else
                    LoginHelper.CreateUser(value);
            }
        }       
    }
}