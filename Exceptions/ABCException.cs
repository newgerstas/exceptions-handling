namespace ExceptionStrategy.Exceptions
{
    public class ABCException : System.Exception
    {
        public ABCException(ErrorCode code, string message)
            : base(message)
        {
            this.Code = "ABC" + code;
        }

        public virtual ErrorCode Code { get; }
    }
}