namespace CRMEducacional.Application.Dtos.Lead.Request;

/// <summary>
/// DTO para receber os dados de um Lead na cria��o ou atualiza��o.
/// </summary>
public class LeadRequestDto
{
    /// <summary>
    /// Construtor padr�o inicializando as propriedades com valores padr�o.
    /// </summary>
    public LeadRequestDto()
    {
        Nome = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        Cpf = string.Empty;
    }

    /// <summary>
    /// CPF do Lead.
    /// </summary>
    /// <example>686.420.180-54</example>
    [Required(ErrorMessage = "O CPF � obrigat�rio.")]
    public string Cpf { get; set; }

    /// <summary>
    /// Email do Lead.
    /// </summary>
    /// <example>exemplo@dominio.com</example>
    [Required(ErrorMessage = "O email � obrigat�rio.")]
    public string Email { get; set; }

    /// <summary>
    /// Nome do Lead.
    /// </summary>
    /// <example>Jo�o da Silva</example>
    [Required(ErrorMessage = "O nome � obrigat�rio.")]
    public string Nome { get; set; }

    /// <summary>
    /// Telefone do Lead.
    /// </summary>
    /// <example>(11) 99417-0751</example>
    [Required(ErrorMessage = "O telefone � obrigat�rio.")]
    public string Telefone { get; set; }

    /// <summary>
    /// Cria uma nova inst�ncia da entidade Lead com base nos dados do DTO.
    /// </summary>
    /// <param name="request">Dados do Lead a serem usados para a cria��o.</param>
    /// <returns>Uma inst�ncia da entidade <see cref="Core.Entities.Lead" />.</returns>
    public static Core.Entities.Lead Create(LeadRequestDto request)
    {
        return new Core.Entities.Lead(request.Nome, request.Email, request.Telefone, request.Cpf);
    }

    /// <summary>
    /// Atualiza uma inst�ncia existente da entidade Lead com base nos dados do DTO e no ID fornecido.
    /// </summary>
    /// <param name="request">Dados do Lead a serem usados para a atualiza��o.</param>
    /// <param name="id">ID do Lead a ser atualizado.</param>
    /// <returns>Uma inst�ncia atualizada da entidade <see cref="Core.Entities.Lead" />.</returns>
    public static Core.Entities.Lead Update(LeadRequestDto request, int id)
    {
        return new Core.Entities.Lead(id, request.Nome, request.Email, request.Telefone, request.Cpf);
    }
}