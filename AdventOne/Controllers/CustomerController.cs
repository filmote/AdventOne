using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using AdventOne.DAL;
using AdventOne.Models;
using AdventOne.Models.View;
using PagedList;

namespace AdventOne.Controllers {
    public class CustomerController : BaseController {

        private readonly ProjectContext db = new ProjectContext();

        // GET: Customer
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page) {

            bool redirectRequired = false;

            base.SessionHandleIndexAction();

            ViewBag.CurrentSort = sortOrder ?? "";
            ViewBag.EmployeeSortParm = sortOrder == "employee_asc" ? "employee_desc" : "employee_asc";
            ViewBag.CustomerSortParm = sortOrder == "customer_asc" ? "customer_desc" : "customer_asc";

            if (searchString != null) {
                page = 1;
                redirectRequired = true;
            }
            else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var customers = from s in db.Customers
                            select s;

            if (!String.IsNullOrEmpty(searchString)) {
                customers = customers.Where(s => s.Employee.EmployeeName.Contains(searchString) || s.CustomerName.Contains(searchString));
            }

            switch (sortOrder) {

                case "employee_desc":
                    customers = customers.OrderByDescending(s => s.Employee.EmployeeName);
                    break;

                case "employee_asc":
                    customers = customers.OrderBy(s => s.Employee.EmployeeName);
                    break;

                case "customer_desc":
                    customers = customers.OrderByDescending(s => s.CustomerName);
                    break;

                default:
                    customers = customers.OrderBy(s => s.CustomerName);
                    break;
            }

            int pageNumber = (page ?? 1);

            if (redirectRequired) {
                return RedirectToAction("Index", "Customer", new { currentFilter = searchString, pageNumber = pageNumber, sortOrder = sortOrder });
            }
            else {
                return View(customers.ToPagedList(pageNumber, Constants.PageSize));
            }

        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();

            Customer customer = db.Customers.Find(id);
            if (customer == null) {
                return HttpNotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public ActionResult Create() {

            HttpSessionStateBase session = (HttpSessionStateBase)HttpContext.Session;
            base.SessionHandleOtherActions();
            Employee employee = (Employee)session["employee"];
            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "EmployeeName", employee);
            return View();

        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeID,CustomerName")] Customer customer) {
            if (ModelState.IsValid) {
                db.Customers.Add(customer);
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }

            return View(customer);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Customer customer = db.Customers.Find(id);

            if (customer == null) {
                return HttpNotFound();
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "ID", "EmployeeName", customer.EmployeeID);
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CustomerName,EmployeeID")] Customer customer) {

            if (ModelState.IsValid) {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }

            return View(customer);

        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.Find(id);

            if (customer == null) {
                return HttpNotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {

            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return Redirect(base.SessionGetReturnURL());

        }

        public JsonResult GetPaymentTerms(int id) {

            Customer customer = db.Customers.Find(id);
            PaymentTerm paymentTerm = customer.PaymentTerm;

            if (paymentTerm == null) {
                paymentTerm = db.PaymentTerms.SingleOrDefault(s => s.Code == "NET14DAYS");
            }

            var j = "{\"id\":" + paymentTerm.ID + "}";
            return Json(j, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Search(string term) {

            if (term == null)
                term = "";

            var customers = from c in db.Customers
                            where c.CustomerName.Contains(term)
                            orderby c.CustomerName
                            select new AutoCompleteEntry {
                                ID = c.ID,
                                Label = c.CustomerName
                            };

            return Json(customers.ToList());

        }

        [HttpGet]
        public PartialViewResult SearchDialog(String searchTerm, bool resultsOnly) {

            if (searchTerm == null) searchTerm = "";

            var customers = from c in db.Customers
                            where c.CustomerName.Contains(searchTerm)
                            orderby c.CustomerName 
                            select new AutoCompleteEntry {
                                ID = c.ID,
                                Label = c.CustomerName
                            };

            ViewBag.Customers = customers.Take(5);
            ViewBag.ResultsOnly = resultsOnly;

            return PartialView("_CustomerSearch");

        }

        protected override void Dispose(bool disposing) {

            if (disposing) {
                db.Dispose();
            }

            base.Dispose(disposing);

        }

    }

}
