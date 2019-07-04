using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdventOne.Controllers {

    public class BaseController : Controller {

        protected void sessionHandleIndexAction() {

            Stack<String> referrers = (Stack<String>)HttpContext.Session["referrers"];
            if (referrers.Count() > 0) referrers.Clear();
            referrers.Push(this.Request.RawUrl);

        }

        protected void sessionHandleOtherActions() {

            Stack<String> referrers = (Stack<String>)HttpContext.Session["referrers"];
            while (referrers.Contains(this.Request.RawUrl)) {
                referrers.Pop();
            }
            if (referrers.Count > 0) { ViewBag.ReturnURL = referrers.Peek(); }
            referrers.Push(this.Request.RawUrl);

        }

        protected String sessionGetReturnURL() {

            String returnURL = "";

            Stack<String> referrers = (Stack<String>)HttpContext.Session["referrers"];
            if (referrers.Count > 0) {
                referrers.Pop();
                returnURL = referrers.Pop();
            }
            return returnURL;

        }
    }

}