using ExceptionStrategy.Exceptions;

namespace ExceptionStrategy.Domain.Users
{
    public class InvalidPasswordException : UserRegistrationException
    {
        public InvalidPasswordException(ErrorCode code, string message, string password)
            : base("PWD" + code, $"Password '{password}' is invalid. {message}")
        {
            this.Data["password"] = SecurePassword(password);
        }

        public static string SecurePassword(string password)
        {
            // hide actual password
            return new string('*', password.Length);
        }
    }
}