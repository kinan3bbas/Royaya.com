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
    public class Replacement:BasicModel
    {
        public string OldinterpretatorId { get; set; }


        public ApplicationUser Oldinterpretator { get; set; }

        public string NewinterpretatorId { get; set; }


        public ApplicationUser Newinterpretator { get; set; }

        public int DreamId { get; set; }

        public Dream Dream { get; set; }

        public string Reason { get; set; }
    }
}