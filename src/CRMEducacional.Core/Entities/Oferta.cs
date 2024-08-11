namespace CRMEducacional.Core.Entities;

public class Oferta : BaseEntity
{
    [JsonConstructor]
    public Oferta()
    {
        Nome = string.Empty;
        Descricao = string.Empty;
        VagasDisponiveis = 1;
        Inscricoes = new List<Inscricao>();
    }

    // Construtor para inserção
    public Oferta(string nome, string descricao, int vagasDisponiveis)
    {
        Nome = nome;
        Descricao = descricao;
        VagasDisponiveis = vagasDisponiveis;
        Inscricoes = new List<Inscricao>();
    }

    // Construtor para atualização
    public Oferta(int id, string nome, string descricao, int vagasDisponiveis) : base(id)
    {
        Nome = nome;
        Descricao = descricao;
        VagasDisponiveis = vagasDisponiveis;
        Inscricoes = new List<Inscricao>();
    }

    public string Descricao { get; private set; }

    // Propriedade de navegação para Inscricoes
    public ICollection<Inscricao> Inscricoes { get; private set; }

    public string Nome { get; private set; }

    public int VagasDisponiveis { get; private set; }

    // Método para atualizar os dados da Oferta
    public void AtualizarOferta(string nome, string descricao, int vagasDisponiveis)
    {
        Nome = nome;
        Descricao = descricao;
        VagasDisponiveis = vagasDisponiveis;
    }
}