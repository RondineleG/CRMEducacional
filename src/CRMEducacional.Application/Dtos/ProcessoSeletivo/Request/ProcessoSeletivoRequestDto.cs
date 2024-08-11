namespace CRMEducacional.Application.Dtos.ProcessoSeletivo.Request;

/// <summary>
/// DTO para receber os dados de um Processo Seletivo na criação ou atualização.
/// </summary>
public class ProcessoSeletivoRequestDto
{
    /// <summary>
    /// Construtor padrão inicializando as propriedades com valores padrão.
    /// </summary>
    public ProcessoSeletivoRequestDto()
    {
        Nome = string.Empty;
    }

    /// <summary>
    /// Data de início do Processo Seletivo.
    /// </summary>
    /// <example>2024-08-01</example>
    [Required(ErrorMessage = "A data de início é obrigatória.")]
    public DateTime DataInicio { get; set; }

    /// <summary>
    /// Data de término do Processo Seletivo.
    /// </summary>
    /// <example>2024-08-31</example>
    [Required(ErrorMessage = "A data de término é obrigatória.")]
    public DateTime DataTermino { get; set; }

    /// <summary>
    /// Nome do Processo Seletivo.
    /// </summary>
    /// <example>Processo Seletivo 2024</example>
    [Required(ErrorMessage = "O nome do processo seletivo é obrigatório.")]
    public string Nome { get; set; }

    /// <summary>
    /// Cria uma nova instância da entidade Processo Seletivo com base nos dados do DTO.
    /// </summary>
    /// <param name="request">Dados do Processo Seletivo a serem usados para a criação.</param>
    /// <returns>Uma instância da entidade <see cref="Core.Entities.ProcessoSeletivo" />.</returns>
    public static Core.Entities.ProcessoSeletivo Create(ProcessoSeletivoRequestDto request)
    {
        return new Core.Entities.ProcessoSeletivo(
            request.Nome,
            request.DataInicio,
            request.DataTermino
        );
    }

    /// <summary>
    /// Atualiza uma instância existente da entidade Processo Seletivo com base nos dados do DTO e
    /// no ID fornecido.
    /// </summary>
    /// <param name="request">Dados do Processo Seletivo a serem usados para a atualização.</param>
    /// <param name="id">ID do Processo Seletivo a ser atualizado.</param>
    /// <returns>
    /// Uma instância atualizada da entidade <see cref="Core.Entities.ProcessoSeletivo" />.
    /// </returns>
    public static Core.Entities.ProcessoSeletivo Update(ProcessoSeletivoRequestDto request, int id)
    {
        return new Core.Entities.ProcessoSeletivo(
            id,
            request.Nome,
            request.DataInicio,
            request.DataTermino
        );
    }
}