using System;
namespace Core.Interfaces.Infrastructure
{
    public interface IOtpGenerator
    {
        string Generate(string key, int expireTimeMinutes = 5, int digitsCount = 4);

        bool Verify(string key, string token, int expireTimeMinutes = 5, int digitsCount = 4);
    }
}

