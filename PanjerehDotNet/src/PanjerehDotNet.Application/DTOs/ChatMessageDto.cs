namespace PanjerehDotNet.Application.DTOs;
public class ChatMessageDto {
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public int AdId { get; set; }
    public string AdTitle { get; set; } = string.Empty;
}
