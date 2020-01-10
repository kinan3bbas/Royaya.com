using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RoyayaControlPanel.com.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using Royaya.com.ViewModels;
using System.Data.Entity;
using Royaya.com.Models;
using System.Net;
using System.Net.Http;
using RoyayaControlPanel.com.ViewModels;

namespace RoyayaControlPanel.com.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult UsersList()
        {
            fillUserData();
            var users = db.Users.ToList();
            return View(users);
        }

        [HttpGet]
        public ActionResult Interpreters()
        {
            fillUserData();
            var users = db.Users.Where(a => a.Type.Equals("Interpreter")).ToList();
            return View(users);
        }

        public async Task<ActionResult> EditInterpreter(string id)
        {

            ApplicationUser user = db.Users.Where(a => a.Id.Equals(id)).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            InterpreterUserModel temp = new InterpreterUserModel();
            temp.Id = user.Id;
            temp.Name = user.Name;
            temp.Status = user.Status;
            temp.Sex = user.Sex;
            temp.Country = user.Country;
            temp.JobDescription = user.JobDescription;
            temp.JoiningDate = user.JoiningDate;
            temp.MartialStatus = user.MartialStatus;
            temp.Age = user.Age;
            temp.PersonalDescription = user.PersonalDescription;
            temp.FireBaseId = user.FireBaseId;
            temp.VerifiedInterpreter = user.verifiedInterpreter;
            temp.Type = user.Type;

            temp.numbOfDreamsInOneDay = user.numbOfDreamsInOneDay;
            return View(temp);
        }

        [HttpGet]
        public JsonResult VerifyInterpreter(String id)
        {
            ApplicationUser user = db.Users.Where(a => a.Id.Equals(id)).FirstOrDefault();
            user.verifiedInterpreter = true;
            user.LastModificationDate = DateTime.Now;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        // POST: EditInterpreter/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditInterpreter(InterpreterUserModel tempUser)
        {
            ApplicationUser user = db.Users.Find(tempUser.Id);
            if (ModelState.IsValid)
            {
                user.Sex = tempUser.Sex;
                user.Status = tempUser.Status;
                user.Name = tempUser.Name;
                user.Country = tempUser.Country;
                user.MartialStatus = tempUser.MartialStatus;
                user.JobDescription = tempUser.JobDescription;
                user.JoiningDate = tempUser.JoiningDate;
                user.numbOfDreamsInOneDay = tempUser.numbOfDreamsInOneDay;
                user.Age = tempUser.Age;
                user.Type = tempUser.Type;
                
                user.FireBaseId = tempUser.FireBaseId;
                user.PersonalDescription = tempUser.PersonalDescription;
                user.CreationDate = DateTime.Now;
                user.LastModificationDate = DateTime.Now;
                if (user.verifiedInterpreter != tempUser.VerifiedInterpreter) {
                    if (user.verifiedInterpreter == false)
                    {
                        NotificationLog log = new NotificationLog();
                        log.User = user;
                        log.UserId = user.Id;
                        log.message = "Congratulation your account has been promoted to be a verified interpreter!";
                        log.CreationDate = DateTime.Now;
                        log.LastModificationDate = DateTime.Now;
                        db.NotificationLogs.Add(log);
                        db.SaveChanges();

                    }
                    else
                    {
                        NotificationLog log = new NotificationLog();
                        log.User = user;
                        log.UserId = user.Id;
                        log.message = "You aren't interpreter anymore!";
                        log.CreationDate = DateTime.Now;
                        log.LastModificationDate = DateTime.Now;
                        db.NotificationLogs.Add(log);
                        db.SaveChanges();
                    }
                    user.verifiedInterpreter = tempUser.VerifiedInterpreter;
                }
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Interpreters");
            }

            return View(tempUser);
        }


        [HttpGet]
        public ActionResult AssignUserRole(string UserId)
        {
            fillUserData();
            ViewBag.SelectedRole = new SelectList(db.Roles, "Id", "Name");

            var roleViewModel = new AssignRoleViewModel()
            {
                UserId = UserId,
                UserName = db.Users.Find(UserId).UserName
            };

            return View(roleViewModel);
        }

        [HttpPost]
        public ActionResult AssignUserRole(AssignRoleViewModel model)
        {
            fillUserData();
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.UserId);

                if (user != null)
                {
                    if (user.Roles.Count(x => x.UserId == model.UserId && x.RoleId == model.SelectedRole) > 0)
                    {
                        ModelState.AddModelError("", "The User already assigned to the specified Role!");
                        ViewBag.SelectedRole = new SelectList(db.Roles, "Id", "Name");
                        return View(model);
                    }
                    else
                    {
                        user.Roles.Add(new IdentityUserRole()
                        {
                            UserId = model.UserId,
                            RoleId = model.SelectedRole
                        });

                        db.SaveChanges();

                        return RedirectToAction("UsersByRole", new { RoleId = model.SelectedRole });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The User does not exists in the database!");
                    ViewBag.SelectedRole = new SelectList(db.Roles, "Id", "Name");
                    return View(model);
                }

            }
            return View(model);
        }

        [HttpGet]
        public ActionResult UsersByRole(string RoleId)
        {
            fillUserData();
            var users = db
                .Users
                .Where(u => u.Roles.Count(role => role.RoleId.Equals(RoleId, StringComparison.InvariantCultureIgnoreCase)) > 0)
                .ToList();
            return View(users);
        }
        public void fillUserData()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(User.Identity.GetUserId());
            ViewBag.CurrentUser = user;
        }

        [Route("getFastestInterpretor")]
        [HttpGet]
        public JsonResult getFastestInterpretator()
        {
            var users = db.Users.Where(a => a.Type.Equals("Interpreter") && a.Status.Equals("Active")).ToList();
            List<InterpreterViewModel> finalResult = new List<InterpreterViewModel>();
            foreach (var user in users)
            {

                InterpreterViewModel temp = new InterpreterViewModel();
                temp.id = user.Id;
                temp.Email = user.Email;
                temp.pictureId = user.PictureId;
                temp.Name = user.Name;
                temp.numberOfAllDreams = user.Dreams.Count;
                temp.Rating = temp.numberOfAllDreams > 0 ? user.Dreams.Where(b => b.Status.Equals("Done")).Average(a => a.UserRating) : 0;
                temp.numberOfDreamsByDay = user.numbOfDreamsInOneDay;
                temp.numberOfActiveDreams = user.Dreams.Where(a => a.Status.Equals("Active")).ToList().Count;
                temp.numberOfDoneDreams = user.Dreams.Where(a => a.Status.Equals("Done")).ToList().Count;
                temp.speed = temp.numberOfActiveDreams > 0 ? temp.numberOfDoneDreams / temp.numberOfActiveDreams : 0;
                finalResult.Add(temp);


            }
            return Json(finalResult, JsonRequestBehavior.AllowGet);
        }

        // GET api/Account/getAllStatistics
        [Route("getAllStatistics")]
        [HttpGet]
        public JsonResult getAllStatistics()
        {
            List<Dream> dreams = db.Dreams.ToList();
            var users = db.Users.ToList(); ;
            StatisticsViewModel result = new StatisticsViewModel();

            //foreach (var item in users.ToList())
            //{
            //    if (item.Type.Equals("Client"))
            //        result.allClients++;
            //    else if (item.Type.Equals("Interpreter"))
            //        result.allInterpreters++;
            //    else if (item.Type.Equals("Admin"))
            //        result.allAdmins++;
            //}
            result.AllDreams = dreams.Count();
            result.allActiveDreams = dreams.Where(a => a.Status.Equals("Active")).Count();
            result.allDoneDreams = dreams.Where(a => a.Status.Equals("Done")).Count();
            result.allUsers = users.Count();
            result.allClients = db.Users.Where(a => a.Type.Equals("Client")).Count();
            result.allInterpreters = db.Users.Where(a => a.Type.Equals("Interpreter")).Count();
            result.allAdmins = db.Users.Where(a => a.Type.Equals("Admin")).Count();
            //result.allClients = users.Where(a => a.Type.Equals("Client")).Count()>0?0: users.Where(a => a.Type.Equals("Client")).Count();
            //result.allInterpreters = users.Where(a => a.Type.Equals("Interpreter")).Count()>0? users.Where(a => a.Type.Equals("Interpreter")).Count():0;
            //result.allAdmins = users.Where(a => a.Type.Equals("Admin")).Count()>0?users.Where(a => a.Type.Equals("Admin")).Count():0;


            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PersonalPage(String id)
        {
            fillUserData();
            ApplicationUser temp = db
                .Users
                .Where(e => e.Id.Equals(id))
                .FirstOrDefault();
            if (temp == null)
            {
                return HttpNotFound();
            }
            //var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //var user = userManager.FindById(User.Identity.GetUserId());
            //ViewBag.CurrentUser = user;

            ViewBag.userId = id;
            return View(temp);
        }

        public async Task<ActionResult> InterpreterDreams(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            List<Dream> dreams = new List<Dream>();
            if (user.Type.Equals("Interpreter"))
                dreams = db.Dreams.Where(a => a.interpretatorId.Equals(id)).Include(d => d.path).OrderByDescending(r => r.CreationDate).ToList();
            if (user.Type.Equals("Client"))
                dreams = db.Dreams.Where(a => a.Creator.Equals(id)).OrderByDescending(r => r.CreationDate).ToList();
            ViewBag.userId = id;
            ViewBag.Type = user.Type;
            return View(dreams.ToList());
        }
        public async Task<ActionResult> InterpreterRatings(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            var dreams = db.Dreams.Where(a => a.interpretatorId.Equals(id)).OrderByDescending(r => r.CreationDate);
            ViewBag.userId = id;
            ViewBag.Type = user.Type;
            return View(dreams.ToList());
        }

        public async Task<ActionResult> InterpreterRatio(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            var ratios = db.InterpreterRatios.Where(a => a.interpretatorId.Equals(id)).OrderByDescending(r => r.CreationDate);
            ViewBag.userId = id;
            ViewBag.Type = user.Type;
            List<InterprationPath> paths = db.InterprationPaths.ToList();
            List<InterpreterRatioViewModel> rows = new List<InterpreterRatioViewModel>();
            foreach (var item in ratios)
            {
                InterpreterRatioViewModel temp = new InterpreterRatioViewModel();
                temp.CreationDate = item.CreationDate;
                temp.id = item.id;
                temp.interpretatorId = item.interpretatorId;
                temp.pathId = item.pathId;
                temp.CreatorName = user.Name;
                temp.CreatorId = item.Creator;
                temp.interpretatorName = item.interpretator != null ? item.interpretator.Name : "";
                temp.pathCost = paths.Where(a=>a.id.Equals(temp.pathId)) != null ? paths.Where(a => a.id.Equals(temp.pathId)).FirstOrDefault().Cost : -1;
                temp.Ratio = item.ratio;
                rows.Add(temp);
            }
            return View(rows);
        }

        // GET: InterpreterRatios/Create
        public ActionResult InterpreterRatioCreate(String id)
        {
            InterpreterRatio ratio = new InterpreterRatio();
            ratio.interpretatorId = id;

            ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Cost");
            return View(ratio);
        }

        // POST: InterpreterRatios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InterpreterRatioCreate( InterpreterRatio interpreterRatio)
        {
           
                interpreterRatio.CreationDate = DateTime.Now;
                interpreterRatio.LastModificationDate = DateTime.Now;
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(User.Identity.GetUserId());
            //interpreterRatio.path = db.InterprationPaths.Where(a => a.id.Equals(interpreterRatio.pathId)).FirstOrDefault() ;
                interpreterRatio.interpretator = db.Users.Find(interpreterRatio.interpretatorId);
                interpreterRatio.Creator = user.Id;
                db.InterpreterRatios.Add(interpreterRatio);
                await db.SaveChangesAsync();
                return RedirectToAction("InterpreterRatio",new {id=interpreterRatio.interpretatorId });
        

            //ViewBag.interpretatorId = new SelectList(db.Users, "Id", "Sex", interpreterRatio.interpretatorId);
            //ViewBag.pathId = new SelectList(db.InterprationPaths, "id", "Status", interpreterRatio.pathId);
            //return View(interpreterRatio);
        }
        public JsonResult getInterpreterInfo(string phoneNumber)
        {
            ApplicationUser user = db.Users.Where(a => a.PhoneNumber.Equals(phoneNumber)).FirstOrDefault();
            if (user == null) {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("User Can't be found!"),
                    ReasonPhrase = "User Canot be found!" + phoneNumber
                };
                return Json(resp,JsonRequestBehavior.AllowGet);
            }
                
            List<InterpreterViewModel> finalResult = new List<InterpreterViewModel>();
            string userId = user.Id;
            List<Dream> dreams = db.Dreams.Where(a=>a.interpretatorId.Equals(userId)).ToList();
            List<InterpreterRatio> ratios = db.InterpreterRatios.Where(a => a.interpretatorId.Equals(userId)).ToList();
            List<InterprationPath> paths = db.InterprationPaths.ToList();
            InterpreterViewModel temp = new InterpreterViewModel();
            temp.id = user.Id;
            temp.Email = user.Email;
            temp.pictureId = user.PictureId;
            temp.Name = user.Name;
            temp.numberOfAllDreams = dreams.Count()>0? dreams.Count():0;
            temp.Rating = temp.numberOfAllDreams > 0 ? Math.Round(dreams.Where(b => b.Status.Equals("Done")).Average(a => a.UserRating),2) : 0;
            temp.numberOfDreamsByDay = user.numbOfDreamsInOneDay;
            temp.numberOfActiveDreams = dreams.Where(a => a.Status.Equals("Active")).Count()>0? dreams.Where(a => a.Status.Equals("Active")).ToList().Count():0;
            temp.numberOfDoneDreams = dreams.Where(a => a.Status.Equals("Done")).Count()>0? dreams.Where(a => a.Status.Equals("Done")).ToList().Count():0;
            temp.speed = temp.numberOfActiveDreams > 0 ? temp.numberOfDoneDreams / temp.numberOfActiveDreams : 0;
            double balance = 0.0;
            foreach (var item in ratios)
            {
                List<Dream> tempdream = dreams.Where(a => a.pathId.Equals(item.pathId)&&a.Status.Equals("Done")).ToList();
                double cost = paths.Where(a => a.id.Equals(item.pathId)).FirstOrDefault().Cost;
                if (tempdream != null)
                    balance += tempdream.Count * cost * (item.ratio/100);
            }
            temp.balance =Math.Round(balance,2);
            finalResult.Add(temp);



            return  Json(temp, JsonRequestBehavior.AllowGet);
        }
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}