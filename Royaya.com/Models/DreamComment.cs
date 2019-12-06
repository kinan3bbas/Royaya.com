using Royaya.com.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Royaya.com.Models
{
    public class DreamComment : BasicModel
    {
        [Display(Name = "Text")]
        public String Text { get; set; }

        public int DreamId { get; set; }

        public Dream Dream { get; set; }


    }
}