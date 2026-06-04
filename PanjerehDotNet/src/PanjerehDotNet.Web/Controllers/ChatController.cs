using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PanjerehDotNet.Application.Services;
using System.Security.Claims;
namespace PanjerehDotNet.Web.Controllers;
[Authorize] [ApiController] [Route("api/[controller]")]
public class ChatController : ControllerBase {
    private readonly IChatService _chatService;
    public ChatController(IChatService chatService) { _chatService = chatService; }
    [HttpGet("history")] public async Task<IActionResult> GetChatHistory(int otherUserId, int adId) {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var history = await _chatService.GetChatHistoryAsync(currentUserId, otherUserId, adId);
        return Ok(history);
    }
    [HttpGet("conversations")] public async Task<IActionResult> GetConversations() {
        var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var conversations = await _chatService.GetUserConversationsAsync(currentUserId);
        return Ok(conversations);
    }
}
