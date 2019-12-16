using Louw.PublicSuffix;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebLinkCrawler
{
    public partial class Form1 : Form
    {
        private string mSelectedFilePath;
        private string mSelectedDriverPath;
        private string mOutputFilePath;

        private WebDriverController mDriverController;
        private LinkDbController mDbController;

        private int mPageLoadTimeout = 4000;

        private bool mFlagStop = false;
        private int initialLinkCount;
        private int mTaskCount = 0;
        private List<bool> taskResults = new List<bool>();
        public Form1()
        {
            InitializeComponent();

            // cbDriverType.SelectedIndex = 0;

            mSelectedFilePath = tbFile.Text;
            // mSelectedDriverPath = tbDriverPath.Text;
            mOutputFilePath = tbOutputCSVFile.Text;

            this.btnStart.EnabledChanged += BtnStart_EnabledChanged;
            this.btnStop.EnabledChanged += BtnStop_EnabledChanged;
            this.btnFix.EnabledChanged += BtnFix_EnabledChanged;
            if (!InitializeControllers())
                return;
        }

        private void BtnFix_EnabledChanged(object sender, EventArgs e)
        {
            //if (btnFix.Enabled)
            //    btnFix.BackColor = Color.Orange;
            //else
            //    btnFix.BackColor = Color.FromArgb(33, 33, 33);
        }

        private List<string> driverStatues = new List<string>();

        private void BtnStop_EnabledChanged(object sender, EventArgs e)
        {
            //if (btnStop.Enabled)
            //    btnStop.BackColor = Color.FromArgb(192, 0, 0);
            //else
            //    btnStop.BackColor = Color.FromArgb(33, 33, 33);
        }

        private void BtnStart_EnabledChanged(object sender, EventArgs e)
        {
            //if (btnStart.Enabled)
            //    btnStart.BackColor = Color.Green;
            //else
            //    btnStart.BackColor = Color.FromArgb(33, 33, 33);
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(ofd.FileName))
            {
                mSelectedFilePath = ofd.FileName;
                tbFile.Text = ofd.FileName;
            }
        }

        private void btnDriverPathSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(ofd.SelectedPath))
            {
                mSelectedDriverPath = ofd.SelectedPath;
                // tbDriverPath.Text = ofd.SelectedPath;
            }
        }

        private void btnOutputFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Select a .txt file to export links";
            sfd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(sfd.FileName))
            {
                mOutputFilePath = sfd.FileName;
                tbOutputCSVFile.Text = sfd.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {


            if (!InitializeControllers())
                return;

            Thread t = new Thread(new ThreadStart(() =>
            {
                StartProcessing();
            }))
            {
                IsBackground = true,
                Name = "Process",
                Priority = ThreadPriority.AboveNormal
            };

            t.Start();
        }


        private bool InitializeControllers()
        {

            try
            {
                mDriverController = new WebDriverController();
            }
            catch (Exception)
            {
                MessageBox.Show("Web Driver'ın kurulu olduğu path'i lütfen doğru giriniz.", "Selenyum");
                return false;
            }


            mDbController = new LinkDbController((int)numMaxNumberDomainCollected.Value, (int)numMaxNumberLinkDiversity.Value, (int)numTimeout.Value, (int) numTaskCount.Value);
            return true;
            
        }

        private int mProcessedUrlCount = 0;

        private void StartProcessing()
        {
            ClearConsole();
            ClearProcessInfo();

            this.BeginInvoke((MethodInvoker)delegate
           {
               btnStart.Enabled = false;
               btnStop.Enabled = true;
               btnFix.Enabled = false;
           });


            /// Önce girilen linkler başlangıç için okunur.
            List<string> tempList = new List<string>();
            bool isRead = mDbController.TryReadAllLines(mSelectedFilePath, out tempList);

            if (!isRead)
            {
                MessageBox.Show("Girilen input dosyasındaki linkler okunamadı! İşlem iptal ediliyor.");
                return;
            }

            // Listeye ekle.
            mDbController.AddRangeCandidates(tempList);
            // Başlangıçta txt'den okunan toplam link sayısı
            initialLinkCount = tempList.Count;
            // Memory'i boşalt
            tempList.Clear();

            var totalLinks = (int)numMaxNumberDomainCollected.Value * (int)numMaxNumberLinkDiversity.Value;


            CustomLink curLink = new CustomLink("");
            List<CustomLink> currentLinks = new List<CustomLink>();
            mFlagStop = false;

            int connectionErrorCount = 0;

            WriteToConsole("Going to process initial links to create candidate urls");
            bool initialLinksProcessed = mDbController.ProcessInitialLinks().Result;
            WriteToConsole("Candidate urls are initialized");
            mProcessedUrlCount = 0;

            Thread worker = new Thread(async () =>
            {
                while (mDbController.GetCandidatesCount() > 0 && !mDbController.IsSearchCompleted)
                {
                    SetTaskCount(0);
                    if (mFlagStop)
                    {
                        WriteToConsole(":( Operation is terminated by the user. Going to save the current results.");
                        btnStop.BeginInvoke((MethodInvoker)delegate
                        {
                            btnStop.Enabled = false;
                        });
                        break;
                    }

                    currentLinks = await mDbController.TakeMax();

                    List<Task<bool>> extractTasks = new List<Task<bool>>();
                    
                    var tasks = new List<Task<Tuple<List<string>, List<DomainInfo>>>>();
                    driverStatues = new List<string>();
                    taskResults = new List<bool>();

                    SetTaskCount(currentLinks.Count);
                    for (int i = 0; i < currentLinks.Count; i++)
                    {
                        taskResults.Add(false);
                        driverStatues.Add("");
                        tasks.Add(GetCustomerShoppingPointsAsync(currentLinks[i], i));
                    }

                    await Task.WhenAll(tasks);

                    ClearConsole();
                    SetTaskCount(0);
                    WriteToConsole("#### Whole tasks are finished for this loop ###");
                    List<string> candidateUrls = new List<string>();
                    List<DomainInfo> candidateDomains = new List<DomainInfo>();
                    for (int i = 0; i < tasks.Count; i++)
                    {
                        var taskingResult = tasks[i].Result;
                        if (taskingResult != null)
                        {
                            candidateDomains.AddRange(taskingResult.Item2);
                            candidateUrls.AddRange(taskingResult.Item1);
                        }
                        
                    }

                    TaskHelper.RemoveTaskList(ref tasks);

                    WriteToConsole("#### Going to insert candidates into the database  ###");
                    await mDbController.InsertCandidatesToDatabase(new Tuple<List<string>, List<DomainInfo>>(candidateUrls, candidateDomains));


                    var candidateCount = mDbController.GetCandidatesCount();
                    int toplamİslenen = mDbController.GetCount();
                    SetLastIteratioInformation("", toplamİslenen, candidateCount, initialLinkCount);


                    /// UPDATED: 13.12.2019
                    /// Added : saving results after each iteration.
                    mDbController.Save(mOutputFilePath);

                    ClearConsole();
                }

                WriteToConsole("#### URL extraction is Finished ###");

                mDbController.Save(mOutputFilePath);

                WriteToConsole("#### URLs are saved ###");

                btnStart.BeginInvoke((MethodInvoker)delegate
                {
                    btnStart.Enabled = true;
                    btnFix.Enabled = true;
                });

            });

            worker.IsBackground = true;
            worker.Start();

        }

        private async Task<Tuple<List<string>, List<DomainInfo>>> GetCustomerShoppingPointsAsync(CustomLink curLink , int taskNumber)
        {

            string driverResult = "Driver :" + taskNumber.ToString("0000") + " WebPage : " + curLink.Url.ToString(20, ' ', true);
            try
            {
                
                if (mFlagStop)
                {
                    // WriteToConsole(":( Operation is terminated by the user. Going to save the current results.");
                    btnStop.BeginInvoke((MethodInvoker)delegate
                    {
                        btnStop.Enabled = false;
                    });
                    UpdateTaskCount(taskNumber, true);
                    mProcessedUrlCount++;
                    driverResult += "\t  operation canceled by user";
                    //driverStatues[taskNumber] = driverResult;
                    Thread.Sleep(100);
                    WriteToConsole(driverResult);
                    return null;
                }

                // WriteToConsole("TaskNumber:" + taskNumber.ToString() + "Driver kitlendi.");
                List<string> tempList = new List<string>();
                var webDriver = new WebDriverController();

                try
                {
                    await webDriver.Navigate(curLink.Url , mPageLoadTimeout);
                    Thread.Sleep(mPageLoadTimeout);
                }
                catch (Exception ex)
                {
                    Thread.Sleep(100);
                    driverResult += "\t  webpage could not load. Ignoring this url";
                    WriteToConsole(driverResult);
                    UpdateTaskCount(taskNumber, true);
                    return null;
                }

                Thread.Sleep(200);
                tempList = await webDriver.ExtractLinks();
                Thread.Sleep(200);
                driverResult += "\t  total extracted links :" + tempList.Count.ToString("00000");

                var remainingLinkTuple = await mDbController.EliminateLinksWhoDoesntHaveDomainInfo(tempList);
                Thread.Sleep(200);

                try
                {
                    driverResult += "\t  remaining url count after elimination :" + remainingLinkTuple.Item1.Count.ToString("00000");
                    Thread.Sleep(200);
                    WriteToConsole(driverResult);

                }
                catch (Exception)
                {
                    
                }
                // WriteToConsole("Candidates are : " + mDbController.GetCandidates());
                UpdateTaskCount(taskNumber, true);

                return remainingLinkTuple; 
            }
            catch (Exception ex)
            {
                UpdateTaskCount(taskNumber, true);
                mProcessedUrlCount++;
                driverResult = "\t  task is chrashed with exception : " + ex.Message;
                Thread.Sleep(100);
                WriteToConsole(driverResult);
                return null;
            }

        }

       private void WriteToConsole(string pText)
        {
            if (this.rtbConsole.InvokeRequired)
            {
                this.rtbConsole.BeginInvoke((MethodInvoker)delegate
                {
                    rtbConsole.AppendText(Environment.NewLine + pText);
                });
            }
            else
                rtbConsole.AppendText(Environment.NewLine + pText);

            Console.WriteLine(pText);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }


        private void UpdateTaskCount(int index, bool state)
        {
            lock(taskResults)
            {
                taskResults[index] = state;

                int count =taskResults.Count(x => x == false);
                SetTaskCount(count);
            }
        }
        private void SetTaskCount(int count)
        {

            mTaskCount = count;
            lblTaskCount.BeginInvoke((MethodInvoker)delegate
           {
               lblTaskCount.Text = mTaskCount.ToString();
           });
        }
        private void SetLastIteratioInformation(string pUrl, int totalLinks, int adayListesi,  int initialLinks)
        {
            this.BeginInvoke((MethodInvoker)delegate
           {
               this.lblTotalUrl.Text = totalLinks.ToString();
               this.lblnitialUrl.Text = initialLinks.ToString();
               this.remainingUrls.Text = adayListesi.ToString();
               this.lblNewUrl.Text = (totalLinks - initialLinks).ToString();
               this.lblProcessedUrls.Text = mProcessedUrlCount.ToString();
           });
            if (totalLinks >= initialLinks)
            {
                WriteToConsole("Total Candidates Count : " + adayListesi.ToString() + " where  From initial links : " + initialLinks.ToString());
            }
            else
                WriteToConsole("Total Candidates Count : " + adayListesi.ToString());
        }
        private void ClearConsole()
        {
            this.rtbConsole.BeginInvoke((MethodInvoker)delegate
           {
               this.rtbConsole.Clear();
           });
        }

        private void ClearProcessInfo()
        {
            this.BeginInvoke((MethodInvoker)delegate
           {
               this.lblTotalUrl.Text = "------";
               this.lblnitialUrl.Text = "------";
               this.remainingUrls.Text = "------";
               this.lblProcessedUrls.Text = "------";
               this.lblNewUrl.Text = "------";
           });
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            
            WriteToConsole("#### Operation Cancellation is selected from the user. ###");

            mFlagStop = true;

            btnStop.BeginInvoke((MethodInvoker)delegate
            {
                btnStop.Enabled = false;
            });

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void btnFix_Click(object sender, EventArgs e)
        {
            /// Önce girilen linkler başlangıç için okunur.
            List<string> tempList = new List<string>();
            bool isRead = mDbController.TryReadAllLines(mOutputFilePath, out tempList);

            if (!isRead)
            {
                MessageBox.Show("Girilen output dosyasındaki linkler okunamadı! İşlem iptal ediliyor.");
                return;
            }

            // Fixing last character of urls
            var newList = tempList.Select(s =>  mDbController.FixLastCharacter(s)).ToList();
            // Selecting only unique ones.
            var uniques = newList.Distinct();
            // Checking contains conditions
            var remaining = uniques.Where(url => (url.Length > 5) && (url.StartsWith("wwww") || url.StartsWith("http"))).ToList();
            // TADA now save the remainings.
            try
            {
                mDbController.UpdateOutput(mOutputFilePath, remaining);
                MessageBox.Show("Çıktı dosyası başarı ile düzeltildi.");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Çıktı dosyasını düzeltme işlemi başarısız oldu!");
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
