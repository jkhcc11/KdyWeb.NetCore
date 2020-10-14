using System.Linq;
using System.Text;
using System.Threading;
using HtmlAgilityPack;

namespace KdyWeb.Utility
{
    /// <summary>
    /// Html解析扩展
    /// </summary>
    public static class HtmlParseExt
    {
        ///  <summary>
        ///  使用Xpath单个匹配内容
        ///  </summary>
        /// <param name="htmlSource">html源码</param>
        /// <param name="xpath">xpath语法</param>
        /// <param name="attrName">
        /// Html标签 属性名
        ///  <remarks>eg:
        ///  html 获取html源码
        ///  text 获取文本
        ///  src 获取图片
        ///  href 获取连接
        /// 等</remarks>
        /// </param>
        /// <returns></returns>
        public static string GetValueByXpath(this string htmlSource, string xpath, string attrName)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlSource);

            //匹配Xpath
            var firstChild = doc.DocumentNode.SelectSingleNode(xpath);
            if (firstChild == null)
            {
                return string.Empty;
            }

            //html标签名
            if (firstChild.Name.ToLower() == "meta")
            {
                return firstChild.GetAttributeValue("content", "");
            }

            switch (attrName)
            {
                case "html":
                    {
                        return firstChild.InnerHtml.InnerHtmlHandler();
                    }
                case "text":
                    {
                        //直接获取
                        return firstChild.InnerText.InnerHtmlHandler();
                    }
                case "src":
                    {
                        #region 有几种情况 SRc、src、SRC
                        var v = firstChild.GetAttributeValue(attrName, "");
                        if (v.IsEmptyExt() == false)
                        {
                            return v;
                        }

                        v = firstChild.GetAttributeValue("SRc", "");
                        if (v.IsEmptyExt())
                        {
                            v = firstChild.GetAttributeValue("SRC", "");
                        }

                        return v;
                        #endregion
                    }
                default:
                    {
                        return firstChild.GetAttributeValue(attrName, "").InnerHtmlHandler();
                    }
            }

            //找不到返回文本
            return firstChild.InnerText;
        }

        ///  <summary>
        ///  使用Xpath获取节点
        ///  </summary>
        /// <param name="htmlSource">html源码</param>
        /// <param name="xpath">xpath语法</param>
        /// <returns></returns>
        public static HtmlNode GetHtmlNodeByXpath(this string htmlSource, string xpath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlSource);
            return doc.DocumentNode.SelectSingleNode(xpath);
        }

        /// <summary>
        /// 将html转换为NodeCollection集合
        /// </summary>
        /// <param name="htmlSource">要解析的html文本</param>
        /// <param name="xpath">xpath语法</param>
        /// <returns>返回HtmlNode集合</returns>
        public static HtmlNodeCollection GetNodeCollection(this string htmlSource, string xpath)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlSource);
            var rootNode = doc.DocumentNode;
            return rootNode.SelectNodes(xpath);
        }

        /// <summary>
        /// Html人名或其他 处理移除首尾空格 
        /// </summary>
        /// <remarks>
        ///   美国 / 英国 =>   美国,英国
        ///   哈利波特1：神秘的魔法石(港/台) / 哈1 / Harry Potter and the Philosopher's Stone  =>哈利波特1：神秘的魔法石(港/台),哈1,Harry Potter and the Philosopher's Stone
        /// </remarks>
        /// <param name="innerText">待处理的人名</param>
        /// <param name="split">分隔符</param>
        /// <returns></returns>
        public static string HtmlPersonNameHandler(this string innerText, params char[] split)
        {
            if (innerText.IsEmptyExt() || split == null || split.Any() == false)
            {
                return innerText;
            }

            var result = new StringBuilder();
            foreach (var item in innerText.Split(split))
            {
                result.Append($"{item.Trim()},");
            }

            return result.ToString().Trim(',');
        }

        /// <summary>
        /// 中英文人名处理 
        /// </summary>
        /// <remarks>
        ///   阿尔弗雷德·伊诺奇 Alfred Enoch =>   阿尔弗雷德·伊诺奇
        ///   杰拉丁·萨莫维尔 Geraldine Somerville,中文 英文,中文 英文 =>杰拉丁·萨莫维尔,中文,中文
        /// </remarks>
        /// <param name="innerText">待处理的人名</param>
        /// <param name="split">分隔符</param>
        /// <param name="firstSplit">取首个分隔符</param>
        /// <returns></returns>
        public static string HtmlPersonNameHandler(this string innerText, char split, char firstSplit)
        {
            if (innerText.IsEmptyExt())
            {
                return innerText;
            }

            var result = new StringBuilder();
            foreach (var item in innerText.Split(split))
            {
                result.Append($"{item.Split(firstSplit).First().Trim()}{split}");
            }

            return result.ToString().Trim(split);
        }
    }
}
