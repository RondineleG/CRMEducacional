namespace CRMEducacional.Application.Dtos.Oferta.Response;

/// <summary>
/// DTO de resposta para a entidade Oferta.
/// </summary>
public class OfertaResponseDto
{
    /// <summary>
    /// Construtor padrão inicializando as propriedades com valores padrão.
    /// </summary>
    public OfertaResponseDto()
    {
        Nome = string.Empty;
        Descricao = string.Empty;
    }

    /// <summary>
    /// Descrição da oferta.
    /// </summary>
    /// <example>Curso de Engenharia de Software</example>
    public string Descricao { get; set; }

    /// <summary>
    /// ID da oferta.
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Nome da oferta.
    /// </summary>
    /// <example>Engenharia de Software</example>
    public string Nome { get; set; }

    /// <summary>
    /// Número de vagas disponíveis para a oferta.
    /// </summary>
    /// <example>30</example>
    public int VagasDisponiveis { get; set; }

    /// <summary>
    /// Converte uma entidade <see cref="Core.Entities.Oferta" /> para um DTO de resposta <see
    /// cref="OfertaResponseDto" />.
    /// </summary>
    /// <param name="oferta">A entidade Oferta a ser convertida.</param>
    /// <returns>Um objeto <see cref="OfertaResponseDto" /> contendo os dados da oferta.</returns>
    public static OfertaResponseDto FromEntity(Core.Entities.Oferta oferta)
    {
        return new OfertaResponseDto
        {
            Id = oferta.Id,
            Nome = oferta.Nome,
            Descricao = oferta.Descricao,
            VagasDisponiveis = oferta.VagasDisponiveis
        };
    }
}