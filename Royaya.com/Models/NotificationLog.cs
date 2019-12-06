using Royaya.com.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Royaya.com.Models
{
    public class NotificationLog:BasicModel
    {
        [Display (Name ="Message")]
        public string message { get; set; }

        public String UserId { get; set; }

        public ApplicationUser User { get; set; }

    }
}