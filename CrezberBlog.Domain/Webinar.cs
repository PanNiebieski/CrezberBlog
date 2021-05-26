using System;

namespace CrezberBlog.Domain
{
    public class Webinar
    {
        public string Id { get; set; }

        public string ImageMainUrl { get; set; }

        public string ImagePresentation { get; set; }

        public string UrlEvent { get; set; }

        public string UrlPresentation { get; set; }

        public string UrlBlogCourse { get; set; }

        public string UrlCodeSource { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool Visible { get; set; }
        public bool AlreadyHappend { get; set; }

        public string When { get; set; }

        public DateTime? WhenDataTime
        {
            get
            {
                DateTime t;
                bool worked = DateTime.TryParse(When, out t);

                if (worked)
                    return t;
                else
                    return null;
            }
        }

        public string PrivateNotes { get; set; }

        public WebinarUrl Watch { get; set; }

    }
}
