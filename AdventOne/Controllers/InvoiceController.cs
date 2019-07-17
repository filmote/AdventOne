using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using AdventOne.DAL;
using AdventOne.Models;
using PagedList;

namespace AdventOne.Controllers {

    public class InvoiceController : BaseController {

        private ProjectContext db = new ProjectContext();

        // GET: Invoice
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page) {

            bool redirectRequired = false;
            base.SessionHandleIndexAction();

            IPrincipal user = HttpContext.User;

            sortOrder = sortOrder ?? "employee_asc";
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmployeeSortParm = sortOrder == "employee_asc" ? "employee_desc" : "employee_asc";
            ViewBag.CustomerSortParm = sortOrder == "customer_asc" ? "customer_desc" : "customer_asc";
            ViewBag.ProjectSortParm = sortOrder == "project_asc" ? "project_desc" : "project_asc";
            ViewBag.StatusSortParm = sortOrder == "status_asc" ? "status_desc" : "status_asc";
            ViewBag.InvoiceSortParm = sortOrder == "invoice_asc" ? "invoice_desc" : "invoice_asc";
            ViewBag.AmountSortParm = sortOrder == "amount_asc" ? "amount_desc" : "amount_asc";
            ViewBag.InvoiceDateSortParm = sortOrder == "invoicedate_asc" ? "invoicedate_desc" : "invoicedate_asc";
            ViewBag.DueDateSortParm = sortOrder == "duedate_asc" ? "duedate_desc" : "duedate_asc";
            ViewBag.ExpectedPaymentSortParm = sortOrder == "expectedpayment_asc" ? "expectedpayment_desc" : "expectedpayment_asc";
            ViewBag.DaysOverdueSortParm = sortOrder == "daysoverdue_asc" ? "daysoverdue_desc" : "daysoverdue_asc";

            if (searchString != null) {
                page = 1;
                redirectRequired = true;
            }
            else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var invoices = from s in db.Invoices
                           select s;

            if (!String.IsNullOrEmpty(searchString)) {
                invoices = invoices.Where(s => s.Project.Employee.EmployeeName.Contains(searchString) || s.Project.Customer.CustomerName.Contains(searchString) || s.Project.ProjectName.Contains(searchString));
            }

            switch (sortOrder) {

                case "employee_desc":
                    invoices = invoices.OrderByDescending(s => s.Project.Employee.EmployeeName);
                    break;

                case "employee_asc":
                    invoices = invoices.OrderBy(s => s.Project.Employee.EmployeeName);
                    break;

                case "customer_desc":
                    invoices = invoices.OrderByDescending(s => s.Project.Customer.CustomerName);
                    break;

                case "customer_asc":
                    invoices = invoices.OrderBy(s => s.Project.Customer.CustomerName);
                    break;

                case "project_desc":
                    invoices = invoices.OrderByDescending(s => s.Project.ProjectName);
                    break;

                case "project_asc":
                    invoices = invoices.OrderBy(s => s.Project.ProjectName);
                    break;

                case "status_desc":
                    invoices = invoices.OrderByDescending(s => s.Status);
                    break;

                case "status_asc":
                    invoices = invoices.OrderBy(s => s.Status);
                    break;

                case "invoice_desc":
                    invoices = invoices.OrderByDescending(s => s.InvoiceNumber);
                    break;

                case "invoice_asc":
                    invoices = invoices.OrderBy(s => s.InvoiceNumber);
                    break;

                case "amount_desc":
                    invoices = invoices.OrderByDescending(s => s.Amount);
                    break;

                case "amount_asc":
                    invoices = invoices.OrderBy(s => s.Amount);
                    break;

                case "invoicedate_desc":
                    invoices = invoices.OrderByDescending(s => s.InvoiceDate);
                    break;

                case "invoicedate_asc":
                    invoices = invoices.OrderBy(s => s.InvoiceDate);
                    break;

                case "duedate_desc":
                    invoices = invoices.OrderByDescending(s => s.DueDate);
                    break;

                case "duedate_asc":
                    invoices = invoices.OrderBy(s => s.DueDate);
                    break;

                case "expectedpayment_desc":
                    invoices = invoices.OrderByDescending(s => s.ExpectedPaymentDate);
                    break;

                case "expectedpayment_asc":
                    invoices = invoices.OrderBy(s => s.ExpectedPaymentDate);
                    break;

                case "daysoverdue_desc":
                    invoices = invoices.OrderByDescending(s => s.DaysOverdue);
                    break;

                case "daysoverdue_asc":
                    invoices = invoices.OrderBy(s => s.DaysOverdue);
                    break;



            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            if (redirectRequired) {
                return RedirectToAction("Index", "Invoice", new { currentFilter = searchString, pageNumber = pageNumber, sortOrder = sortOrder });
            }
            else {

                return View(invoices.ToPagedList(pageNumber, pageSize));
            }

        }

        // GET: Invoice/Details/5
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Invoice invoice = db.Invoices.Find(id);

            if (invoice == null) {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoice/Create
        public ActionResult Create(int? projectId) {

            base.SessionHandleOtherActions();

            if (projectId != null) {
                Project project = db.Projects.Find(projectId);
                ViewBag.Project = project;
            }
            else {
                ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName");
            }
            return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,InvoiceNumber,InvoiceDate,DueDate,ExpectedPaymentDate,Amount")] Invoice invoice) {

            if (ModelState.IsValid) {

                if (invoice.Status == InvoiceStatus.Open) {
                    invoice.DaysOverdue = ((int)(invoice.ExpectedPaymentDate - invoice.DueDate).Days);
                }
                else {
                    invoice.DaysOverdue = 0;
                }

                db.Invoices.Add(invoice);
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", invoice.ProjectID);
            return View(invoice);
        }

        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Invoice invoice = db.Invoices.Find(id);

            if (invoice == null) {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", invoice.ProjectID);
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectID,InvoiceNumber,InvoiceDate,DueDate,ExpectedPaymentDate,Status,Amount")] Invoice invoice) {
            if (ModelState.IsValid) {

                if (invoice.Status == InvoiceStatus.Open) {
                    invoice.DaysOverdue = ((int)(invoice.ExpectedPaymentDate - invoice.DueDate).Days);
                }
                else {
                    invoice.DaysOverdue = 0;
                }

                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", invoice.ProjectID);
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Invoice invoice = db.Invoices.Find(id);

            if (invoice == null) {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return Redirect(base.SessionGetReturnURL());
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }

}
