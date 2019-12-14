using Newtonsoft.Json;
using Royaya.com.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Royaya.com.Models
{
    public class ContactUs:BasicModel
    {
        [Required]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

    }
}