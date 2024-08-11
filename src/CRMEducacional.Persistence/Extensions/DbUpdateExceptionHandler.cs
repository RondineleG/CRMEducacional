namespace CRMEducacional.Persistence.Extensions;

public static class DbUpdateExceptionHandler
{
    public static string HandleDbUpdateException(DbUpdateException exception)
    {
        var innerException = exception.InnerException;
        while (innerException?.InnerException != null)
        {
            innerException = innerException.InnerException;
        }

        var errorMessage = innerException?.Message ?? exception.Message;

        if (errorMessage.Contains("duplicate key"))
        {
            return "Violação de restrição única. Já existe um registro com os mesmos dados únicos.";
        }
        else if (errorMessage.Contains("The INSERT statement conflicted with the FOREIGN KEY constraint"))
        {
            return "Violação de chave estrangeira. O registro referenciado não existe.";
        }
        else if (errorMessage.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
        {
            return "Não é possível excluir este registro porque ele está sendo referenciado por outros registros.";
        }
        else if (errorMessage.Contains("Invalid column name"))
        {
            return "Erro de esquema do banco de dados. Uma coluna especificada não existe.";
        }
        else if (errorMessage.Contains("String or binary data would be truncated"))
        {
            return "Os dados fornecidos são muito longos para o campo correspondente no banco de dados.";
        }
        else if (errorMessage.Contains("Cannot insert the value NULL into column"))
        {
            return "Tentativa de inserir um valor nulo em um campo que não permite nulos.";
        }

        return $"Erro ao atualizar o banco de dados: {errorMessage}";
    }
}