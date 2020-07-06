using NodaTime;
using Royaya.com.Controllers;
using Royaya.com.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Royaya.com.Extras
{
    public class DreamSwitchingLibrary
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static CoreController core = new CoreController();

        public static List<Dream> getDreamsForUpdate()
        {
            LocalDateTime now = new LocalDateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            DateTime yesterday = DateTime.Now.AddHours(-48);
            //List <Dream> dreams= db.Dreams.
            //    Where(a => (Period.Between(new LocalDateTime(a.CreationDate.Year, a.CreationDate.Month, a.CreationDate.Day, a.CreationDate.Hour,
            //    a.CreationDate.Minute, a.CreationDate.Second), now)).Hours >= 24
            //    && a.Status.Equals("Active"))
            //    .Include("interpretator").ToList();
            List<Dream> dreams = db.Dreams.
                Where(a=>a.Status.Equals("Active")
                && a.CreationDate.CompareTo(yesterday)<=0)
                .Include("interpretator").ToList();
            
            return dreams;
        }

        public static UserInfoViewModel getFastestInterpreter(List<Dream> dreams,List<ApplicationUser>users)
        {
            

            List<UserInfoViewModel> result = new List<UserInfoViewModel>();
            foreach (var user in users)
            {
                result.Add(new UserInfoViewModel
                {
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
                    HasRegistered = user.verifiedInterpreter,
                    NumberOfActiveDreams = dreams.Where(a =>a.interpretatorId.Equals(user.Id)&& a.Status.Equals("Active")).ToList().Count(),
                    NumberOfDoneDreams = dreams.Where(a => a.interpretatorId.Equals(user.Id) && a.Status.Equals("Done")).ToList().Count()


                });
            }

            return result.OrderBy(b => b.NumberOfActiveDreams).First();
        }

        public static Replacement AddReplacement(Dream dream, ApplicationUser newInterpretator)
        {
            Replacement replacement = new Replacement();
            replacement.LastModificationDate = DateTime.Now;
            replacement.Newinterpretator = newInterpretator;
            replacement.NewinterpretatorId = newInterpretator.Id;
            replacement.Oldinterpretator = dream.interpretator;
            replacement.OldinterpretatorId = dream.interpretator.Id;
            replacement.Dream = dream;
            replacement.DreamId = dream.id;
            replacement.Reason = "Waiting dream for more than 24 hour";
            replacement.CreationDate = DateTime.Now;

            return replacement;
        }
    }
}