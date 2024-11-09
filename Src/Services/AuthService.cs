using System.Security.Claims;
using user_ms.Src.Exceptions;
using user_ms.Src.Repositories.Interfaces;
using user_ms.Src.Services.Interfaces;
using DotNetEnv;
using Grpc.Core;

namespace user_ms.Src.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapperService _mapperService;
        private readonly string _jwtSecret;

        public AuthService(IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IMapperService mapperService
        )
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapperService = mapperService;
            _jwtSecret = Env.GetString("JWT_SECRET") ?? throw new InvalidJwtException("JWT_SECRET not found");
        }

        public string GetUserEmailInToken(ServerCallContext callContext)
        {
            var httpUser = GetHttpUser(callContext);

            //Get Claims from JWT
            var userEmail = httpUser.FindFirstValue(ClaimTypes.Email) ??
                throw new UnauthorizedAccessException("Invalid user email in token");
            return userEmail;
        }

        public string GetUserRoleInToken(ServerCallContext callContext)
        {
            var httpUser = GetHttpUser(callContext);

            //Get Claims from JWT
            var userRole = httpUser.FindFirstValue(ClaimTypes.Role) ??
                throw new UnauthorizedAccessException("Invalid role in token");
            return userRole;
        }

        #region PRIVATE_METHODS

        private ClaimsPrincipal GetHttpUser(ServerCallContext callContext)
        {
            var httpContext = callContext.GetHttpContext();
                if (!httpContext.User.Identity?.IsAuthenticated ?? false)
                    throw new UnauthorizedAccessException("User not authenticated");
            
            return httpContext.User;
        }

        #endregion

    }
}