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
    [Authorize(Roles = "Admin")]
    public class InterprationPathsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InterprationPaths
        public async Task<ActionResult> Index()
        {
            return View(await db.InterprationPaths.ToListAsync());
        }

        // GET: InterprationPaths/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterprationPath interprationPath = await db.InterprationPaths.FindAsync(id);
            if (interprationPath == null)
            {
                return HttpNotFound();
            }
            return View(interprationPath);
        }

        // GET: InterprationPaths/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InterprationPaths/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Cost,Status")] InterprationPath interprationPath)
        {
            if (ModelState.IsValid)
            {
                interprationPath.CreationDate = DateTime.Now;
                interprationPath.LastModificationDate = DateTime.Now;
                db.InterprationPaths.Add(interprationPath);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(interprationPath);
        }

        // GET: InterprationPaths/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterprationPath interprationPath = await db.InterprationPaths.FindAsync(id);
            if (interprationPath == null)
            {
                return HttpNotFound();
            }
            return View(interprationPath);
        }

        // POST: InterprationPaths/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Cost,Status")] InterprationPath tempinterprationPath)
        {
            InterprationPath path = db.InterprationPaths.Find(tempinterprationPath.id);
            if (ModelState.IsValid)
            {
                path.Cost = tempinterprationPath.Cost;
                path.Status = tempinterprationPath.Status;
                path.CreationDate = DateTime.Now;
                path.LastModificationDate = DateTime.Now;
                db.Entry(path).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(path);
        }

        // GET: InterprationPaths/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterprationPath interprationPath = await db.InterprationPaths.FindAsync(id);
            if (interprationPath == null)
            {
                return HttpNotFound();
            }
            return View(interprationPath);
        }

        // POST: InterprationPaths/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InterprationPath interprationPath = await db.InterprationPaths.FindAsync(id);
            db.InterprationPaths.Remove(interprationPath);
            await db.SaveChangesAsync();
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
