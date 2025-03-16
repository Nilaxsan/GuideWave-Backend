using GuideWave.Models;

namespace GuideWave.Services.JWTService
{
    public interface IJwtService
    {
        string GenerateToken_Tourist(Tourists tourists);

        string GenerateToken_Guide(Guide guide);
    }
}
