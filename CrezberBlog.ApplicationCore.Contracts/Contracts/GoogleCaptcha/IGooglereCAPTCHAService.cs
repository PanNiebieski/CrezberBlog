using CrezberBlog.ApplicationCore.Dto;
using System.Threading.Tasks;

namespace CrezberBlog.ApplicationCore.Contracts
{
    public interface IGooglereCAPTCHAService
    {
        Task<GoogleREspo> CaptchaVerfication(string _token);
    }
}
