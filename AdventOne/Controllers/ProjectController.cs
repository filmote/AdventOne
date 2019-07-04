using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdventOne.DAL;
using AdventOne.Models;
using PagedList;

namespace AdventOne.Controllers {

    public class ProjectController : BaseController {

        private ProjectContext db = new ProjectContext();

        // GET: Projects
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page) {

            base.sessionHandleIndexAction();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmployeeSortParm = sortOrder == "employee_asc" ? "employee_desc" : "employee_asc";
            ViewBag.CustomerSortParm = sortOrder == "customer_asc" ? "customer_desc" : "customer_asc";
            ViewBag.ProjectSortParm = sortOrder == "project_asc" ? "project_desc" : "project_asc";
            ViewBag.StatusSortParm = sortOrder == "status_asc" ? "status_desc" : "status_asc";

            if (searchString != null) {
                page = 1;
            }
            else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var projects = from s in db.Projects
                            select s;

            if (!String.IsNullOrEmpty(searchString)) {
                projects = projects.Where(s => s.Employee.EmployeeName.Contains(searchString) || s.Customer.CustomerName.Contains(searchString) || s.ProjectName.Contains(searchString));
            }

            //if (!String.IsNullOrEmpty(searchString)) {
            //    projects = projects.Where(s => s.Employee.EmployeeName.Contains(searchString) || s.Customer.CustomerName.Contains(searchString) || s.ProjectName.Contains(searchString));
            //}

            switch (sortOrder) {

                case "employee_desc":
                    projects = projects.OrderByDescending(s => s.Employee.EmployeeName);
                    break;

                case "employee_asc":
                    projects = projects.OrderBy(s => s.Employee.EmployeeName);
                    break;

                case "customer_desc":
                    projects = projects.OrderByDescending(s => s.Customer.CustomerName);
                    break;

                case "customer_asc":
                    projects = projects.OrderBy(s => s.Customer.CustomerName);
                    break;

                case "project_desc":
                    projects = projects.OrderByDescending(s => s.ProjectName);
                    break;

                case "project_asc":
                    projects = projects.OrderBy(s => s.ProjectName);
                    break;

                case "status_desc":
                    projects = projects.OrderByDescending(s => s.Status);
                    break;

                default:
                    projects = projects.OrderBy(s => s.Status);
                    break;

            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(projects.ToPagedList(pageNumber, pageSize));

        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id) {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.sessionHandleOtherActions();

            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create() {
            
            ViewBag.CustomerId = new SelectList(db.Customers, "ID", "CustomerName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CustomerId,ProjectName,Status")] Project project) {

            if (ModelState.IsValid) {
                db.Projects.Add(project);
                db.SaveChanges();
                return Redirect(base.sessionGetReturnURL());
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.sessionHandleOtherActions();
            Project project = db.Projects.Find(id);

            if (project == null) {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "ID", "CustomerName", project.CustomerID);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerId,ProjectName,Status")] Project project) {
            if (ModelState.IsValid) {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(base.sessionGetReturnURL());
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "ID", "CustomerName", project.CustomerID);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);

            if (project == null) {
                return HttpNotFound();
            }

            return View(project);

        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {

            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return Redirect(base.sessionGetReturnURL());

        }

        protected override void Dispose(bool disposing) {

            if (disposing) {
                db.Dispose();
            }

            base.Dispose(disposing);

        }

    }

}
