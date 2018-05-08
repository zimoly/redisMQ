using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace JobSchedule
{
    public class FileJobService : JobService<FileJob>
    {
        protected override string GroupName
        {
            get
            {
                return "Job测试";
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected override string JobName
        {
            get
            {
                return "Job测试—文件写入测试";
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        protected override ITrigger GetTrigger()
        {
            var trigger = TriggerBuilder.Create()
                 .WithIdentity("FileJobTrigger", "FileTrigger")
                 .WithSimpleSchedule(x => x.WithIntervalInSeconds(60)
                 .RepeatForever())
                 .Build();
            return trigger;
                
        }
    }
}
