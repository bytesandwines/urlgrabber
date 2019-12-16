using Louw.PublicSuffix;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebLinkCrawler
{

    public class CustomLink
    {
        public string Url;
        public bool IsProcessed;

        private string mDomainName;
        public string DomainName { get => mDomainName; }

        public CustomLink(string pDomainName)
        {
            mDomainName = pDomainName;
        }
    }
  
    public class LinkDbController
    {

        /// <summary>
        /// A dictonary where
        /// Key   : unique domain name
        /// Value : list of links belonged to that unique domain name. The numbe of links will be determined in the constructor.
        /// </summary>
        private Dictionary<string, List<CustomLink>> mDic;
        private int mMaxNumberDomainCollected;
        private int mMaxNumberLinkDiversity;

        /// <summary>
        /// Her bir url adayı bu listede toplanacak ve bu listeden çekilip işleme tabi tutalacaktır.
        /// Dictionaryi içerisinde mMaxNumberLinkDiversity'den fazla yer alan domainler bu listeden silinecektir.
        /// </summary>
        private List<string> mCandidateUrls;


        private DomainParser mDomainParser;
        private int mWebRequestTimeout = 3000;
        int maxParallelProcess = 100;

        public LinkDbController(int pMaxNumberDomainCollected, int pMaxNumberLinkDiversity, int webRequestTimeout =3000, int parallelDriverCount = 350)
        {
            mDic = new Dictionary<string, List<CustomLink>>();
            mCandidateUrls = new List<string>();
            

            /// Updated: fcdalgic, 01.11.2019
            /// Timeout süresinin kullanıcı arayüzünden seçilebilmesi için eklenmiştir.
            this.mWebRequestTimeout = webRequestTimeout;

            mMaxNumberDomainCollected = pMaxNumberDomainCollected;

            mMaxNumberLinkDiversity = pMaxNumberLinkDiversity;

            this.maxParallelProcess = parallelDriverCount;
            mDomainParser = new DomainParser(new FileTldRuleProvider(@"data\effective_tld_names.dat"));//  new DomainParser(new WebTldRuleProvider());

            /// Nager.PublicSuffix.
           // mDomainParser = new DomainParser(new FileTldRuleProvider(@"data\effective_tld_names.dat"));
        }

        public void AddUrl(string pUrl)
        {
            this.mCandidateUrls.Add(pUrl);
        }

        public void AddRangeCandidates(List<string> pUrlList)
        {
            lock (mCandidateUrls)
            {
                this.mCandidateUrls.AddRange(pUrlList);
            }
        }

        private void AddRangeToDictionary(List<string> urlList, List<DomainInfo> domainList )
        {
            for (int i = 0; i < urlList.Count; i++)
            {
                string url =  urlList[i];
                string host = GetHost(domainList[i]);
                if (!mDic.ContainsKey(host))
                {
                    var tempLink = new CustomLink(host)
                    {
                        IsProcessed = false,
                        Url = url
                    };

                    mDic[host] = new List<CustomLink>();
                    mDic[host].Add(tempLink);
                }
                else
                {
                    if (mDic[host].Count > mMaxNumberLinkDiversity)
                        continue;

                    var tempLink = new CustomLink(host)
                    {
                        IsProcessed = false,
                        Url = url
                    };

                    mDic[host].Add(tempLink);
                }
            }
        }

        public int GetCandidatesCount()
        {
            return this.mCandidateUrls.Count;
        }

        public string GetCandidates()
        {
            string result = "";
            foreach (var candidate in mCandidateUrls)
            {
                result += Environment.NewLine + candidate;
            }

            return result;
        }

        public async Task<bool> ProcessInitialLinks()
        {
            try
            {
                // HTTP, HTTPS veya www ile başlamayan tüm linkleri elemine et!
                mCandidateUrls = mCandidateUrls.Where(url => (url.Length > 5) && (url.StartsWith("wwww") || url.StartsWith("http"))).ToList();

                List<Task<DomainInfo>> tasks = new List<Task<DomainInfo>>();
                Console.WriteLine("Adayların tamamına ait domain bilgileri alınmak üzere taskler oluşturuluyor....");
                DateTime dt1 = DateTime.Now;
                for (int i = 0; i < mCandidateUrls.Count; i++)
                {
                    Task<DomainInfo> task = mDomainParser.ParseAsync(mCandidateUrls[i]);
                    tasks.Add(task);
                }

                Console.WriteLine("Aday domain bilgilerinin alınması için beklenecek....");
                var allDomainTasks = await Task<DomainInfo>.WhenAll(tasks);

                // Dispose all tasks
                TaskHelper.RemoveTaskList(ref tasks);

                var taskResults = allDomainTasks.ToList();
                List<int> removalList = new List<int>();
                for (int i = 0; i < taskResults.Count; i++)
                {
                    if (taskResults[i] == null)
                        removalList.Add(i);
                }

                for (int i = 0; i < removalList.Count; i++)
                {
                    mCandidateUrls.RemoveAt(removalList[i] - i);
                    taskResults.RemoveAt(removalList[i] - i);
                }

                removalList = new List<int>();
                int numberOfCandidate = 0;

                var domainInfos = taskResults.ToList();
                for (int i = 0; i < mCandidateUrls.Count; i++)
                {

                    string host = GetHost(domainInfos[i]);
                    if (string.IsNullOrEmpty(host))
                    {
                        continue;
                    }

                    string tempUrl = mCandidateUrls[i];
                    if (mDic.Keys.Contains(host))
                    {
                        if (mDic[host].Count > mMaxNumberLinkDiversity)
                        {
                            removalList.Add(i);
                            continue;
                        }
                    }
                    else
                    {
                        if (mDic.Keys.Count > mMaxNumberDomainCollected)
                        {
                            removalList.Add(i);
                            continue;
                        }

                        mDic.Add(host, new List<CustomLink>());

                        if (mDic.Keys.Count == mMaxNumberDomainCollected)
                        {
                            removalList.Add(i);
                        }

                    }

                    var tempLink = new CustomLink(host)
                    {
                        IsProcessed = false,
                        Url = tempUrl
                    };

                    mDic[host].Add(tempLink);

                }

                for (int i = 0; i < removalList.Count; i++)
                {
                    mCandidateUrls.RemoveAt(removalList[i] - i);
                    taskResults.RemoveAt(removalList[i] - i);
                }

                List<string> tempCandidates = new List<string>();
                removalList = new List<int>();
                for (int i = 0; i < mCandidateUrls.Count; i++)
                {
                    string host = GetHost(domainInfos[i]);
                    if (mDic[host].Count == mMaxNumberLinkDiversity)
                    {
                        removalList.Add(i);
                    }
                }

                for (int i = 0; i < removalList.Count; i++)
                {
                    mCandidateUrls.RemoveAt(removalList[i] - i);
                    taskResults.RemoveAt(removalList[i] - i);
                }


                /// ADDED: 16.12.2019 
                /// Store only one sample per each domain in the candidate urls
                mCandidateUrls = new List<string>();
                foreach (var host in mDic.Keys)
                {
                    mCandidateUrls.Add(mDic[host][0].Url);
                }


            }
            catch (Exception)
            {
                return false;
            }

            Console.WriteLine("Aday sayisi : " + mCandidateUrls.Count.ToString());
            return true;
        }

        
        public async Task<List<CustomLink>> TakeMax()
        {
            List<CustomLink> val = new List<CustomLink>();

            int processCount = Math.Min(maxParallelProcess, mCandidateUrls.Count);
            List<Task<DomainInfo>> tasks = new List<Task<DomainInfo>>();
            for (int i = 0; i < processCount; i++)
            {
                Task<DomainInfo> task = mDomainParser.ParseAsync(mCandidateUrls[i]);
                tasks.Add(task);
            }

            var allDomainTasks = await Task<DomainInfo>.WhenAll(tasks);

            TaskHelper.RemoveTaskList(ref tasks);

            var domainResults = allDomainTasks.ToList();


            for (int i = 0; i < processCount; i++)
            {
                string host = GetHost(domainResults[i]);
                CustomLink tempLink = new CustomLink(host)
                {
                    IsProcessed = false,
                    Url = mCandidateUrls[0]
                };

                mCandidateUrls.RemoveAt(0);
                val.Add(tempLink);
            }

            return val;
        }



        public async Task<CustomLink> ProcessAndTakeFirst()
        {
            CustomLink tempLink = null;
            try
            {
                tempLink = new CustomLink("");
                DomainInfo info = await mDomainParser.ParseAsync(mCandidateUrls[0]);
                string host = GetHost(info);
                tempLink = new CustomLink(host)
                {
                    IsProcessed = false,
                    Url = mCandidateUrls[0]
                };

            }
            catch (Exception)
            {
                tempLink = null;
                
            }
            finally
            {
                mCandidateUrls.RemoveAt(0);
            }

            return tempLink;

        }


        public async Task<Tuple<List<string>, List<DomainInfo>>> EliminateLinksWhoDoesntHaveDomainInfo(List<string> pUrls)
        {
            try
            {
                if (pUrls == null || pUrls.Count < 1)
                    return null;

                List<int> removalList = new List<int>();


                // HTTP, HTTPS veya www ile başlamayan tüm linkleri elemine et!
                pUrls = pUrls.Where(url => (url.Length > 5) && (url.StartsWith("wwww") || url.StartsWith("http"))).ToList();

                List<Task<DomainInfo>> tasks = new List<Task<DomainInfo>>();
                Console.WriteLine("Adayların tamamına ait domain bilgileri alınmak üzere taskler oluşturuluyor....");
                DateTime dt1 = DateTime.Now;
                for (int i = 0; i < pUrls.Count; i++)
                {
                    Task<DomainInfo> task = mDomainParser.ParseAsync(pUrls[i]);
                    tasks.Add(task);
                }

                Console.WriteLine("Aday domain bilgilerinin alınması için beklenecek....");
                var allDomainTasks = await Task<DomainInfo>.WhenAll(tasks);
                var taskResults = allDomainTasks.ToList();

                for (int i = 0; i < taskResults.Count; i++)
                {
                    if (taskResults[i] == null)
                        removalList.Add(i);
                }

                for (int i = 0; i < removalList.Count; i++)
                {
                    pUrls.RemoveAt(removalList[i] - i);
                    taskResults.RemoveAt(removalList[i] - i);
                }

                var tuple =  new Tuple<List<string>, List<DomainInfo>>(pUrls, taskResults);

                tuple = await RemoveDuplicatesFromTuple(tuple);

                tuple = await RemoveCandidatesByLimit(tuple);

                return tuple;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<Tuple<List<string>, List<DomainInfo>>> EliminateLinksWhichCannotBeAccessed(List<string> pUrls, List<DomainInfo> domainInfos)
        {
            var tuple = await EliminateLinksWhoAreNotAlive(pUrls, domainInfos); // removalTask.Result;

            return tuple;
        }


        public async Task<Tuple<List<string>, List<DomainInfo>>> RemoveDuplicatesFromTuple(Tuple<List<string>, List<DomainInfo>> tuple)
        {
            var urlList = tuple.Item1;
            var domainList = tuple.Item2;
            List<int> removalList = new List<int>();

            Dictionary<string, int> linkOccurrence = new Dictionary<string, int>();
            for (int i = 0; i < urlList.Count; i++)
            {
                var url =  ShortenUrl(urlList[i]);

                if (!linkOccurrence.Keys.Contains(url))
                    linkOccurrence.Add(url, 1);
                else
                    removalList.Add(i);

                //for (int j = 0; j < urlList.Count; j++)
                //{
                //    if (i != j)
                //    {
                //        var url1 = ShortenUrl(urlList[i]);
                //        var url2 = ShortenUrl(urlList[j]);

                //        if (url1 == url2)
                //        {
                //            if (!removalList.Contains(i))
                //                removalList.Add(i);
                //        }
                //    }
                //}
            }

            for (int i = 0; i < removalList.Count; i++)
            {
                urlList.RemoveAt(removalList[i] - i);
                domainList.RemoveAt(removalList[i] - i);
            }

            return new Tuple<List<string>, List<DomainInfo>>(urlList, domainList);
        }

        public async Task<Tuple<List<string>, List<DomainInfo>>> RemoveCandidatesByLimit(Tuple<List<string>, List<DomainInfo>> tuple)
        {
            var urlList = tuple.Item1;
            var domainList = tuple.Item2;
            List<int> removalList = new List<int>();
            Dictionary<string, int> hostList = new Dictionary<string, int>();
            for (int i = 0; i < domainList.Count; i++)
            {
                string host = GetHost(domainList[i]);

                if (!hostList.ContainsKey(host))
                {
                    hostList.Add(host, 0);
                }
                else
                {
                    int occurance = hostList[host];
                    if (occurance > mMaxNumberLinkDiversity)
                    {
                        removalList.Add(i);
                    }
                    else
                    {
                        hostList[host] = hostList[host] + 1;
                    }
                }
            }

            for (int i = 0; i < removalList.Count; i++)
            {
                urlList.RemoveAt(removalList[i] - i);
                domainList.RemoveAt(removalList[i] - i);
            }

            return new Tuple<List<string>, List<DomainInfo>>(urlList, domainList);
        }

        private string ShortenUrl(string url)
        {
            url.Replace(".", "");
            url.Replace("www", "");
            url.Replace("https", "");
            url.Replace("http", "");

            return url;
        }

        public async Task<Tuple<List<string>, List<DomainInfo>>> InsertCandidatesToDatabase(Tuple<List<string>, List<DomainInfo>> tuple)
        {
            
            var urlList = tuple.Item1;
            var domainList = tuple.Item2;

            



            try
            {
                
                int numberOfCandidate = 0;
                List<int> removalList = new List<int>();

                lock (mDic)
                {
                    for (int i = 0; i < urlList.Count; i++)
                    {

                        bool removeAndContinueFlag = false;

                        var tempUrl = urlList[i];
                        var domainInfo = domainList[i];
                        var host = GetHost(domainInfo);

                        if (ContainsRestrictedCharacters(tempUrl) || tempUrl.Length < 5 || string.IsNullOrEmpty(host))
                        {
                            removeAndContinueFlag = true;
                        }
                        /// UPDATED: fcdalgic, 01.11.2019
                        /// Selman hocanın isteği üzerine içerisinde HTTP, HTTPS veya WWW içermeyen tüm linklerin elemine edilmesi için eklenmiştir.
                        else if (!ContainsDesiredUrlParts(tempUrl))
                            removeAndContinueFlag = true;
                        else if (mDic.ContainsKey(host))
                        {
                            // Maximum link çeşitliliğine ulaşılmış ise artık daha fazla bu domainden eklemeyeceğiz.
                            if (mDic[host].Count > mMaxNumberLinkDiversity)
                            {
                                removeAndContinueFlag = true;
                            }
                            //else if (numberOfCandidate > mMaxNumberLinkDiversity)
                                //removeAndContinueFlag = true;
                            else
                            {
                                foreach (var customUrl in mDic[host])
                                {
                                    if (customUrl.Url == tempUrl)
                                    {
                                        removeAndContinueFlag = true;
                                        break;
                                    }
                                    else
                                    {
                                        // HTTP ve HTTPS olarak ayrı olup aslında aynı linke ait olabilirler!
                                        if (customUrl.Url.Contains("https") && tempUrl.Contains("https"))
                                            numberOfCandidate++;
                                        else if (!customUrl.Url.Contains("https") && !tempUrl.Contains("https"))
                                            numberOfCandidate++;
                                        else
                                        {
                                            if (customUrl.Url.Contains("https"))
                                            {
                                                customUrl.Url = customUrl.Url.Remove(0, 8);
                                            }
                                            else if (customUrl.Url.Contains("http"))
                                            {
                                                customUrl.Url = customUrl.Url.Remove(0, 7);
                                            }
                                            if (tempUrl.Contains("https"))
                                            {
                                                tempUrl = tempUrl.Remove(0, 8);
                                            }
                                            else if (tempUrl.Contains("http"))
                                            {
                                                tempUrl = tempUrl.Remove(0, 7);
                                            }

                                            tempUrl = tempUrl.Replace("www.", "");
                                            customUrl.Url = customUrl.Url.Replace("www.", "");

                                            if (customUrl.Url == tempUrl)
                                            {
                                                removeAndContinueFlag = true;
                                                break;
                                            }

                                            numberOfCandidate++;
                                        }
                                    }
                                }

                                if (numberOfCandidate >= mMaxNumberLinkDiversity)
                                    removeAndContinueFlag = true;

                                if (!removeAndContinueFlag)
                                {
                                    // Listedeki bu eleman maksimumum çeşitliliğe ulaşmamış olduğu için silmiyoruz devam ediyoruz.
                                    continue;
                                }
                            }
                        }
                        else
                            continue;

                        if (removeAndContinueFlag)
                        {
                            removalList.Add(i);
                            continue;
                        }
                    }

                    for (int i = 0; i < removalList.Count; i++)
                    {
                        urlList.RemoveAt(removalList[i] - i);
                        domainList.RemoveAt(removalList[i] - i);
                    }
                }


                AddRangeCandidates(urlList);
                AddRangeToDictionary(urlList, domainList);

            }
            catch (Exception ex)
            {
                return null;
            }

            return new Tuple<List<string>, List<DomainInfo>>(urlList,domainList);

        }

  

        /// <summary>
        /// Eliminates the given url list by applying following rules.
        /// 1. The maximum link variety could only be the given mMaxNumberLinkDiversity
        /// 2. 
        /// </summary>
        /// <param name="pUrls"></param>
        /// <returns></returns>
        public async Task<List<string>> EliminateExtractedLinks(string pDomain, List<string> pUrls)
        {

            if (pUrls == null || pUrls.Count < 1)
                return null;

            List<int> removalList = new List<int>();

            int numberOfCandidate = 0;


            // HTTP, HTTPS veya www ile başlamayan tüm linkleri elemine et!
            pUrls = pUrls.Where(url => (url.Length > 5) && (url.StartsWith("wwww") || url.StartsWith("http"))).ToList();

            List <Task<DomainInfo>> tasks = new List<Task<DomainInfo>>();
            Console.WriteLine("Adayların tamamına ait domain bilgileri alınmak üzere taskler oluşturuluyor....");
            DateTime dt1 = DateTime.Now;
            for (int i = 0; i < pUrls.Count; i++)
            {
                Task<DomainInfo> task = mDomainParser.ParseAsync(pUrls[i]);
                tasks.Add(task);
            }

            Console.WriteLine("Aday domain bilgilerinin alınması için beklenecek....");
            var allDomainTasks = await Task<DomainInfo>.WhenAll(tasks);
            var taskResults = allDomainTasks.ToList();

            for (int i = 0; i < taskResults.Count; i++)
            {
                if (taskResults[i] == null)
                    removalList.Add(i);
            }

            for (int i = 0; i < removalList.Count; i++)
            {
                pUrls.RemoveAt(removalList[i] - i);
                taskResults.RemoveAt(removalList[i] - i);
            }

            removalList = new List<int>();

            lock (mDic)
            {
                for (int i = 0; i < pUrls.Count; i++)
                {

                    bool removeAndContinueFlag = false;

                    var tempUrl = pUrls[i];
                    var domainInfo = taskResults[i];
                    var host = GetHost(domainInfo);

                    if (ContainsRestrictedCharacters(tempUrl) || tempUrl.Length < 5 || string.IsNullOrEmpty(host))
                    {
                        removeAndContinueFlag = true;
                    }
                    /// UPDATED: fcdalgic, 01.11.2019
                    /// Selman hocanın isteği üzerine içerisinde HTTP, HTTPS veya WWW içermeyen tüm linklerin elemine edilmesi için eklenmiştir.
                    else if (!ContainsDesiredUrlParts(tempUrl))
                        removeAndContinueFlag = true;
                    else if (mDic.ContainsKey(host))
                    {
                        // Maximum link çeşitliliğine ulaşılmış ise artık daha fazla bu domainden eklemeyeceğiz.
                        if (mDic[host].Count > mMaxNumberLinkDiversity)
                        {
                            removeAndContinueFlag = true;
                        }
                        else if (numberOfCandidate > mMaxNumberLinkDiversity)
                            removeAndContinueFlag = true;
                        else
                        {
                            foreach (var customUrl in mDic[host])
                            {
                                if (customUrl.Url == tempUrl)
                                {
                                    removeAndContinueFlag = true;
                                    break;
                                }
                                else
                                {
                                    // HTTP ve HTTPS olarak ayrı olup aslında aynı linke ait olabilirler!
                                    if (customUrl.Url.Contains("https") && tempUrl.Contains("https"))
                                        numberOfCandidate++;
                                    else if (!customUrl.Url.Contains("https") && !tempUrl.Contains("https"))
                                        numberOfCandidate++;
                                    else
                                    {
                                        if (customUrl.Url.Contains("https"))
                                        {
                                            customUrl.Url = customUrl.Url.Remove(0, 8);
                                        }
                                        else if (customUrl.Url.Contains("http"))
                                        {
                                            customUrl.Url = customUrl.Url.Remove(0, 7);
                                        }
                                        if (tempUrl.Contains("https"))
                                        {
                                            tempUrl = tempUrl.Remove(0, 8);
                                        }
                                        else if (tempUrl.Contains("http"))
                                        {
                                            tempUrl = tempUrl.Remove(0, 7);
                                        }

                                        tempUrl = tempUrl.Replace("www.", "");
                                        customUrl.Url = customUrl.Url.Replace("www.", "");

                                        if (customUrl.Url == tempUrl)
                                        {
                                            removeAndContinueFlag = true;
                                            break;
                                        }

                                        numberOfCandidate++;
                                    }
                                }
                            }

                            if (numberOfCandidate >= mMaxNumberLinkDiversity)
                                removeAndContinueFlag = true;

                            if (!removeAndContinueFlag)
                            {
                                // Listedeki bu eleman maksimumum çeşitliliğe ulaşmamış olduğu için silmiyoruz devam ediyoruz.
                                continue;
                            }
                        }
                    }
                    else
                        continue;

                    if (removeAndContinueFlag)
                    {
                        removalList.Add(i);
                        continue;
                    }
                }

            }

            var domainInfos = taskResults.ToList();
            for (int i = 0; i < removalList.Count; i++)
            {
                pUrls.RemoveAt(removalList[i] - i);
                domainInfos.RemoveAt(removalList[i] - i);
            }

            
            var tuple = await EliminateLinksWhoAreNotAlive(pUrls, domainInfos); // removalTask.Result;

            lock (mDic)
            {
                for (int i = 0; i < tuple.Item1.Count; i++)
                {

                    string host = GetHost(tuple.Item2[i]);
                    if (string.IsNullOrEmpty(host))
                    {
                        continue;
                    }

                    string tempUrl = tuple.Item1[i];
                    if (mDic.Keys.Contains(host))
                    {
                        if (mDic[host].Count > mMaxNumberLinkDiversity)
                            continue;
                    }
                    else
                    {
                        if (mDic.Keys.Count >= mMaxNumberDomainCollected)
                            continue;

                        mDic.Add(host, new List<CustomLink>());

                    }

                    var tempLink = new CustomLink(host)
                    {
                        IsProcessed = false,
                        Url = tempUrl
                    };

                    mDic[host].Add(tempLink);

                }
            }
            
            pUrls=  tuple.Item1;
            return pUrls;
        }


        private bool ContainsDesiredUrlParts(string tempUrl)
        {
            if (tempUrl.StartsWith("http") || tempUrl.StartsWith("www"))
                return true;

            return false;
        }

        private bool CheckGivenUrlIsValidAndAlive(string tempUrl)
        {

            // Step 1:  Check validty
            Uri uriResult;
            bool result = Uri.TryCreate(tempUrl, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
                return false;

            // Step 2 :  Check website status
            WebRequest request = WebRequest.Create(tempUrl);
            request.Timeout = mWebRequestTimeout;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                    return false;
            }
            catch (Exception)
            {
                Console.WriteLine(tempUrl +  " web Sitesine ait  hayatta mı bilgisi alırken zaman aşımına uğradı. Websitesi ölü olarak değerlendirilecektir.");
                return false;
            }


            return true;
        }
        
        private async Task<Tuple<List<string>, List<DomainInfo>>> EliminateLinksWhoAreNotAlive(List<string> pUrls, List<DomainInfo> domainInfos)
        {
            bool[] availabilities = new bool[pUrls.Count];
            List<Task<bool>> tasks = new List<Task<bool>>();

            Console.WriteLine("Taskler oluşturuluyor....");
            DateTime dt1 = DateTime.Now;
            for (int i = 0; i < pUrls.Count; i++)
            {
                Task<bool> task = CheckGivenUrlIsValidAndAliveAsync(pUrls[i]);
                tasks.Add(task);
            }

            Console.WriteLine("Tasklerin bitmesi için beklenecek....");
            var results = await Task<bool>.WhenAll(tasks);
            DateTime dt2 = DateTime.Now;
            Console.WriteLine("Taskler bitti toplam geçen zaman : " + (dt2- dt1).TotalMilliseconds.ToString() + " ms" );
            List<int> removalList = new List<int>();

            
            for (int i = 0; i < results.Length; i++)
            {
                if (!results[i])
                    removalList.Add(i);
            }

            for (int i = 0; i < removalList.Count; i++)
            {
                pUrls.RemoveAt(removalList[i] - i);
                domainInfos.RemoveAt(removalList[i] - i);
            }
            Tuple<List<string>, List<DomainInfo>> val = new Tuple<List<string>, List<DomainInfo>>(pUrls , domainInfos);
            return val;
        }
        private async Task<bool> CheckGivenUrlIsValidAndAliveAsync(string tempUrl)
        {

            // Step 1:  Check validty
            Uri uriResult;
            bool result = Uri.TryCreate(tempUrl, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
                return false;

            // Step 2 :  Check website status
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(tempUrl);
            request.Timeout = mWebRequestTimeout;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                result = !(response == null || response.StatusCode != HttpStatusCode.OK);
                response.Close();

                return result;
            }
            catch (Exception)
            {
                Console.WriteLine(tempUrl + " web Sitesine ait  hayatta mı bilgisi alırken zaman aşımına uğradı. Websitesi ölü olarak değerlendirilecektir.");
                return false;
            }
        }
        private bool ContainsRestrictedCharacters(string pUrl)
        {
            if (pUrl == null || String.IsNullOrEmpty(pUrl)  || pUrl.Contains('#'))
                return true;

            return false;
        }

        private string GetHost(DomainInfo domainInfo)
        {
            try
            {
                return domainInfo.Domain; // + "." + domainInfo.Tld;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured during extracting hostname from given url. Message : " + ex.Message + Environment.NewLine +
                    "StackTrace : " + ex.StackTrace);

                return string.Empty;
            }
            
        }

        public int GetCount()
        {
            var count = 0;
            foreach (var key in mDic.Keys)
            {
                count += mDic[key].Count;
            }

            return count;
        }

        public bool IsSearchCompleted
        {
            get
            {
                if (GetCount() >= mMaxNumberDomainCollected * mMaxNumberLinkDiversity)
                    return true;

                return false;
            }
        }
        public void Save(string filePath)
        {
            var outputList = new List<string>();
            lock(mDic)
            {
                foreach (var key in mDic.Keys)
                {
                    foreach (var customUrl in mDic[key])
                    {
                        outputList.Add(customUrl.Url);
                    }
                }
            }
          
            
            File.WriteAllLines(filePath, outputList);
        }

        public void UpdateOutput(string filePath, List<string> urls)
        {
            File.WriteAllLines(filePath, urls);
        }



        public bool TryReadAllLines(string pPath, out List<string> output)
        {
            output = null;
            try
            {
                output = File.ReadAllLines(pPath).ToList();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public string GetDictionaryContent()
        {
            string result = "";
            lock (mDic)
            {
                if (mDic == null || mDic.Keys.Count < 1)
                {
                    result = "Either dictionary not found or is empty";
                }
                else
                {
                    result += "Total Parsed Domain : " + mDic.Keys.Count + Environment.NewLine;
                    foreach (var key in mDic.Keys)
                    {
                        result += key + " : " + mDic[key].Count + Environment.NewLine;
                    }
                }
            }
            

            return result;

        }

        public string FixLastCharacter(string input)
        {
            if (input.Last() == '/')
                input = input.Remove(input.Length - 1, 1);

            return input;
        }
    }
}
