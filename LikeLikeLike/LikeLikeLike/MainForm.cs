using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FBLikeLikeLike
{
    public partial class MainForm : Form
    {

        /* ログ関連 起動してからの総イイネ数 */
        public int TotalLikeNumber = 0;

        /* クリックのディレイ制御の設定値 */
        public int ClickDelay_MinSec = 1;
        public int ClickDelay_MaxSec = 3;
        public int ClickDelay_CurrentDelaySec = 0;
        public int MaxLikeCountPerCycle = 15; // １サイクルあたりのいいね数
        public long ClickDelay_StartTime = -1;
        public string ClickDelay_StatusText = "準備中です.しばらくお待ちください.";
        public float ClickDelay_ElapsedTime { get; private set; }

        /* アプリケーション管理 */
        bool AutoLikeApplication_IsRun = false; // アプリの状態. Stop/Startに利用


        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 背景の色をフェイスブックぽく設定
            this.BackColor = Color.FromArgb(66, 103, 178);
            this.button_Run.BackColor = Color.FromArgb(245, 246, 247);

            /* タスクストップ用のトークン */
            //((Control)this.webBrowser1).Enabled = false;
            this.webBrowser1.Navigate(@"https://www.facebook.com/");
            webBrowser1.DocumentCompleted += WebBrowser1_DocumentCompleted;
            webBrowser1.Navigating += WebBrowser1_Navigating;

            // レジストリに保存していたFBのアカウント情報をを読み出してUIに設定
            Properties.Settings.Default.Reload();
            textBox_FB_ID.Text = Properties.Settings.Default.facebook_id;
            textBox_FB_Password.Text = Properties.Settings.Default.facebook_pw;

            // ２タブ目の設定画面の設定値のデフォルトを設定
            numericUpDown_ClickDelay_MinSec.Value = ClickDelay_MinSec;
            numericUpDown_ClickDelay_MaxSec.Value = ClickDelay_MaxSec;
            numericUpDown_MaxLikeCountPerCycle.Value = ClickDelay_CurrentDelaySec;

        }

        /// <summary>
        /// webBrowserが更新し始めたときに呼ばれるイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            Debug.WriteLine("WebBrowser1_Navigating");
            this.textBox_BrowserURL.Text = "(移動中) " + this.webBrowser1.Url.ToString();

            /* ボタンの操作拒否 */
            this.button_Run.Enabled = false;
            this.button_Stop.Enabled = false;
        }

        /// <summary>
        /// WebBrowserの更新が終わったときに呼ばれるイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Debug.WriteLine("WebBrowser1_DocumentCompleted");
            this.textBox_BrowserURL.Text = this.webBrowser1.Url.ToString();

            /* ボタン操作可能に */
            this.button_Run.Enabled = true;
            this.button_Stop.Enabled = true;
        }

        /// <summary>
        /// 自動いいね押下アプリのメインループ
        /// </summary>
        private async void timer_Scrayping_Tick(object sender, EventArgs e)
        {
            // タイマーを停止
            this.timer_Scrayping.Enabled = false;

            // (動作継続判定) アプリ動作状態が停止希望なら停止させる
            if (this.AutoLikeApplication_IsRun == false)
            {
                this.timer_Scrayping.Enabled = false; // タイマーストップ
                return;
            }

            // HTML要素の取得
            List<HtmlElement> UFI_LikeLinks = FacebookHacking.Get_LikeLinkes(webBrowser1);
            List<HtmlElement> UFI_UnLikeLinks = FacebookHacking.Get_UnLikeLinkes(webBrowser1);


            // (動作継続判定) 1. いいねボタンが一つも無い場合は終了 | タイマー継続
            if (UFI_LikeLinks.Count <= 0)
            {
                this.timer_Scrayping.Enabled = true; // タイマー継続
                return;
            }
            // (動作継続判定) 2. 未いいねボタンが一つも無い場合は終了 | タイマー継続
            if (UFI_UnLikeLinks.Count <= 0)
            {
                this.timer_Scrayping.Enabled = true; // タイマー継続
                return;
            }
            // (動作継続判定) 3. ある程度スクレイピングした段階で終了, リフレッシュ or アプリストップ
            int LikedPerPage = UFI_LikeLinks.Count - UFI_UnLikeLinks.Count; // いいねされた数を計算
            if (LikedPerPage > this.MaxLikeCountPerCycle)
            {
                this.AutoLikeApplication_IsRun = false; // アプリストップ
                this.timer_Scrayping.Enabled = false; // タイマーストップ

                this.ClickDelay_StatusText = $"{LikedPerPage}個いいね済みです。時間を置いて実行してください。";
                this.webBrowser1.Refresh();
                this.webBrowser1.Update();
                return;
            }



            int n = 0;

            /* ターゲットのいいね(最も先頭)に移動 */
            if (UFI_UnLikeLinks.Count > 0)
            {
                Debug.WriteLine($"elm number : {n}");

                HtmlElement elm = UFI_UnLikeLinks[n];
                Point point = new Point(elm.OffsetRectangle.Left, elm.OffsetRectangle.Top);

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

                await Task.Run(() =>
                {

                    int Div =60;
                    for (int i = 30; i <= Div; i++)
                    {
                        int newY = (int)(point.Y * (i / (float)Div));


                        Point deltaPoint = new Point(point.X, newY);

                        webBrowser1.BeginInvoke(new Action(() =>
                        {
                            webBrowser1.Document.Window.ScrollTo(deltaPoint);
                            webBrowser1.Update();
                        }
                        ));
                        Thread.Sleep(10);
                        this.webBrowser1.BeginInvoke(new Action(() => this.webBrowser1.Update()));

                    }

                });


                this.webBrowser1.Update();
            }

            /* 途中終了判定 */
            if (this.AutoLikeApplication_IsRun == false)
            {
                this.timer_Scrayping.Enabled = false; // タイマーストップ
                return;
            }

            /* ディレイ */

            /* クリックディレイの設定項目 */
            this.ClickDelay_MinSec = (int)3;
            this.ClickDelay_MaxSec = (int)7;
            this.ClickDelay_MinSec = (int)numericUpDown_ClickDelay_MinSec.Value;
            this.ClickDelay_MaxSec = (int)numericUpDown_ClickDelay_MaxSec.Value;
            this.MaxLikeCountPerCycle = (int)numericUpDown_MaxLikeCountPerCycle.Value;


            /* 次のクリックまでのディレイ時間の計算 */
            System.Random rand = new System.Random();
            this.ClickDelay_CurrentDelaySec = rand.Next(this.ClickDelay_MinSec, this.ClickDelay_MaxSec);


            this.ClickDelay_StatusText = $"[{UFI_LikeLinks.Count - UFI_UnLikeLinks.Count}/{UFI_LikeLinks.Count}]";
            this.ClickDelay_StatusText += $" ディレイ{ClickDelay_CurrentDelaySec}s ";

            Debug.WriteLine("delaytime: " + this.ClickDelay_CurrentDelaySec + " sec");


            /* 残り時間計算用:　待ち開始の時間 */
            this.ClickDelay_StartTime = DateTime.Now.Ticks;

            await Task.Run(() =>
             {
                 /* CSSで色を変更 */
                 HtmlElement elm = UFI_UnLikeLinks[n];
                 elm.Style = "outline: solid 1px #4267B2";

                 do
                 {
                     float alpha = (ClickDelay_CurrentDelaySec - ClickDelay_ElapsedTime) / ClickDelay_CurrentDelaySec;

                     elm.Style = $"background: linear-gradient(90deg, white, rgba(97,144,232,{1}) {(int)(alpha * 100)}%, white {(int)(alpha * 100) - 20}%);";

                     this.ClickDelay_ElapsedTime = ClickDelay_CurrentDelaySec - (DateTime.Now.Ticks - ClickDelay_StartTime) / 10000000f;

                     // 経過時間の計算

                     /* キャンセル終了 : 途中終了判定 */
                     if (this.AutoLikeApplication_IsRun == false)
                     {
                         this.ClickDelay_StartTime = -1;

                         this.timer_Scrayping.Enabled = false; // タイマーストップ
                         return;
                     }

                 } while (ClickDelay_ElapsedTime > 0.0f);

                 this.ClickDelay_ElapsedTime = 0.0f; // タイマーリセット

             });


            /* クリック! */

            await Task.Run(() =>
                {
                    HtmlElement elm = UFI_UnLikeLinks[n];

                    elm.InvokeMember("click");

                    this.TotalLikeNumber++;

                    this.webBrowser1.BeginInvoke(new Action(() => this.webBrowser1.Update()));

                    Thread.Sleep(1000); // wait

                    elm.Style = "outline: none";
                    this.webBrowser1.BeginInvoke(new Action(() => this.webBrowser1.Update()));

                });


            /* タイマー再開 */
            if (this.AutoLikeApplication_IsRun == false)
            {
                this.timer_Scrayping.Enabled = false;
                return;
            }
            else
            {
                this.timer_Scrayping.Enabled = true;
                return;
            }


        }

        /// <summary>
        /// アプリ状態の表示用タイマー
        /// </summary>
        private void timer_FormStatusUpdater_Tick(object sender, EventArgs e)
        {
            string TextProgressBar = "";

            if (this.ClickDelay_StartTime > 0.0f)
            {
                TextProgressBar = (this.ClickDelay_ElapsedTime).ToString("F1") + "秒 ";

                int it = (int)Math.Ceiling(ClickDelay_ElapsedTime / 0.5f);
                for (int i = 0; i < it; i++)
                {
                    TextProgressBar += "|";
                }
            }

            this.Text = "総いいね数:" + this.TotalLikeNumber + " ";
            this.Text += this.ClickDelay_StatusText + TextProgressBar;

            // 総いいね数を表示
            this.label_LikeCount.Text = this.TotalLikeNumber.ToString();
        }


        /// <summary>
        /// アプリスタートボタンのイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_RunnStop_MianScheduler_Click(object sender, EventArgs e)
        {
            this.timer_Scrayping.Enabled = true; // タイマースタート
            this.AutoLikeApplication_IsRun = true; // アプリ動作要求状態スタート
        }

        /// <summary>
        /// アプリストップボタンのイベント
        /// </summary>
        private void button_Stop_Click(object sender, EventArgs e)
        {
            this.AutoLikeApplication_IsRun = false; // アプリ動作要求状態ストップ

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(@"http://facebook.com/");
        }

        private void button_Reflesh_Click(object sender, EventArgs e)
        {
            this.webBrowser1.Refresh();
        }

        private void button_Login_Click(object sender, EventArgs e)
        {

            string id = textBox_FB_ID.Text;
            string pw = textBox_FB_Password.Text;



            /* input要素を全部取得 */
            HtmlElementCollection InputCollection = webBrowser1.Document.GetElementsByTagName("input");

            /* input要素のコレクションから, email入力Boxとパスワード入力Boxの要素を取得. jQuery的な */
            foreach (HtmlElement input_element in InputCollection)
            {
                /* アカウント入力用box */
                if (input_element.GetAttribute("name") == "email")
                {
                    input_element.SetAttribute("value", id);
                }

                /* パスワード入力ボックス */
                if (input_element.GetAttribute("name") == "pass")
                {
                    input_element.SetAttribute("value", pw);
                }
            }

            /* id=loginbuttonのボタン要素を取得 */
            HtmlElement btn = webBrowser1.Document.GetElementById("loginbutton");
            /* ボタンをクリック! */
            btn.InvokeMember("click");
        }

        private void textBox_FB_ID_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.facebook_id = textBox_FB_ID.Text;
            Properties.Settings.Default.Save();
        }

        private void textBox_FB_Password_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.facebook_pw = textBox_FB_Password.Text;
            Properties.Settings.Default.Save();
        }

        private void checkBox_FB_PW_Cheked_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_FB_PW_Cheked.Checked)
            {
                // 表示する
                textBox_FB_Password.UseSystemPasswordChar = false;
            }
            else
            {
                // 非表示
                textBox_FB_Password.UseSystemPasswordChar = true;
            }
        }
    }


}
