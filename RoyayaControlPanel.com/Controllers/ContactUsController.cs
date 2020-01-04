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
using RoyayaControlPanel.com.ViewModels;

namespace RoyayaControlPanel.com.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContactUsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContactUs
        public async Task<ActionResult> Index()
        {
            List<ContactUs> list = db.ContactUss.ToList();
            List<ApplicationUser> users = db.Users.ToList();
            List<ContactUsViewModel> result = new List<ContactUsViewModel>();
            foreach (var item in list)
            {
                ContactUsViewModel temp = new ContactUsViewModel();
                temp.id = item.id;
                temp.creatorId = item.Creator;
                temp.Email = item.Email;
                temp.JobDescription = item.JobDescription;
                temp.PhoneNumber = item.PhoneNumber;
                temp.Name = item.Name;
                temp.CreationDate = item.CreationDate;
                temp.Message = item.Message;
                temp.Status = item.Status;
                temp.creatorName = users.Where(a => a.Id.Equals(item.Creator)).FirstOrDefault() != null ? users.Where(a => a.Id.Equals(item.Creator)).FirstOrDefault().Name : "";
                ApplicationUser interpreter = db.Users.Where(a => a.PhoneNumber.Equals(item.PhoneNumber)).FirstOrDefault();
                if (interpreter != null)
                {
                    temp.interpreterName = interpreter.Name;
                    temp.interpreterId = interpreter.Id;
                }
                else {
                    temp.interpreterName = "";
                    temp.interpreterId = "";
                }
                result.Add(temp);
            }
            
            return View(result);
        }

        // GET: ContactUs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUs contactUs = await db.ContactUss.FindAsync(id);
            if (contactUs == null)
            {
                return HttpNotFound();
            }
            return View(contactUs);
        }

        // GET: ContactUs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Message,Email,Name,JobDescription,PhoneNumber,CreationDate,LastModificationDate,Creator,Modifier,AttachmentId")] ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                db.ContactUss.Add(contactUs);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contactUs);
        }

        // GET: ContactUs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUs contactUs = await db.ContactUss.FindAsync(id);
            if (contactUs == null)
            {
                return HttpNotFound();
            }
            return View(contactUs);
        }

        // POST: ContactUs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Message,Email,Name,JobDescription,PhoneNumber,CreationDate,LastModificationDate,Creator,Modifier,AttachmentId")] ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactUs).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contactUs);
        }
        [HttpPost]
        public JsonResult addResponse(int id, string message) {
            ContactUs item = db.ContactUss.Find(id);
            item.Message = message;
            item.LastModificationDate = DateTime.Now;
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        // GET: ContactUs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUs contactUs = await db.ContactUss.FindAsync(id);
            if (contactUs == null)
            {
                return HttpNotFound();
            }
            return View(contactUs);
        }

        // POST: ContactUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ContactUs contactUs = await db.ContactUss.FindAsync(id);
            db.ContactUss.Remove(contactUs);
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
