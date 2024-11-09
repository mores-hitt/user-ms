namespace user_ms.Src.Exceptions
{
    public class DisabledUserException : Exception
    {
        public DisabledUserException() { }

        public DisabledUserException(string? message)
            : base(message) { }

        public DisabledUserException(string? message, Exception? innerException)
            : base(message, innerException) { }
    }
}