namespace PanjerehDotNet.Domain.Entities;
public class ChatMessage {
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;
    public int SenderId { get; set; }
    public User? Sender { get; set; }
    public int ReceiverId { get; set; }
    public User? Receiver { get; set; }
    public int AdvertisementId { get; set; }
    public Advertisement? Advertisement { get; set; }
}
