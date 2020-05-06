using Newtonsoft.Json;
using Royaya.com.Extras;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Royaya.com.Models
{
    public class WebSiteStatistics : BasicModel
    {
        public int numberOfVisits { get; set; }
    }
}