using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ServiceStack.Redis;
using Redis.RedisConfig;

namespace Redis
{
    public partial class RedisTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //RedisUtility.SetCatch<string>("teststring", "helloword", 24 * 60 * 60);
            string testStr = RedisUtility.GetCatch<string>("teststring");
            Response.Write(testStr);
           
        }
    }
}