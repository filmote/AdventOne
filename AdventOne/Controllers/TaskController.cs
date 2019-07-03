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
    public class TaskController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Task
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.Project);
            return View(tasks.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Stack<String> referrers = (Stack<String>)HttpContext.Session["referrers"];
            referrers.Push(this.Request.RawUrl);


            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Task/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName");
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,Description,FullText")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();

                Project project = db.Projects.Find(task.ProjectID);
                return RedirectToAction("Details", "Project", new { id = task.ProjectID });
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", task.ProjectID);
            return View(task);
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", task.ProjectID);
            return View(task);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectID,Description,FullText")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

                Project project = db.Projects.Find(task.ProjectID);
                return RedirectToAction("Details", "Project", new { id = task.ProjectID });
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", task.ProjectID);
            return View(task);
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();

            Project project = db.Projects.Find(task.ProjectID);
            return RedirectToAction("Details", "Project", new { id = task.ProjectID });

            //            return RedirectToAction("Index");
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(string action, int id) {
            Task task = db.Tasks.Find(id);
            Project project = db.Projects.Find(task.ProjectID);
            return RedirectToAction("Details", "Project", new { id = task.ProjectID });

            //            return RedirectToAction("Index");
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
