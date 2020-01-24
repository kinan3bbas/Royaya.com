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
        [Display(Name = "Status",ResourceType = typeof(Resources.Dream))]
        public String Status { get; set; } //Active, Done

        [JsonProperty]
        [Display(Name = "Description", ResourceType = typeof(Resources.Dream))]
        public string Description { get; set; }

        [JsonProperty]
        [Display(Name = "interpretatorId", ResourceType = typeof(Resources.Dream))]
        public string interpretatorId { get; set; }

        [JsonProperty]
        public ApplicationUser interpretator { get; set; }

        [JsonProperty]
        [Display(Name = "Cost", ResourceType = typeof(Resources.Dream))]
        public int pathId { get; set; }

        [JsonProperty]
        public InterprationPath path { get; set; }



        [JsonProperty]
        [Display(Name = "Explanation", ResourceType = typeof(Resources.Dream))]
        public string Explanation { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "ExplanationDate", ResourceType = typeof(Resources.Dream))]
        public DateTime? ExplanationDate { get; set; }

        [Display(Name = "UserRating", ResourceType = typeof(Resources.Dream))]
        public int UserRating { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "RatingDate", ResourceType = typeof(Resources.Dream))]
        public DateTime? RatingDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "InterpretationStartDate", ResourceType = typeof(Resources.Dream))]
        public DateTime? InterpretationStartDate { get; set; }

        public long numberOfViews { get; set; }

        public long numberOfLikes { get; set; }

    }
}