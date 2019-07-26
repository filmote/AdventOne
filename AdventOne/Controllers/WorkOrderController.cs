using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AdventOne.Controllers;
using AdventOne.DAL;
using AdventOne.Models;

namespace AdventOne.Views {

    public class WorkOrderController : BaseController {

        private ProjectContext db = new ProjectContext();

        // GET: WorkOrder
        public ActionResult Index() {
            var workOrders = db.WorkOrders.Include(w => w.Project);
            return View(workOrders.ToList());
        }

        // GET: WorkOrder/Details/5
        public ActionResult Details(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();

            WorkOrder workOrder = db.WorkOrders.Find(id);

            if (workOrder == null) {
                return HttpNotFound();
            }

            return View(workOrder);

        }

        // GET: WorkOrder/Create
        public ActionResult Create(int? projectId) {

            base.SessionHandleOtherActions();
            ViewBag.ProjectID = projectId;
            return View();

        }

        // POST: WorkOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,ProjectID")] WorkOrder workOrder) {
            if (ModelState.IsValid) {
                db.WorkOrders.Add(workOrder);
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL()); 
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", workOrder.ProjectID);
            return View(workOrder);
        }

        // GET: WorkOrder/Edit/5
        public ActionResult Edit(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            WorkOrder workOrder = db.WorkOrders.Find(id);

            if (workOrder == null) {
                return HttpNotFound();
            }

            ViewBag.ProjectID = workOrder.Project.ID;
            return View(workOrder);
        }

        // POST: WorkOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Description,ProjectID")] WorkOrder workOrder) {
            if (ModelState.IsValid) {
                db.Entry(workOrder).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", workOrder.ProjectID);
            return View(workOrder);
        }

        // GET: WorkOrder/Delete/5
        public ActionResult Delete(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            WorkOrder workOrder = db.WorkOrders.Find(id);

            if (workOrder == null) {
                return HttpNotFound();
            }

            return View(workOrder);

        }

        // POST: WorkOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            WorkOrder workOrder = db.WorkOrders.Find(id);
            db.WorkOrders.Remove(workOrder);
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
