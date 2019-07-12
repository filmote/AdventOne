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
    public class TaskController : BaseController
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

            base.sessionHandleOtherActions();

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
        public ActionResult Create(int? projectId) {

            base.sessionHandleOtherActions();
            ViewBag.ProjectID = projectId;
            return View();

        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,Description,FullText,RevenueType,Price")] Task task) {

            if (ModelState.IsValid) {

                Project project = db.Projects.Find(task.ProjectID);
                task.Sequence = project.Tasks.Count();
                db.Tasks.Add(task);
                db.SaveChanges();

                decimal revenue = 0M;
                decimal cos = 0M;
                decimal margin = 0M;

                foreach (Task newTask in project.Tasks) {

                    switch (newTask.RevenueType) {

                        case RevenueType.REV:
                            revenue = revenue + newTask.Price;
                            margin = margin + newTask.Price;
                            break;

                        case RevenueType.COS:
                            cos = cos + newTask.Price;
                            margin = margin - newTask.Price;
                            break;

                    }

                }

                project.Revenue = revenue;
                project.COS = cos;
                project.Margin = margin;
                db.SaveChanges();

                return Redirect(base.sessionGetReturnURL());
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "ProjectName", task.ProjectID);
            return View(task);

        }

        // GET: Task/Edit/5
        public ActionResult Edit(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.sessionHandleOtherActions();
            Task task = db.Tasks.Find(id);

            if (task == null) {
                return HttpNotFound();
            }

            ViewBag.ProjectID = task.Project.ID;
            return View(task);

        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectID,Description,FullText,Sequence,RevenueType,Price")] Task task)
        {
            if (ModelState.IsValid)  {

                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

                decimal revenue = 0M;
                decimal cos = 0M;
                decimal margin = 0M;

                Project project = db.Projects.Find(task.ProjectID);
                foreach(Task newTask in project.Tasks) {

                    switch (newTask.RevenueType) {

                        case RevenueType.REV:
                            revenue = revenue + newTask.Price;
                            margin = margin + newTask.Price;
                            break;

                        case RevenueType.COS:
                            cos = cos + newTask.Price;
                            margin = margin - newTask.Price;
                            break;

                    }

                }

                project.Revenue = revenue;
                project.COS = cos;
                project.Margin = margin;
                db.SaveChanges();

                return Redirect(base.sessionGetReturnURL());
            }

            return View(task);

        }

        // GET: Task/Delete/5
        public ActionResult Delete(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.sessionHandleOtherActions();
            Task task = db.Tasks.Find(id);

            if (task == null) {
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

            decimal revenue = 0M;
            decimal cos = 0M;
            decimal margin = 0M;
            Project project = db.Projects.Find(task.ProjectID);

            foreach (Task newTask in project.Tasks) {

                switch (newTask.RevenueType) {

                    case RevenueType.REV:
                        revenue = revenue + newTask.Price;
                        margin = margin + newTask.Price;
                        break;

                    case RevenueType.COS:
                        cos = cos + newTask.Price;
                        margin = margin - newTask.Price;
                        break;

                }

            }

            project.Revenue = revenue;
            project.COS = cos;
            project.Margin = margin;
            db.SaveChanges();

            project = db.Projects.Find(task.ProjectID);
            return Redirect(base.sessionGetReturnURL());

        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(string action, int id) {
            Task task = db.Tasks.Find(id);
            Project project = db.Projects.Find(task.ProjectID);
            return Redirect(base.sessionGetReturnURL());

        }

        // GET: Task/MoveUp
        public ActionResult MoveUp(int projectId, int taskId) {

            Project project = db.Projects.Find(projectId);
            if (project == null) {
                return HttpNotFound();
            }

            Task taskToMove = db.Tasks.Find(taskId);
            if (project == null) {
                return HttpNotFound();
            }

            foreach (Task task in project.Tasks) {

                if (task.Sequence == taskToMove.Sequence - 1) {

                    task.Sequence = taskToMove.Sequence;
                    taskToMove.Sequence = task.Sequence - 1;
                    db.SaveChanges();

                }

            }

            return RedirectToAction("Details", "Project", new { id = projectId });

        }

        // GET: Task/MoveDown
        public ActionResult MoveDown(int projectId, int taskId) {

            Project project = db.Projects.Find(projectId);
            if (project == null) {
                return HttpNotFound();
            }

            Task taskToMove = db.Tasks.Find(taskId);
            if (project == null) {
                return HttpNotFound();
            }

            foreach (Task task in project.Tasks) {

                if (task.Sequence == taskToMove.Sequence + 1) {

                    taskToMove.Sequence = task.Sequence;
                    task.Sequence = taskToMove.Sequence - 1;
                    db.SaveChanges();
                    break;

                }

            }

            return RedirectToAction("Details", "Project", new { id = projectId });

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
