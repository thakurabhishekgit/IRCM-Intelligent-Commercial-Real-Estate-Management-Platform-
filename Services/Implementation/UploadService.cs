using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using IRCM.Configurations;
using IRCM.DTOs.Upload;
using IRCM.Interfaces;
using Microsoft.Extensions.Options;

namespace IRCM.Services.Implementation;

public class UploadService : IUploadService
{
    private readonly Cloudinary _cloudinary;

    public UploadService(
        IOptions<CloudinarySettings> config
    )
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<UploadResponseDto?>
    UploadThumbnailAsync(IFormFile file)
{
    try
    {
        Console.WriteLine(
            "========== UPLOAD STARTED =========="
        );

        // =========================
        // FILE CHECK
        // =========================

        if (file == null)
        {
            Console.WriteLine(
                "File is NULL"
            );

            return null;
        }

        Console.WriteLine(
            $"File Name: {file.FileName}"
        );

        Console.WriteLine(
            $"Content Type: {file.ContentType}"
        );

        Console.WriteLine(
            $"File Size: {file.Length}"
        );

        if (file.Length == 0)
        {
            Console.WriteLine(
                "File length is 0"
            );

            return null;
        }

        // =========================
        // CLOUDINARY CONFIG
        // =========================

        Console.WriteLine(
            "Preparing stream..."
        );

        await using var stream =
            file.OpenReadStream();

        Console.WriteLine(
            "Stream opened successfully"
        );

        // =========================
        // UPLOAD PARAMS
        // =========================

        var uploadParams =
            new ImageUploadParams
            {
                File = new FileDescription(
                    file.FileName,
                    stream
                ),

                Folder =
                    "IRCM/PropertyThumbnails"
            };

        Console.WriteLine(
            "Upload params created"
        );

        // =========================
        // CLOUDINARY REQUEST
        // =========================

        Console.WriteLine(
            "Sending image to Cloudinary..."
        );

        var uploadResult =
            await _cloudinary.UploadAsync(
                uploadParams
            );

        Console.WriteLine(
            "Cloudinary response received"
        );

        // =========================
        // ERROR CHECK
        // =========================

        if (uploadResult.Error != null)
        {
            Console.WriteLine(
                $"Cloudinary Error: {uploadResult.Error.Message}"
            );

            return null;
        }

        Console.WriteLine(
            $"Upload Success URL: {uploadResult.SecureUrl}"
        );

        Console.WriteLine(
            "========== UPLOAD SUCCESS =========="
        );

        return new UploadResponseDto
        {
            PublicUrl =
                uploadResult.SecureUrl.ToString(),

            PublicId =
                uploadResult.PublicId
        };
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            "========== UPLOAD FAILED =========="
        );

        Console.WriteLine(
            $"Exception Type: {ex.GetType().Name}"
        );

        Console.WriteLine(
            $"Exception Message: {ex.Message}"
        );

        Console.WriteLine(
            $"Stack Trace: {ex.StackTrace}"
        );

        if (ex.InnerException != null)
        {
            Console.WriteLine(
                $"Inner Exception: {ex.InnerException.Message}"
            );
        }

        return null;
    }
}
}