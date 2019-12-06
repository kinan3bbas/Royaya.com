using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Royaya.com.ScheduledJobs
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().GetAwaiter().GetResult();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<NotificationJob>().Build();

            //ITrigger trigger = TriggerBuilder.Create()
            //.WithIdentity("trigger1", "group1")
            //.StartNow()
            //.WithSimpleSchedule(x => x
            //.WithIntervalInSeconds(10)
            //.RepeatForever())
            //.Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger3", "group1")
            .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(13, 30))
            .ForJob(job)
            .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}