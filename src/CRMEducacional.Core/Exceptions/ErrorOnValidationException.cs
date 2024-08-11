namespace CRMEducacional.Core.Exceptions;

public class ErrorOnValidationException(string message) : CustomException(message)
{
}