using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanjerehDotNet.Application.DTOs;
using PanjerehDotNet.Application.Services;
using PanjerehDotNet.Domain.Interfaces;
using PanjerehDotNet.Domain.Entities;
using System.Security.Claims;

namespace PanjerehDotNet.Web.Pages
{
    [Authorize]
    public class ChatModel : PageModel
    {
        private readonly IChatService _chatService;
        private readonly IUnitOfWork _unitOfWork;

        public ChatModel(IChatService chatService, IUnitOfWork unitOfWork)
        {
            _chatService = chatService;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ChatMessageDto> Conversations { get; set; } = new List<ChatMessageDto>();
        public IEnumerable<ChatMessageDto> Messages { get; set; } = new List<ChatMessageDto>();
        public int CurrentUserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int OtherUserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int AdId { get; set; }

        public User OtherUser { get; set; }

        public async Task OnGetAsync()
        {
            CurrentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            Conversations = await _chatService.GetUserConversationsAsync(CurrentUserId);

            if (OtherUserId > 0 && AdId > 0)
            {
                Messages = await _chatService.GetChatHistoryAsync(CurrentUserId, OtherUserId, AdId);
                OtherUser = await _unitOfWork.Users.GetByIdAsync(OtherUserId);
            }
        }
    }
}
