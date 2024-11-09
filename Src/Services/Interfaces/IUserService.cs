using Grpc.Core;
using Google.Protobuf.Collections;
using user_ms.Src.Protos;

namespace user_ms.Src.Services.Interfaces
{
    public interface IUsersService
    {

        public Task<UserDto> GetProfile(ServerCallContext context);

        public Task<UserDto> EditProfile(EditProfileDto editProfileDto, ServerCallContext context);

        public Task<UserDto> GetByEmail(string email);
        
        public Task<RepeatedField<UserProgressDto>> GetUserProgress(ServerCallContext context);

        public Task SetUserProgress(UpdateUserProgressDto subjects, ServerCallContext context);
        
    }
}