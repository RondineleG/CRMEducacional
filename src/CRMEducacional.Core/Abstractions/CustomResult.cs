namespace CRMEducacional.Core.Abstractions;

/// <summary>
/// Representa o resultado de uma operação, contendo status, mensagens de erro e outras informações relevantes.
/// </summary>
public class CustomResult : ICustomResultValidations, ICustomResultError, IRequestEntityWarning
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="CustomResult"/> com o status de sucesso.
    /// </summary>
    public CustomResult()
    {
        Status = ECustomResultStatus.Success;
    }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="CustomResult"/> com uma mensagem de erro.
    /// </summary>
    /// <param name="message">A mensagem de erro.</param>
    public CustomResult(string message)
    {
        AddError(message);
    }

    /// <summary>
    /// Inicializa uma nova instância de <see cref="CustomResult"/> com um ID e uma mensagem.
    /// </summary>
    /// <param name="id">O ID associado ao resultado.</param>
    /// <param name="message">A mensagem associada ao resultado.</param>
    public CustomResult(string id, string message)
    {
        Id = id;
        Message = message;
    }

    /// <summary>
    /// Obtém ou define a data e hora em que o resultado foi gerado.
    /// </summary>
    public DateTime Date { get; set; } = DateTime.Now;

    /// <summary>
    /// Obtém ou define um dicionário de erros de entidade, onde a chave é o nome da entidade e o valor é uma lista de erros associados.
    /// </summary>
    public Dictionary<string, List<string>> EntityErrors { get; set; } = [];

    /// <summary>
    /// Obtém ou define um aviso de entidade, se aplicável.
    /// </summary>
    public EntityWarning? EntityWarning { get; protected init; }

    /// <summary>
    /// Obtém ou define um erro geral, se aplicável.
    /// </summary>
    public Error? Error { get; protected init; }

    /// <summary>
    /// Obtém ou define uma lista de erros gerais.
    /// </summary>
    public List<string> GeneralErrors { get; set; } = [];

    /// <summary>
    /// Obtém ou define o ID associado ao resultado.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define a mensagem associada ao resultado.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Obtém ou define o status do resultado da operação.
    /// </summary>
    public ECustomResultStatus Status { get; set; }

    /// <summary>
    /// Obtém ou define uma coleção de validações aplicadas ao resultado.
    /// </summary>
    public IEnumerable<Validation> Validations { get; protected init; } = Enumerable.Empty<Validation>();

    /// <summary>
    /// Cria um novo resultado indicando que a entidade já existe.
    /// </summary>
    /// <param name="entity">O nome da entidade.</param>
    /// <param name="id">O ID da entidade.</param>
    /// <param name="description">A descrição do aviso.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.EntityAlreadyExists"/>.</returns>
    public static CustomResult EntityAlreadyExists(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityAlreadyExists,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    /// <summary>
    /// Cria um novo resultado indicando que a entidade tem um erro.
    /// </summary>
    /// <param name="entity">O nome da entidade.</param>
    /// <param name="id">O ID da entidade.</param>
    /// <param name="description">A descrição do erro.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.EntityHasError"/>.</returns>
    public static CustomResult EntityHasError(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityHasError,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    /// <summary>
    /// Cria um novo resultado indicando que a entidade não foi encontrada.
    /// </summary>
    /// <param name="entity">O nome da entidade.</param>
    /// <param name="id">O ID da entidade.</param>
    /// <param name="description">A descrição do erro.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.EntityNotFound"/>.</returns>
    public static CustomResult EntityNotFound(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityNotFound,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    /// <summary>
    /// Cria um novo resultado de sucesso.
    /// </summary>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.Success"/>.</returns>
    public static CustomResult Success() => new CustomResult { Status = ECustomResultStatus.Success };

    /// <summary>
    /// Cria um novo resultado com um erro geral.
    /// </summary>
    /// <param name="message">A mensagem de erro.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.HasError"/>.</returns>
    public static CustomResult WithError(string message)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            Error = new Error(message)
        };

    /// <summary>
    /// Cria um novo resultado com um erro geral a partir de uma exceção.
    /// </summary>
    /// <param name="exception">A exceção que gerou o erro.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.HasError"/>.</returns>
    public static CustomResult WithError(Exception exception) => WithError(exception.Message);

    /// <summary>
    /// Cria um novo resultado com uma lista de erros gerais.
    /// </summary>
    /// <param name="generalErrors">A lista de erros gerais.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.HasError"/>.</returns>
    public static CustomResult WithError(List<string> generalErrors)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            GeneralErrors = generalErrors
        };

    /// <summary>
    /// Cria um novo resultado com erros de entidade.
    /// </summary>
    /// <param name="entityErrors">Um dicionário contendo erros de entidade.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.EntityHasError"/>.</returns>
    public static CustomResult WithError(Dictionary<string, List<string>> entityErrors)
        => new()
        {
            Status = ECustomResultStatus.EntityHasError,
            EntityErrors = entityErrors
        };

    /// <summary>
    /// Cria um novo resultado com um erro específico.
    /// </summary>
    /// <param name="error">O erro específico.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.HasError"/>.</returns>
    public static CustomResult WithError(Error error)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            Error = error
        };

    /// <summary>
    /// Cria um novo resultado indicando que não há conteúdo disponível.
    /// </summary>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.NoContent"/>.</returns>
    public static CustomResult WithNoContent() => new CustomResult { Status = ECustomResultStatus.NoContent };

    /// <summary>
    /// Cria um novo resultado com validações específicas.
    /// </summary>
    /// <param name="validations">As validações a serem aplicadas.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.HasValidation"/>.</returns>
    public static CustomResult WithValidations(params Validation[] validations)
        => new()
        {
            Status = ECustomResultStatus.HasValidation,
            Validations = validations
        };

    /// <summary>
    /// Cria um novo resultado com uma coleção de validações.
    /// </summary>
    /// <param name="validations">A coleção de validações.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.HasValidation"/>.</returns>
    public static CustomResult WithValidations(IEnumerable<Validation> validations)
        => WithValidations(validations.ToArray());

    /// <summary>
    /// Cria um novo resultado com uma validação específica.
    /// </summary>
    /// <param name="propertyName">O nome da propriedade que falhou na validação.</param>
    /// <param name="description">A descrição do erro de validação.</param>
    /// <returns>Um novo <see cref="CustomResult"/> com o status <see cref="ECustomResultStatus.HasValidation"/>.</returns>
    public static CustomResult WithValidations(string propertyName, string description)
        => WithValidations(new Validation(propertyName, description));

    /// <summary>
    /// Adiciona um erro de entidade ao resultado.
    /// </summary>
    /// <param name="entity">O nome da entidade.</param>
    /// <param name="message">A mensagem de erro.</param>
    public void AddEntityError(string entity, string message)
    {
        Status = ECustomResultStatus.EntityHasError;
        if (!EntityErrors.TryGetValue(entity, out var value))
        {
            value = new List<string>();
            EntityErrors[entity] = value;
        }
        value.Add(message);
    }

    /// <summary>
    /// Adiciona um erro geral ao resultado.
    /// </summary>
    /// <param name="message">A mensagem de erro.</param>
    public void AddError(string message)
    {
        Status = ECustomResultStatus.HasError;
        GeneralErrors.Add(message);
    }
}

/// <summary>
/// Representa o resultado de uma operação contendo dados específicos do tipo <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">O tipo de dados retornado pela operação.</typeparam>
public class CustomResult<T> : CustomResult, ICustomResult<T>
{
    /// <summary>
    /// Obtém os dados retornados pela operação.
    /// </summary>
    public T? Data { get; private init; }

    /// <summary>
    /// Cria um novo resultado indicando que a entidade já existe.
    /// </summary>
    /// <param name="entity">O nome da entidade.</param>
    /// <param name="id">O ID da entidade.</param>
    /// <param name="description">A descrição do aviso.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.EntityAlreadyExists"/>.</returns>
    public new static CustomResult<T> EntityAlreadyExists(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityAlreadyExists,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    /// <summary>
    /// Cria um novo resultado indicando que a entidade tem um erro.
    /// </summary>
    /// <param name="entity">O nome da entidade.</param>
    /// <param name="id">O ID da entidade.</param>
    /// <param name="description">A descrição do erro.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.EntityHasError"/>.</returns>
    public new static CustomResult<T> EntityHasError(string entity, object id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityHasError,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    /// <summary>
    /// Cria um novo resultado indicando que a entidade não foi encontrada.
    /// </summary>
    /// <param name="entity">O nome da entidade.</param>
    /// <param name="id">O ID da entidade.</param>
    /// <param name="description">A descrição do erro.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.EntityNotFound"/>.</returns>
    public new static CustomResult<T> EntityNotFound(string entity, object? id, string description)
        => new()
        {
            Status = ECustomResultStatus.EntityNotFound,
            EntityWarning = new EntityWarning(entity, id, description)
        };

    /// <summary>
    /// Implicitamente converte os dados em um resultado de sucesso.
    /// </summary>
    /// <param name="data">Os dados retornados pela operação.</param>
    public static implicit operator CustomResult<T>(T data) => Success(data);

    /// <summary>
    /// Implicitamente converte uma exceção em um resultado de erro.
    /// </summary>
    /// <param name="ex">A exceção a ser convertida.</param>
    public static implicit operator CustomResult<T>(Exception ex) => WithError(ex);

    /// <summary>
    /// Implicitamente converte validações em um resultado de erro.
    /// </summary>
    /// <param name="validations">As validações a serem aplicadas.</param>
    public static implicit operator CustomResult<T>(Validation[] validations) => WithValidations(validations);

    /// <summary>
    /// Implicitamente converte uma validação em um resultado de erro.
    /// </summary>
    /// <param name="validation">A validação a ser aplicada.</param>
    public static implicit operator CustomResult<T>(Validation validation) => WithValidations(validation);

    /// <summary>
    /// Cria um novo resultado de sucesso com os dados fornecidos.
    /// </summary>
    /// <param name="data">Os dados retornados pela operação.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.Success"/>.</returns>
    public static CustomResult<T> Success(T data)
        => new()
        {
            Data = data,
            Status = ECustomResultStatus.Success
        };

    /// <summary>
    /// Cria um novo resultado de erro com uma mensagem específica.
    /// </summary>
    /// <param name="message">A mensagem de erro.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.HasError"/>.</returns>
    public new static CustomResult<T> WithError(string message)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            Error = new Error(message)
        };

    /// <summary>
    /// Cria um novo resultado de erro a partir de uma exceção.
    /// </summary>
    /// <param name="exception">A exceção que gerou o erro.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.HasError"/>.</returns>
    public new static CustomResult<T> WithError(Exception exception) => WithError(exception.Message);

    /// <summary>
    /// Cria um novo resultado de erro com uma lista de erros gerais.
    /// </summary>
    /// <param name="generalErrors">A lista de erros gerais.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.HasError"/>.</returns>
    public new static CustomResult<T> WithError(List<string> generalErrors)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            GeneralErrors = generalErrors
        };

    /// <summary>
    /// Cria um novo resultado de erro com erros de entidade.
    /// </summary>
    /// <param name="entityErrors">Um dicionário contendo erros de entidade.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.EntityHasError"/>.</returns>
    public new static CustomResult<T> WithError(Dictionary<string, List<string>> entityErrors)
        => new()
        {
            Status = ECustomResultStatus.EntityHasError,
            EntityErrors = entityErrors
        };

    /// <summary>
    /// Cria um novo resultado de erro com um erro específico.
    /// </summary>
    /// <param name="error">O erro específico.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.HasError"/>.</returns>
    public new static CustomResult<T> WithError(Error error)
        => new()
        {
            Status = ECustomResultStatus.HasError,
            Error = error
        };

    /// <summary>
    /// Cria um novo resultado indicando que não há conteúdo disponível.
    /// </summary>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.NoContent"/>.</returns>
    public new static CustomResult<T> WithNoContent()
        => new()
        {
            Status = ECustomResultStatus.NoContent
        };

    /// <summary>
    /// Cria um novo resultado com validações específicas.
    /// </summary>
    /// <param name="validations">As validações a serem aplicadas.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.HasValidation"/>.</returns>
    public new static CustomResult<T> WithValidations(params Validation[] validations)
        => new()
        {
            Status = ECustomResultStatus.HasValidation,
            Validations = validations
        };

    /// <summary>
    /// Cria um novo resultado com uma validação específica.
    /// </summary>
    /// <param name="propertyName">O nome da propriedade que falhou na validação.</param>
    /// <param name="description">A descrição do erro de validação.</param>
    /// <returns>Um novo <see cref="CustomResult{T}"/> com o status <see cref="ECustomResultStatus.HasValidation"/>.</returns>
    public new static CustomResult<T> WithValidations(string propertyName, string description)
        => WithValidations(new Validation(propertyName, description));
}

/// <summary>
/// Interface para representar o resultado de uma operação.
/// </summary>
public interface ICustomResult
{
    /// <summary>
    /// Obtém o status do resultado da operação.
    /// </summary>
    ECustomResultStatus Status { get; }
}

/// <summary>
/// Interface para representar o resultado de uma operação que retorna dados do tipo <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">O tipo de dados retornado pela operação.</typeparam>
public interface ICustomResult<out T> : ICustomResult
{
    /// <summary>
    /// Obtém os dados retornados pela operação.
    /// </summary>
    T? Data { get; }
}

/// <summary>
/// Interface para representar um resultado de operação que pode conter um erro.
/// </summary>
public interface ICustomResultError : ICustomResult
{
    /// <summary>
    /// Obtém o erro associado ao resultado, se houver.
    /// </summary>
    Error? Error { get; }
}

/// <summary>
/// Interface para representar um resultado de operação que pode conter validações.
/// </summary>
public interface ICustomResultValidations : ICustomResult
{
    /// <summary>
    /// Obtém a coleção de validações aplicadas ao resultado.
    /// </summary>
    IEnumerable<Validation> Validations { get; }
}

/// <summary>
/// Interface para representar um resultado de operação que pode conter um aviso de entidade.
/// </summary>
public interface IRequestEntityWarning : ICustomResult
{
    /// <summary>
    /// Obtém o aviso de entidade associado ao resultado, se houver.
    /// </summary>
    EntityWarning? EntityWarning { get; }
}

/// <summary>
/// Enum que define os possíveis status de um resultado de operação.
/// </summary>
public enum ECustomResultStatus
{
    /// <summary>
    /// A operação foi bem-sucedida.
    /// </summary>
    Success,

    /// <summary>
    /// A operação contém validações.
    /// </summary>
    HasValidation,

    /// <summary>
    /// A operação contém um erro.
    /// </summary>
    HasError,

    /// <summary>
    /// A entidade não foi encontrada.
    /// </summary>
    EntityNotFound,

    /// <summary>
    /// A entidade contém um erro.
    /// </summary>
    EntityHasError,

    /// <summary>
    /// A entidade já existe.
    /// </summary>
    EntityAlreadyExists,

    /// <summary>
    /// Não há conteúdo disponível.
    /// </summary>
    NoContent
}

/// <summary>
/// Representa um erro de validação.
/// </summary>
/// <param name="PropertyName">O nome da propriedade que falhou na validação.</param>
/// <param name="Description">A descrição do erro de validação.</param>
public record Validation(string PropertyName, string Description);

/// <summary>
/// Representa um erro associado a uma operação.
/// </summary>
/// <param name="Description">A descrição do erro.</param>
public record Error(string Description);

/// <summary>
/// Representa um aviso associado a uma entidade.
/// </summary>
/// <param name="Name">O nome da entidade.</param>
/// <param name="Id">O ID da entidade.</param>
/// <param name="Message">A mensagem do aviso.</param>
public record EntityWarning(string Name, object? Id, string Message);
