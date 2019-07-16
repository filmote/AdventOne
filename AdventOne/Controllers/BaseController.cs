using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdventOne.Authorization;

namespace AdventOne.Controllers {

    public class BaseController : Controller {

        protected void SessionHandleIndexAction() {

            Stack<String> referrers = (Stack<String>)HttpContext.Session["referrers"];
            if (referrers.Count() > 0) referrers.Clear();
            referrers.Push(this.Request.RawUrl);

        }

        protected void SessionHandleOtherActions() {

            Stack<String> referrers = (Stack<String>)HttpContext.Session["referrers"];
            while (referrers.Contains(this.Request.RawUrl)) {
                referrers.Pop();
            }
            if (referrers.Count > 0) { ViewBag.ReturnURL = referrers.Peek(); }
            referrers.Push(this.Request.RawUrl);

        }

        protected void SessionHandlerAppendTabNumber(int tabNumber) {

            Stack<String> referrers = (Stack<String>)HttpContext.Session["referrers"];

            String baseURL = referrers.Pop();

            if (baseURL.Contains("tabNumber")) {

                if (baseURL.IndexOf('&', baseURL.IndexOf("tabNumber")) > 0) {
                    //TODO
                }
                else {
                    baseURL = baseURL.Substring(0, baseURL.IndexOf("tabNumber")) + "tabNumber=" + tabNumber;
                }

            }
            else {
                if (baseURL.Contains("?")) {
                    baseURL = baseURL + "&tabNumber=" + tabNumber;
                }
                else {
                    baseURL = baseURL + "?tabNumber=" + tabNumber;
                }
            }
            referrers.Push(baseURL);
        }

        protected String SessionGetReturnURL() {

            String returnURL = "";

            Stack<String> referrers = (Stack<String>)HttpContext.Session["referrers"];
            if (referrers.Count == 1) {
                returnURL = referrers.Pop();
            }
            else if (referrers.Count > 1) {
                referrers.Pop();
                returnURL = referrers.Pop();
            }
            return returnURL;

        }

        protected bool IsInRole(String roleNames) {

            return CustomAuthorization.IsInRole(roleNames);

        }

    }

}