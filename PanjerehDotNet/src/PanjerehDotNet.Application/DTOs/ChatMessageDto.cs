namespace PanjerehDotNet.Application.DTOs;
public class ChatMessageDto {
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public int AdvertisementId { get; set; }
    public string AdvertisementTitle { get; set; } = string.Empty;
}
