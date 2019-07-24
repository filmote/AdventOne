using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web.Mvc;
using AdventOne.DAL;
using AdventOne.Models;
using PagedList;

namespace AdventOne.Controllers {

    public class ProjectController : BaseController {

        private readonly ProjectContext db = new ProjectContext();

        // GET: Projects
        public ActionResult Index(string sortOrder, string currentSearchString, string searchString, int? currentStatusFilter, int? statusFilter, int? page) {

            bool redirectRequired = false;
            base.SessionHandleIndexAction();
            IPrincipal user = HttpContext.User;

            sortOrder = sortOrder ?? "project_asc";
            if (currentStatusFilter == null && statusFilter == null) currentStatusFilter = (int)ProjectStatus.Open;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmployeeSortParm = sortOrder == "employee_asc" ? "employee_desc" : "employee_asc";
            ViewBag.CustomerSortParm = sortOrder == "customer_asc" ? "customer_desc" : "customer_asc";
            ViewBag.ProjectSortParm = sortOrder == "project_asc" ? "project_desc" : "project_asc";
            ViewBag.ProjectStatusSortParm = sortOrder == "projectStatus_asc" ? "projectStatus_desc" : "projectStatus_asc";
            ViewBag.SalesStageSortParm = sortOrder == "salesStage_asc" ? "salesStage_desc" : "salesStage_asc";
            ViewBag.InvoiceDateSortParm = sortOrder == "invoiceDate_asc" ? "invoiceDate_desc" : "invoiceDate_asc";

            if (searchString != null || statusFilter != null) {
                page = 1;
                redirectRequired = true;
            }
            else {
                searchString = currentSearchString;
                statusFilter = currentStatusFilter;
            }

            ViewBag.CurrentSearchString = searchString;


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

                case "projectStatus_desc":
                    projects = projects.OrderByDescending(s => s.ProjectStatus);
                    break;

                case "projectStatus_asc":
                    projects = projects.OrderBy(s => s.ProjectStatus);
                    break;

                case "invoiceDate_desc":
                    projects = projects.OrderByDescending(s => s.InvoiceDate);
                    break;

                case "invoiceDate_asc":
                    projects = projects.OrderBy(s => s.InvoiceDate);
                    break;

                case "salesStage_desc":
                    projects = projects.OrderByDescending(s => s.SalesStage);
                    break;

                default:
                    projects = projects.OrderBy(s => s.SalesStage);
                    break;

            }

            if (statusFilter != null && statusFilter != int.MinValue) {

                ProjectStatus? status = (ProjectStatus)Enum.ToObject(typeof(ProjectStatus), statusFilter);

                if (status != null) {

                    //invoices = invoices.Where(s => s.Status.Equals(status));
                    projects = projects.Where(s => s.ProjectStatus == status);

                }
            }

            List<SelectListItem> statuses = base.ToSelectList<ProjectStatus>(statusFilter);
            ViewBag.CurrentStatusFilter = statusFilter;
            ViewBag.StatusFilter = statuses;

            int pageNumber = (page ?? 1);

            if (redirectRequired) {
                return RedirectToAction("Index", "Project", new { currentSearchString = searchString, pageNumber = pageNumber, sortOrder = sortOrder, currentStatusFilter = statusFilter });
            }
            else {

                return View(projects.ToPagedList(pageNumber, Constants.PageSize));
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

            Project project = new Project();
            project.SalesStage = SalesStage.LookingGood;
            project.Location = Location.VIC;
            project.Branch = Branch.A1;
            project.ProjectStatus = ProjectStatus.Open;

            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "EmployeeName", employeeId);
            ViewBag.CustomerId = new SelectList(db.Customers, "ID", "CustomerName", customerId);
            return View(project);
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeId,CustomerId,ProjectName,ProjectStatus,Division,Location,Branch,InvoiceDate,SalesStage,PaymentTerms")] Project project) {

            if (ModelState.IsValid) {
                db.Projects.Add(project);
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "EmployeeName", project.EmployeeID);
            ViewBag.CustomerId = new SelectList(db.Customers, "ID", "CustomerName", project.CustomerID);
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
        public ActionResult Edit([Bind(Include = "ID,EmployeeId,CustomerId,ProjectName,ProjectStatus,Division,Location,Branch,InvoiceDate,SalesStage")] Project project) {

            if (ModelState.IsValid) {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "EmployeeName", project.EmployeeID);
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

            Project project = db.Projects.Find(id);

            StringBuilder sb = new StringBuilder();

            sb.Append("<html><body>Quote<br><img src='\\A1Letterhead.png' /><br>");

            sb.Append("<table width=100% border=1>");
            foreach (Task task in project.Tasks) {
                sb.Append("<tr><td>");
                sb.Append(task.Description);
                sb.Append(task.FullText);
                sb.Append("</td></tr>");
            }

            sb.Append("</table>");
            sb.Append("</body></html>");

            IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
            //            Renderer.RenderHtmlAsPdf(sb.ToString()).SaveAs("html-string.pdf");
            Renderer.PrintOptions.MarginTop = 40;  //millimeters
            Renderer.PrintOptions.MarginLeft = 20;  //millimeters
            Renderer.PrintOptions.MarginRight = 20;  //millimeters
            Renderer.PrintOptions.MarginBottom = 40;  //millimeters
            Renderer.PrintOptions.DPI = 300;
            var PDF = Renderer.RenderHtmlAsPdf(sb.ToString(), "");

            Attachment attachment = new Attachment();
            attachment.ContentType = "application/pdf";
            attachment.FileType = FileType.Quotation;
            attachment.Project = project;
            attachment.Content = PDF.BinaryData;// res;
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

                case 3: // Invoices
                    return PartialView("_Invoices", project);

                default:
                    var entry = db.Entry(project);
                    entry.Collection(e => e.Tasks)
                         .Query()
                         .OrderBy(c => c.Sequence)
                         .Load();
                    return PartialView("_Tasks", project);

            }

        }

    }

}
