namespace CRMEducacional.Application.Dtos.ProcessoSeletivo.Response;

/// <summary>
/// DTO de resposta para a entidade Processo Seletivo.
/// </summary>
public class ProcessoSeletivoResponseDto
{
    /// <summary>
    /// Construtor padrão inicializando as propriedades com valores padrão.
    /// </summary>
    public ProcessoSeletivoResponseDto()
    {
        Nome = string.Empty;
    }

    /// <summary>
    /// Data de início do processo seletivo.
    /// </summary>
    /// <example>2024-08-01T00:00:00</example>
    public DateTime DataInicio { get; set; }

    /// <summary>
    /// Data de término do processo seletivo.
    /// </summary>
    /// <example>2024-08-15T00:00:00</example>
    public DateTime DataTermino { get; set; }

    /// <summary>
    /// ID do processo seletivo.
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Nome do processo seletivo.
    /// </summary>
    /// <example>Vestibular 2024</example>
    public string Nome { get; set; }

    /// <summary>
    /// Converte uma entidade <see cref="Core.Entities.ProcessoSeletivo" /> para um DTO de resposta
    /// <see cref="ProcessoSeletivoResponseDto" />.
    /// </summary>
    /// <param name="processoSeletivo">A entidade Processo Seletivo a ser convertida.</param>
    /// <returns>
    /// Um objeto <see cref="ProcessoSeletivoResponseDto" /> contendo os dados do processo seletivo.
    /// </returns>
    public static ProcessoSeletivoResponseDto FromEntity(Core.Entities.ProcessoSeletivo processoSeletivo)
    {
        return new ProcessoSeletivoResponseDto
        {
            Id = processoSeletivo.Id,
            Nome = processoSeletivo.Nome,
            DataInicio = processoSeletivo.DataInicio,
            DataTermino = processoSeletivo.DataTermino
        };
    }
}