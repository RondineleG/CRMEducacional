namespace CRMEducacional.Application.Dtos.Oferta.Request;

/// <summary>
/// DTO para receber os dados de uma Oferta na cria��o ou atualiza��o.
/// </summary>
public class OfertaRequestDto
{
    /// <summary>
    /// Construtor padr�o inicializando as propriedades com valores padr�o.
    /// </summary>
    public OfertaRequestDto()
    {
        Nome = string.Empty;
        Descricao = string.Empty;
    }

    /// <summary>
    /// Descri��o da Oferta.
    /// </summary>
    /// <example>Curso de programa��o avan�ada</example>
    [Required(ErrorMessage = "A descri��o da oferta � obrigat�ria.")]
    public string Descricao { get; set; }

    /// <summary>
    /// Nome da Oferta.
    /// </summary>
    /// <example>Programa��o Avan�ada</example>
    [Required(ErrorMessage = "O nome da oferta � obrigat�rio.")]
    public string Nome { get; set; }

    /// <summary>
    /// N�mero de vagas dispon�veis para a Oferta.
    /// </summary>
    /// <example>30</example>
    [Required(ErrorMessage = "Vagas dispon�veis da oferta � obrigat�rio.")]
    public int VagasDisponiveis { get; set; }

    /// <summary>
    /// Cria uma nova inst�ncia da entidade Oferta com base nos dados do DTO.
    /// </summary>
    /// <param name="request">Dados da Oferta a serem usados para a cria��o.</param>
    /// <returns>Uma inst�ncia da entidade <see cref="Core.Entities.Oferta" />.</returns>
    public static Core.Entities.Oferta Create(OfertaRequestDto request)
    {
        return new Core.Entities.Oferta(
            request.Nome,
            request.Descricao,
            request.VagasDisponiveis
        );
    }

    /// <summary>
    /// Atualiza uma inst�ncia existente da entidade Oferta com base nos dados do DTO e no ID fornecido.
    /// </summary>
    /// <param name="request">Dados da Oferta a serem usados para a atualiza��o.</param>
    /// <param name="id">ID da Oferta a ser atualizada.</param>
    /// <returns>Uma inst�ncia atualizada da entidade <see cref="Core.Entities.Oferta" />.</returns>
    public static Core.Entities.Oferta Update(OfertaRequestDto request, int id)
    {
        return new Core.Entities.Oferta(
            id,
            request.Nome,
            request.Descricao,
            request.VagasDisponiveis
        );
    }
}