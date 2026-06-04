using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PanjerehDotNet.Application.Interfaces;
using PanjerehDotNet.Domain.Entities;
using PanjerehDotNet.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace PanjerehDotNet.Application.Services;
public class AuthService : IAuthService {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private static readonly Dictionary<string, string> _otpCache = new();
    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration) {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }
    public async Task<string> GenerateOtpAsync(string phoneNumber) {
        var otp = new Random().Next(100000, 999999).ToString();
        _otpCache[phoneNumber] = otp;
        return otp;
    }
    public async Task<string?> VerifyOtpAndGenerateTokenAsync(string phoneNumber, string otp) {
        if (_otpCache.TryGetValue(phoneNumber, out var cachedOtp) && cachedOtp == otp) {
            var user = await _unitOfWork.Users.GetByPhoneNumberAsync(phoneNumber);
            if (user == null) {
                user = new User { PhoneNumber = phoneNumber };
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CompleteAsync();
            }
            _otpCache.Remove(phoneNumber);
            return GenerateJwtToken(user);
        }
        return null;
    }
    private string GenerateJwtToken(User user) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "SecretKeyForPanjerehDotNetProject2024");
        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
