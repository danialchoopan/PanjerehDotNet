using Microsoft.AspNetCore.SignalR;
using PanjerehDotNet.Domain.Entities;
using PanjerehDotNet.Domain.Interfaces;
namespace PanjerehDotNet.Infrastructure.Hubs;
public class ChatHub : Hub {
    private readonly IUnitOfWork _unitOfWork;
    public ChatHub(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }
    public async Task SendMessage(int receiverId, int adId, string content) {
        var senderId = int.Parse(Context.UserIdentifier ?? "0");
        var message = new ChatMessage { SenderId = senderId, ReceiverId = receiverId, AdvertisementId = adId, Content = content, SentAt = DateTime.UtcNow };
        await _unitOfWork.Chats.AddAsync(message);
        await _unitOfWork.CompleteAsync();
        await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId, adId, content, message.SentAt);
    }
}
