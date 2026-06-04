using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanjerehDotNet.Domain.Interfaces;
using PanjerehDotNet.Domain.Entities;
namespace PanjerehDotNet.Web.Pages;

public class AdminModel : PageModel {
    private readonly IUnitOfWork _unitOfWork;
    public AdminModel(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public dynamic Stats { get; set; } = null!;
    public IEnumerable<Advertisement> RecentAds { get; set; } = new List<Advertisement>();
    public IEnumerable<AdReport> Reports { get; set; } = new List<AdReport>();
    public async Task OnGetAsync() {
        var all = await _unitOfWork.Advertisements.GetAllAsync();
        var reports = await _unitOfWork.Reports.GetAllAsync();
        Stats = new {
            TotalAds = all.Count(),
            ApprovedAds = all.Count(a => a.IsApproved),
            PendingAds = all.Count(a => !a.IsApproved),
            ReportCount = reports.Count()
        };
        RecentAds = all.OrderByDescending(a => a.CreatedAt).Take(10);
        Reports = reports.OrderByDescending(r => r.CreatedAt).Take(10);
    }
}
