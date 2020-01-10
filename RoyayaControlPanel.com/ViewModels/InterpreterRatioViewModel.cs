using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoyayaControlPanel.com.ViewModels
{
    public class InterpreterRatioViewModel
    {
        

        public string interpretatorId { get; set; }

        public String interpretatorName { get; set; }

        public int pathId { get; set; }

        public double pathCost { get; set; }

        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        public DateTime CreationDate { get; set; }

        public int id { get; set; }

        public double Ratio { get; set; }

        public InterpreterRatioViewModel() { }

    }
}