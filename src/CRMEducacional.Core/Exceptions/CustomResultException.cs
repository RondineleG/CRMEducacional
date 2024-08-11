namespace CRMEducacional.Core.Exceptions;

public class CustomResultException : Exception
{
    public CustomResultException(CustomResult customResult)
    {
        CustomResult = customResult;
    }

    public CustomResultException(params Validation[] validations)
        : this(CustomResult.WithValidations(validations)) { }

    public CustomResultException(Exception exception)
        : this(CustomResult.WithError(exception)) { }

    public CustomResult CustomResult { get; }
}