
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using PagedList;
using RoyayaControlPanel.com.Models;

namespace RoyayaControlPanel.com.Controllers
{
    public class RolesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        
        //
        // GET: /Roles/
        public ActionResult Index(int? page)
        {
            fillUserData();
            var Roles = db.Roles.OrderByDescending(r => r.Name);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(Roles.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Roles/Details/5
        public ActionResult Details(string id)
        {
            fillUserData();
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // GET: /Roles/Create
        public ActionResult Create( )
        {
            fillUserData();
            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            fillUserData();
            if (ModelState.IsValid)
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(role);
        }

        //
        // GET: /Roles/Edit/5
        public ActionResult Edit(string id)
        {
            fillUserData();
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(IdentityRole role)
        {
            fillUserData();
            if (ModelState.IsValid)
            {
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(role);
        }

        //
        // GET: /Roles/Delete/5
        public ActionResult Delete(string id)
        {
            fillUserData();
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        public ActionResult About1()
        {
            return View();
        }

        public ActionResult AdminPage()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(User.Identity.GetUserId());
            fillUserData();
            if (userManager.IsInRole(user.Id, "GuestProspect") && !userManager.IsInRole(user.Id, "Guest"))
            {
                Session["EMAIL"] = user.Email;
                Session["USER"] = user.Id;
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View();
        }


        public ActionResult View1()
        {
            return View();
        }
        //
        // POST: /Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(IdentityRole role)
        {
            fillUserData();
            try
            {
                var myrole = db.Roles.Find(role.Id);
                // TODO: Add delete logic here
                db.Roles.Remove(myrole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(role);
            }
        }

        public void fillUserData()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(User.Identity.GetUserId());
            ViewBag.CurrentUser = user;
        }
    }
}
