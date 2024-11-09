using Grpc.Core;

namespace user_ms.Src.Services.Interfaces
{
    public interface IAuthService
    {
        public string GetUserEmailInToken(ServerCallContext context);

    }
}