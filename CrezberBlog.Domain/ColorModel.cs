using System;
using System.Diagnostics.CodeAnalysis;

namespace CrezberBlog.ApplicationCore.Domain
{
    public class ColorModel : IComparable<ColorModel>
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string BackgroundColor { get; set; }

        public string HeaderColor { get; set; }

        public string BodyColor { get; set; }

        public string BodyColor25 { get; set; }

        public string BodyColor30 { get; set; }

        public string BodyColor35 { get; set; }

        public float CorrectionFactor { get; set; }


        public int CompareTo([AllowNull] ColorModel other)
        {
            if (other.Key == this.Key)
                return 0;
            else
                return Key.CompareTo(other);
        }
    }
}
