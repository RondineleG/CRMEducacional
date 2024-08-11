namespace CRMEducacional.Core.Exceptions;

public abstract class CustomException(string message) : SystemException(message)
{
}