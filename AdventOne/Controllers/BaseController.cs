using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdventOne.Authorization;

namespace AdventOne.Controllers {

    public class BaseController : Controller {

        protected List<SelectListItem> ToSelectList<T>(int? selectedKey) {

            List<SelectListItem> selectList = new List<SelectListItem>();
            SelectListItem item = new SelectListItem {
                Text = "< All >",
                Value = int.MinValue.ToString()
            };
            selectList.Add(item);

            Dictionary<int, string> enumValues = MapEnumToDictionary<T>();
            foreach (KeyValuePair<int, String> filter in enumValues) {

                item = new SelectListItem {
                    Text = filter.Value,
                    Value = filter.Key.ToString(),
                    Selected = ((selectedKey ?? -1) == filter.Key)
                };
                selectList.Add(item);

            }

            return selectList;

        }

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

        public Dictionary<int, string> MapEnumToDictionary<T>() {
            // Ensure T is an enumerator
            if (!typeof(T).IsEnum) {
                throw new ArgumentException("T must be an enumerator type.");
            }

            // Return Enumertator as a Dictionary
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(i => (int)Convert.ChangeType(i, i.GetType()), t => t.ToString());
        }

    }

}