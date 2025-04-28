using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string?> UploadImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            await using var stream = file.OpenReadStream();
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
               
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult?.SecureUrl?.AbsoluteUri;
        }
        public async Task<string?> UploadPdfAsync(byte[] pdfBytes, string fileName)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
                return null;

            using var stream = new MemoryStream(pdfBytes);
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = "Contracts",
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            return uploadResult?.SecureUrl?.AbsoluteUri;
        }
        public async Task<byte[]> DownloadPdfAsync(string fileUrl)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(fileUrl);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to download PDF. Status Code: {response.StatusCode}");
                }

                return await response.Content.ReadAsByteArrayAsync();
            }
        }


    }
}
