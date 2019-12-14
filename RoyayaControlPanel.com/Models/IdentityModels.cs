using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Royaya.com.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace RoyayaControlPanel.com.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Creation Date")]
        public DateTime? CreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Last Modification Date")]
        public DateTime? LastModificationDate { get; set; }



        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Martial Status")]
        public string MartialStatus { get; set; }

        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Joining Date")]
        public DateTime? JoiningDate { get; set; }

        [Display(Name = "Picture Id")]
        public string PictureId { get; set; }

        public int numbOfDreamsInOneDay { get; set; }

        public ICollection<Dream> Dreams { get; set; }

        public int Age { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<InterprationPath> InterprationPaths { get; set; }

        public DbSet<Dream> Dreams { get; set; }

        public DbSet<ContactUs> ContactUss { get; set; }

        public DbSet<DreamComment> DreamComments { get; set; }

        public DbSet<NotificationLog> NotificationLogs { get; set; }
        public DbSet<UsersDeviceTokens> UsersDeviceTokens { get; set; }


        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}