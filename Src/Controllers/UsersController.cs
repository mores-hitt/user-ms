using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using user_ms.Src.Protos;

using user_ms.Src.Services.Interfaces;

namespace user_ms.Src.Controllers
{
    public class UsersController : UserGrpc.UserGrpcBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [Authorize]
        public override Task<UserDto> GetProfile(Empty request, ServerCallContext context)
        {
            return _usersService.GetProfile(context);
        }

        [Authorize]
        public override async Task<UserDto> EditProfile(EditProfileDto request, ServerCallContext context)
        {
            return await _usersService.EditProfile(request, context);
        }

        [Authorize]
        public override async Task<GetUserProgressResponse> GetUserProgress(Empty request, ServerCallContext context)
        {
            var userProgress = await _usersService.GetUserProgress(context);
            return new GetUserProgressResponse
            {
                UserProgress = { userProgress }
            };

        }
    

        [Authorize]
        public override async Task<Empty> SetUserProgress(UpdateUserProgressDto request, ServerCallContext context)
        {
            await _usersService.SetUserProgress(request, context);
            return new Empty();
        }
    
    }
}