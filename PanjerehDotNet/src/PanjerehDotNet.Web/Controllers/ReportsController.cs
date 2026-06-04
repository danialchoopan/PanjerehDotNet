using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanjerehDotNet.Domain.Entities;
using PanjerehDotNet.Domain.Interfaces;
using System.Security.Claims;
namespace PanjerehDotNet.Web.Controllers;
[Authorize] [ApiController] [Route("api/[controller]")]
public class ReportsController : ControllerBase {
    private readonly IUnitOfWork _unitOfWork;
    public ReportsController(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    [HttpPost] public async Task<IActionResult> ReportAd([FromBody] AdReport report) {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null) report.UserId = int.Parse(userId);
        report.CreatedAt = DateTime.UtcNow;
        await _unitOfWork.Reports.AddAsync(report);
        await _unitOfWork.CompleteAsync();
        return Ok(new { message = "گزارش شما ثبت شد" });
    }
}
