using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace JobSchedule
{
   public abstract class JobService<T>where T:IJob
    {
        protected abstract string JobName { get; set; }
        protected abstract string GroupName { get; set; }
        private IJobDetail GetJobDetail()
        {
            var job =JobBuilder.Create<T>()
                .WithIdentity(JobName, GroupName)
                .Build();
            return job;
        }
        protected abstract ITrigger GetTrigger();
        public void AddJobToSchedule(IScheduler scheduler)
        {
            scheduler.ScheduleJob(GetJobDetail(), GetTrigger());
        }
    }
}
