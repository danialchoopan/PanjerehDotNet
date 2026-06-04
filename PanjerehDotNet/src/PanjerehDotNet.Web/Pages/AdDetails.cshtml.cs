using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanjerehDotNet.Application.DTOs;
using PanjerehDotNet.Application.Services;
namespace PanjerehDotNet.Web.Pages;
public class AdDetailsModel : PageModel {
    private readonly IAdService _adService;
    public AdDetailsModel(IAdService adService) { _adService = adService; }
    public AdvertisementDto Ad { get; set; } = null!;
    public async Task<IActionResult> OnGetAsync(int id) {
        var ad = await _adService.GetAdByIdAsync(id);
        if (ad == null) return NotFound();
        Ad = ad;
        return Page();
    }
}
