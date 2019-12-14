using Newtonsoft.Json;
using Royaya.com.Extras;
using RoyayaControlPanel.com.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Royaya.com.Models
{


    [JsonObject(MemberSerialization.OptIn)]
    public class Dream:BasicModel
    {
        [JsonProperty]
        [Display(Name = "Status")]
        public String Status { get; set; } //Active, Done

        [JsonProperty]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [JsonProperty]
        public string interpretatorId { get; set; }

        [JsonProperty]
        public ApplicationUser interpretator { get; set; }

        [JsonProperty]
        public int pathId { get; set; }

        [JsonProperty]
        public InterprationPath path { get; set; }



        [JsonProperty]
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

    }
}