using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Royaya.com.Models;

namespace Royaya.com.ViewModels
{
    public class InterpretorPlans
    {
        public string id { get; set; }

        public InterprationPath path { get; set; }
        public long numberOfDreams { get; set; }
        public string waitingTime { get; set; }


    }

   
}