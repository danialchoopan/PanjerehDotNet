using PanjerehDotNet.Application.DTOs;
using PanjerehDotNet.Domain.Interfaces;
namespace PanjerehDotNet.Application.Services;
public interface IChatService {
    Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int user1Id, int user2Id, int adId);
    Task<IEnumerable<ChatMessageDto>> GetUserConversationsAsync(int userId);
}
public class ChatService : IChatService {
    private readonly IUnitOfWork _unitOfWork;
    public ChatService(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(int user1Id, int user2Id, int adId) {
        var messages = await _unitOfWork.Chats.GetChatHistoryAsync(user1Id, user2Id, adId);
        return messages.Select(m => new ChatMessageDto {
            Id = m.Id, Content = m.Content, SentAt = m.SentAt, IsRead = m.IsRead, SenderId = m.SenderId,
            SenderName = m.Sender?.FullName ?? m.Sender?.PhoneNumber ?? "Unknown",
            AdvertisementId = m.AdvertisementId, AdvertisementTitle = m.Advertisement?.Title ?? ""
        });
    }
    public async Task<IEnumerable<ChatMessageDto>> GetUserConversationsAsync(int userId) {
        var messages = await _unitOfWork.Chats.GetUserConversationsAsync(userId);
        return messages.Select(m => new ChatMessageDto {
            Id = m.Id, Content = m.Content, SentAt = m.SentAt, IsRead = m.IsRead, SenderId = m.SenderId,
            SenderName = m.Sender?.FullName ?? m.Sender?.PhoneNumber ?? "Unknown",
            AdvertisementId = m.AdvertisementId, AdvertisementTitle = m.Advertisement?.Title ?? ""
        });
    }
}
