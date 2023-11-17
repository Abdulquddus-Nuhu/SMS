using Access.API.Models.Responses;
using Access.Models.Responses;

namespace Access.API.Services.Interfaces
{
    public interface ITokenService
    {
        public TokenResult GetToken(PersonaResponse persona);
    }
}
