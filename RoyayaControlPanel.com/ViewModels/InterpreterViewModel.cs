using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Royaya.com.ViewModels
{
    public class InterpreterViewModel
    {
        public string id { get; set; }
        public string Email { get; set; }

        public int numberOfDreamsByDay { get; set; }

        public string pictureId { get; set; }

        public string Name { get; set; }

        public double Rating { get; set; }

        public int numberOfActiveDreams { get; set; }

        public int numberOfDoneDreams { get; set; }

        public int numberOfAllDreams { get; set; }

        public double speed { get; set; }

        

    }
}