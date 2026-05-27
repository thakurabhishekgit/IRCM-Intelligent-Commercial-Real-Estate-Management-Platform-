using IRCM.DTOs.Upload;

namespace IRCM.Interfaces;

public interface IUploadService
{
    Task<UploadResponseDto?>
        UploadThumbnailAsync(
            IFormFile file
        );
}