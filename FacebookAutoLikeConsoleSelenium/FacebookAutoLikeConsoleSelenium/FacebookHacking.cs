using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacebookAutoLikeConsoleSelenium
{
    public class FacebookHacking
    {

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
        /// <summary>
        /// いいねされていないいいねボタン要素のみをしゅとく
        /// </summary>
        public static List<HtmlElement> Get_UnLikeLinkes(WebBrowser webBrowser)
        {
            // HTML要素の取得
            List<HtmlElement> UFI_LikeLinks = FacebookHacking.Get_LikeLinkes(webBrowser);
            List<HtmlElement> UFI_UnLikeLinks = new List<HtmlElement>();
            foreach (HtmlElement elm in UFI_LikeLinks)
            {
                if (elm.GetAttribute("aria-pressed") == "false")
                {
                    /*un presed*/
                    UFI_UnLikeLinks.Add(elm);

                };

            }

            return UFI_UnLikeLinks;
        }

        /// <summary>
        /// class.UFILikeLinkにマッチする要素をList形式で返却
        /// </summary>
        public static List<HtmlElement> Get_LikeLinkes(WebBrowser webBrowser)
        {
            List<HtmlElement> UFILikeLink_List = new List<HtmlElement>();

            /* search */
            HtmlElementCollection a_elms = webBrowser.Document.GetElementsByTagName("a");

            foreach (HtmlElement a_elm in a_elms)
            {
                if (a_elm.GetAttribute("className").StartsWith("UFILikeLink"))
                {
                    if (a_elm.GetAttribute("className").Contains("UFIReactionLink"))
                    {
                        // リアクションのいいねぼたｎ
                        //UFILikeLink_List.Add(a_elm);
                    }
                    else
                    {
                        // メイン記事のいいねボタン
                        UFILikeLink_List.Add(a_elm);
                    }
                }

            }

            return UFILikeLink_List;
        }


    }
}
