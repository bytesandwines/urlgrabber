using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace WebLinkCrawler
{
    public class WebDriverController
    {
        
        HtmlWeb mAgiltiyDriver;
        private HtmlDocument mHTMLDoc;
        /// <summary>
        /// Page loading timeout in ms
        /// </summary>
        public int PageLoadTimeout = 3000;
        private string mCurrentLink = "";
        public WebDriverController()
        {
            mAgiltiyDriver = new HtmlWeb();
            mHTMLDoc = new HtmlDocument();
        }
        private System.Timers.Timer mRequestTimer;
        CancellationTokenSource mRequstTokenSource;
        CancellationToken token;
        bool isCompleted = false;
        public async Task Navigate(string pUrl, int timeout = 2000)
        {
            // mDriver.Navigate().GoToUrl(pUrl);
            mCurrentLink = pUrl;
            //mRequestTimer = new System.Timers.Timer(timeout);
            //mRequestTimer.Elapsed += MRequestTimer_Elapsed;
            //mRequstTokenSource = new CancellationTokenSource();
            //isCompleted = false;
            //token = mRequstTokenSource.Token;
            //mRequestTimer.Start();
            
            mHTMLDoc = await mAgiltiyDriver.LoadFromWebAsync(pUrl, token);
            isCompleted = true;
        }

        private void MRequestTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!isCompleted)
                mRequstTokenSource.Cancel();

            mRequestTimer.Stop();
            mRequestTimer = null;
        }

        public async Task<List<string>> ExtractLinks()
        {
            List<string> result = new List<string>();
            var nodeCollection = mHTMLDoc.DocumentNode.SelectNodes("//a");

            if (nodeCollection != null)
            {
                foreach (HtmlNode node in mHTMLDoc.DocumentNode.SelectNodes("//a"))
                {
                    if (node != null && node.HasAttributes)
                    {

                        string curUrl = node.GetAttributeValue("href", string.Empty);

                        if (!string.IsNullOrEmpty(curUrl) && curUrl != "/")
                        {
                            if (curUrl.First() == '/')
                            {
                                // curUrl =  curUrl.Remove(0, 1);
                                curUrl = mCurrentLink + curUrl;
                                // To be able to prevent double depth on url.
                                curUrl = curUrl.Replace("//", "/");
                            }

                            if (curUrl.Last() == '/')
                            {
                                curUrl = curUrl.Remove(curUrl.Length - 1, 1);
                            }

                            result.Add(curUrl);
                        }
                            
                    }
                }
            }
           
            return result;
        }

        public void CloseDriver()
        {
            
        }


    }
}
