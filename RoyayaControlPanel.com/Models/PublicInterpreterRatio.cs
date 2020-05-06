using Royaya.com.Extras;
using Royaya.com.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoyayaControlPanel.com.Models
{
    public class PublicInterpreterRatio : BasicModel
    {

        public double ratio { get; set; }

        public int pathId { get; set; }


        public InterprationPath path { get; set; }
    }
}