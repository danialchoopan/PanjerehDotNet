using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanjerehDotNet.Application.Services;
using PanjerehDotNet.Domain.Interfaces;
namespace PanjerehDotNet.Web.Controllers;
[Authorize(Roles = "Admin")] [ApiController] [Route("api/[controller]")]
public class AdminController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAdService _adService;
    public AdminController(IUnitOfWork unitOfWork, IAdService adService) { _unitOfWork = unitOfWork; _adService = adService; }
    [HttpGet("stats")] public async Task<IActionResult> GetStats() {
        var allAds = await _unitOfWork.Advertisements.GetAllAsync();
        return Ok(new { totalAds = allAds.Count() });
    }
    [HttpPost("approve-ad/{id}")] public async Task<IActionResult> ApproveAd(int id) {
        var result = await _adService.ApproveAdAsync(id);
        if (!result) return NotFound();
        return Ok(new { message = "Ad approved" });
    }
}
