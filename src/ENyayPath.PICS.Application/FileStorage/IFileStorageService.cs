using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.FileStorage
{
    public interface IFileStorageService
    {
         Task<string> SaveAsync(IFormFile file, string relativeDirectory);
         Task<Stream> OpenReadAsync(string relativePath);
    }
}
