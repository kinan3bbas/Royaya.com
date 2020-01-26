using PagedList;
using Royaya.com.Models;
using RoyayaControlPanel.com.Models;
using RoyayaControlPanel.com.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RoyayaControlPanel.com.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Home()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Main()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult ContactUs()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dream dream =  db.Dreams.Find(id);
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


        [AllowAnonymous]
        public JsonResult GetDreams(int? page, string searchString, string status)
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
                    CreationDate = item.CreationDate,
                    numberOfLikes = item.numberOfLikes,
                    numberOfViews = item.numberOfViews,

                    AllItems = dreams.Count()
                };
                dreamsView.Add(temp);
            }
            return Json(dreamsView.ToPagedList(pageNumber, pageSize), JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}