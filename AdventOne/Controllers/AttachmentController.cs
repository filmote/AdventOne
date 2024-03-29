﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdventOne.DAL;
using AdventOne.Models;
using AdventOne.Utils;

namespace AdventOne.Controllers {
    public class AttachmentController : BaseController
    {
        private ProjectContext db = new ProjectContext();

        // GET: Attachment
        public ActionResult Index()
        {
            base.SessionHandleIndexAction();
            var attachments = db.Attachments.Include(a => a.Project);
            return View(attachments.ToList());
        }

        // GET: Attachment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Attachment attachment = db.Attachments.Find(id);

            if (attachment == null) {
                return HttpNotFound();
            }
            return View(attachment);
        }

        // GET: Attachment/Create
        public ActionResult Create(int? projectId) {
            // ViewBag.CustomerId = new SelectList(db.Customers, "ID", "CustomerName");
            base.SessionHandleOtherActions();
            ViewBag.ProjectId = projectId;
            return View();
        }

        // POST: Attachment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Description,FileName,FileType,ContentType,ProjectId")] Attachment attachment, HttpPostedFileBase upload) {

            if (ModelState.IsValid) {

                attachment.FileName = System.IO.Path.GetFileName(upload.FileName);
                //Attachment avatar = null;
                //if (upload != null && upload.ContentLength > 0) {
                //    avatar = new Attachment 
                //        FileName = System.IO.Path.GetFileName(upload.FileName),
                //        FileType = FileType.Quotation,
                //        ContentType = upload.ContentType
                //    };
                //    using (var reader = new System.IO.BinaryReader(upload.InputStream)) {
                //        avatar.Content = reader.ReadBytes(upload.ContentLength);
                //    }

                using (var reader = new System.IO.BinaryReader(upload.InputStream)) {
                    attachment.Content = reader.ReadBytes(upload.ContentLength);
                }

                attachment.ContentType = MimeType.GetMimeMapping(upload.FileName);

                db.Attachments.Add(attachment);
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }

        

            return View();
        }

        // GET: Attachment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Attachment attachment = db.Attachments.Find(id);

            if (attachment == null)  {
                return HttpNotFound();
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", attachment.ProjectId);
            return View(attachment);
        }

        // POST: Attachment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName,ContentType,Content,FileType,ProjectId")] Attachment attachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attachment).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect(base.SessionGetReturnURL());
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "ID", "ProjectName", attachment.ProjectId);
            return View(attachment);
        }

        // GET: Attachment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            base.SessionHandleOtherActions();
            Attachment attachment = db.Attachments.Find(id);

            if (attachment == null) {
                return HttpNotFound();
            }

            return View(attachment);

        }

        // POST: Attachment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Attachment attachment = db.Attachments.Find(id);
            db.Attachments.Remove(attachment);
            db.SaveChanges();
            return Redirect(base.SessionGetReturnURL());
        }

        public ActionResult Download(int id) {
            Attachment attachment = db.Attachments.Find(id);
            return File(attachment.Content, attachment.ContentType, attachment.FileName);
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
