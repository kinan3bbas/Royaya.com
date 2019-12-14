using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyayaControlPanel.com.ViewModels
{
    public class DreamViewModel
    {
        [Display(Name = "Status")]
        public String Status { get; set; } //Active, Done

        [Display(Name = "Description")]
        public string Description { get; set; }

        public string interpretatorId { get; set; }

        public String interpretatorName { get; set; }

        public int pathId { get; set; }

        public double pathCost { get; set; }


        [Display(Name = "Explanation")]
        public string Explanation { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Explanation Date")]
        public DateTime? ExplanationDate { get; set; }

        [Display(Name = "User's Rating")]
        public int UserRating { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Rating Date")]
        public DateTime? RatingDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Interpretation Start Date")]
        public DateTime? InterpretationStartDate { get; set; }

        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        public DateTime CreationDate { get; set; }

        public int id { get; set; }

    }
}