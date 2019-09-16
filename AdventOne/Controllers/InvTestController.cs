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
    public class InvTestController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: InvTest
        public ActionResult Index()
        {
            return View(db.InvTests.ToList());
        }

        // GET: InvTest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvTest invTest = db.InvTests.Find(id);
            if (invTest == null)
            {
                return HttpNotFound();
            }
            return View(invTest);
        }

        // GET: InvTest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvTest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceID,InvoiceNumber,InvoiceDate")] InvTest invTest)
        {
            if (ModelState.IsValid)
            {
                db.InvTests.Add(invTest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invTest);
        }

        // GET: InvTest/Edit/5
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Invoice invoice = db.Invoices.Find(id);  
            InvTest invoice = db.InvTests.Include(i => i.InvoiceLines)
            .Where(c => c.InvoiceID == id).FirstOrDefault();

            if (invoice == null) {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: InvTest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //                            <input data-val="true" data-val-number="The field InvoiceLineId must be a number." data-val-required="The InvoiceLineId field is required." id="InvoiceLines_4__InvoiceLineId" name="InvoiceLines[4].InvoiceLineId" type="hidden" value="" />
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceID,InvoiceNumber,InvoiceDate,InvoiceLines")] InvTest invTest) { 
            if (ModelState.IsValid) {

                Dictionary<int, InvLineTest> invoiceLines = new Dictionary<int, InvLineTest>();
                Dictionary<int, InvLineTest> removeLines = new Dictionary<int, InvLineTest>();

                // Update new rows ..
                foreach (InvLineTest invLineTest in invTest.InvoiceLines) {

                    if (invLineTest.InvoiceLineId == -1) {
                        db.InvLineTests.Add(invLineTest);
                        db.SaveChanges();
                    }

                    invoiceLines.Add(invLineTest.InvoiceLineId, invLineTest);

                }

                // Remove deleted rows ..

                var invLineTests = from s in db.InvLineTests
                                  select s;
                invLineTests = invLineTests.Where(s => s.InvoiceID == invTest.InvoiceID);

                foreach (InvLineTest invLineTest in invLineTests) {

                    if (!invoiceLines.ContainsKey(invLineTest.InvoiceLineId)) {
                        db.InvLineTests.Remove(invLineTest);

                    }
                    else {
                        db.Entry(invLineTest).State = EntityState.Detached;
                    }
                }

                db.SaveChanges();

                db.Entry(invTest).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");

            }

            return View(invTest);

        }

        // GET: InvTest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvTest invTest = db.InvTests.Find(id);
            if (invTest == null)
            {
                return HttpNotFound();
            }
            return View(invTest);
        }

        // POST: InvTest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvTest invTest = db.InvTests.Find(id);
            db.InvTests.Remove(invTest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Task/Delete/5
        public PartialViewResult AppendLine(int maxRowId, int invoiceId) {

            InvLineTest invLineTest = new InvLineTest();

            invLineTest.InvoiceID = invoiceId;
            invLineTest.InvoiceLineId = -1;
            @ViewBag.RowId = maxRowId;

            return PartialView("_InvoiceLine", invLineTest);

        }

        public PartialViewResult ReplaceLine(int rowId, int invoiceId, int invoiceLineItemId, String itemDescription, String itemName, int quantity, decimal value) {

            InvLineTest invLineTest = new InvLineTest();

            invLineTest.ItemName = itemName;
            invLineTest.ItemDescription = itemDescription;
            invLineTest.Quantity = quantity;
            invLineTest.Value = value;
            invLineTest.InvoiceID = invoiceId;
            invLineTest.InvoiceLineId = invoiceLineItemId;
            @ViewBag.RowId = rowId;

            return PartialView("_InvoiceLine", invLineTest);

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
