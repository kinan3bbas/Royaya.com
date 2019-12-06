using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Royaya.com.Models;
using System.Data.Entity;
using Royaya.com.Extras;
using Royaya.com.Controllers;
using System.Diagnostics;

namespace Royaya.com.ScheduledJobs
{
    public class NotificationJob: IJob
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        async Task IJob.Execute(IJobExecutionContext context)
        {
            AccountController accountController = new AccountController();
            Debug.WriteLine("Job Has Strated");
            //List<ApplicationUser> users = db.Users.Include("Dreams").
            //    Where(a =>a.Type.Equals("Client")
            //        && a.Dreams.Where(b => b.Status.Equals("Active")).Count() > 1).ToList();
            List<Dream> dreams = db.Dreams.Where(a => a.Status.Equals("Active")).ToList();
            List<String> users = dreams.Select(a => a.Creator).ToList();
            List<UsersDeviceTokens> tokenObjets = db.UsersDeviceTokens.Where(a => users.Contains(a.UserId)).ToList();
            //var map = new Dictionary<String, String>();
            foreach (var dream in dreams)
            {
                //map.Add(item.Creator, accountController.getExpectedDate(item));
                foreach (var item in users)
                {
                    string token = tokenObjets.Where(a => a.UserId.Equals(item)).FirstOrDefault().token;
                    String message = accountController.getExpectedDate(dream);
                    NotificationLog log = new NotificationLog();
                    log.message = message;
                    log.UserId = item;
                    log.CreationDate = DateTime.Now;
                    log.LastModificationDate = DateTime.Now;
                    db.NotificationLogs.Add(log);
                    db.SaveChanges();
                    string[] tokens = token.Split(';');
                    var pushSent = PushNotificationLogic.SendPushNotification(tokens, "Upgrade your plan", 
                        message, "");
                }
            }

            await Console.Out.WriteLineAsync("Greetings from HelloJob!");


        }
    }

}