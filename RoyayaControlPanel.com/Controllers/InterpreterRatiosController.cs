using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Royaya.com.Models;
using RoyayaControlPanel.com.Models;

namespace RoyayaControlPanel.com.Controllers
{
    public class InterpreterRatiosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InterpreterRatios
        public async Task<ActionResult> Index()
        {
            var interpreterRatios = db.InterpreterRatios.Include(i => i.interpretator).Include(i => i.path);
            return View(await interpreterRatios.ToListAsync());
        }

        // GET: InterpreterRatios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterpreterRatio interpreterRatio = await db.InterpreterRatios.FindAsync(id);
            if (interpreterRatio == null)
            {
                return HttpNotFound();
            }
            return View(interpreterRatio);
        }

        // GET: InterpreterRatios/Create
        public ActionResult Create()
        {
            ViewBag.interpretatorId = new SelectList(db.Users, "Id", "Sex");
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Status");
            return View();
        }

        // POST: InterpreterRatios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,interpretatorId,ratio,pathId,CreationDate,LastModificationDate,Creator,Modifier,AttachmentId")] InterpreterRatio interpreterRatio)
        {
            if (ModelState.IsValid)
            {
                db.InterpreterRatios.Add(interpreterRatio);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.interpretatorId = new SelectList(db.Users, "Id", "Sex", interpreterRatio.interpretatorId);
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Status", interpreterRatio.pathId);
            return View(interpreterRatio);
        }

        // GET: InterpreterRatios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterpreterRatio interpreterRatio = await db.InterpreterRatios.FindAsync(id);
            if (interpreterRatio == null)
            {
                return HttpNotFound();
            }
            ViewBag.interpretatorId = new SelectList(db.Users, "Id", "Sex", interpreterRatio.interpretatorId);
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Status", interpreterRatio.pathId);
            return View(interpreterRatio);
        }

        // POST: InterpreterRatios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,interpretatorId,ratio,pathId,CreationDate,LastModificationDate,Creator,Modifier,AttachmentId")] InterpreterRatio interpreterRatio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interpreterRatio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.interpretatorId = new SelectList(db.Users, "Id", "Sex", interpreterRatio.interpretatorId);
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Status", interpreterRatio.pathId);
            return View(interpreterRatio);
        }

        // GET: InterpreterRatios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterpreterRatio interpreterRatio = await db.InterpreterRatios.FindAsync(id);
            if (interpreterRatio == null)
            {
                return HttpNotFound();
            }
            return View(interpreterRatio);
        }

        // POST: InterpreterRatios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InterpreterRatio interpreterRatio = await db.InterpreterRatios.FindAsync(id);
            db.InterpreterRatios.Remove(interpreterRatio);
            await db.SaveChangesAsync();
            return RedirectToAction("Interpreters","Account");
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
