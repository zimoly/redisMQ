using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace 异步操作
{
    public partial class asyncForm : System.Web.UI.Page
    {
        private IAsyncResult r = null;
        private delegate string Async(string strMsg);
        public delegate string AsyncMethodCaller(int callDuration, out int threadId);
        protected void Page_Load(object sender, EventArgs e)
        {
            #region
            // The asynchronous method puts the thread id here.
            int threadId;
            // Create the delegate.
            AsyncMethodCaller caller = new AsyncMethodCaller(TestMethod);

            // Initiate the asychronous call.
            IAsyncResult result = caller.BeginInvoke(3000,
                out threadId, null, null);
            string returnValue = caller.EndInvoke(out threadId, result);
            #endregion

            #region
            //Async async = null;
            //async = new Async(o =>
            //        {

            //            return "异步返回值";
            //        });
            //r = async.BeginInvoke("异步测试", new AsyncCallback(Done), async);
            //r.AsyncWaitHandle.WaitOne();

            #endregion

        }
        public string TestMethod(int callDuration, out int threadId)
        {
            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;
            return String.Format("My call time was {0}.", callDuration.ToString());
        }
        private void Done(IAsyncResult resule)
        {
            try
            {
                Async async = resule.AsyncState as Async;
                var s = async.EndInvoke(r);
                this.Label1.Text = s;
                r.AsyncWaitHandle.Close();
                r = null;
            }
            catch (Exception ee)
            {
            }

        }
    }

}