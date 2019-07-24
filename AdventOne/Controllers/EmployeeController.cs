using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AdventOne.Authorization;
using AdventOne.DAL;
using AdventOne.Models;
using AdventOne.Models.View;
using PagedList;

namespace AdventOne.Controllers {
    public class EmployeeController : BaseController
    {
        private readonly ProjectContext db = new ProjectContext();

        // GET: Employee
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page) {

            bool redirectRequired = false;
            base.SessionHandleIndexAction();

            ViewBag.CurrentSort = sortOrder ?? "employee_asc";
            ViewBag.EmployeeSortParm = sortOrder == "employee_asc" ? "employee_desc" : "employee_asc";
            ViewBag.EmailSortParm = sortOrder == "email_asc" ? "email_desc" : "email_asc";

            if (searchString != null) {
                page = 1;
                redirectRequired = true;
            }
            else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var employees = from s in db.Employees
                            select s;

            if (!String.IsNullOrEmpty(searchString)) {
                employees = employees.Where(s => s.EmployeeName.Contains(searchString) || s.EmailAddress.Contains(searchString));

            }

            switch (ViewBag.CurrentSort) {

                case "employee_asc":
                    employees = employees.OrderBy(s => s.EmployeeName);
                    break;

                case "employee_desc":
                    employees = employees.OrderByDescending(s => s.EmployeeName);
                    break;

                case "email_asc":
                    employees = employees.OrderBy(s => s.EmailAddress);
                    break;

                case "email_desc":
                    employees = employees.OrderByDescending(s => s.EmailAddress);
                    break;

            }

            int pageNumber = (page ?? 1);

            if (redirectRequired) {
                return RedirectToAction("Index", "Employee", new { currentFilter = searchString, pageNumber = pageNumber, sortOrder = sortOrder });
            }
            else {

                return View(employees.ToPagedList(pageNumber, Constants.PageSize));
            }

        }

        // GET: Employee/Details/5
        public ActionResult Details(int? id, int? tabNumber) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();

            Employee employee = db.Employees.Find(id);

            if (employee == null) {
                return HttpNotFound();
            }

            PopulateAssignedPermissionData(employee);
            ViewBag.ActiveTab = tabNumber ?? 0;
            return View(employee);
        }

        // GET: Employee/Create
        public ActionResult Create() {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorization(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ID,EmployeeName")] Employee employee) {

            if (ModelState.IsValid) {
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employee/Edit/5
        [CustomAuthorization(Roles="Admin")]
        public ActionResult Edit(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();

            Employee employee = db.Employees
                .Include(i => i.Permissions)
                .Where(i => i.ID == id)
                .Single();
            PopulateAssignedPermissionData(employee);

            if (employee == null) {
                return HttpNotFound();
            }

            return View(employee);

        }




        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorization(Roles="Admin")]
        public ActionResult Edit(int? id, string name, string[] selectedPermissions)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();

            var employeeToUpdate = db.Employees
               .Include(i => i.Permissions)
               .Where(i => i.ID == id)
               .Single();

            if (TryUpdateModel(employeeToUpdate, "", new string[] { "EmployeeName", "EmailAddress" })) {

                try {

                    UpdateEmployeePermissions(selectedPermissions, employeeToUpdate);

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */) {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedPermissionData(employeeToUpdate);
            return View(employeeToUpdate);
        }


        private void PopulateAssignedPermissionData(Employee employee) {

            var allPermissions = db.Permissions;
            var employeePermissions = new HashSet<int>(employee.Permissions.Select(c => c.ID));
            var viewModel = new List<AssignedPermissionData>();

            foreach (var permission in allPermissions) {
                viewModel.Add(new AssignedPermissionData {
                    PermissionID = permission.ID,
                    PermissionName = permission.PermissionName,
                    Assigned = employeePermissions.Contains(permission.ID)
                });
            }

            ViewBag.Permissions = viewModel;

        }

        private void UpdateEmployeePermissions(string[] selectedPermissions, Employee employeeToUpdate) {

            if (selectedPermissions == null) {
                employeeToUpdate.Permissions = new List<Permission>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedPermissions);
            var instructorCourses = new HashSet<int>(employeeToUpdate.Permissions.Select(c => c.ID));

            foreach (var permission in db.Permissions) {

                if (selectedCoursesHS.Contains(permission.ID.ToString())) {
                    if (!instructorCourses.Contains(permission.ID)) {
                        employeeToUpdate.Permissions.Add(permission);
                    }
                }
                else {
                    if (instructorCourses.Contains(permission.ID)) {
                        employeeToUpdate.Permissions.Remove(permission);
                    }
                }

            }

        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Employee employee = db.Employees.Find(id);

            if (employee == null) {
                return HttpNotFound();
            }

            return View(employee);

        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public PartialViewResult DisplaySubView(int tabId, int employeeId) {

            base.SessionHandlerAppendTabNumber(tabId);
            Employee employee = db.Employees.Find(employeeId);

            switch (tabId) {

                case 1: // Attachments
                    return PartialView("_Projects", employee);

                default:
                    return PartialView("_Customers", employee);

            }

        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}
