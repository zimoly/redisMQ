using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFlight.Common.JobServer.Model
{
    public class SchedulerConfigModel
    {
        /// <summary>
        /// 主健
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Job所属组别
        /// </summary>
        public string JobGroup { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 请求方式 1：get；2：post
        /// </summary>
        public int RequestType { get; set; }

        /// <summary>
        /// 请求URL
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// Corn表达式
        /// </summary>
        public string CronTab { get; set; }

        /// <summary>
        /// Job备注
        /// </summary>
        public string JobRemark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public string LastUpdateUser { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否启用 0：未启用；1：启用
        /// </summary>
        public int IsEnable { get; set; }

        /// <summary>
        /// 数据状态【1新增 2更新 3删除】
        /// </summary>
        public int Status { get; set; }
    }
}
