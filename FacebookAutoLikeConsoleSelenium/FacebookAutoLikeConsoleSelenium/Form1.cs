using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacebookAutoLikeConsoleSelenium
{
    public partial class Form1 : Form
    {
        private System.Threading.CancellationTokenSource tokenSource;
        private System.Threading.CancellationToken cancelToken;

        public int ClickDelay_MinSec = 1;
        public int ClickDelay_MaxSec = 3;
        public int ClickDelay_CurrentDelaySec = 0;
        public long ClickDelay_StartTime = -1;
        public string ClickDelay_StatusText = "準備中です.しばらくお待ちください.";

        TaskRunner taskRunner;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(66, 103, 178);
            this.button_Run.BackColor = Color.FromArgb(245, 246, 247);

            /* タスクストップ用のトークン */
            //((Control)this.webBrowser1).Enabled = false;
            this.webBrowser1.Navigate(@"https://www.facebook.com/");
            webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;
            webBrowser1.Navigating += WebBrowser1_Navigating;

        }

        bool MainTask_State = false;

        private void WebBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            Debug.WriteLine("WebBrowser1_Navigating");
            this.textBox_BrowserURL.Text = "(移動中) " + this.webBrowser1.Url.ToString();

        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Debug.WriteLine("WebBrowser1_DocumentCompleted");
            this.textBox_BrowserURL.Text = this.webBrowser1.Url.ToString();
        }

        private async void timer_Scrayping_Tick(object sender, EventArgs e)
        {
            this.timer_Scrayping.Enabled = false;

            if (this.MainTask_State == false)
            {
                this.timer_Scrayping.Enabled = false;
                return;
            }

            Debug.WriteLine("timer_Scrayping_Tick");

            // HTML要素の取得
            List<HtmlElement> UFI_LikeLinks = FacebookHacking.Get_LikeLinkes(webBrowser1);
            List<HtmlElement> UFI_UnLikeLinks = FacebookHacking.Get_UnLikeLinkes(webBrowser1);

            Debug.WriteLine(this.ClickDelay_StatusText);

            /* ターゲットのいいねに移動 */
            if (UFI_UnLikeLinks.Count <= 0)
            {
                this.ClickDelay_StatusText = $"全てのいいねを完了しました";
                return;
            }

            /* ターゲットのいいね(最も先頭)に移動 */
            if (UFI_UnLikeLinks.Count > 0)
            {
                //var rand0 = new System.Random();
                //int n = rand0.Next(0, UFI_UnLikeLinks.Count - 1);
                int n = 0;
                Debug.WriteLine($"elm number : {n}");

                //Form1.MoveElementPosition(webBrowser1, UFI_UnLikeLinks[n]);
                //var point = Form1.GetOffset(UFI_UnLikeLinks[n]);
                //point.X = 0;

                //get element pos
                HtmlElement elm = UFI_UnLikeLinks[n];
                Point point = new Point(elm.OffsetRectangle.Left, elm.OffsetRectangle.Top);
                //get the parents pos
                HtmlElement tempEl = elm.OffsetParent;
                while (tempEl != null)
                {
                    point.X += tempEl.OffsetRectangle.Left;
                    point.Y += tempEl.OffsetRectangle.Top;
                    tempEl = tempEl.OffsetParent;
                }
                point.X = 0;

                if (point.Y == 0)
                {
                    Debug.WriteLine("elm posion is 0 !!! " + point.ToString());

                }
                Debug.WriteLine("elm posion :" + point.ToString());
                point.Y -= this.webBrowser1.Height / 2;
                webBrowser1.Document.Window.ScrollTo(point);
                this.webBrowser1.Update();
            }


            if (this.MainTask_State == false)
            {
                this.timer_Scrayping.Enabled = false;
                return;
            }

            /* ディレイ */

            /* クリックディレイの設定項目 */
            this.ClickDelay_MinSec = (int)3;
            this.ClickDelay_MaxSec = (int)10;

            /* 次のクリックまでのディレイ時間の計算 */
            System.Random rand = new System.Random();
            this.ClickDelay_CurrentDelaySec = rand.Next(this.ClickDelay_MinSec, this.ClickDelay_MaxSec);


            this.ClickDelay_StatusText = $"[{UFI_LikeLinks.Count - UFI_UnLikeLinks.Count}/{UFI_LikeLinks.Count}]";
            this.ClickDelay_StatusText += $" ディレイ{ClickDelay_CurrentDelaySec}s ";

            Debug.WriteLine("delaytime: " + this.ClickDelay_CurrentDelaySec + " sec");


            /* 残り時間計算用:　待ち開始の時間 */
            this.ClickDelay_StartTime = DateTime.Now.Ticks;


            /* クリック! */
            {
                int n = 0;
                HtmlElement elm = UFI_UnLikeLinks[n];
                elm.Style = "outline: solid 1px #FF0000";
            }


            await Task.Run(() =>
             {
                 Thread.Sleep(ClickDelay_CurrentDelaySec * 1000);
             });

            this.ClickDelay_StartTime = -1;

            /* クリック! */
            {
                int n = 0;
                HtmlElement elm = UFI_UnLikeLinks[n];
                elm.InvokeMember("click");
            }

            this.webBrowser1.Update();

            Thread.Sleep(1 * 1000);

            /* クリック! */
            {
                int n = 0;
                HtmlElement elm = UFI_UnLikeLinks[n];
                elm.Style = "outline: none";
            }

            this.webBrowser1.Update();


            /* タイマー再開 */
            if (this.MainTask_State == false)
            {
                this.timer_Scrayping.Enabled = false;
                return;
            }
            else
            {
                this.timer_Scrayping.Enabled = true;

            }


        }

        private void button_RunnStop_MianScheduler_Click(object sender, EventArgs e)
        {
            this.button_Run.Enabled = false;
            this.button_Stop.Enabled = true;

            this.timer_Scrayping.Enabled = true;
            this.MainTask_State = true;
        }

        private void button_Stop_Click(object sender, EventArgs e)
        {
            this.button_Run.Enabled = true;
            this.button_Stop.Enabled = false;

            //this.timer_Scrayping.Enabled = false;
            this.MainTask_State = false;

        }

        private async void timer_MainSchedulerTimer_Tick(object sender, EventArgs e)
        {
            return;
            this.timer_MainSchedulerTimer.Enabled = false;


            this.webBrowser1.Navigate(@"http://takachan.hatenablog.com/entry/2017/09/09/225342");

            this.tokenSource = new System.Threading.CancellationTokenSource();
            this.cancelToken = this.tokenSource.Token;

            /* クリックディレイの設定項目 */
            this.ClickDelay_MinSec = (int)1;
            this.ClickDelay_MaxSec = (int)3;
            this.ClickDelay_CurrentDelaySec = (int)0;

            // HTML要素の取得
            List<HtmlElement> UFILikeLinks = FacebookHacking.Get_LikeLinkes(webBrowser1);
            HtmlElementCollection a_elms = webBrowser1.Document.GetElementsByTagName("a");
            int Nlike = a_elms.Count;

            // if(UFILikeLinks[0].GetAttribute("aria-pressed") == "true") { /*presed*/};
            // UFILikeLinks[0].GetAttribute("className") == "UFILikeLink UFIReactionLink";


            await Task.Run(() =>
            {
                Debug.WriteLine("Wait Start");

                System.Random rand = new System.Random();
                this.ClickDelay_StatusText = "";

                // HTML要素の取得

                Debug.WriteLine($"Nlike{Nlike}");
                for (int i = 0; i < Nlike; i++)
                {
                    /* キャンセル処理 */
                    if (this.cancelToken.IsCancellationRequested)
                    {
                        Debug.WriteLine("Cancel");
                        this.ClickDelay_StatusText = "キャンセルされました";
                        break;
                    }


                    /* 次のクリックまでのディレイ時間 */
                    this.ClickDelay_CurrentDelaySec = rand.Next(this.ClickDelay_MinSec, this.ClickDelay_MaxSec);

                    /* 残り時間計算用:　待ち開始の時間 */
                    this.ClickDelay_StartTime = DateTime.Now.Ticks;

                    /* 現在の状況 */
                    this.ClickDelay_StatusText = $"[{i}/{Nlike}]";

                    /* ClickDelay */
                    // T.B.D : この間はキャンセルができない.非同期かする
                    Thread.Sleep(this.ClickDelay_CurrentDelaySec * 1000);

                    /* LikeBtn.Click(); */

                    Debug.WriteLine($"[{i}/30] Clicked");
                }

                Debug.WriteLine("Wait End");
            }, this.cancelToken);

            // タイマー継続の判定
            if (this.tokenSource.IsCancellationRequested)
                this.timer_MainSchedulerTimer.Enabled = false;
            else
                this.timer_MainSchedulerTimer.Enabled = true;

        }

        private void timer_FormStatusUpdater_Tick(object sender, EventArgs e)
        {
            string next_time = "";
            string progress_bar = "";

            if (this.ClickDelay_StartTime != -1)
            {
                long dt = DateTime.Now.Ticks - this.ClickDelay_StartTime;
                //next_time = "次のクリックまで";
                next_time += (this.ClickDelay_CurrentDelaySec - dt / 10000000f).ToString("F1") + "秒";
                int it = (int)Math.Ceiling((this.ClickDelay_CurrentDelaySec - dt / 10000000f) / 0.5f);
                for (int i = 0; i < it; i++)
                {
                    progress_bar += "|";
                }
            }

            this.Text = this.ClickDelay_StatusText + next_time + progress_bar;
        }
        /// <summary>
        /// HTML要素に移動
        /// </summary>
        public static void MoveElementPosition(WebBrowser webBrowser, HtmlElement elm)
        {
            var point = GetOffset(elm);
            point.X = 0;
            point.Y += 100;
            webBrowser.Document.Window.ScrollTo(point);
        }
        /// <summary>
        /// HTML要素の絶対位置を取得
        /// </summary>
        public static Point GetOffset(HtmlElement el)
        {
            //get element pos
            Point pos = new Point(el.OffsetRectangle.Left, el.OffsetRectangle.Top);

            //get the parents pos
            HtmlElement tempEl = el.OffsetParent;
            while (tempEl != null)
            {
                pos.X += tempEl.OffsetRectangle.Left;
                pos.Y += tempEl.OffsetRectangle.Top;
                tempEl = tempEl.OffsetParent;
            }
            return pos;
        }

    }


    public class TaskRunner
    {
        private WebBrowser webbrowser;
        private int waitSec; // メインループのwait時間
        private CancellationTokenSource tokenSource;
        private CancellationToken cancelToke;

        public bool IsRunning = false;

        private Task task;

        public TaskRunner(WebBrowser webbrowser, int waitSec = 5)
        {
            this.webbrowser = webbrowser;
            this.waitSec = waitSec;
            this.IsRunning = false;
        }

        public void Start()
        {
            if (IsRunning == true)
            {
                Debug.WriteLine("[WARNING] task is running");
                return;
            }

            // タスクの実行
            this.tokenSource = new System.Threading.CancellationTokenSource();
            this.cancelToke = this.tokenSource.Token;
            Task task = Task.Run(() => TaskMainloop(this.webbrowser, this.waitSec, this.cancelToke), cancelToke);
            this.IsRunning = true;
        }

        public void Stop()
        {
            this.tokenSource.Cancel();
            this.tokenSource.Dispose();
        }

        public static void TaskMainloop(WebBrowser webbrowser, int waitSec, CancellationToken cancelToken)
        {
            int ct = 0;

            while (true)
            {

                if (cancelToken.IsCancellationRequested)
                {
                    Debug.WriteLine(" 処理停止");
                    break;
                }

                Thread.Sleep(waitSec * 1000);
                ct++;
                Debug.WriteLine("{1}回目のループです", ct.ToString());

                /* ここでタスクを実行 */
                webbrowser.Navigate(@"http://google.com");

            }
        }
    }
}
