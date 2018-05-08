using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Quartz;
using IFlight.Common.JobServer.Model;
using IFlight.Common.JobServer.Common;
using Newtonsoft.Json;
using System.Net.Http;

namespace JobSchedule
{
  public class FileJob:IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //获得requestUrl和requestType
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            string jobJson = dataMap.GetString("jobJson");
            writeLog($"jobJson:{ jobJson}");
            var result = JsonConvert.DeserializeObject<SchedulerRequest>(jobJson);
            try
            {
                writeLog($"jobJson:{ result.RequestUrl}","1");
                var resultMsg = string.Empty;
                HttpClient client = new HttpClient();
                //GET请求
                if (result.RequestType == (int)JobEnum.RequestType.Get)
                {
                    resultMsg = client.GetAsync(result.RequestUrl).Result.Content.ReadAsStringAsync().Result;
                    writeLog($"GET请求resultMsg:{ resultMsg}");
                }
                //POST请求
                else if (result.RequestType == (int)JobEnum.RequestType.Post)
                {
                    SchedulerRequest re = new SchedulerRequest();
                    var content = JsonConvert.SerializeObject(re);
                    resultMsg = client.PostAsync(result.RequestUrl, new StringContent(content)).Result.Content.ReadAsStringAsync().Result;
                    writeLog($"POST请求resultMsg:{ resultMsg}");
                }
                //保存结果到数据库
                if (resultMsg.ToUpper().Contains("SUCCESS"))
                    result.LogStatus = (int)JobEnum.LogStatus.SuccessInfo;
                else
                    result.LogStatus = (int)JobEnum.LogStatus.FailedInfo;
                result.ReturnMsg = resultMsg.Length > 800 ? resultMsg.Substring(0, 800) : resultMsg;
                SchedulerConfigDB.InsertLog(result);
            }
            catch (Exception ex)
            {
                //设置将自动去除这个任务的触发器,所以这个任务不会再执行 
                //JobExecutionException ex_job = new JobExecutionException(ex);
                //ex_job.UnscheduleAllTriggers = true;
                //保存到数据库
                result.ReturnMsg = "IJob-Exception: " + (ex.ToString().Length > 450 ? ex.ToString().Substring(0, 450) : ex.ToString());
                result.LogStatus = (int)JobEnum.LogStatus.ExceptInfo;
                SchedulerConfigDB.InsertLog(result);
            }
        }
        public void writeLog(string msg,string type="0")
        {
            string filePath = @"D:\软件测试\jobtest.txt";
            if (type == "1")
            {
                filePath = @"D:\软件测试\jobTime.txt";
            }
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now},{msg}！");
            }
        }
    }
}
