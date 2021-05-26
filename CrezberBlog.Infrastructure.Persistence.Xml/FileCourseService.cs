using CrezberBlog.ApplicationCore.Contracts;
using CrezberBlog.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CrezberBlog.Infrastructure.Persistence.Xml
{
    public class FileCourseService : ICourseService
    {
        private readonly IWebInfoAboutFolders _webHostEnvironment;

        public FileCourseService(IWebInfoAboutFolders webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;

        }

        public Task<List<Course>> GetCourses(ref List<Post> posts)
        {
            //XElement courses = XElement.Load(_webHostEnvironment.WebRootPath + "\\Posts\\" + "0_xml_data\\courses.xml");

            string _folder = _webHostEnvironment.GetWebRootPath() + "\\" + "Posts\\0_xml_data\\Courses";

            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            List<Course> c = new List<Course>();

            // Can this be done in parallel to speed it up?
            foreach (string file in Directory.EnumerateFiles(_folder, "*.xml", SearchOption.TopDirectoryOnly))
            {
                XElement item = XElement.Load(file);

                Course cc = new Course();

                cc.Name = item.Element("name").Value;
                cc.ForBlogId = int.Parse(item.Element("forBlogId").Value);
                cc.Description = item.Element("description").Value;
                cc.Image = item.Element("image").Value;
                cc.Level = int.Parse(item.Element("level").Value);
                cc.Year = int.Parse(item.Element("year").Value);
                cc.UrlName = item.Element("urlName").Value;

                cc.NumberText = item.Element("numberString").Value;

                bool oo = true;
                var visibleXML = item.Element("visible");
                if (visibleXML != null)
                    _ = bool.TryParse(visibleXML.Value, out oo);
                cc.Visible = oo;

                bool showonmaigepagedfault = false;
                var showOnMainPageXML = item.Element("showOnMainPage");
                if (showOnMainPageXML != null)
                    _ = bool.TryParse(item.Element("showOnMainPage").Value, out showonmaigepagedfault);
                cc.ShowOnMainPage = showonmaigepagedfault;

                cc.SubCategory = "";
                var subcategoryXML = item.Element("subCategory");
                if (subcategoryXML != null)
                    cc.SubCategory = subcategoryXML.Value;

                var thebest = item.Element("thebest");
                if (thebest == null)
                    cc.TheBest = true;
                else
                    cc.TheBest = bool.Parse(thebest.Value);

                int importanceOrder = 200;
                var importanceOrderXML = item.Element("importantceOrder");
                if (importanceOrderXML != null)
                    _ = int.TryParse(item.Element("importantceOrder").Value, out importanceOrder);
                cc.ImportantceOrder = importanceOrder;

                //cc.CourseIndexContentString = item.Element("contentControl").Value + ".cshtml";

                List<CourseItem> items = new List<CourseItem>();



                foreach (var item2 in item.Descendants("items").Descendants("item"))
                {
                    CourseItem ia = new CourseItem();

                    ia.Index = int.Parse(item2.Element("index").Value);
                    ia.Title = item2.Element("title").Value;
                    ia.Slug = item2.Element("slug").Value;

                    var visible = item2.Element("visible");


                    if (visible == null)
                        ia.Visible = true;
                    else
                        ia.Visible = bool.Parse(visible.Value);



                    try
                    {
                        ia.Post = posts.FirstOrDefault(k => k.Slug == ia.Slug);

                        if (ia.Post == null)
                            ia.Visible = false;
                        else
                        {
                            ia.Post.Course = cc;
                            ia.Post.CourseItem = ia;
                        }

                        items.Add(ia);
                        ia.Course = cc;
                    }
                    catch (Exception)
                    {
                        //string mydocpath = HttpContext.Current.Server.MapPath("~/my");

                        //using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\222.txt"))
                        //{

                        //    outputFile.WriteLine(ia.Slug + " " + DateTime.Now);
                        //}

                    }

                }



                cc.Items = items.OrderBy(k => k.Index).ToList();
                c.Add(cc);

            }




            return Task.FromResult(c);
        }

        public async Task SaveCourse(Course course)
        {
            string _folder = _webHostEnvironment.GetWebRootPath() + "\\" + "Posts\\0_xml_data\\Courses";
            string filePath = Path.Combine(_folder, course.UrlName.ToLowerInvariant() + ".xml");

            XDocument doc = new XDocument(
                new XElement("course",
                    new XElement("forBlogId", course.ForBlogId),
                    new XElement("name", course.Name),
                    new XElement("urlName", course.UrlName.ToLowerInvariant()),
                    new XElement("year", course.Year),
                    new XElement("level", course.Level),
                    new XElement("image", course.Image),
                    new XElement("visible", course.Visible),
                    new XElement("isBig", course.isBig),
                    new XElement("completed", course.Completed),
                    new XElement("description", course.Description),
                    new XElement("numberString", course.NumberText),
                    new XElement("items", string.Empty),
                    new XElement("showOnMainPage", course.ShowOnMainPage),
                    new XElement("importantceOrder", course.ImportantceOrder),
                    new XElement("subCategory", course.SubCategory),
                    new XElement("thebest", course.TheBest)
                ));
            XElement categories = doc.XPathSelectElement("course/items");

            foreach (var item in course.Items)
            {
                categories.Add(new XElement("item",
                    new XElement("slug", item.Slug),
                    new XElement("title", item.Title),
                    new XElement("index", item.Index),
                    new XElement("visible", true)
                    ));
            }

            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                await doc.SaveAsync(fs, SaveOptions.None, CancellationToken.None).ConfigureAwait(false);
            }


        }

        public Task DeleteCourse(Course course)
        {
            string _folder = _webHostEnvironment.GetWebRootPath() + "\\" + "Posts\\0_xml_data\\Courses";
            string filePath = Path.Combine(_folder, course.UrlName.ToLowerInvariant() + ".xml");

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return Task.CompletedTask;
        }

        private static string ReadValue(XElement doc, XName name, string defaultValue = "")
        {
            if (doc.Element(name) != null)
                return doc.Element(name)?.Value;

            return defaultValue;
        }
    }
}
