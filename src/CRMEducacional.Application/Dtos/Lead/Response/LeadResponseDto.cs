namespace CRMEducacional.Application.Dtos.Lead.Response;

/// <summary>
/// DTO de resposta para a entidade Lead.
/// </summary>
public class LeadResponseDto
{
    /// <summary>
    /// Construtor padrão inicializando as propriedades com valores padrão.
    /// </summary>
    public LeadResponseDto()
    {
        CPF = string.Empty;
        Email = string.Empty;
        Nome = string.Empty;
        Telefone = string.Empty;
    }

    /// <summary>
    /// CPF do lead.
    /// </summary>
    /// <example>123.456.789-00</example>
    public string CPF { get; set; }

    /// <summary>
    /// Email do lead.
    /// </summary>
    /// <example>exemplo@dominio.com</example>
    public string Email { get; set; }

    /// <summary>
    /// ID do lead.
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>
    /// Nome do lead.
    /// </summary>
    /// <example>João da Silva</example>
    public string Nome { get; set; }

    /// <summary>
    /// Telefone do lead.
    /// </summary>
    /// <example>(11) 91234-5678</example>
    public string Telefone { get; set; }

    /// <summary>
    /// Converte uma entidade <see cref="Core.Entities.Lead" /> para um DTO de resposta <see
    /// cref="LeadResponseDto" />.
    /// </summary>
    /// <param name="lead">A entidade Lead a ser convertida.</param>
    /// <returns>Um objeto <see cref="LeadResponseDto" /> contendo os dados do lead.</returns>
    public static LeadResponseDto FromEntity(Core.Entities.Lead lead)
    {
        return new LeadResponseDto
        {
            Id = lead.Id,
            Nome = lead.Nome,
            Email = lead.Email,
            Telefone = lead.Telefone,
            CPF = lead.CPF
        };
    }
}