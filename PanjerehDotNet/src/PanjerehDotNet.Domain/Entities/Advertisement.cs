namespace PanjerehDotNet.Domain.Entities;
public class Advertisement {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsApproved { get; set; } = false;
    public int UserId { get; set; }
    public User? User { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string City { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public ICollection<AdImage> Images { get; set; } = new List<AdImage>();
}
public class AdImage {
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public int AdvertisementId { get; set; }
}
