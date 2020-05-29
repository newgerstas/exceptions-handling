namespace ExceptionStrategy.Domain.Users
{
    public class DuplicateUserNameException : UserRegistrationException
    {
        public DuplicateUserNameException(string userName)
            : base("DUPL", $"Duplicate user name '{userName}' during user registration")
        {
            this.Data["userName"] = userName;
        }
    }
}