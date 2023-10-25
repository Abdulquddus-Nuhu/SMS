using System;
using Module.Access.Models.Response;

namespace Module.Access.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GetTokenAsync(PersonaResponse persona);
    }
}

