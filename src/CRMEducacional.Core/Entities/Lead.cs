namespace CRMEducacional.Core.Entities;

public class Lead : BaseEntity
{
    [JsonConstructor]
    public Lead()
    {
        Nome = string.Empty;
        Email = string.Empty;
        Telefone = string.Empty;
        CPF = string.Empty;
        Inscricoes = [];
    }

    public Lead(string nome, string email, string telefone, string cpf)
    {
        Nome = nome;
        Email = email;
        Telefone = RemoverCaracteresNaoNumericos(telefone);
        CPF = RemoverCaracteresNaoNumericos(cpf);
        Inscricoes = [];
    }

    public Lead(int id, string nome, string email, string telefone, string cpf) : base(id)
    {
        Nome = nome;
        Email = email;
        Telefone = RemoverCaracteresNaoNumericos(telefone);
        CPF = RemoverCaracteresNaoNumericos(cpf);
        Inscricoes = [];
    }

    public string CPF { get; private set; }

    public string Email { get; private set; }

    public ICollection<Inscricao> Inscricoes { get; private set; }

    public string Nome { get; private set; }

    public string Telefone { get; private set; }

    private static string RemoverCaracteresNaoNumericos(string valor) => new(valor.Where(char.IsDigit).ToArray());
}