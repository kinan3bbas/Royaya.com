using Newtonsoft.Json;
using Royaya.com.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Royaya.com.Models
{


    public class Dream:BasicModel
    {
        
        [Display(Name = "Status")]
        public String Status { get; set; } //Active, Done

        
        [Display(Name = "Description")]
        public string Description { get; set; }

        
        public string interpretatorId { get; set; }

        
        public ApplicationUser interpretator { get; set; }

        
        public int pathId { get; set; }

        
        public InterprationPath path { get; set; }



        
        [Display(Name = "Explanation")]
        public string Explanation { get; set; }

        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Explanation Date")]
        public DateTime? ExplanationDate { get; set; }

        
        [Display(Name ="User's Rating")]
        public int UserRating { get; set; }

        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Rating Date")]
        public DateTime? RatingDate { get; set; }

        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Interpretation Start Date")]
        public DateTime? InterpretationStartDate { get; set; }

        
        public long numberOfViews { get; set; }
        
        public long numberOfLikes { get; set; }

        public string CreatorFireBaseId { get; set; }
        public string InterpreterFireBaseId { get; set; }

        public bool PaidDream { get; set; }

    }
}