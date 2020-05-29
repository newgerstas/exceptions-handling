using ExceptionStrategy.Exceptions;

namespace ExceptionStrategy.Domain.Users
{
    public class UserException : ABCException
    {
        public UserException(ErrorCode code, string message)
            : base("USER" + code, message) { }
    }
}