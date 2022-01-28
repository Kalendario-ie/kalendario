using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Kalendario.Application.Common.Interfaces;

namespace Kalendario.Api.Services;

public class ImageUploaderService : IImageUploaderService
{
    private readonly Cloudinary _cloudinary;
    
    public ImageUploaderService(IConfiguration configuration)
    {
        var cloudinaryConfig = configuration.GetSection("Cloudinary");
        
        var account =
            new Account(cloudinaryConfig["Cloud"], cloudinaryConfig["ApiKey"], cloudinaryConfig["ApiSecret"]);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<string> UploadImage(string name, Stream file)
    {
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(name, file)
        };
        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return "https://res.cloudinary.com/gchahm/" + uploadResult.FullyQualifiedPublicId;
    }
}