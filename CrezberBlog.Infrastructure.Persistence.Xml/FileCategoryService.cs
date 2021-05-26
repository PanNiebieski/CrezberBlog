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
    public class FileCategoryService : ICategoryService
    {
        private List<CategoryNew> _categories = new List<CategoryNew>();

        private readonly IWebInfoAboutFolders _webHostEnvironment;

        public FileCategoryService(IWebInfoAboutFolders webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task AddCategoryAsync(CategoryNew color)
        {
            if (_categories.Count == 0)
                Categories();

            var itemToRemove = _categories.SingleOrDefault
                (r => r.Name.ToLowerInvariant()
                == color.Name.ToLowerInvariant());

            if (itemToRemove != null)
                _categories.Remove(itemToRemove);

            _categories.Add(color);
            string _file = _webHostEnvironment.GetWebRootPath() + "\\Posts\\" + "0_xml_data\\" + "Categories.xml";

            XDocument doc = new XDocument(
                new XElement("Categories"
                ));

            XElement cates = doc.XPathSelectElement("Categories");
            foreach (var cc in _categories)
            {
                cates.Add(new XElement("Category",
                        new XElement("NameInPosts", cc.Name),
                        new XElement("DisplayName", cc.DisplayName),
                        new XElement("ForBlogId", cc.ForBlogId),
                        new XElement("Color", cc.Color),
                        new XElement("SubCategory", cc.Color)
                    )
                );
            }
            _categories = new List<CategoryNew>();

            using (var fs = new FileStream(_file, FileMode.Create, FileAccess.ReadWrite))
            {
                await doc.SaveAsync(fs, SaveOptions.None, CancellationToken.None).ConfigureAwait(false);
            }
        }

        public IList<CategoryNew> Categories()
        {
            if (_categories.Count == 0)
            {
                string _file = _webHostEnvironment.GetWebRootPath() + "\\Posts\\" + "0_xml_data\\" + "Categories.xml";

                //if (!Directory.Exists(_folder))
                //    Directory.CreateDirectory(_folder);

                XElement doc = XElement.Load(_file);

                foreach (var node in doc.Elements("Category"))
                {
                    CategoryNew cm = new CategoryNew();

                    cm.Name = node.Element("NameInPosts").Value;
                    cm.ForBlogId = int.Parse(node.Element("ForBlogId").Value);
                    cm.DisplayName = node.Element("DisplayName").Value;
                    cm.Color = node.Element("Color").Value;
                    //cm.SubCategory = node.Element("SubCategory").Value;

                    //cm.Key = node.Element("Key").Value;
                    //cm.Value = node.Element("Value").Value;


                    //var f = node.Element("CorrectionFactor");
                    //if (f != null)
                    //{
                    //    float s;
                    //    var t = float.TryParse(f.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out s);
                    //    if (t)
                    //        cm.CorrectionFactor = s;
                    //}

                    _categories.Add(cm);
                }


            }

            return _categories;
        }

        public CategoryNew FindCategory(string name)
        {
            if (_categories.Count == 0)
                Categories();

            var color = _categories.FirstOrDefault(k =>
            k.Name.ToLowerInvariant() == name.ToLowerInvariant());

            return color;
        }
    }
}
