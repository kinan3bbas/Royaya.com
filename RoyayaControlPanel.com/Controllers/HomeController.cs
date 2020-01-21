using Royaya.com.Models;
using RoyayaControlPanel.com.Models;
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