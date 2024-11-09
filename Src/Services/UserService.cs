using Grpc.Core;
using user_ms.Src.Exceptions;
using user_ms.Src.Models;
using user_ms.Src.Repositories.Interfaces;
using user_ms.Src.Services.Interfaces;

using Google.Protobuf.Collections;
using user_ms.Src.Protos;

namespace user_ms.Src.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapperService _mapperService;
        private readonly IAuthService _authService;

        public UsersService(IUnitOfWork unitOfWork, IMapperService mapperService, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapperService = mapperService;
            _authService = authService;
        }

        public Task<UserDto> GetProfile(ServerCallContext context)
        {
            var userEmail = _authService.GetUserEmailInToken(context);
            return GetByEmail(userEmail);
        }

        public async Task<UserDto> GetByEmail(string email)
        {
            var user = await GetUserByEmail(email);
            return _mapperService.Map<User, UserDto>(user);
        }

        private async Task<User> GetUserByEmail(string email)
        {
            var user = await _unitOfWork.UsersRepository.GetByEmail(email)
                                        ?? throw new EntityNotFoundException($"User with email: {email} not found");

            return user;
        }

        public async Task<UserDto> EditProfile(EditProfileDto editProfileDto, ServerCallContext context)
        {
            var userEmail = _authService.GetUserEmailInToken(context);
            var user = await GetUserByEmail(userEmail);
            // UpdateFields
            user.Name = editProfileDto.Name;
            user.FirstLastName = editProfileDto.FirstLastName;
            user.SecondLastName = editProfileDto.SecondLastName;

            var updatedUser = await _unitOfWork.UsersRepository.Update(user);

            return _mapperService.Map<User, UserDto>(updatedUser);
        }

    
        public async Task<RepeatedField<UserProgressDto>> GetUserProgress(ServerCallContext context)
        {
            var userId = await GetUserIdByToken(context);

            var userProgress = await _unitOfWork.UsersRepository.GetProgressByUser(userId) ?? new List<UserProgress>();

            return _mapperService.MapRepeatedField<UserProgress, UserProgressDto>(userProgress);
        }

        public async Task SetUserProgress(UpdateUserProgressDto subjects, ServerCallContext context)
        {
            var userId = await GetUserIdByToken(context);

            var subjectsId = await MapAndValidateToSubjectId(subjects);
            var subjectsToAdd = subjectsId.Item1;
            var subjectsToDelete = subjectsId.Item2;
            // Get Current User Progress
            var userProgress = await _unitOfWork.UsersRepository.GetProgressByUser(userId) ?? new List<UserProgress>();

            var progressToAdd = subjectsToAdd.Select(s =>
            {
                var foundUserProgress = userProgress.FirstOrDefault(up => up.SubjectId == s);

                if (foundUserProgress is not null)
                    throw new DuplicateEntityException($"Subject with ID: {foundUserProgress.Subject.Code} already exists");

                return new UserProgress()
                {
                    SubjectId = s,
                    UserId = userId,
                };
            }).ToList();

            var progressToRemove = subjectsToDelete.Select(s =>
            {
                if (userProgress.FirstOrDefault(up => up.SubjectId == s) is null)
                    throw new EntityNotFoundException($"Subject with ID: {s} not found");

                return new UserProgress()
                {
                    SubjectId = s,
                    UserId = userId,
                };
            }).ToList();

            var addResult = await _unitOfWork.UsersRepository.AddProgress(progressToAdd);
            var removeResult = await _unitOfWork.UsersRepository.RemoveProgress(progressToRemove, userId);

            if (!removeResult && !addResult)
                throw new InternalErrorException("Cannot update user progress");
        }


        #region PRIVATE_METHODS

        private async Task<int> GetUserIdByToken(ServerCallContext context)
        {
            var userEmail = _authService.GetUserEmailInToken(context);
            var user = await _unitOfWork.UsersRepository.GetByEmail(userEmail) ??
                          throw new EntityNotFoundException("User not found");
            return user.Id;
        }

        private async Task<Tuple<List<int>, List<int>>> MapAndValidateToSubjectId(UpdateUserProgressDto subjects)
        {
            var allSubjects = await _unitOfWork.SubjectsRepository.Get();
            var subjectsToAdd = subjects.AddSubjects;
            var subjectsToDelete = subjects.DeleteSubjects;

            var mappedSubjectsToAdd = subjectsToAdd.Select(s =>
            {
                s = s.ToLower();
                var foundSubject = allSubjects.FirstOrDefault(sub => sub.Code == s)
                    ?? throw new EntityNotFoundException($"Subject with ID: {s} not found");
                return foundSubject.Id;
            }).ToList();

            var mappedSubjectsToDelete = subjectsToDelete.Select(s =>
            {
                s = s.ToLower();
                var foundSubject = allSubjects.FirstOrDefault(sub => sub.Code == s)
                    ?? throw new EntityNotFoundException($"Subject with ID: {s} not found");
                return foundSubject.Id;
            }).ToList();

            return new Tuple<List<int>, List<int>>(mappedSubjectsToAdd, mappedSubjectsToDelete);

        }

        #endregion

    }

}