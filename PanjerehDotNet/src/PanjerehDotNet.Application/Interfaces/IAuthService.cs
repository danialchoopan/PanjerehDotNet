namespace PanjerehDotNet.Application.Interfaces;
public interface IAuthService {
    Task<string> GenerateOtpAsync(string phoneNumber);
    Task<string?> VerifyOtpAndGenerateTokenAsync(string phoneNumber, string otp);
}
