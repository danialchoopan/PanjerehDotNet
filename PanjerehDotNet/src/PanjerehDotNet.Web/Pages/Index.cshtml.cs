using Microsoft.AspNetCore.Mvc.RazorPages;
using PanjerehDotNet.Application.DTOs;
using PanjerehDotNet.Application.Services;
namespace PanjerehDotNet.Web.Pages;
public class IndexModel : PageModel {
    private readonly IAdService _adService;
    public IndexModel(IAdService adService) { _adService = adService; }
    public IEnumerable<AdvertisementDto> Ads { get; set; } = new List<AdvertisementDto>();
    public async Task OnGetAsync(int? cat) { Ads = await _adService.GetAdsAsync(1, 20, cat, null); }
}
