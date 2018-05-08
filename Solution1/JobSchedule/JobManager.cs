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
    public partial class JobManager : ServiceBase
    {        public JobManager()
        {
            InitializeComponent();
            System.Timers.Timer t = new System.Timers.Timer();
            t.Elapsed += new System.Timers.ElapsedEventHandler(TimedExecuteJob);
            t.Interval = 6000;
            t.AutoReset = true;
            t.Enabled = true;
        }
        //string filePath = @"D:\软件测试\jobtest.txt";
        protected override void OnStart(string[] args)
        {
            try
            {
                List<SchedulerConfigModel> list = SchedulerConfigDB.GetSchedulerRecord((int)JobEnum.DataType.EnableData);
                if (list != null && list.Count > 0)
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        ExecuteWebApi(list[i]);
                    }
                }
            }
            catch (Exception ex)
            {
              
            }
        }

        protected override void OnStop()
        {
            SchedulerHelper op = new SchedulerHelper();
            op.shutdown();
            ScheduleBase.Scheduler.Shutdown(true);
            //using (FileStream stream = new FileStream(filePath, FileMode.Append))
            //using (StreamWriter writer = new StreamWriter(stream))
            //{
            //    writer.WriteLine($"{DateTime.Now},服务停止！");
            //}
        }
        public void TimedExecuteJob(object source, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                SchedulerHelper op = new SchedulerHelper();
                //查询变动的数据
                List<SchedulerConfigModel> list = SchedulerConfigDB.GetSchedulerRecord((int)JobEnum.DataType.UpdateData);

                if (list != null && list.Count > 0)
                {
                    for (var i = 0; i < list.Count; i++)
                    {
                        //新增|更新
                        if (list[i].Status == (int)JobEnum.DataState.Insert || list[i].Status == (int)JobEnum.DataState.Update)
                        {
                            //判断Job是否存在，存在并删除                      
                            var _delete = op.IsExistsDelJob(list[i].JobName, list[i].JobGroup);
                            if (_delete)
                            {
                                //启用新的Job
                                var _update = ExecuteWebApi(list[i]);
                                if (_update)
                                    //将数据状态更新为0
                                    SchedulerConfigDB.UpdateStatus(list[i].ID);
                            }
                        }
                        //禁用Job
                        else if (list[i].Status == (int)JobEnum.DataState.Delete)
                        {
                            //判断Job是否存在，存在并删除
                            var _delete = op.IsExistsDelJob(list[i].JobName, list[i].JobGroup);

                            if (_delete)
                                //将数据状态更新为0
                                SchedulerConfigDB.UpdateStatus(list[i].ID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        private bool ExecuteWebApi(SchedulerConfigModel list)
        {

            try
            {
                var flag = true;
                SchedulerHelper scheduler = new SchedulerHelper();

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

                DateTimeOffset? end = null;
                if (request.EndTime != null)
                {
                    end = DateTime.SpecifyKind(Convert.ToDateTime(request.EndTime), DateTimeKind.Local);
                    if (request.EndTime < DateTime.Now)
                        flag = false;
                }
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
                     .EndAt(end)
                     .Build();

                    DateTimeOffset dt = scheduler.scheduleJob(jobDetail, tigger);

                }
                else
                {

                }

                return true;
            }
            catch (Exception ex)
            {
              
                //插入日志
                SchedulerRequest request = new SchedulerRequest();
                request.JobGroup = list.JobGroup;
                request.JobName = list.JobName;
                request.RequestType = list.RequestType;
                request.RequestUrl = list.RequestUrl;
                request.ReturnMsg = "Job-Exception: " + (ex.ToString().Length > 450 ? ex.ToString().Substring(0, 450) : ex.ToString());
                request.LogStatus = (int)JobEnum.LogStatus.ExceptInfo;
                SchedulerConfigDB.InsertLog(request);
                //数据状态更新为0,防止重复执行
                SchedulerConfigDB.UpdateStatus(list.ID);
                return false;
            }
        }
    }
}
