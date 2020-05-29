namespace ExceptionStrategy.Domain.Users
{
    public class UserNotFoundException : UserException
    {
        public UserNotFoundException(int userId)
            : base("MISSING", $"User with '{userId}' is missing.")
        {
            this.Data["userId"] = userId;
        }
    }
}