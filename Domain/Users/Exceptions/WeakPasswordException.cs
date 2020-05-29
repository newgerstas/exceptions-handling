namespace ExceptionStrategy.Domain.Users
{
    public class WeakPasswordException : InvalidPasswordException
    {
        public WeakPasswordException(string password, int minLength)
            : base("WEAK", $"Password '{password}' is too weak. Must be at least {minLength} length.", password)
        {
            this.Data["minLength"] = minLength;
        }
    }
}