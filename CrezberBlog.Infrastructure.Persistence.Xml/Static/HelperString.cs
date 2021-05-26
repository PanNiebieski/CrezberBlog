using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CrezberBlog.Infrastructure.Persistence.Xml
{
    public static class HelperString
    {
        public static string TrimPost(string content, Uri url)
        {
            int index = content.IndexOf("[more]");

            //string link = "<a href=" + url.ToString() + ">więcej</a>";

            if (index != -1)
                return content.Substring(0, index);

            return content;
        }

        public static List<int> AllIndexesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }

        public static string TrimPostWhitoutMetroTag(string content, Uri url)
        {
            int index = content.IndexOf("[more]");

            //string link = "<a href=" + url.ToString() + ">więcej</a>";
            string result = content;
            if (index != -1)
                result = content.Substring(0, index);

            result = result.Replace("[metro]", "");

            return result;
        }

        public static string FirstCharToUpper(this string input) =>
     input switch
     {
         null => "",
         "" => "",
         _ => input.First().ToString().ToUpper() + input.Substring(1)
     };

        public static string FixProgrammerCategories(this string input) =>
input switch
{
    null => throw new ArgumentNullException(nameof(input)),
    "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
    "wpf" => "WPF",
    "wzorce-projektowe" => "Wzorce Projektowe",
    "azure" => "AZURE",
    "javaScript" => "JavaScript",
    "java" => "Java",
    "asp.net" => "ASP.NET",
    "visual-studio" => "Visual Studio",
    "sql" => "SQL",
    "Triki z Windows" => "Triki z Windows",
    "wcf" => "WCF",
    "css" => "CSS",
    "objective-C" => "Objective-C",
    "xaml" => "XAML",
    _ => input.First().ToString().ToUpper() + input.Substring(1)
};


        public static string GetTagName(this CloudImage en)
        {
            switch (en)
            {
                case CloudImage.Green:
                    return "cloud-green";
                case CloudImage.Orange:
                    return "cloud-orange";
                case CloudImage.Blue:
                    return "cloud-blue";
                case CloudImage.Blue2:
                    return "cloud-blue2";
                case CloudImage.Black:
                    return "cloud-black";
                case CloudImage.Gray:
                    return "cloud-gray";
                default:
                    return "cloud-green";
            }
        }

        public static string GetTagName(this StaticMetroImage staticMetro)
        {
            switch (staticMetro)
            {
                case StaticMetroImage.Download:
                    return "download";
                case StaticMetroImage.See:
                    return "see";
                case StaticMetroImage.Email:
                    return "email";
                default:
                    return "download";
            }
        }

        public static string GetImageName(this CloudImage en)
        {
            switch (en)
            {
                case CloudImage.Green:
                    return "pytanie2.png";
                case CloudImage.Orange:
                    return "wykrzyknik2.png";
                case CloudImage.Blue:
                    return "lampka.PNG";
                case CloudImage.Blue2:
                    return "time2.png";
                case CloudImage.Black:
                    return "Exam.png";
                case CloudImage.Gray:
                    return "";
                default:
                    return "pytanie2.png";
            }
        }

        public static string GetCssName(this CloudImage en)
        {
            switch (en)
            {
                case CloudImage.Green:
                    return "myfun";
                case CloudImage.Orange:
                    return "mywarring";
                case CloudImage.Blue:
                    return "myinfo";
                case CloudImage.Blue2:
                    return "myt";
                case CloudImage.Black:
                    return "myexam";
                case CloudImage.Gray:
                    return "myde";
                default:
                    return "cloud-green";
            }
        }

        public static string CreateCloud(string title, string content, CloudImage ci)
        {
            string contentTitle = "<div id=\"stitle\">" + title + "</div>";

            string mainTag = "<div class=\"cloud " + ci.GetCssName() + " \">";

            string img = "<img class=\"cloudimage\" " +
                "style=\"margin: 5px 10px 0 5px\" alt=\"Alternate Text\" src=\"/my/" + ci.GetImageName() + "\">";

            string img2 = "<span class=\"cloudimg \" ></span>";

            string cont = "<span class=\"cloudcontent\" >";

            if (ci == CloudImage.Gray)
                img = "";

            string aa = mainTag + contentTitle + img2 + cont + content + "</span>" + "</div>";

            return aa;

        }

        public static string CreateYoutubeVideo(string url)
        {
            return "<iframe width=\"560\" height=\"315\" " +
                "src=\"" + url + "\" " +
                "frameborder=\"0\" allowfullscreen></iframe>";
        }

        public static string CreateStaticMetroIcon(string url, StaticMetroImage image)
        {
            string ss = "white-128";

            string im = "save-disk.png";
            string text = "Pobierz Kod";

            string color = "7B00FF";

            if (image == StaticMetroImage.See)
            {
                im = "eye-button.png";
                text = "Przykład";
                color = "9C8C5A";
            }

            if (image == StaticMetroImage.Email)
            {
                im = "email.png";
                text = "Email napisz";
                color = "FF7900";
                url = "http://cezarywalenciuk.pl/views/pages/main/contact";
            }

            string hh = "<a href=\"" + url + "\"  style=\" display: inline-block; \">" + "  " +
            "<span class=\"metroWindowsBackground2 fix2\" style=\" background-color:#" + color + " !important\">" +
                "<span class=\"metroWindowsImage\" style=\" background-image: url('/posts/programing/icons_other/" + ss + "/" + im + "') !important; \" ></span>" +
                "<span class=\"metroWindowsWords\">" + text + "</span>" +
            "</span>" +
            "</a>";

            return hh;
        }



        //public static string GetFixedContent(string con, Post post)
        //{
        //    string result = con;

        //    string metrot = "Analfabeta";
        //    string metroI = "android-logo.png";

        //    metrot = post.Variables.ValueForKey("metroT");
        //    metroI = post.Variables.ValueForKey("metroI") + ".png";

        //    //if (post.SpecialVariables.ContainsKey("metroT"))
        //    //{
        //    //    metrot = post.SpecialVariables["metroT"];
        //    //}


        //    //if (post.SpecialVariables.ContainsKey("metroI"))
        //    //{
        //    //    metroI = post.SpecialVariables["metroI"] + ".png";
        //    //}

        //    string color = Tools.GetColorVariable(post, 0);

        //    //string ss = "white-128";
        //    string hh = "";

        //    //TODO:PROBLEM1
        //    //var course = Courses.GetCourseAndItemCourseByPost(post);
        //    //if (course.CourseItem == null)
        //    //{
        //    //    hh = "<a href=\"" + post.AbsoluteUrl + "\">" + "  " +
        //    //    "<span class=\"metroWindowsBackground\" style=\" background-color:#" + color + " !important\">" +
        //    //        "<span class=\"metroWindowsImage\" style=\" background-image: url('/posts/programing/icons/" + ss + "/" + metroI + "') !important; \" ></span>" +
        //    //        "<span class=\"metroWindowsWords\">" + metrot + "</span>" +
        //    //    "</span>" +
        //    //    "</a>";
        //    //}
        //    //else
        //    //{
        //    //    hh = "" + "  " +
        //    //       "<span class=\"metroWindowsBackground2  \" style=\" background-color:#" + color + " !important\">" +
        //    //           "<a href=\"" + post.AbsoluteUrl + "\">" +
        //    //               "<span class=\"metroWindowsImage\" style=\" background-image: url('/posts/programing/icons/" + ss + "/" + metroI + "') !important; \" ></span>" +
        //    //               "<span class=\"metroWindowsWords\">" + metrot + "</span>" +
        //    //           "</a>" +
        //    //           "<a href=\"http://cezarywalenciuk.pl/blog/programing/kurs/" + course.Course.UrlName + "\">" +
        //    //               "<span class=\"metroUnderLetter\" style=\" background-color:#" + color + " !important\" >" + course.Course.NumberText + "" + course.CourseItem.Index + "</span>" +
        //    //           "</a>" +
        //    //       "</span>"
        //    //     ;
        //    //}

        //    result = result.Replace("[metro]", hh);


        //    foreach (CloudImage suit in Enum.GetValues(typeof(CloudImage)))
        //    {
        //        result = ReplaceIfCloudExist(result, suit);
        //    }

        //    foreach (StaticMetroImage item in Enum.GetValues(typeof(StaticMetroImage)))
        //    {

        //        result = AddStaticMetroImages(result, item);
        //    }

        //    result = InsertYoutube(result);

        //    return result;
        //}

        public static string AddStaticMetroImages(string result, StaticMetroImage stat)
        {


            while (true)
            {
                string find1 = "[" + stat.GetTagName();
                string find2 = "[/" + stat.GetTagName() + "]";

                var rr = result.IndexOf(find1);

                var rr2 = result.IndexOf(find2);

                if (rr == -1 || rr2 == -1)
                {
                    break;
                }

                string cont = result.Substring(find1.Count() + rr
                    , rr2 + find2.Count() - (find1.Count() + rr));

                int t = cont.IndexOf("]");

                string content = cont.Substring(t + 1, cont.Length - (t + 1 + find2.Length));

                string html = CreateStaticMetroIcon(content, stat);

                result = result.ReplaceAt(rr, rr2 + find2.Count() - (rr), html);


            }

            return result;
        }

        public static string InsertYoutube(string result)
        {
            while (true)
            {
                string find1 = "[" + "yt]";
                string find2 = "[/" + "yt" + "]";

                var rr = result.IndexOf(find1);

                var rr2 = result.IndexOf(find2);

                if (rr == -1 || rr2 == -1)
                {
                    break;
                }

                string cont = result.Substring(find1.Count() + rr
                    , rr2 + find2.Count() - (find1.Count() + rr));

                int t = cont.IndexOf("]");

                string content = cont.Substring(t + 1, cont.Length - (t + 1 + find2.Length));

                string html = CreateYoutubeVideo(content);

                result = result.ReplaceAt(rr, rr2 + find2.Count() - (rr), html);


            }

            return result;
        }

        public static string ReplaceIfCloudExist(string result, CloudImage cloudName)
        {
            while (true)
            {
                string find1 = "[" + cloudName.GetTagName();
                string find2 = "[/" + cloudName.GetTagName() + "]";

                var rr = result.IndexOf(find1);

                var rr2 = result.IndexOf(find2);

                if (rr == -1 || rr2 == -1)
                {
                    break;
                }

                string cont = result.Substring(find1.Count() + rr
                    , rr2 + find2.Count() - (find1.Count() + rr));

                int t = cont.IndexOf("]");

                int t2 = cont.IndexOf(find2) + find2.Count();

                string titleC = cont.Substring(1, t - 1);

                string content = cont.Substring(t + 1, cont.Length - (t + 1 + find2.Length));

                string html = CreateCloud(titleC, content, cloudName);

                result = result.ReplaceAt(rr, rr2 + find2.Count() - (rr), html);


            }

            return result;
        }

        //public static string GetFixedContentThumb(Post post)
        //{
        //    string metrot = "Analfabeta";
        //    string metroI = "android-logo.png";

        //    metrot = post.Variables.ValueForKey("metroT");
        //    metroI = post.Variables.ValueForKey("metroI") + ".png";

        //    string color = Tools.GetColorVariable(post, 0);

        //    string ss = "white-128";





        //    string hh = "<a href=\"" + post.AbsoluteUrl() + "\">" + "  " +
        //        "<span class=\"metroWindowsBackground fix2 \" style=\" background-color:#" + color + " !important\">" +
        //            "<span class=\"metroWindowsImage\" style=\" background-image: url('/posts/programing/icons/" + ss + "/" + metroI + "') !important; \" ></span>" +
        //            "<span class=\"metroWindowsWords\">" + metrot + "</span>" +
        //        "</span>" +
        //        "</a>";

        //    return hh;




        //}


        public static string RemoveWordMore(this string content)
        {
            content = content.Replace("[more]", "");

            return content;
        }

        public static string RemoveWordMetro(this string content)
        {
            content = content.Replace("[metro]", "");

            return content;
        }

        public static string CleanContent(string content, bool removeHtml, bool lesscontent = false)
        {
            if (removeHtml)
            {
                content = StripHtml(content);
            }

            content =
                content.Replace("\\", string.Empty).Replace("|", string.Empty).Replace("(", string.Empty).Replace(
                    ")", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty).Replace("*", string.Empty).
                    Replace("?", string.Empty).Replace("}", string.Empty).Replace("{", string.Empty).Replace(
                        "^", string.Empty).Replace("+", string.Empty);

            var words = content.Split(new[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var word in
                words.Select(t => t.ToLowerInvariant().Trim()).Where(word => word.Length > 1 && word != "[more]"))
            {
                sb.AppendFormat("{0} ", word);

                if (lesscontent && sb.Length > 1040)
                    break;
            }

            return sb.ToString();
        }

        public static string StripHtml(string html)
        {
            return StringIsNullOrWhitespace(html) ? string.Empty : RegexStripHtml.Replace(html, string.Empty).Trim();
        }

        public static bool StringIsNullOrWhitespace(string value)
        {
            return ((value == null) || (value.Trim().Length == 0));
        }

        private static readonly Regex RegexStripHtml = new Regex("<[^>]*>", RegexOptions.Compiled);

        public static IEnumerable<TKey> RandomValues<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            Random rand = new Random();
            List<TKey> values = Enumerable.ToList(dict.Keys);
            int size = dict.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }

        public static string TrimPost2(string content, Uri url)
        {
            int index = content.IndexOf("[more]");

            //string link = "<a href=" + url.ToString() + ">więcej</a>";

            string s = content;
            if (index != -1)
                s = content.Substring(0, index);



            s = s.Replace("<p ", "<p class='difp' ");
            //s = s.Replace("</p>", "</span>");
            return s;
        }

        //public static string WriteCSS(params string[] csss)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    foreach (var item in csss)
        //    {
        //        string file = item;

        //        if (item.Contains(".css"))
        //            file = item + ".css";

        //        sb.AppendLine("<link href=\"" + Blog.FingerPrint("/themes/" + Blog.Theme + "/css/" + file) + "\"rel=\"stylesheet\" />");
        //    }

        //    return sb.ToString();
        //}


        //public static HtmlString Square(string name, int i, int font)
        //{
        //    string s = "<a href=/" + Url.Category(name) + ">" +
        //                   "<span class=\"span-" + i + " bigtitle\">" +
        //                       "<span class=\"font-" + font + "\">" +
        //                            name +
        //                       "</span> " +
        //                   "</span>" +
        //               "</a>";

        //    return new HtmlString(s);
        //}


    }

    public enum SquareSize
    {

    }

    public enum CloudImage
    {
        Green,
        Orange,
        Blue,
        Black,
        Gray,
        Blue2
    }

    public enum StaticMetroImage
    {
        Download,
        See,
        Email
    }
}
