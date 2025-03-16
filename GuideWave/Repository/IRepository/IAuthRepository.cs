namespace GuideWave.Repository.IRepository
{
    public interface IAuthRepository
    {
        Task SendVerificationCodeAsync(string email);
        //public Task<bool> VerifyOtpAsync(string verificationCode);
        //public Task<bool> ResetPasswordAsync(string newPassword);
    }
}
