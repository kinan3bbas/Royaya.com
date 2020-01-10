using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Royaya.com.Models;
using Royaya.com.Providers;
using Royaya.com.Results;
using System.Linq;
using System.Data.Entity;
using Royaya.com.ViewModels;
using NodaTime;
using System.Net;
using Quartz;
using Quartz.Impl;
using Royaya.com.ScheduledJobs;

namespace Royaya.com.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();
        private CoreController core = new CoreController();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
            ApplicationUser user = core.getCurrentUser();
            UsersDeviceTokens token = db.UsersDeviceTokens.Where(a => a.UserId.Equals(user.Id)).FirstOrDefault();
            if (user.Type == "Client") {
                return new UserInfoViewModel
                {
                    Email = User.Identity.GetUserName(),
                    Age = user.Age,
                    Country = user.Country,
                    JobDescription = user.JobDescription,
                    JoiningDate = user.JoiningDate,
                    Name = user.Name,
                    MartialStatus = user.MartialStatus,
                    Sex = user.Sex,
                    Status = user.Status,
                    Type = user.Type,
                    phoneNumber = user.PhoneNumber,
                    PersonalDescription = user.PersonalDescription,
                    FireBaseId=user.FireBaseId,
                    Id=user.Id

                };
            }
            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                Age = user.Age,
                Country=user.Country,
                JobDescription=user.JobDescription,
                JoiningDate=user.JoiningDate,
                Name=user.Name,
                MartialStatus=user.MartialStatus,
                numbOfDreamsInOneDay=user.numbOfDreamsInOneDay,
                PictureId=user.PictureId,
                Sex=user.Sex,
                Status=user.Status,
                Type=user.Type,
                phoneNumber=user.PhoneNumber,
                PersonalDescription=user.PersonalDescription,
                FireBaseId = user.FireBaseId,
                Id=user.Id,
                HasRegistered=user.verifiedInterpreter


            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            ApplicationUser tempUser = db.Users.Where(a => a.UserName.Equals(model.Username)||a.PhoneNumber.Equals(model.PhoneNumber)).FirstOrDefault();
            if (tempUser != null)
                await core.throwExcetpion("Username or phone number is already taken!");



            model.Email =model.Username+"@Test.com";
            IdentityResult result=null;

            if (model.Type.Equals("Admin"))
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Name = model.Name,
                    Status = model.Status,
                    Type = model.Type,
                    CreationDate = DateTime.Now,
                    FireBaseId=model.FireBaseId

                                                    ,
                    LastModificationDate = DateTime.Now
                };
                result = await UserManager.CreateAsync(user, model.Password);
                await UserManager.AddToRoleAsync(user.Id, "Admin");
            }
            
            if (model.Type.Equals("Client"))
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Age=model.Age,
                    Country = model.Country,
                    MartialStatus = model.MartialStatus
                                                    ,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber
                                                    ,
                    Sex = model.Sex,
                    Status = model.Status,
                    Type = model.Type,
                    CreationDate = DateTime.Now
                                                    ,
                    JobDescription = model.JobDescription,

                    LastModificationDate = DateTime.Now,
                    PersonalDescription=model.PersonalDescription,
                    FireBaseId=model.FireBaseId
                };
                result = await UserManager.CreateAsync(user, model.Password);
            }

            if (model.Type.Equals("Interpreter"))
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Age = model.Age,
                    JobDescription = model.JobDescription,
                    Country=model.Country,
                    JoiningDate = DateTime.Now,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber
                                                    ,
                    PictureId = model.PictureId,
                    Sex = model.Sex,
                    Status = model.Status,
                    Type = model.Type,
                    CreationDate = DateTime.Now
                                                    ,
                    LastModificationDate = DateTime.Now,
                    numbOfDreamsInOneDay = model.numbOfDreamsInOneDay,
                    PersonalDescription=model.PersonalDescription,
                    FireBaseId=model.FireBaseId
                };
                result = await UserManager.CreateAsync(user, model.Password);
            }
            //IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok(result);
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }


        // GET api/Account/getFastestInterpretor
        [Route("getFastestInterpretor")]
        public async Task<IHttpActionResult> getFastestInterpretator()
        {
            var users = db.Users.Where(a => a.Type.Equals("Interpreter")&&a.Status.Equals("Active")).Include("Dreams").ToList();
            List<InterpreterViewModel> finalResult = new List<InterpreterViewModel>();
            foreach (var user in users)
            {
                
                InterpreterViewModel temp = new InterpreterViewModel();
                temp.id = user.Id;
                temp.Email = user.Email;
                temp.pictureId = user.PictureId;
                temp.Name = user.Name;
                temp.numberOfAllDreams = user.Dreams.Count;
                temp.Rating= temp.numberOfAllDreams > 0 ? user.Dreams.Where(b => b.Status.Equals("Done")).Average(a => a.UserRating) : 0;
                temp.numberOfDreamsByDay = user.numbOfDreamsInOneDay;
                temp.numberOfActiveDreams = user.Dreams.Where(a => a.Status.Equals("Active")).ToList().Count;
                temp.numberOfDoneDreams = user.Dreams.Where(a => a.Status.Equals("Done")).ToList().Count;
                temp.speed =temp.numberOfActiveDreams>0?temp.numberOfDoneDreams / temp.numberOfActiveDreams:0;
                temp.PhoneNumber = user.PhoneNumber;
                temp.Status = user.Status;
                temp.JobDescription = user.JobDescription;
                temp.sex = user.Sex;
                temp.Age = user.Age;
                temp.MartialStatus = user.MartialStatus;
                temp.country = user.Country;
                temp.personalDescription = user.PersonalDescription;
                temp.fireBaseId = user.FireBaseId;
                temp.HasRegistered = user.verifiedInterpreter;
                
                finalResult.Add(temp);

                
            }
                return Ok(finalResult.OrderByDescending(a=>a.speed).OrderByDescending(b=>b.Rating).ToList());
        }

        [AllowAnonymous]
        // GET api/Account/getAllStatistics
        [Route("getAllStatistics")]
        public async Task<IHttpActionResult> getAllStatistics()
        {
            List<Dream> dreams = db.Dreams.ToList();
            List<ApplicationUser> users = db.Users.ToList();
            StatisticsViewModel result = new StatisticsViewModel();
            result.AllDreams = dreams.Count();
            result.allActiveDreams = dreams.Where(a => a.Status.Equals("Active")).Count();
            result.allDoneDreams= dreams.Where(a => a.Status.Equals("Done")).Count();
            result.allClients = db.Users.Where(a => a.Type.Equals("Client")) != null ? db.Users.Where(a => a.Type.Equals("Client")).Count() : 0;
            result.allInterpreters = db.Users.Where(a => a.Type.Equals("Interpreter")).Count();
            result.allAdmins= db.Users.Where(a => a.Type.Equals("Admin")).Count();
            result.allUsers = users.Count();

            return Ok(result);
        }


        // POST api/Account/getWaitingTimeForSingleDream
        [Route("getWaitingTimeForSingleDream")]
        public async Task<IHttpActionResult> getWaitingTimeForSingleDream(int id)
        {
            if (id == null)
                return BadRequest("Dream id is null!");
            Dream dream = db.Dreams.Where(a => a.id.Equals(id)).FirstOrDefault();
            if (dream == null)
                return BadRequest("No matching dream!");

           
            
            return Ok(getExpectedDate(dream));
        }

        public String getExpectedDate(Dream dream) {
            if (dream.interpretatorId == null)
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("This dream hasn't been assigned to interpreter yet"),
                    ReasonPhrase = "This dream hasn't been assigned to interpreter yet"
                };
                throw new HttpResponseException(resp);
            }
            if (dream.pathId == null)
            {
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent("This dream hasn't been assigned to any plan yet!"),
                        ReasonPhrase = "This dream hasn't been assigned to any plan yet!"
                    };
                    throw new HttpResponseException(resp);
                }
            }
           
            ApplicationUser interpreter = db.Users.Find(dream.interpretatorId);
            InterprationPath path = db.InterprationPaths.Find(dream.pathId);
            long totalOfActiveDreams = db.Dreams.Where(a => a.Status.Equals("Active")
            &&a.interpretatorId.Equals(interpreter.Id)).ToList().Count();
            long numberOfDreamsinOneDay = interpreter.numbOfDreamsInOneDay;

            return getWaitingTimeMessage(Double.Parse(numberOfDreamsinOneDay.ToString()),
                Double.Parse(totalOfActiveDreams.ToString()));
            
        }


        public  string getWaitingTimeMessage(double x,double y)
        {
            if (x != null && x != 0)
            {
                double duration = (y / x) * 24 * 60 * 60;

                LocalDateTime d1 = new LocalDateTime();
                LocalDateTime d2 = d1.PlusSeconds((long)duration);
                Period period = Period.Between(d1, d2);
                int years = period.Years;
                int months = period.Months;
                int days = period.Days;
                long hours = period.Hours;
                long minutes = period.Minutes;
                long seconds = period.Seconds;
                string result = "Your average waiting time is " + (years > 0 ? years + " years " : "") +
                                (months > 0 ? months + " months " : "") +
                                (days > 0 ? days + " days " : "") +
                                (hours > 0 ? hours + " hours " : "") +
                                 (minutes > 0 ? minutes + " minutes " : "");
                return result;
            }
            return "";
        }

        [Route("GetUsersList")]
        public async Task<IHttpActionResult> GetUsersList()
        {
            
            return Ok(db.Users.Where(a=>a.Status.Equals("Active")).ToList());
        }

        [AllowAnonymous]
        [Route("GetInterpreters")]
        public async Task<IHttpActionResult> GetInterpreters()
        {
            List<ApplicationUser> users = db.Users.Where(a => a.Status.Equals("Active") && a.Type.Equals("Interpreter")).Include("Dreams").ToList();

            List<UserInfoViewModel> result = new List<UserInfoViewModel>();
            foreach (var user in users)
            {
                result.Add(new UserInfoViewModel
                {
                    Email = User.Identity.GetUserName(),
                    Age = user.Age,
                    Country = user.Country,
                    JobDescription = user.JobDescription,
                    JoiningDate = user.JoiningDate,
                    Name = user.Name,
                    MartialStatus = user.MartialStatus,
                    numbOfDreamsInOneDay = user.numbOfDreamsInOneDay,
                    PictureId = user.PictureId,
                    Sex = user.Sex,
                    Status = user.Status,
                    Type = user.Type,
                    phoneNumber = user.PhoneNumber,
                    PersonalDescription = user.PersonalDescription,
                    FireBaseId = user.FireBaseId,
                    Id = user.Id,
                    HasRegistered=user.verifiedInterpreter,
                    NumberOfActiveDreams = user.Dreams.Where(a => a.Status.Equals("Active")).ToList().Count(),
                    NumberOfDoneDreams= user.Dreams.Where(a => a.Status.Equals("Done")).ToList().Count()


                });
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [Route("GetSingleUserInfo")]
        public UserInfoViewModel GetSingleUserInfo([FromUri]string id)
        {

            ApplicationUser user = db.Users.Find(id);
            UsersDeviceTokens token = db.UsersDeviceTokens.Where(a => a.UserId.Equals(user.Id)).FirstOrDefault();
            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                Age = user.Age,
                Country = user.Country,
                JobDescription = user.JobDescription,
                JoiningDate = user.JoiningDate,
                Name = user.Name,
                MartialStatus = user.MartialStatus,
                numbOfDreamsInOneDay = user.numbOfDreamsInOneDay,
                PictureId = user.PictureId,
                Sex = user.Sex,
                Status = user.Status,
                Type = user.Type,
                phoneNumber = user.PhoneNumber,
                PersonalDescription=user.PersonalDescription,
                FireBaseId = user.FireBaseId,
                Id = user.Id,
                HasRegistered=user.verifiedInterpreter
                

            };
        }


        // POST api/Account/getWaitingTimeForSingleDream
        [Route("Like")]
        public async Task<IHttpActionResult> LikeDream([FromUri] int id)
        {
            Dream dream = db.Dreams.Find(id);
            dream.numberOfLikes++;
            db.Entry(dream).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(dream);
        }

        [Route("GetRemainingTimeForaPlan")]
        public async Task<IHttpActionResult> getRemainingTimeForAPlan(String id,int pathId)
        {
            ApplicationUser interpreter = db.Users.Where(a=>a.Id.Equals(id)).FirstOrDefault();
            InterprationPath path = db.InterprationPaths.Find(pathId);
            long numberOfDreamsinOneDay =(long) interpreter.numbOfDreamsInOneDay;
            long totalOfActiveDreams = db.Dreams.Where(a => a.Status.Equals("Active")
            && a.interpretatorId.Equals(id)).ToList().Count();
            

            return Ok(getWaitingTimeMessage(Double.Parse(numberOfDreamsinOneDay.ToString()),
                Double.Parse(totalOfActiveDreams.ToString())).Replace("Your average waiting time is ",""));
        }

        [AllowAnonymous]
        [Route("GetNotifications")]
        public async Task<IHttpActionResult> GetNotifications(String id)
        {

            return Ok(db.NotificationLogs.Where(a => a.UserId.Equals(id) && a.seen == false).ToList());
        }

        [AllowAnonymous]
        [Route("SeeNotification")]
        public async Task<IHttpActionResult> SeeNotification(int id)
        {
            NotificationLog log = db.NotificationLogs.Find(id);
            log.seen = true;
            db.Entry(log).State = EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }
        // POST api/Account/getWaitingTimeForSingleDream
        [Route("test")]
        public async Task<IHttpActionResult> Test()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<NotificationJob>().Build();
            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger3", "group1")
            .Build();

            await scheduler.ScheduleJob(job, trigger);
            return Ok();
        }

        // POST api/Account/updateUserInfo
        [Route("updateUserInfo")]
        public async Task<IHttpActionResult> updateUserInfo([FromUri]string id,[FromBody]updateUserInfoBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = db.Users.Find(id);
            if (user == null)
                await core.throwExcetpion("No matching user!");
            if (model.Age != null)
                user.Age = model.Age;
            if (model.Country != null)
                user.Country = model.Country;
            if (model.JobDescription != null)
                user.JobDescription = model.JobDescription;
            if (model.MartialStatus != null)
                user.MartialStatus = model.MartialStatus;
            if (model.Name != null)
                user.Name = model.Name;
            if (model.numbOfDreamsInOneDay != null)
                user.numbOfDreamsInOneDay = model.numbOfDreamsInOneDay;
            if (model.PhoneNumber != null)
                user.PhoneNumber = model.PhoneNumber;
            if (model.PictureId != null)
                user.PictureId = model.PictureId;
            if (model.Sex != null)
                user.Sex = model.Sex;
            if (model.Status != null)
                user.Status = model.Status;
            if (model.PersonalDescription != null)
                user.PersonalDescription = model.PersonalDescription;
            if (model.FireBaseId != null)
                user.FireBaseId = model.FireBaseId;

            user.LastModificationDate = DateTime.Now;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(user);
        }

        // POST api/Account/verifyAccount
        [Route("verifyAccount")]
        public async Task<IHttpActionResult> verifyAccount([FromUri]string id, [FromBody]updateUserInfoBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = db.Users.Find(id);
            if (user == null)
                await core.throwExcetpion("No matching user!");
            if (model.Age != null)
                user.Age = model.Age;
            if (model.Country != null)
                user.Country = model.Country;
            if (model.JobDescription != null)
                user.JobDescription = model.JobDescription;
            if (model.MartialStatus != null)
                user.MartialStatus = model.MartialStatus;
            if (model.Name != null)
                user.Name = model.Name;
            if (model.numbOfDreamsInOneDay != null)
                user.numbOfDreamsInOneDay = model.numbOfDreamsInOneDay;
            if (model.PhoneNumber != null)
                user.PhoneNumber = model.PhoneNumber;
            if (model.PictureId != null)
                user.PictureId = model.PictureId;
            if (model.Sex != null)
                user.Sex = model.Sex;
            if (model.Status != null)
                user.Status = model.Status;

            user.LastModificationDate = DateTime.Now;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Ok(user);
        }

        #region Helpers


        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }


}
