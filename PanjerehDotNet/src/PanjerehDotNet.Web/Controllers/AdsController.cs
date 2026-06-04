using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanjerehDotNet.Application.DTOs;
using PanjerehDotNet.Application.Services;
using System.Security.Claims;
namespace PanjerehDotNet.Web.Controllers;
[ApiController] [Route("api/[controller]")]
public class AdsController : ControllerBase {
    private readonly IAdService _adService;
    private readonly IWebHostEnvironment _env;
    public AdsController(IAdService adService, IWebHostEnvironment env) {
        _adService = adService;
        _env = env;
    }
    [HttpGet] public async Task<IActionResult> GetAds(int page = 1, int pageSize = 10, int? categoryId = null, string? query = null) {
        var ads = await _adService.GetAdsAsync(page, pageSize, categoryId, query);
        return Ok(ads);
    }
    [HttpGet("{id}")] public async Task<IActionResult> GetAd(int id) {
        var ad = await _adService.GetAdByIdAsync(id);
        if (ad == null) return NotFound();
        return Ok(ad);
    }
    [Authorize] [HttpPost] public async Task<IActionResult> CreateAd([FromForm] CreateAdvertisementDto dto, List<IFormFile> images) {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var imageUrls = new List<string>();

        var uploadDir = Path.Combine(_env.WebRootPath, "uploads");
        if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

        foreach (var image in images) {
            if (image.Length > 0) {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploadDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    await image.CopyToAsync(stream);
                }
                imageUrls.Add($"/uploads/{fileName}");
            }
        }

        var adId = await _adService.CreateAdAsync(dto, userId, imageUrls);
        return CreatedAtAction(nameof(GetAd), new { id = adId }, new { id = adId });
    }
}
