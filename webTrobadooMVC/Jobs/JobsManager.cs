using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Logging;
using Quartz;
using Quartz.Impl;

namespace trobadoo.com.web.Jobs
{
    public class JobsManager
    {
       private static ILog Log = LogManager.GetLogger("JobsManager");

       public static void initJobs()
        {
  
            try
            {
                // construct a scheduler factory
                ISchedulerFactory schedFact = new StdSchedulerFactory();
 
                // get a scheduler
                IScheduler sched = schedFact.GetScheduler();
                sched.Start();
 
                IJobDetail job = JobBuilder.Create<DatabaseUpdateJob>()
                    .WithIdentity("myJob", "group1")
                    .Build();
 
                ITrigger trigger = TriggerBuilder.Create()
                    .WithDailyTimeIntervalSchedule
                    (s =>
                        s.WithIntervalInSeconds(30)
                            .OnEveryDay()
                            //.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(10, 15))
                    )
                    .Build();
 
                sched.ScheduleJob(job, trigger);
            }
            catch (ArgumentException e)
            {
                Log.Error(e);
            } 
        }
    }
}