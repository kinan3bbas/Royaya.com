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
using PagedList;
using RoyayaControlPanel.com.ViewModels;

namespace RoyayaControlPanel.com.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DreamsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dreams
        public async Task<ActionResult> Index(int? page, string searchString, string status)
        {
            var users = db.Users;
            var dreams = db.Dreams.Include(d => d.interpretator).Include(d => d.path).OrderByDescending(r => r.CreationDate);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (searchString != null && !searchString.Equals(""))
                dreams = dreams.Where(a => a.Description.Contains(searchString)).OrderByDescending(r => r.CreationDate);
            if (status != null && !status.Equals(""))
                dreams = dreams.Where(a => a.Status.Equals(status)).OrderByDescending(r => r.CreationDate);
            //dreams = dreams.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
            List<DreamViewModel> dreamsView = new List<DreamViewModel>();
            foreach (var item in dreams)
            {
                DreamViewModel temp = new DreamViewModel
                {
                    id = item.id,
                    CreatorId = item.Creator,

                    //temp.CreatorName = users.Where(a => a.Id.Equals(item.Creator)).FirstOrDefault()!=null?users.Where(a => a.Id.Equals(item.Creator)).FirstOrDefault().Name:"";
                    Description = item.Description,
                    Explanation = item.Explanation,
                    ExplanationDate = item.ExplanationDate,
                    InterpretationStartDate = item.InterpretationStartDate,
                    interpretatorId = item.interpretatorId != null ? item.interpretatorId : "",
                    interpretatorName = item.interpretator != null ? item.interpretator.Name : "",
                    pathCost = item.path != null ? item.path.Cost : -1,
                    pathId = item.pathId != null ? item.pathId : 0,
                    RatingDate = item.RatingDate,
                    Status = item.Status,
                    UserRating = item.UserRating,
                    CreationDate = item.CreationDate
                };
                dreamsView.Add(temp);
            }
            return View(dreamsView.ToPagedList(pageNumber, pageSize));
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetDreams(string searchString,string status, int page)
        {
            var users = db.Users;
            var dreams = db.Dreams.Include(d => d.interpretator).Include(d => d.path).OrderByDescending(r => r.CreationDate);
            int pageSize = 10;
            int pageNumber = (page == 0 || page == null) ? 1: page;
            if (searchString != null && !searchString.Equals(""))
                dreams = dreams.Where(a => a.Description.Contains(searchString)).OrderByDescending(r => r.CreationDate);
            if (status != null && !status.Equals(""))
                dreams = dreams.Where(a => a.Status.Equals(status)).OrderByDescending(r => r.CreationDate);
            dreams = dreams.Skip((pageNumber-1)*pageSize).Take(pageSize).OrderByDescending(r => r.CreationDate); ;
            List<DreamViewModel> dreamsView = new List<DreamViewModel>();
            foreach (var item in dreams)
            {
                item.numberOfViews++;
                db.Entry(item).State = EntityState.Modified;
                DreamViewModel temp = new DreamViewModel
                {
                    id = item.id,
                    CreatorId = item.Creator,

                    //temp.CreatorName = users.Where(a => a.Id.Equals(item.Creator)).FirstOrDefault()!=null?users.Where(a => a.Id.Equals(item.Creator)).FirstOrDefault().Name:"";
                    Description = item.Description,
                    Explanation = item.Explanation,
                    ExplanationDate = item.ExplanationDate,
                    InterpretationStartDate = item.InterpretationStartDate,
                    interpretatorId = item.interpretatorId != null ? item.interpretatorId : "",
                    interpretatorName = item.interpretator != null ? item.interpretator.Name : "",
                    pathCost = item.path != null ? item.path.Cost : -1,
                    pathId = item.pathId != null ? item.pathId : 0,
                    RatingDate = item.RatingDate,
                    Status = item.Status,
                    UserRating = item.UserRating,
                    CreationDate = item.CreationDate,
                    numberOfLikes=item.numberOfLikes,
                    numberOfViews=item.numberOfViews,
                    
                    AllItems = dreams.Count()
                };
                dreamsView.Add(temp);
            }
            db.SaveChanges();
            //return Json(dreamsView.ToPagedList(pageNumber, pageSize),JsonRequestBehavior.AllowGet);
            return Json(dreamsView, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetSingleDream(int? id)
        {
            
            Dream item = db.Dreams.Find(id);

            DreamViewModel temp = new DreamViewModel();
            temp.CreatorId = item.Creator;

            //temp.CreatorName = users.Where(a => a.Id.Equals(item.Creator)).FirstOrDefault()!=null?users.Where(a => a.Id.Equals(item.Creator)).FirstOrDefault().Name:"";
            temp.Description = item.Description;
            temp.Explanation = item.Explanation;
            temp.ExplanationDate = item.ExplanationDate;
            temp.InterpretationStartDate = item.InterpretationStartDate;
            temp.interpretatorId = item.interpretatorId != null ? item.interpretatorId : "";
            temp.interpretatorName = item.interpretator != null ? item.interpretator.Name : "";
            temp.pathCost = item.path != null ? item.path.Cost : -1;
            temp.pathId = item.pathId != null ? item.pathId : 0;
            temp.RatingDate = item.RatingDate;
            temp.Status = item.Status;
            temp.UserRating = item.UserRating;
            temp.CreationDate = item.CreationDate;
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dream dream = await db.Dreams.FindAsync(id);
            if (dream == null)
            {
                return HttpNotFound();
            }
            return View(dream);
        }

        [AllowAnonymous]
        public async Task<ActionResult> DreamPage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dream dream = await db.Dreams.FindAsync(id);
            if (dream == null)
            {
                return HttpNotFound();
            }

            dream.numberOfViews++;
            db.Entry(dream).State = EntityState.Modified;
            db.SaveChanges();

            ApplicationUser creator = db.Users.Where(a => a.Id.Equals(dream.Creator)).FirstOrDefault();
            ApplicationUser Interpreter = db.Users.Where(a => a.Id.Equals(dream.interpretatorId)).FirstOrDefault();
            InterprationPath path = db.InterprationPaths.Where(a => a.id.Equals(dream.pathId)).FirstOrDefault();
            if (path.Cost == 0)
                ViewBag.Type = "رؤية مجانية";
            else
                ViewBag.Type = "رؤية مدفوعة";

            ViewBag.InterpreterName = Interpreter != null ? Interpreter.Name : "";
            ViewBag.CreatorName = creator != null ? creator.Name : "";
            return View(dream);
        }

        // GET: Dreams/Create
        public ActionResult Create()
        {
            ViewBag.interpretatorId = new SelectList(db.Users.Where(a => a.Type.Equals("Interpreter")), "Id", "Name");
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Cost");
            return View();
        }

        // POST: Dreams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,Status,Description,interpretatorId,pathId,Explanation,ExplanationDate,UserRating,RatingDate,InterpretationStartDate,CreationDate,LastModificationDate,Creator,Modifier,AttachmentId")] Dream dream)
        {
            if (ModelState.IsValid)
            {
                dream.CreationDate = DateTime.Now;
                dream.LastModificationDate = DateTime.Now;
                db.Dreams.Add(dream);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.interpretatorId = new SelectList(db.Users, "Id", "Sex", dream.interpretatorId);
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Status", dream.pathId);
            return View(dream);
        }

        // GET: Dreams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dream dream = await db.Dreams.FindAsync(id);
            if (dream == null)
            {
                return HttpNotFound();
            }
            ViewBag.interpretatorId = new SelectList(db.Users, "Id", "Name", dream.interpretatorId);
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Cost", dream.pathId);
            return View(dream);
        }

        // POST: Dreams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,Status,Description,interpretatorId,pathId,Explanation")] Dream tempdream)
        {
            Dream dream = db.Dreams.Find(tempdream.id);
            if (ModelState.IsValid)
            {

                dream.Status = tempdream.Status;
                dream.Description = tempdream.Description;
                dream.interpretatorId = tempdream.interpretatorId;
                dream.pathId = tempdream.pathId;
                dream.Explanation = tempdream.Explanation;
                dream.LastModificationDate = DateTime.Now;
                db.Entry(dream).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.interpretatorId = new SelectList(db.Users, "Id", "Sex", dream.interpretatorId);
            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Status", dream.pathId);
            return View(dream);
        }

        // GET: Dreams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dream dream = await db.Dreams.FindAsync(id);
            if (dream == null)
            {
                return HttpNotFound();
            }
            return View(dream);
        }

        // POST: Dreams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Dream dream = await db.Dreams.FindAsync(id);
            db.Dreams.Remove(dream);
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
