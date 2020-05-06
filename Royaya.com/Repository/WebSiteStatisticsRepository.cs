using Royaya.com.Controllers;
using Royaya.com.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Royaya.com.Repository
{
    public class WebSiteStatisticsRepository
    {
        private CoreController core= new CoreController();
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Create(WebSiteStatistics item)
        {
            item.CreationDate = DateTime.Now;
            item.LastModificationDate = DateTime.Now;
            item.Creator = core.getCurrentUser().Id;
            item.Modifier = core.getCurrentUser().Id;
            db.WebSiteStatistics.Add(item);
            db.SaveChanges();
        }
    }
}