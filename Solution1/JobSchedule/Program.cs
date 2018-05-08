using System;
using System.Collections.Generic;
using System.ServiceProcess;
using IFlight.Common.JobServer.Common;
using IFlight.Common.JobServer.Model;
using Quartz;
using Quartz.Impl.Triggers;
using Newtonsoft.Json;

namespace JobSchedule
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new JobManager()
            //};
            //ServiceBase.Run(ServicesToRun);

            //test　Code
            //FileJob job = new FileJob();
            //job.Execute(null);

            #region
            List<SchedulerConfigModel> list1 = SchedulerConfigDB.GetSchedulerRecord((int)JobEnum.DataType.UpdateData);
            if (list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    var list = list1[i];
                    SchedulerHelper scheduler = new SchedulerHelper();
                    var flag = true;
                    SchedulerRequest request = new SchedulerRequest();
                    request.JobName = list.JobName;
                    request.JobGroup = list.JobGroup;
                    request.TriggerName = list.JobName + "Trigger";
                    request.TriggerGroupName = list.JobGroup + "Trigger";
                    request.CronTab = list.CronTab;
                    request.StartTime = list.StartTime;
                    if (list.EndTime != null)
                        request.EndTime = list.EndTime;
                    else
                        request.EndTime = null;
                    request.RequestType = list.RequestType;
                    request.RequestUrl = list.RequestUrl;
                    var json = JsonConvert.SerializeObject(request);
                    scheduler.IsExistsDelJob(request.JobName, request.JobGroup);
                    if (flag)
                    {
                        IJobDetail jobDetail = JobBuilder.Create<FileJob>()
                         .WithIdentity(request.JobName, request.JobGroup)
                         .UsingJobData("jobJson", json)
                         .Build();

                        CronTriggerImpl tigger = (CronTriggerImpl)TriggerBuilder.Create()
                         .WithIdentity(request.TriggerName, request.TriggerGroupName)
                         .WithCronSchedule(request.CronTab)
                         .ForJob(request.JobName, request.JobGroup)
                         .StartNow()
                         .Build();

                        DateTimeOffset dt = scheduler.scheduleJob(jobDetail, tigger);

                    }
                }
               
            }
            #endregion
        }
    }
}
