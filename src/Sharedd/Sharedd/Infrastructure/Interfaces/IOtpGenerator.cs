﻿using System;
namespace Shared.Infrastructure.Interfaces
{
    public interface IOtpGenerator
    {
        string Generate(string key, int expireTimeMinutes = 5, int digitsCount = 4);

        bool Verify(string key, string token, int expireTimeMinutes = 5, int digitsCount = 4);
    }
}

