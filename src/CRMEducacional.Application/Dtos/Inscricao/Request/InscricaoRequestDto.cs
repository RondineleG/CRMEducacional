namespace CRMEducacional.Application.Dtos.Inscricao.Request;

/// <summary>
/// DTO para receber os dados de uma Inscrição na criação ou atualização.
/// </summary>
public class InscricaoRequestDto
{
    /// <summary>
    /// Construtor padrão inicializando as propriedades com valores padrão.
    /// </summary>
    public InscricaoRequestDto()
    {
        NumeroInscricao = string.Empty;
        Data = DateTime.Now;
        Status = string.Empty;
    }

    /// <summary>
    /// Data da inscrição.
    /// </summary>
    /// <example>2024-08-09</example>
    [Required(ErrorMessage = "A Data da inscrição é obrigatória")]
    public DateTime Data { get; set; }

    /// <summary>
    /// ID do lead relacionado à inscrição.
    /// </summary>
    [Required(ErrorMessage = "O ID do lead é obrigatório")]
    public int LeadId { get; set; }

    /// <summary>
    /// Número da inscrição.
    /// </summary>
    /// <example>INS-2024-12345</example>
    [Required(ErrorMessage = "O número da inscrição é obrigatório")]
    public string NumeroInscricao { get; set; }

    /// <summary>
    /// ID da oferta relacionada à inscrição.
    /// </summary>
    [Required(ErrorMessage = "O ID da oferta é obrigatório")]
    public int OfertaId { get; set; }

    /// <summary>
    /// ID do processo seletivo relacionado à inscrição.
    /// </summary>
    [Required(ErrorMessage = "O ID do processo seletivo é obrigatório")]
    public int ProcessoSeletivoId { get; set; }

    /// <summary>
    /// Status da inscrição.
    /// </summary>
    /// <example>Pendente</example>
    [Required(ErrorMessage = "O status da inscrição é obrigatório")]
    public string Status { get; set; }

    /// <summary>
    /// Cria uma nova instância da entidade Inscrição com base nos dados do DTO.
    /// </summary>
    /// <param name="request">Dados da inscrição a serem usados para a criação.</param>
    /// <returns>Uma instância da entidade <see cref="Core.Entities.Inscricao" />.</returns>
    public static Core.Entities.Inscricao Create(InscricaoRequestDto request)
    {
        return new Core.Entities.Inscricao(
            request.NumeroInscricao,
            request.Data,
            request.Status,
            request.LeadId,
            request.ProcessoSeletivoId,
            request.OfertaId
        );
    }

    /// <summary>
    /// Atualiza uma instância existente da entidade Inscrição com base nos dados do DTO e no ID fornecido.
    /// </summary>
    /// <param name="request">Dados da inscrição a serem usados para a atualização.</param>
    /// <param name="id">ID da Inscrição a ser atualizada.</param>
    /// <returns>Uma instância atualizada da entidade <see cref="Core.Entities.Inscricao" />.</returns>
    public static Core.Entities.Inscricao Update(InscricaoRequestDto request, int id)
    {
        return new Core.Entities.Inscricao(
            id,
            request.NumeroInscricao,
            request.Data,
            request.Status,
            request.LeadId,
            request.ProcessoSeletivoId,
            request.OfertaId
        );
    }
}