using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFlight.Common.JobServer.Common
{
    public class JobEnum
    {
        public enum DataType
        {
            /// <summary>
            /// 启用数据
            /// </summary>
            EnableData = 1,

            /// <summary>
            /// 变动数据
            /// </summary>
            UpdateData = 2
        }

        public enum DataState
        {
            /// <summary>
            /// 新增
            /// </summary>
            Insert = 1,

            /// <summary>
            /// 更新
            /// </summary>
            Update = 2,

            /// <summary>
            /// 删除
            /// </summary>
            Delete = 3
        }

        public enum RequestType
        {
            /// <summary>
            /// 请求方式GET
            /// </summary>
            Get = 1,

            /// <summary>
            /// 请求方式POST
            /// </summary>
            Post = 2
        }

        public enum LogStatus
        {
            /// <summary>
            /// 成功的日志消息
            /// </summary>
            SuccessInfo = 0,

            /// <summary>
            /// 失败的日志消息
            /// </summary>
            FailedInfo = 1,

            /// <summary>
            /// 异常的日志消息
            /// </summary>
            ExceptInfo = 2
        }
    }
}
