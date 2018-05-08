using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IFlight.Common.JobServer.Common
{
    public class SchedulerHelper
    {

        private readonly IScheduler scheduler;
        public SchedulerHelper()
        {
            ISchedulerFactory sf = new StdSchedulerFactory();
            try
            {
                scheduler = sf.GetScheduler();
            }
            catch (SchedulerException e)
            {
               
            }
        }

        /// <summary>
        /// 关闭调度信息
        /// </summary>
        public void shutdown()
        {
            scheduler.Shutdown();
        }

        /// <summary>
        /// 添加调度的job信息 
        /// </summary>
        /// <param name="jobdetail">Job信息</param>
        /// <param name="trigger">触发器信息</param>
        /// <returns></returns>
        public DateTimeOffset scheduleJob(IJobDetail jobdetail, ITrigger trigger)
        {
            scheduler.Start();
            return scheduler.ScheduleJob(jobdetail, trigger);
        }

        /// <summary>
        /// 返回触发器状态
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TriggerState triggerStatus(TriggerKey key)
        {
            TriggerState state = scheduler.GetTriggerState(key);
            return state;
        }

        /// <summary>
        /// 判断Job是否存在，存在并删除
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public bool IsExistsDelJob(string JobName, string JobGroup)
        {
            var result = false;
            JobKey key = new JobKey(JobName, JobGroup);
            var is_exists = scheduler.CheckExists(key);
            if (is_exists)
                result = scheduler.DeleteJob(key);
            else
                result = true;

            return result;
        }
    }
}
