using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RoyayaControlPanel.com.Models;

namespace RoyayaControlPanel.com.Controllers
{
    public class PublicInterpreterRatiosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PublicInterpreterRatios
        public ActionResult Index()
        {
            var publicInterpreterRatios = db.PublicInterpreterRatios.Include(p => p.path);
            return View(publicInterpreterRatios.ToList());
        }

        // GET: PublicInterpreterRatios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicInterpreterRatio publicInterpreterRatio = db.PublicInterpreterRatios.Find(id);
            if (publicInterpreterRatio == null)
            {
                return HttpNotFound();
            }
            return View(publicInterpreterRatio);
        }

        // GET: PublicInterpreterRatios/Create
        public ActionResult Create()
        {
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Cost");
            return View();
        }

        // POST: PublicInterpreterRatios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( PublicInterpreterRatio publicInterpreterRatio)
        {
            if (ModelState.IsValid)
            {
                publicInterpreterRatio.CreationDate = DateTime.Now;
                publicInterpreterRatio.LastModificationDate = DateTime.Now;
                db.PublicInterpreterRatios.Add(publicInterpreterRatio);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Status", publicInterpreterRatio.pathId);
            return View(publicInterpreterRatio);
        }

        // GET: PublicInterpreterRatios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicInterpreterRatio publicInterpreterRatio = db.PublicInterpreterRatios.Find(id);
            if (publicInterpreterRatio == null)
            {
                return HttpNotFound();
            }
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Cost", publicInterpreterRatio.pathId);
            return View(publicInterpreterRatio);
        }

        // POST: PublicInterpreterRatios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( PublicInterpreterRatio publicInterpreterRatio)
        {
            if (ModelState.IsValid)
            {
                PublicInterpreterRatio temp = db.PublicInterpreterRatios.Find(publicInterpreterRatio.id);
                temp.LastModificationDate = DateTime.Now;
                db.Entry(publicInterpreterRatio).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Status", publicInterpreterRatio.pathId);
            return View(publicInterpreterRatio);
        }

        // GET: PublicInterpreterRatios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublicInterpreterRatio publicInterpreterRatio = db.PublicInterpreterRatios.Find(id);
            if (publicInterpreterRatio == null)
            {
                return HttpNotFound();
            }
            return View(publicInterpreterRatio);
        }

        // POST: PublicInterpreterRatios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PublicInterpreterRatio publicInterpreterRatio = db.PublicInterpreterRatios.Find(id);
            db.PublicInterpreterRatios.Remove(publicInterpreterRatio);
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
