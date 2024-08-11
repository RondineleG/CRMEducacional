namespace CRMEducacional.Persistence.Extensions;

public static class CustomMessageHandler
{
    public static string DbErrorMessage(DbUpdateException exception)
    {
        return DbUpdateExceptionHandler.HandleDbUpdateException(exception);
    }

    public static string EntityNotFound(string entityName, int id)
    {
        return $"{entityName} não encontrado(a) com o Id: {id}.";
    }

    public static string EntityNotFound(string entityName, string parameterName)
    {
        return $"{entityName} não encontrado(a) com o parâmetro: {parameterName}.";
    }

    public static string InvalidParameter(string parameterName)
    {
        return $"O parâmetro :'{parameterName}' é inválido ou não foi fornecido.";
    }

    public static string UnexpectedError(string operation, string message)
    {
        return $"Erro inesperado ao {operation}: {message}";
    }
}