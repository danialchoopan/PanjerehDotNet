using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanjerehDotNet.Application.Services;
using PanjerehDotNet.Application.DTOs;
using PanjerehDotNet.Domain.Entities;
using PanjerehDotNet.Domain.Interfaces;
using System.Security.Claims;

namespace PanjerehDotNet.Web.Pages
{
    [Authorize]
    public class PostAdModel : PageModel
    {
        private readonly IAdService _adService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;

        public PostAdModel(IAdService adService, IUnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            _adService = adService;
            _unitOfWork = unitOfWork;
            _environment = environment;
        }

        public List<Category> Categories { get; set; } = new();

        [BindProperty]
        public AdInputModel Input { get; set; } = new();

        public class AdInputModel
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
            public string City { get; set; } = string.Empty;
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public async Task OnGetAsync()
        {
            Categories = (await _unitOfWork.Categories.GetAllAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(List<IFormFile> Images)
        {
            if (!ModelState.IsValid)
            {
                Categories = (await _unitOfWork.Categories.GetAllAsync()).ToList();
                return Page();
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString)) return RedirectToPage("/Login");

            var userId = int.Parse(userIdString);

            var imageUrls = new List<string>();
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

            foreach (var file in Images)
            {
                if (file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                    imageUrls.Add("/uploads/" + fileName);
                }
            }

            var dto = new CreateAdvertisementDto
            {
                Title = Input.Title,
                Description = Input.Description,
                Price = Input.Price,
                CategoryId = Input.CategoryId,
                City = Input.City,
                Latitude = Input.Latitude,
                Longitude = Input.Longitude
            };

            await _adService.CreateAdAsync(dto, userId, imageUrls);

            return RedirectToPage("/Index");
        }
    }
}
