using Models.Responses;

namespace Core.Interfaces.Services
{
    public interface ITokenService
    {
        public TokenResult GetToken(PersonaResponse persona);
    }
}
