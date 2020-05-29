namespace ExceptionStrategy.Exceptions
{
    public interface IErrorVocabulary
    {
        ErrorInfo Translate(ABCException exception);
    }
}