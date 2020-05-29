namespace ExceptionStrategy.Exceptions
{
    public interface IErrorVocabularyProvider
    {
        IErrorVocabulary Create(string culture);
    }
}