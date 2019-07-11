using AdventOne.DAL;
using AdventOne.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace AdventOne.Authorization {

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CustomAuthorization : AuthorizeAttribute {

        public static bool IsInRole(String role) {

            HttpContext httpContext = HttpContext.Current;
            IPrincipal user = httpContext.User;

            if (!user.Identity.IsAuthenticated) {
                return false;
            }

            HttpSessionState session = (HttpSessionState)httpContext.Session;
            Dictionary<String, Permission> permissions = (Dictionary<String, Permission>)session["permissions"]; 

            if (permissions == null) {
                permissions = retrievePermissions(user);
                session["permissions"] = permissions;
            }

            Employee employee = (Employee)session["employee"];

            if (employee == null) {
                employee = retrieveEmployee(user);
                session["employee"] = employee;
            }

            String[] roles = role.Split(',');

            foreach (String roleName in roles) {

                if (permissions.ContainsKey(roleName.Trim().ToLower())) return true;

            }

            return false;

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext) {

            if (httpContext == null) {
                throw new ArgumentNullException("httpContext");
            }

            IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated) {
                return false;
            }

            HttpSessionStateBase session = httpContext.Session;
            Dictionary<String, Permission> permissions = (Dictionary<String, Permission>)session["permissions"];
            String role = this.Roles;
            
            if (permissions == null) {

                permissions = retrievePermissions(user);
                session["permissions"] = permissions;

            }

            Employee employee = (Employee)session["employee"];

            if (employee == null) {
                employee = retrieveEmployee(user);
                session["employee"] = employee;
            }

            String[] roles = role.Split(',');

            foreach (String roleName in roles) {

                if (permissions.ContainsKey(roleName.Trim().ToLower())) return true;

            }

            return false;

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated) {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Unauthorised" }));
            }

        }

        protected static Dictionary<String, Permission> retrievePermissions(IPrincipal user) {

            Dictionary<String, Permission> newPermissions = new Dictionary<string, Permission>();

            using (var db = new ProjectContext()) {

                //var permissions = from s in db.Permissions
                //                  where s.Employees.All(e => e.EmailAddress == user.Identity.Name.ToLower())
                //                  select s;

                var permissions = from s in db.Permissions.Where(s => s.Employees.Any(e => e.EmailAddress == user.Identity.Name.ToLower()))
                                  select s;

                List<Permission> lstPermissions = permissions.ToList<Permission>();

                foreach (Permission permission in lstPermissions) {
                    newPermissions.Add(permission.PermissionName.ToLower(), permission);
                }

            }

            return newPermissions;

        }


        protected static Employee retrieveEmployee(IPrincipal user) {

            using (var db = new ProjectContext()) {

                var employee = from s in db.Employees.Where(s => s.EmailAddress == user.Identity.Name.ToLower())
                                  select s;

                return employee.FirstOrDefault();

            }

        }

    }

}