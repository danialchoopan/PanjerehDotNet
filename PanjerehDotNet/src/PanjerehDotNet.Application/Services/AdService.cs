using PanjerehDotNet.Application.DTOs;
using PanjerehDotNet.Domain.Entities;
using PanjerehDotNet.Domain.Interfaces;
namespace PanjerehDotNet.Application.Services;
public interface IAdService {
    Task<IEnumerable<AdvertisementDto>> GetAdsAsync(int page, int pageSize, int? categoryId, string? searchQuery);
    Task<AdvertisementDto?> GetAdByIdAsync(int id);
    Task<int> CreateAdAsync(CreateAdvertisementDto dto, int userId, List<string> imageUrls);
    Task<bool> ApproveAdAsync(int adId);
}
public class AdService : IAdService {
    private readonly IUnitOfWork _unitOfWork;
    public AdService(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IEnumerable<AdvertisementDto>> GetAdsAsync(int page, int pageSize, int? categoryId, string? searchQuery) {
        var ads = await _unitOfWork.Advertisements.GetPagedAsync(page, pageSize, categoryId, searchQuery);
        return ads.Select(MapToDto);
    }
    public async Task<AdvertisementDto?> GetAdByIdAsync(int id) {
        var ad = await _unitOfWork.Advertisements.GetByIdAsync(id);
        return ad != null ? MapToDto(ad) : null;
    }
    public async Task<int> CreateAdAsync(CreateAdvertisementDto dto, int userId, List<string> imageUrls) {
        var ad = new Advertisement {
            Title = dto.Title, Description = dto.Description, Price = dto.Price, CategoryId = dto.CategoryId,
            UserId = userId, City = dto.City, District = dto.District, Latitude = dto.Latitude, Longitude = dto.Longitude,
            CreatedAt = DateTime.UtcNow, Images = imageUrls.Select(url => new AdImage { Url = url }).ToList()
        };
        await _unitOfWork.Advertisements.AddAsync(ad);
        await _unitOfWork.CompleteAsync();
        return ad.Id;
    }
    public async Task<bool> ApproveAdAsync(int adId) {
        var ad = await _unitOfWork.Advertisements.GetByIdAsync(adId);
        if (ad == null) return false;
        ad.IsApproved = true;
        await _unitOfWork.CompleteAsync();
        return true;
    }
    private AdvertisementDto MapToDto(Advertisement ad) => new AdvertisementDto {
        Id = ad.Id, Title = ad.Title, Description = ad.Description, Price = ad.Price, CreatedAt = ad.CreatedAt,
        CategoryName = ad.Category?.Name ?? "", City = ad.City, District = ad.District,
        Latitude = ad.Latitude, Longitude = ad.Longitude, UserId = ad.UserId,
        Images = ad.Images.Select(i => i.Url).ToList()
    };
}
