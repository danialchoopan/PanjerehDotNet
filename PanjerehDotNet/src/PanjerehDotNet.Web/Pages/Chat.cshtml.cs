using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanjerehDotNet.Application.DTOs;
using PanjerehDotNet.Application.Services;
using System.Security.Claims;
namespace PanjerehDotNet.Web.Pages;
[Authorize]
public class ChatModel : PageModel {
    private readonly IChatService _chatService;
    public ChatModel(IChatService chatService) { _chatService = chatService; }
    public IEnumerable<ChatMessageDto> Conversations { get; set; } = new List<ChatMessageDto>();
    public int CurrentUserId { get; set; }
    public async Task OnGetAsync() {
        CurrentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Conversations = await _chatService.GetUserConversationsAsync(CurrentUserId);
    }
}
