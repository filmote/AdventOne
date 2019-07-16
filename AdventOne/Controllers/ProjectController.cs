using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AdventOne.DAL;
using AdventOne.Models;
using PagedList;

namespace AdventOne.Controllers {

    public class ProjectController : BaseController {

        private readonly ProjectContext db = new ProjectContext();

        // GET: Projects
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page) {

            bool redirectRequired = false;
            base.SessionHandleIndexAction();
            IPrincipal user = HttpContext.User;

            sortOrder = sortOrder ?? "project_asc";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmployeeSortParm = sortOrder == "employee_asc" ? "employee_desc" : "employee_asc";
            ViewBag.CustomerSortParm = sortOrder == "customer_asc" ? "customer_desc" : "customer_asc";
            ViewBag.ProjectSortParm = sortOrder == "project_asc" ? "project_desc" : "project_asc";
            ViewBag.StatusSortParm = sortOrder == "status_asc" ? "status_desc" : "status_asc";

            if (searchString != null) {
                page = 1;
                redirectRequired = true;
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

            if (!base.IsInRole("Admin,SalesManager")) { 
                projects = projects.Where(s => s.Employee.EmailAddress == user.Identity.Name.ToLower());
            }

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

            if (redirectRequired) {
                return RedirectToAction("Index", "Project", new { currentFilter = searchString, pageNumber = pageNumber, sortOrder = sortOrder });
            }
            else {

                return View(projects.ToPagedList(pageNumber, pageSize));
            }

        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id, int? tabNumber) {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();

            //Project project = db.Projects.Find(id);

            var project = db.Projects.Find(id);
            var entry = db.Entry(project);
            entry.Collection(e => e.Tasks)
                 .Query()
                 .OrderBy(c => c.Sequence)
                 .Load();


            if (project == null)
            {
                return HttpNotFound();
            }

            ViewBag.ActiveTab = tabNumber ?? 0;
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create(int? employeeId, int? customerId) {

            base.SessionHandleOtherActions();

            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "EmployeeName", employeeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "ID", "CustomerName", customerId);
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeId,CustomerId,ProjectName,Status")] Project project) {

            if (ModelState.IsValid) {
                db.Projects.Add(project);
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Project project = db.Projects.Find(id);

            if (project == null) {
                return HttpNotFound();
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "EmployeeName", project.EmployeeID);
            ViewBag.CustomerId = new SelectList(db.Customers, "ID", "CustomerName", project.CustomerID);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeId,CustomerId,ProjectName,Status")] Project project) {
            if (ModelState.IsValid) {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "ID", "CustomerName", project.CustomerID);
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
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
            return Redirect(base.SessionGetReturnURL());

        }

        protected override void Dispose(bool disposing) {

            if (disposing) {
                db.Dispose();
            }

            base.Dispose(disposing);

        }

        // GET: Projects/Delete/5
        public ActionResult RenderPDF(int id) {

            Byte[] res = null;
            Project project = db.Projects.Find(id);

            StringBuilder sb = new StringBuilder();

            sb.Append("<html><body>Quote<br>");

            foreach (Task task in project.Tasks) {
                sb.Append(task.Description);
                sb.Append(task.FullText);
            }

            sb.Append("</body></html>");


            using (MemoryStream ms = new MemoryStream()) {
                var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(sb.ToString(), PdfSharp.PageSize.A4);
                pdf.Save(ms);
                res = ms.ToArray();
            }

            Attachment attachment = new Attachment();
            attachment.ContentType = "application/pdf";
            attachment.FileType = FileType.Quotation;
            attachment.Project = project;
            attachment.Content = res;
            attachment.FileName = "Quote_" + DateTime.Now.ToString("yyyyMMdd");
            attachment.Description = "Quote_" + DateTime.Now.ToString("yyyyMMdd");
            db.Attachments.Add(attachment);
            db.SaveChanges();

            return File(attachment.Content, attachment.ContentType, attachment.FileName);
        }

        [HttpPost]
        public PartialViewResult DisplaySubView(int tabId, int projectId) {

            base.SessionHandlerAppendTabNumber(tabId);
            Project project = db.Projects.Find(projectId);

            switch (tabId) {

                case 1: // Attachments
                    return PartialView("_Attachments", project);

                case 2: // Work Orders
                    return PartialView("_WorkOrders", project);

                default:
                    return PartialView("_Tasks", project);

            }

        }

    }

}
