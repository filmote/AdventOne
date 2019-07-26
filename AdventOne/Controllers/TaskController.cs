using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AdventOne.DAL;
using AdventOne.Models;
using AdventOne.Models.View;

namespace AdventOne.Controllers {

    public class TaskController : BaseController {

        private ProjectContext db = new ProjectContext();

        // GET: Task
        public ActionResult Index() {
            var tasks = db.Tasks.Include(t => t.Project);
            return View(tasks.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();

            Task task = db.Tasks.Find(id);
            //task.PopulateFields();

            if (task == null) {
                return HttpNotFound();
            }

            return View(task);

        }

        // GET: Task/Create
        public ActionResult Create(int? projectId) {

            base.SessionHandleOtherActions();
            Project project = db.Projects.Find(projectId);
            ViewBag.Project = project;

            Task task = new Task();
            task.SalesStage = project.SalesStage;
            task.InvoiceDate = project.InvoiceDate;

            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "SupplierName");
            return View(task);

        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProjectID,Description,FullText,RevenueType,ProductQuantity,ProductUnitPrice,HourlyRate,ServicesQuantity,NumberOfHours,SalesStage,InvoiceDate,SupplierID")] Task task) {

            Project project = db.Projects.Find(task.ProjectID);

            // Handle different revenue types ..

            task.CalculateFields(project);

            ModelState.Remove("Quantity");
            ModelState.Remove("UnitPrice");
            ModelState.Remove("ExtendedPrice");

            if (ModelState.IsValid) {

                task.Sequence = project.Tasks.Count();
                db.Tasks.Add(task);
                db.SaveChanges();

                decimal revenue = 0M;
                decimal cos = 0M;
                decimal margin = 0M;

                foreach (Task newTask in project.Tasks) {

                    switch (newTask.RevenueType) {

                        case RevenueType.REV:
                        case RevenueType.SVC:
                            revenue += newTask.ExtendedPrice;
                            margin += newTask.ExtendedPrice;
                            break;

                        case RevenueType.COS:
                            cos += newTask.ExtendedPrice;
                            margin -= newTask.ExtendedPrice;
                            break;

                    }

                }

                project.Revenue = revenue;
                project.COS = cos;
                project.Margin = margin;
                db.SaveChanges();

                return Redirect(base.SessionGetReturnURL());
            }

            ViewBag.Project = project;
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "SupplierName", task.SupplierID);
            return View(task);

        }

        // GET: Task/Edit/5
        public ActionResult Edit(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Task task = db.Tasks.Find(id);

            if (task == null) {
                return HttpNotFound();
            }

            //task.PopulateFields();

            ViewBag.Project = task.Project;
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "SupplierName", task.SupplierID);

            return View(task);

        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProjectID,Description,FullText,RevenueType,ProductQuantity,ProductUnitPrice,HourlyRate,ServicesQuantity,NumberOfHours,SalesStage,InvoiceDate,SupplierID")] Task task) {

            Project project = db.Projects.Find(task.ProjectID);


            // Handle different revenue types ..

            task.CalculateFields(project);

           //ModelState.Remove("Quantity");
            //ModelState.Remove("UnitPrice");
            //ModelState.Remove("ExtendedPrice");

            if (ModelState.IsValid) {

                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();

                decimal revenue = 0M;
                decimal cos = 0M;
                decimal margin = 0M;

                foreach (Task newTask in project.Tasks) {

                    switch (newTask.RevenueType) {

                        case RevenueType.REV:
                        case RevenueType.SVC:
                            revenue += newTask.ExtendedPrice;
                            margin += newTask.ExtendedPrice;
                            break;

                        case RevenueType.COS:
                            cos += newTask.ExtendedPrice;
                            margin -= newTask.ExtendedPrice;
                            break;

                    }

                }

                project.Revenue = revenue;
                project.COS = cos;
                project.Margin = margin;
                db.SaveChanges();

                return Redirect(base.SessionGetReturnURL());
            }

            ViewBag.Project = project;
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "SupplierName", task.SupplierID);
            return View(task);

        }

        // GET: Task/Delete/5
        public ActionResult Delete(int? id) {

            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
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
            int sequence = task.Sequence;

            db.Tasks.Remove(task);
            db.SaveChanges();

            decimal revenue = 0M;
            decimal cos = 0M;
            decimal margin = 0M;
            Project project = db.Projects.Find(task.ProjectID);

            foreach (Task newTask in project.Tasks) {

                switch (newTask.RevenueType) {

                    case RevenueType.REV:
                    case RevenueType.SVC:
                        revenue += newTask.ExtendedPrice;
                        margin += newTask.ExtendedPrice;
                        break;

                    case RevenueType.COS:
                        cos += newTask.ExtendedPrice;
                        margin -= newTask.ExtendedPrice;
                        break;

                }

                if (newTask.Sequence > sequence) {

                    newTask.Sequence--;
                    newTask.CalculateFields(project);
                    db.Entry(newTask).State = EntityState.Modified;

                }

            }

            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();

            project.Revenue = revenue;
            project.COS = cos;
            project.Margin = margin;
            db.SaveChanges();

            return Redirect(base.SessionGetReturnURL());

        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(string action, int id) {

            return Redirect(base.SessionGetReturnURL());

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
                    db.Configuration.ValidateOnSaveEnabled = false;
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

                    task.Sequence = taskToMove.Sequence;
                    taskToMove.Sequence = task.Sequence + 1;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    break;

                }

            }

            return RedirectToAction("Details", "Project", new { id = projectId });

        }

        // GET: Task/Delete/5
        public PartialViewResult Duplicate(int id) {

            base.SessionHandleOtherActions();
            TaskDuplicate taskDuplicate = new TaskDuplicate();
            taskDuplicate.ID = id;
            taskDuplicate.Quantity = 24;

            return PartialView("_Duplicate", taskDuplicate);
        }

        // GET: Task/Delete/5
        [HttpPost]
        public ActionResult Duplicate(int id, int quantity, int taskFrequency) {

            base.SessionHandleOtherActions();
            Task task = db.Tasks.Find(id);


            int sequence = task.Project.Tasks.Count();

            for (int x = 0; x < quantity; x++) {

                Task newTask = (Task)task.Clone();

                switch ((TaskFrequency)taskFrequency) {

                    case TaskFrequency.Monthly:
                        newTask.InvoiceDate = task.InvoiceDate.AddMonths(x + 1);
                        break;

                    case TaskFrequency.Quarterly:
                        newTask.InvoiceDate = task.InvoiceDate.AddMonths(3*(x + 1));
                        break;

                    case TaskFrequency.Yearly:
                        newTask.InvoiceDate = task.InvoiceDate.AddYears(x + 1);
                        break;

                }

                newTask.CalculateFields(task.Project);

                newTask.Sequence = sequence;
                sequence++;
                db.Tasks.Add(newTask);

            }

            db.SaveChanges();

            decimal revenue = 0M;
            decimal cos = 0M;
            decimal margin = 0M;

            Project project = db.Projects.Find(task.ProjectID);
            foreach (Task newTask in project.Tasks) {

                switch (newTask.RevenueType) {

                    case RevenueType.REV:
                    case RevenueType.SVC:
                        revenue += newTask.ExtendedPrice;
                        margin += newTask.ExtendedPrice;
                        break;

                    case RevenueType.COS:
                        cos += newTask.ExtendedPrice;
                        margin -= newTask.ExtendedPrice;
                        break;

                }

            }

            project.Revenue = revenue;
            project.COS = cos;
            project.Margin = margin;
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
