namespace CRMEducacional.Application.Dtos.Oferta.Request;

/// <summary>
/// DTO para receber os dados de uma Oferta na criação ou atualização.
/// </summary>
public class OfertaRequestDto
{
    /// <summary>
    /// Construtor padrão inicializando as propriedades com valores padrão.
    /// </summary>
    public OfertaRequestDto()
    {
        Nome = string.Empty;
        Descricao = string.Empty;
    }

    /// <summary>
    /// Descrição da Oferta.
    /// </summary>
    /// <example>Curso de programação avançada</example>
    [Required(ErrorMessage = "A descrição da oferta é obrigatória.")]
    public string Descricao { get; set; }

    /// <summary>
    /// Nome da Oferta.
    /// </summary>
    /// <example>Programação Avançada</example>
    [Required(ErrorMessage = "O nome da oferta é obrigatório.")]
    public string Nome { get; set; }

    /// <summary>
    /// Número de vagas disponíveis para a Oferta.
    /// </summary>
    /// <example>30</example>
    [Required(ErrorMessage = "Vagas disponíveis da oferta é obrigatório.")]
    public int VagasDisponiveis { get; set; }

    /// <summary>
    /// Cria uma nova instância da entidade Oferta com base nos dados do DTO.
    /// </summary>
    /// <param name="request">Dados da Oferta a serem usados para a criação.</param>
    /// <returns>Uma instância da entidade <see cref="Core.Entities.Oferta" />.</returns>
    public static Core.Entities.Oferta Create(OfertaRequestDto request)
    {
        return new Core.Entities.Oferta(
            request.Nome,
            request.Descricao,
            request.VagasDisponiveis
        );
    }

    /// <summary>
    /// Atualiza uma instância existente da entidade Oferta com base nos dados do DTO e no ID fornecido.
    /// </summary>
    /// <param name="request">Dados da Oferta a serem usados para a atualização.</param>
    /// <param name="id">ID da Oferta a ser atualizada.</param>
    /// <returns>Uma instância atualizada da entidade <see cref="Core.Entities.Oferta" />.</returns>
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