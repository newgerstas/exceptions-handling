using ExceptionStrategy.Exceptions;

namespace ExceptionStrategy.Domain.Users
{
    public class UserRegistrationException : UserException
    {
        public UserRegistrationException(ErrorCode code, string message)
            : base("NEW" + code, message) { }
    }
}