using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Redis
{
    public partial class _Default : Page
    {
        string loginuser = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            loginuser =reloadLoginUser(Request.Cookies);
            var tempStr = Session["LoginUser"];

            if (string.IsNullOrEmpty(loginuser))
            {
                string cookieName = ConfigurationManager.AppSettings["CookieKeyName"];
                HttpCookie cookie = new HttpCookie(cookieName);
                //cookie.Domain = AppConfig.GetAppSetting("Domain");
               
                loginuser = "分布式session测试";
                cookie.Value = loginuser;
                Response.Cookies.Add(cookie);


                Session.Abandon();
                Session["LoginUser"] = loginuser;
                //cookie.Expires.AddDays(1);
            }
            else
            {
                Session["LoginUser"] = loginuser;// "ceshitestsessionetest";
            }
        }
        public static string reloadLoginUser(HttpCookieCollection cookies)
        {
            string cookieName = ConfigurationManager.AppSettings["CookieKeyName"];
            var cookie = cookies[cookieName];
            if (cookie == null || cookie.Value == null) return null;

             return  cookie.Value;
            //string[] account = getAccountCookie(cookies);
            /////0:username 1:sessionId 2:localdate 3:type
            //if (account == null) return null;
            //return  account[0];
          
        }
        private static string[] getAccountCookie(HttpCookieCollection cookies)
        {
            try
            {
                
                string[] account =null;

                ///0:username 1:sessionId 2:localdate 3:type
                if (account.Length < 2) return null;
                return account;
            }
            catch
            {
                return null;
            }
        }
    }
}