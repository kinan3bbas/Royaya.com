using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyayaControlPanel.com.ViewModels
{
    public class ContactUsViewModel
    {
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string creatorName { get; set; }
        public string creatorId { get; set; }
        public string interpreterName { get; set; }
        public string interpreterId { get; set; }

        public int id { get; set; }

        public DateTime  CreationDate { get; set; }

        public string Status { get; set; }
    }
}