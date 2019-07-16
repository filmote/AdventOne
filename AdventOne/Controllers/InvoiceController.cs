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

namespace AdventOne.Controllers {

    public class InvoiceController : BaseController {

        private ProjectContext db = new ProjectContext();

        // GET: Invoice
        public ActionResult Index() {
            base.SessionHandleIndexAction();

            var invoices = db.Invoices.Include(i => i.Project);
            return View(invoices.ToList());
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
        public ActionResult Create() {
            base.SessionHandleOtherActions();

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName");
            return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,InvoiceNumber,InvoiceDate,DueDate,ExpectedPaymentDate")] Invoice invoice) {
            if (ModelState.IsValid) {
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
        public ActionResult Edit([Bind(Include = "ID,ProjectID,InvoiceNumber,InvoiceDate,DueDate,ExpectedPaymentDate")] Invoice invoice) {
            if (ModelState.IsValid) {
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
