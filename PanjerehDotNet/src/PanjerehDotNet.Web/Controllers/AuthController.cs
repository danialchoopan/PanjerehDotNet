using Microsoft.AspNetCore.Mvc;
using PanjerehDotNet.Application.Interfaces;
namespace PanjerehDotNet.Web.Controllers;
[ApiController] [Route("api/[controller]")]
public class AuthController : ControllerBase {
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) { _authService = authService; }
    [HttpPost("send-otp")] public async Task<IActionResult> SendOtp([FromBody] string phoneNumber) {
        var otp = await _authService.GenerateOtpAsync(phoneNumber);
        return Ok(new { message = "OTP sent", otp });
    }
    [HttpPost("verify-otp")] public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request) {
        var token = await _authService.VerifyOtpAndGenerateTokenAsync(request.PhoneNumber, request.Otp);
        if (token == null) return BadRequest("Invalid OTP");
        return Ok(new { token });
    }
}
public class VerifyOtpRequest { public string PhoneNumber { get; set; } = ""; public string Otp { get; set; } = ""; }
