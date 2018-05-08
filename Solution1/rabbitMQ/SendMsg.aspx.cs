using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using RabbitMQ.Client;
using System.IO;


namespace rabbitMQ
{
    public partial class SendMsg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //HttpCookie cookie = new HttpCookie("RedisSessionId", "string value1"); Response.Cookies.Add(cookie);
            Session["UserId"] = 123;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            var factory = new ConnectionFactory() { HostName = "localhost",UserName="guest",Password= "guest" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("testQueue", true, false, false, null);//hello是queue的名字
                    string message = this.TextBox1.Text;
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;
                    channel.BasicPublish("", "testQueue", properties, body);//hello是routing key的名字
                    this.Label1.Text += $" [x] Sent {message}";
                }
            }
        }
    }
}