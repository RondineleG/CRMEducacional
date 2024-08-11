namespace CRMEducacional.Application.Dtos.Inscricao.Response;

/// <summary>
/// DTO de resposta para a entidade Inscricao.
/// </summary>
public class InscricaoResponseDto
{
    /// <summary>
    /// Construtor padrão inicializando as propriedades com valores padrão.
    /// </summary>
    public InscricaoResponseDto()
    {
        NumeroInscricao = string.Empty;
        Status = string.Empty;
    }

    /// <summary>
    /// Data da inscrição.
    /// </summary>
    /// <example>2024-08-09</example>
    public DateTime Data { get; set; }

    /// <summary>
    /// ID da inscrição.
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// ID do lead associado.
    /// </summary>
    /// <example>10</example>
    public int LeadId { get; set; }

    /// <summary>
    /// Número da inscrição.
    /// </summary>
    /// <example>INS-2024-12345</example>
    public string NumeroInscricao { get; set; }

    /// <summary>
    /// ID da oferta associada.
    /// </summary>
    /// <example>5</example>
    public int OfertaId { get; set; }

    /// <summary>
    /// ID do processo seletivo associado.
    /// </summary>
    /// <example>3</example>
    public int ProcessoSeletivoId { get; set; }

    /// <summary>
    /// Status da inscrição.
    /// </summary>
    /// <example>Pendente</example>
    public string Status { get; set; }

    /// <summary>
    /// Converte uma entidade <see cref="Core.Entities.Inscricao" /> para um DTO de resposta <see
    /// cref="InscricaoResponseDto" />.
    /// </summary>
    /// <param name="inscricao">A entidade de inscrição a ser convertida.</param>
    /// <returns>Um objeto <see cref="InscricaoResponseDto" /> contendo os dados da inscrição.</returns>
    public static InscricaoResponseDto FromEntity(Core.Entities.Inscricao inscricao)
    {
        return new InscricaoResponseDto
        {
            Id = inscricao.Id,
            NumeroInscricao = inscricao.NumeroInscricao,
            Data = inscricao.Data,
            Status = inscricao.Status,
            LeadId = inscricao.LeadId,
            ProcessoSeletivoId = inscricao.ProcessoSeletivoId,
            OfertaId = inscricao.OfertaId
        };
    }
}