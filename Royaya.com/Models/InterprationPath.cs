using Royaya.com.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Royaya.com.Models
{
    public class InterprationPath:BasicModel
    {
        [Required]
        [Display(Name = "Cost")]
        public double Cost { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status  { get; set; } // Available, Suspended, Deleted
    }
}