using IRCM.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRCM.Controllers;

[ApiController]
[Route("api/upload")]
public class UploadController : ControllerBase
{
    private readonly IUploadService
        _uploadService;

    public UploadController(
        IUploadService uploadService
    )
    {
        _uploadService = uploadService;
    }

    // =========================
    // UPLOAD THUMBNAIL
    // =========================

    [Authorize(Roles = "Agent,Admin")]
    [HttpPost("thumbnail")]
    public async Task<IActionResult>
        UploadThumbnail(
            IFormFile file
        )
    {
        var result =
            await _uploadService
                .UploadThumbnailAsync(file);

        if (result == null)
        {
            return BadRequest(new
            {
                success = false,
                message =
                    "Image upload failed"
            });
        }

        return Ok(new
        {
            success = true,
            message =
                "Image uploaded successfully",
            data = result
        });
    }
}