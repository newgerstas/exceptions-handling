using ExceptionStrategy.Exceptions;

namespace ExceptionStrategy.Domain.Users
{
    public class InvalidUserNameException : UserRegistrationException
    {
        public InvalidUserNameException(ErrorCode code, string userName)
            : base("NAME" + code, $"User name '{userName}' is invalid.")
        {
            this.Data["userName"] = userName;
        }
    }
}