using Royaya.com.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoyayaControlPanel.com.Models;

namespace RoyayaControlPanel.Repository
{
    public class WebSiteStatisticsRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Create(WebSiteStatistics item)
        {
            item.CreationDate = DateTime.Now;
            item.LastModificationDate = DateTime.Now;
            db.WebSiteStatistics.Add(item);
            db.SaveChanges();
        }
    }
}