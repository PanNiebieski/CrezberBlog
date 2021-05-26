using CrezberBlog.ApplicationCore.Contracts;
using CrezberBlog.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CrezberBlog.Infrastructure.Persistence.Xml
{
    public class FileWebinarServices : IWebinarServices
    {
        public FileWebinarServices(IWebInfoAboutFolders webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public List<Webinar> _webinars = new List<Webinar>();

        private readonly IWebInfoAboutFolders _webHostEnvironment;

        public async Task SaveWebinar(Webinar w)
        {
            string _folder = _webHostEnvironment.GetWebRootPath() + "\\" + "Posts\\0_xml_data\\Webinars";
            string filePath = Path.Combine(_folder, w.Id.ToLowerInvariant() + ".xml");

            XDocument doc = new XDocument(
                new XElement("webinar",
                    new XElement("id", w.Id),
                    new XElement("title", w.Title),
                    new XElement("imageMainUrl", w.ImageMainUrl.ToLowerInvariant()),
                    new XElement("imagePresentationUrl", w.ImagePresentation.ToLowerInvariant()),
                    new XElement("urlEvent", w.UrlEvent),
                    new XElement("urlPresentation", w.UrlPresentation),
                    new XElement("urlCourse", w.UrlBlogCourse),
                    new XElement("urlCodeSource", w.UrlCodeSource),
                    new XElement("visible", w.Visible),
                    new XElement("alreadyHappend", w.AlreadyHappend),
                    new XElement("when", w.When),
                    new XElement("privateNotes", w.PrivateNotes),
                    new XElement("description", w.Description),
                    new XElement("when", w.When),
                    new XElement("url", w.When)
                ));

            XElement ulrs = doc.XPathSelectElement("webinar/url");

            if (w.Watch.Facebook == null)
                w.Watch.Facebook = "";

            if (w.Watch.Youtube == null)
                w.Watch.Youtube = "";

            ulrs.Add(
                    new XElement("facebook", w.Watch.Facebook),
                    new XElement("youtube", w.Watch.Youtube)
            );


            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                await doc.SaveAsync(fs, SaveOptions.None, CancellationToken.None).ConfigureAwait(false);
            }


            _webinars = new List<Webinar>();
        }

        public Task<List<Webinar>> GetWebinars()
        {
            if (_webinars.Count > 0)
                return Task.FromResult(_webinars);

            string _folder = _webHostEnvironment.GetWebRootPath() + "\\" + "Posts\\0_xml_data\\Webinars";

            //L.Log.Info(_folder);

            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            List<Webinar> c = new List<Webinar>();

            // Can this be done in parallel to speed it up?
            foreach (string file in Directory.EnumerateFiles(_folder, "*.xml",
                SearchOption.TopDirectoryOnly))
            {
                XElement item = XElement.Load(file);

                Webinar cm = new Webinar();

                cm.Id = item.Element("id").Value;
                cm.Title = item.Element("title").Value;
                cm.ImageMainUrl = item.Element("imageMainUrl").Value;
                cm.ImagePresentation = item.Element("imagePresentationUrl").Value;
                cm.Description = item.Element("description").Value;
                cm.When = item.Element("when").Value;
                cm.UrlEvent = item.Element("urlEvent").Value;
                cm.UrlPresentation = item.Element("urlPresentation").Value;
                cm.UrlBlogCourse = item.Element("urlCourse").Value;

                var visible = item.Element("visible");

                if (visible == null)
                    cm.Visible = true;
                else
                    cm.Visible = bool.Parse(visible.Value);

                var alreadyHappend = item.Element("alreadyHappend");

                if (alreadyHappend == null)
                    cm.AlreadyHappend = true;
                else
                    cm.AlreadyHappend = bool.Parse(alreadyHappend.Value);

                var urlNode = item.Element("url");

                if (urlNode != null)
                {
                    cm.Watch = new WebinarUrl();
                    cm.Watch.Facebook = urlNode.Element("facebook").Value;
                    cm.Watch.Youtube = urlNode.Element("youtube").Value;
                }

                var urlcode = item.Element("urlCodeSource");

                if (urlcode != null)
                {
                    cm.UrlCodeSource = urlcode.Value;
                }


                c.Add(cm);
            }

            c = c.OrderByDescending(k => k.Id).ToList();

            _webinars = c;

            return Task.FromResult(c);




        }
    }
}
