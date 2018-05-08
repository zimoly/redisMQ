using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using IFlight.Common.JobServer.Common;
using IFlight.Common.JobServer.Model;

namespace JobSchedule
{
  public class SchedulerConfigDB
    {
        public static List<SchedulerConfigModel> GetSchedulerRecord(int type)
        {
            List<SchedulerConfigModel> list = new List<SchedulerConfigModel>();

            SchedulerConfigModel model = null;
            SqlConnection con = SqlHelper.GetConnection();

            StringBuilder sb = new StringBuilder();

            //查询所有启动的数据
            if (type == (int)JobEnum.DataType.EnableData)
            {
                sb.Append("select * from SchedulerConfig WITH(NOLOCK) where IsEnable=1 and Status=0 ");
            }
            //查询所有变动数据[status:1新增 2更新 3删除]
            else if (type == (int)JobEnum.DataType.UpdateData)
            {
                sb.Append(" select * from SchedulerConfig WITH(NOLOCK) where IsEnable=1 and Status in (1,2) ");
                sb.Append(" Union all ");
                sb.Append(" select * from SchedulerConfig WITH(NOLOCK) where IsEnable=0 and Status in (3) ");
            }
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, sb.ToString());
            if (ds != null)
            {
                foreach (DataRow col in ds.Tables[0].Rows)
                {
                    model = new SchedulerConfigModel();
                    model.ID = Convert.ToInt32(col["ID"]);
                    model.JobName = col["JobName"].ToString();
                    model.JobGroup = col["JobGroup"].ToString();
                    model.StartTime = Convert.ToDateTime(col["StartTime"]);
                    if (!string.IsNullOrEmpty(col["EndTime"].ToString()))
                        model.EndTime = Convert.ToDateTime(col["EndTime"]);
                    else
                        model.EndTime = null;
                    model.CronTab = col["CronTab"].ToString();
                    model.RequestUrl = col["RequestUrl"].ToString();
                    model.RequestType = Convert.ToInt32(col["RequestType"]);
                    model.Status = Convert.ToInt32(col["Status"]);
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 更新数据状态
        /// </summary>
        /// <param name="status"></param>
        public static bool UpdateStatus(int ID)
        {
            int num = 0;
            SqlConnection con = SqlHelper.GetConnection();

            StringBuilder sb = new StringBuilder();
            sb.Append("update SchedulerConfig set Status=0 where ID=@ID ");
            SqlParameter[] _param = new SqlParameter[] {
                    new SqlParameter("@ID",ID)
                };
            num = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sb.ToString(), _param);

            return num > 0 ? true : false;
        }
        public static bool InsertLog(SchedulerRequest request)
        {
            int num = 0;
            SqlConnection con = SqlHelper.GetConnection();

            StringBuilder sb = new StringBuilder();
            sb.Append(" insert into SchedulerLog (JobGroup,JobName,RequestType,RequestUrl,ReturnMsg,Status,CreateTime) ");
            sb.Append(" values(@JobGroup, @JobName, @RequestType, @RequestUrl, @ReturnMsg,@Status,@CreateTime) ");
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@JobGroup",request.JobGroup),
                new SqlParameter("@JobName",request.JobName),
                new SqlParameter("@RequestType",request.RequestType),
                new SqlParameter("@RequestUrl",request.RequestUrl),
                new SqlParameter("@ReturnMsg",request.ReturnMsg),
                new SqlParameter("@Status",request.LogStatus),
                new SqlParameter("@CreateTime",DateTime.Now)
            };
            num = SqlHelper.ExecuteNonQuery(con, CommandType.Text, sb.ToString(), param);

            return num > 0 ? true : false;
        }
    }
}
