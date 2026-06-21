using ENyayPath.PICS.Application.FileStorage;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class LocalFileStorageService : IFileStorageService
{
   // private readonly IWebHostEnvironment _env;
    //public LocalFileStorageService(IWebHostEnvironment env) => _env = env;
    

    public async Task<string> SaveAsync(IFormFile file, string relativeDirectory)
    {
        var rootPath = AppContext.BaseDirectory; // Get the base directory of the application
        var uploads = Path.Combine(rootPath ?? "wwwroot", relativeDirectory);
        Directory.CreateDirectory(uploads);
        var name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var path = Path.Combine(uploads, name);
        using var stream = new FileStream(path, FileMode.Create);
        await file.CopyToAsync(stream);
        // return relative URL path
        return $"/{relativeDirectory}/{name}".Replace("\\", "/");
    }

    public Task<Stream> OpenReadAsync(string relativePath)
    {
        var rootPath = AppContext.BaseDirectory; // Get the base directory of the application
        var path = Path.Combine(rootPath ?? "wwwroot", relativePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
        Stream s = File.OpenRead(path);
        return Task.FromResult(s);
    }
}