using Royaya.com.Extras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Royaya.com.Models
{
    public class InterpreterRatio:BasicModel
    {
        public string interpretatorId { get; set; }


        public ApplicationUser interpretator { get; set; }

        public double ratio { get; set; }

        public int pathId { get; set; }


        public InterprationPath path { get; set; }
    }
}