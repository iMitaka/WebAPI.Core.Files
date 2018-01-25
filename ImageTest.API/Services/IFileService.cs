using ImageTest.API.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ImageTest.API.Services
{
    public interface IFileService
    {
        Task<string> GenerateUserAvatarSource(FilePostModel model);
        FileGetModel GetImageFile(string path);
    }
}
