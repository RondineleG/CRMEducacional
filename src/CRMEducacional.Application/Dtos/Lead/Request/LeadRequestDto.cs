namespace CRMEducacional.Application.Dtos.Lead.Request;

/// <summary>
/// DTO para receber os dados de um Lead na criação ou atualização.
/// </summary>
public class LeadRequestDto
{
    /// <summary>
    /// Construtor padrão inicializando as propriedades com valores padrão.
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
    [Required(ErrorMessage = "O CPF é obrigatório.")]
    public string Cpf { get; set; }

    /// <summary>
    /// Email do Lead.
    /// </summary>
    /// <example>exemplo@dominio.com</example>
    [Required(ErrorMessage = "O email é obrigatório.")]
    public string Email { get; set; }

    /// <summary>
    /// Nome do Lead.
    /// </summary>
    /// <example>João da Silva</example>
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; }

    /// <summary>
    /// Telefone do Lead.
    /// </summary>
    /// <example>(11) 99417-0751</example>
    [Required(ErrorMessage = "O telefone é obrigatório.")]
    public string Telefone { get; set; }

    /// <summary>
    /// Cria uma nova instância da entidade Lead com base nos dados do DTO.
    /// </summary>
    /// <param name="request">Dados do Lead a serem usados para a criação.</param>
    /// <returns>Uma instância da entidade <see cref="Core.Entities.Lead" />.</returns>
    public static Core.Entities.Lead Create(LeadRequestDto request)
    {
        return new Core.Entities.Lead(request.Nome, request.Email, request.Telefone, request.Cpf);
    }

    /// <summary>
    /// Atualiza uma instância existente da entidade Lead com base nos dados do DTO e no ID fornecido.
    /// </summary>
    /// <param name="request">Dados do Lead a serem usados para a atualização.</param>
    /// <param name="id">ID do Lead a ser atualizado.</param>
    /// <returns>Uma instância atualizada da entidade <see cref="Core.Entities.Lead" />.</returns>
    public static Core.Entities.Lead Update(LeadRequestDto request, int id)
    {
        return new Core.Entities.Lead(id, request.Nome, request.Email, request.Telefone, request.Cpf);
    }
}