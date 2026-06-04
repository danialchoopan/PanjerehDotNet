namespace PanjerehDotNet.Domain.Entities;

public class AdReport
{
    public int Id { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int AdvertisementId { get; set; }
    public Advertisement? Advertisement { get; set; }

    public int? UserId { get; set; }
    public User? User { get; set; }
}
