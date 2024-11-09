using Grpc.Core;
using Grpc.Core.Interceptors;
using user_ms.Src.Exceptions;
using user_ms.Src.Common.Constants;
using System.Security.Authentication;

namespace user_ms.Src.Middlewares
{
    public class ExceptionHandlingInterceptor : Interceptor
    {
        private readonly ILogger<ExceptionHandlingInterceptor> _logger;
        private readonly Dictionary<Type, (string ErrorMessage, int StatusCode)> exceptionMapping = new()
            {
            { typeof(InvalidCredentialException), (ErrorMessages.InvalidCredentials, 16) },
            { typeof(EntityNotFoundException), (ErrorMessages.EntityNotFound, 5) },
            { typeof(EntityDeletedException), (ErrorMessages.EntityNotDeleted, 2) },
            { typeof(InvalidJwtException), (ErrorMessages.InternalServerError, 3) },
            { typeof(DuplicateUserException), (ErrorMessages.DuplicateUser, 6) },
            { typeof(DisabledUserException), (ErrorMessages.DisabledUser, 9)},
            { typeof(InternalErrorException), (ErrorMessages.InternalServerError, 13)},
            {typeof(UnauthorizedAccessException), (ErrorMessages.UnauthorizedAccess, 16)},
            {typeof(DuplicateEntityException), (ErrorMessages.EntityDuplicated, 3)}
        };
        public ExceptionHandlingInterceptor(ILogger<ExceptionHandlingInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
            TRequest request, 
            ServerCallContext context, 
            UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                return await continuation(request, context);
            }
            catch (Exception ex)
            {
                if (exceptionMapping.TryGetValue(ex.GetType(), out var mapping))
                {
                    await HandleGrcpException(ex, context, mapping.ErrorMessage, mapping.StatusCode);
                }
                else
                {
                    await HandleGrcpException(ex, context, ErrorMessages.InternalServerError, 13);
                }

                return await Task.FromException<TResponse>(ex);
            }
        }

        private Task HandleGrcpException(
            Exception ex,
            ServerCallContext context,
            string errorMessage,
            int statusCode)
        {
            if (statusCode == 13)
                _logger.LogError(ex, ex.Message);
            else
                _logger.LogInformation(ex, ex.Message);

            var metadata = new Metadata
            {
                { "error", errorMessage }
            };

            var status = new Status((StatusCode)statusCode, errorMessage);
            throw new RpcException(status, errorMessage);
        }

    }

}
