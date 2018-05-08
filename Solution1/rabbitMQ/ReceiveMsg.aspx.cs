using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace rabbitMQ
{
    public partial class ReceiveMsg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("testQueue", true, false, false, null);//hello是queue的名字
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("testQueue", true, consumer);//hello是queue的名字,这里可以理解为hello是routing key的名字。因为这个例子没有使用指定名称的exchange（实际上使用的是默认的exchange名字），所以queue的名字和routing key的名字是相同的。在第五篇文章中介绍如果使用了指定名称的exchange，queue name和routing key的关系与用法。
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();//阻塞
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        this.Label1.Text += $" [x] Received {message}";
                    }
                }
            }
        }
    }
}