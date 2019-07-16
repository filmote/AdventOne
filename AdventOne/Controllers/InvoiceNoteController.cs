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

namespace AdventOne.Controllers
{
    public class InvoiceNoteController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: InvoiceNote
        public ActionResult Index()
        {
            var invoiceNotes = db.InvoiceNotes.Include(i => i.Invoice);
            return View(invoiceNotes.ToList());
        }

        // GET: InvoiceNote/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceNote invoiceNote = db.InvoiceNotes.Find(id);
            if (invoiceNote == null)
            {
                return HttpNotFound();
            }
            return View(invoiceNote);
        }

        // GET: InvoiceNote/Create
        public ActionResult Create()
        {
            ViewBag.InvoiceID = new SelectList(db.Invoices, "ID", "InvoiceNumber");
            return View();
        }

        // POST: InvoiceNote/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InvoiceID,Notes,OriginalExpectedPayDate,DueDate,RevisedExpectedPaymentDate")] InvoiceNote invoiceNote)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceNotes.Add(invoiceNote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InvoiceID = new SelectList(db.Invoices, "ID", "InvoiceNumber", invoiceNote.InvoiceID);
            return View(invoiceNote);
        }

        // GET: InvoiceNote/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceNote invoiceNote = db.InvoiceNotes.Find(id);
            if (invoiceNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvoiceID = new SelectList(db.Invoices, "ID", "InvoiceNumber", invoiceNote.InvoiceID);
            return View(invoiceNote);
        }

        // POST: InvoiceNote/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InvoiceID,Notes,OriginalExpectedPayDate,DueDate,RevisedExpectedPaymentDate")] InvoiceNote invoiceNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoiceNote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvoiceID = new SelectList(db.Invoices, "ID", "InvoiceNumber", invoiceNote.InvoiceID);
            return View(invoiceNote);
        }

        // GET: InvoiceNote/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceNote invoiceNote = db.InvoiceNotes.Find(id);
            if (invoiceNote == null)
            {
                return HttpNotFound();
            }
            return View(invoiceNote);
        }

        // POST: InvoiceNote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvoiceNote invoiceNote = db.InvoiceNotes.Find(id);
            db.InvoiceNotes.Remove(invoiceNote);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
