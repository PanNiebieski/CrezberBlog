using CrezberBlog.ApplicationCore.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IColorsServices
    {
        IList<ColorModel> ColorModels();

        ColorModel FindColor(string key);

        Task AddColorAsync(ColorModel color);
    }
}
