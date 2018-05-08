using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFlight.Common.JobServer.Model
{
    public class SchedulerRequest
    {
        /// <summary>
        /// Job所属组别
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// Job方法类
        /// </summary>
        public object JobType { get; set; }

        /// <summary>
        /// 触发器名字
        /// </summary>
        public string TriggerName { get; set; }

        /// <summary>
        /// 触发器组别名字
        /// </summary>
        public string TriggerGroupName { get; set; }

        /// <summary>
        /// Job执行开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Job执行结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string CronTab { get; set; }

        /// <summary>
        /// 请求方式 1：get；2：post
        /// </summary>
        public int RequestType { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        public string RequestUrl { get; set; }
        
        /// <summary>
        /// 返回的信息
        /// </summary>
        public string ReturnMsg { get; set; }

        /// <summary>
        /// 日志数据状态 0成功 1失败 2异常
        /// </summary>
        public int LogStatus { get; set; }

    }
}
