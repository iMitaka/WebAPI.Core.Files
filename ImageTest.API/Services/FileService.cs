using ImageTest.API.Models;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using ImageMagick;

namespace ImageTest.API.Services
{
    public class FileService : IFileService
    {
        #region "Private"
        private const string AvatarFolder = "Avatar";
        private readonly IHostingEnvironment hostingEnvironment;
        #endregion

        #region "Constructor"
        public FileService(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }
        #endregion

        #region "Public methods"

        public FileGetModel GetImageFile(string path)
        {
            var model = new FileGetModel();
            var imagePath = Path.Combine(hostingEnvironment.ContentRootPath, AvatarFolder + "\\userName\\" + path);

            if (IsImageExist(imagePath))
            {
                model.File = System.IO.File.OpenRead(imagePath);
                model.FileType = GetImageTypeFromPath(imagePath);
                return model;
            }

            return null;
        }

        public async Task<string> GenerateUserAvatarSource(FilePostModel model)
        {
            if (model.File == null)
                return null;

            var avatarFileExtension = GetAvatarFileFormat(model.File.FileName);
            if (avatarFileExtension == null || model.File.Length == 0)
                return null;

            var hash = string.Empty;

            using (var md5 = MD5.Create())
            {
                var stream = model.File.OpenReadStream();
                hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            }

            var uploads = Path.Combine(hostingEnvironment.ContentRootPath, AvatarFolder + "\\" + model.UserId);

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var avatarPath = Path.Combine(uploads, $"{hash}{avatarFileExtension}");

            if (!System.IO.File.Exists(avatarPath))
            {
                using (var fileStream = new FileStream(avatarPath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fileStream);
                    Resize(avatarPath, uploads, $"{hash}{avatarFileExtension}");
                }

                File.Delete(avatarPath);
            }

            return $"{hash}{avatarFileExtension}";
        }
        #endregion

        #region "Private methods"

        private string Resize(string imagePath, string globalPath, string imageName, int width = 1200, int height = 1200, int quality = 75)
        {
            var newPath = globalPath + "\\" + width + "x" + height + "x" + imageName;
            using (var image = new MagickImage(imagePath))
            {
                image.Resize(width, height);
                image.Strip();
                image.Quality = quality;
                image.Write(newPath);
            }

            return newPath;
        }

        private static string GetAvatarFileFormat(string fileName)
        {
            string fileFormat = Path.GetExtension(fileName).ToLower();
            switch (fileFormat)
            {
                case ".jpg":
                case ".png":
                case ".gif": return fileFormat;
                default:
                    return null;
            }
        }

        private string GetImageTypeFromPath(string imagePath)
        {
            return imagePath.Split('.').LastOrDefault();
        }

        private bool IsImageExist(string imagePath)
        {
            return System.IO.File.Exists(imagePath);
        }
        #endregion
    }
}
