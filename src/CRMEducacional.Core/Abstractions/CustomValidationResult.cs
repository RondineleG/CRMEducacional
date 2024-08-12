using System.Text.Json;

namespace CRMEducacional.Core.Abstractions;

public sealed class CustomValidationResult
{
    private readonly List<Dictionary<string, string>> _errors = [];

    public IEnumerable<string> Errors => _errors.Select(e => JsonSerializer.Serialize(e));

    public bool IsValid => _errors.Count == 0;

    public CustomValidationResult AddError(string errorMessage, string fieldName = "")
    {
        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            var errorDetails = new Dictionary<string, string>
            {
                { "Campo", fieldName },
                { "Erro", errorMessage }
            };

            _errors.Add(errorDetails);
        }
        return this;
    }

    public CustomValidationResult AddErrorIf(bool condition, string errorMessage, string fieldName = "")
    {
        if (condition)
        {
            AddError(errorMessage, fieldName);
        }
        return this;
    }

    public CustomValidationResult Merge(CustomValidationResult result)
    {
        foreach (var error in result._errors)
        {
            _errors.Add(error);
        }
        return this;
    }

    public string ToJson()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        return JsonSerializer.Serialize(new { Errors = _errors }, options);
    }

    public string ToStringErrors()
    {
        return string.Join("; ", Errors);
    }

    public async Task ValidarEntidadeRelacionada<TEntidade>(
            int id,
    Func<int, Task<CustomResult<TEntidade>>> obterPorId,
    string mensagemErro,
    string nomeCampo,
    Action<TEntidade> setEntidade,
    CustomValidationResult resultadoValidacao)
    {
        var resultado = await obterPorId(id);
        if (resultado.Status != ECustomResultStatus.Success || resultado.Data == null)
        {
            resultadoValidacao.AddError(mensagemErro, nomeCampo);
        }
        else
        {
            setEntidade(resultado.Data);
        }
    }
}