using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Royaya.com.ViewModels
{
    public class InterpreterUserModel
    {
        public string Id { get; set; }
        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Martial Status")]
        public string MartialStatus { get; set; }

        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Joining Date")]
        public DateTime? JoiningDate { get; set; }

        public string Type { get; set; }


        public int Age { get; set; }
        public int numbOfDreamsInOneDay { get; set; }

    }
}