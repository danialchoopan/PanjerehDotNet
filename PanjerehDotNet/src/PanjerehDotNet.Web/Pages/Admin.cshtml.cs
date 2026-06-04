using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanjerehDotNet.Domain.Interfaces;
using PanjerehDotNet.Domain.Entities;
namespace PanjerehDotNet.Web.Pages;
[Authorize(Roles = "Admin")]
public class AdminModel : PageModel {
    private readonly IUnitOfWork _unitOfWork;
    public AdminModel(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public dynamic Stats { get; set; } = null!;
    public IEnumerable<Advertisement> RecentAds { get; set; } = new List<Advertisement>();
    public async Task OnGetAsync() {
        var all = await _unitOfWork.Advertisements.GetAllAsync();
        Stats = new {
            TotalAds = all.Count(),
            ApprovedAds = all.Count(a => a.IsApproved),
            PendingAds = all.Count(a => !a.IsApproved)
        };
        RecentAds = all.OrderByDescending(a => a.CreatedAt).Take(10);
    }
}
