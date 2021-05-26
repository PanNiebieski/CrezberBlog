using CrezberBlog.ApplicationCore.Contracts;
using CrezberBlog.ApplicationCore.Domain;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace CrezberBlog.Infrastructure.Persistence.Xml
{
    public class FileColorsServices : IColorsServices
    {
        private List<ColorModel> _colors = new List<ColorModel>();

        private readonly IWebInfoAboutFolders _webHostEnvironment;


        public FileColorsServices(IWebInfoAboutFolders webHostEnvironment
            )
        {
            _webHostEnvironment = webHostEnvironment;


        }

        public async Task AddColorAsync(ColorModel color)
        {
            if (_colors.Count == 0)
                ColorModels();

            var itemToRemove = _colors.SingleOrDefault(r => r.Key == color.Key);
            if (itemToRemove != null)
                _colors.Remove(itemToRemove);

            _colors.Add(color);
            string _file = _webHostEnvironment.GetWebRootPath() + "\\Posts\\" + "0_xml_data\\" + "Colors.xml";

            XDocument doc = new XDocument(
                new XElement("Colors"
                ));

            XElement colors = doc.XPathSelectElement("Colors");
            foreach (var cc in _colors)
            {

                if (string.IsNullOrWhiteSpace(cc.BodyColor))
                {
                    cc.BodyColor = this.CorrectColor(cc.BackgroundColor, 0.20f);
                }

                if (string.IsNullOrWhiteSpace(cc.BodyColor30))
                {
                    cc.BodyColor30 = this.CorrectColor(cc.BackgroundColor, 0.30f);
                }

                if (string.IsNullOrWhiteSpace(cc.BodyColor25))
                {
                    cc.BodyColor25 = this.CorrectColor(cc.BackgroundColor, 0.25f);
                }

                if (string.IsNullOrWhiteSpace(cc.BodyColor35))
                {
                    cc.BodyColor35 = this.CorrectColor(cc.BackgroundColor, 0.35f);
                }

                if (cc.CorrectionFactor == 0.0f)
                {
                    colors.Add(new XElement("Color",
                        new XElement("Key", cc.Key),
                        new XElement("Value", cc.Value),
                        new XElement("HeaderColor", cc.HeaderColor),
                        new XElement("BackgroundColor", cc.BackgroundColor),
                        new XElement("BodyColor", cc.BodyColor),
                        new XElement("BodyColor25", cc.BodyColor25),
                        new XElement("BodyColor30", cc.BodyColor30),
                        new XElement("BodyColor35", cc.BodyColor35)
                        ));
                }
                else
                    colors.Add(new XElement("Color",
                        new XElement("Key", cc.Key),
                        new XElement("Value", cc.Value),
                        new XElement("HeaderColor", cc.HeaderColor),
                        new XElement("BackgroundColor", cc.BackgroundColor),
                        new XElement("BodyColor", cc.BodyColor),
                        new XElement("BodyColor25", cc.BodyColor25),
                        new XElement("BodyColor30", cc.BodyColor30),
                        new XElement("BodyColor35", cc.BodyColor35),
                        new XElement("CorrectionFactor", cc.CorrectionFactor.ToString("0.0000", CultureInfo.InvariantCulture))));
            }
            _colors = new List<ColorModel>();
            //using (var fs = new FileStream(_file, FileMode.Create, FileAccess.ReadWrite))
            //{
            //    await doc.SaveAsync(fs, SaveOptions.None, CancellationToken.None).ConfigureAwait(false);
            //}

            using (var fs = new FileStream(_file, FileMode.Create, FileAccess.ReadWrite))
            {
                await doc.SaveAsync(fs, SaveOptions.None, CancellationToken.None).ConfigureAwait(false);
            }


        }

        public IList<ColorModel> ColorModels()
        {
            if (_colors.Count == 0)
            {
                string _file = _webHostEnvironment.GetWebRootPath() + "\\Posts\\" + "0_xml_data\\" + "Colors.xml";

                //if (!Directory.Exists(_folder))
                //    Directory.CreateDirectory(_folder);

                XElement doc = XElement.Load(_file);



                foreach (var node in doc.Elements("Color"))
                {
                    ColorModel cm = new ColorModel();

                    cm.Key = node.Element("Key").Value;
                    cm.Value = node.Element("Value").Value;
                    cm.HeaderColor = node.Element("HeaderColor").Value;
                    cm.BackgroundColor = node.Element("BackgroundColor").Value;

                    var check = node.Element("BodyColor");

                    if (check != null)
                    {
                        cm.BodyColor = check.Value;
                    }

                    var check25 = node.Element("BodyColor25");

                    if (check25 != null)
                    {
                        cm.BodyColor25 = check25.Value;
                    }

                    var check30 = node.Element("BodyColor30");

                    if (check30 != null)
                    {
                        cm.BodyColor30 = check30.Value;
                    }

                    var check35 = node.Element("BodyColor35");

                    if (check35 != null)
                    {
                        cm.BodyColor35 = check35.Value;
                    }


                    var f = node.Element("CorrectionFactor");
                    if (f != null)
                    {
                        float s;
                        var t = float.TryParse(f.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out s);
                        if (t)
                            cm.CorrectionFactor = s;
                    }

                    _colors.Add(cm);
                }


            }

            return _colors;
        }

        public ColorModel FindColor(string key)
        {
            if (_colors.Count == 0)
                ColorModels();

            var color = _colors.FirstOrDefault(k => k.Key.ToLowerInvariant() == key.ToLowerInvariant());

            if (color == null)
                return new ColorModel() { Key = "default", Value = "FFFFFF" };

            return color;

        }

        private string CorrectColor(string color, float correctionFactor)
        {

            System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml("#" + color);


            if (correctionFactor != 0.0f)
            {
                float red = (255 - c.R) * correctionFactor + c.R;
                float green = (255 - c.G) * correctionFactor + c.G;
                float blue = (255 - c.B) * correctionFactor + c.B;

                c = System.Drawing.Color.FromArgb((int)red, (int)green, (int)blue);
            }

            string s = Tools.ColorToHexString(c);
            return s;
        }
    }
}
