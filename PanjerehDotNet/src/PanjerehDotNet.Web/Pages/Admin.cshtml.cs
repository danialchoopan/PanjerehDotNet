using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanjerehDotNet.Application.DTOs;
using PanjerehDotNet.Application.Services;
using PanjerehDotNet.Domain.Interfaces;
namespace PanjerehDotNet.Web.Pages;
[Authorize(Roles = "Admin")]
public class AdminModel : PageModel {
    private readonly IUnitOfWork _unitOfWork;
    public AdminModel(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public dynamic Stats { get; set; } = null!;
    public async Task OnGetAsync() {
        var ads = await _unitOfWork.Advertisements.GetAllAsync();
        Stats = new { TotalAds = ads.Count() };
    }
}
