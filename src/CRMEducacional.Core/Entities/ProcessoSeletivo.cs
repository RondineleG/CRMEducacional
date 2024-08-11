namespace CRMEducacional.Core.Entities;

public class ProcessoSeletivo : BaseEntity
{
    [JsonConstructor]
    public ProcessoSeletivo()
    {
        Nome = string.Empty;
        DataInicio = DateTime.MinValue;
        DataTermino = DateTime.MinValue;
        Inscricoes = [];
    }

    // Construtor para inserção
    public ProcessoSeletivo(string nome, DateTime dataInicio, DateTime dataTermino)
    {
        Nome = nome;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        Inscricoes = [];
    }

    // Construtor para atualização
    public ProcessoSeletivo(int id, string nome, DateTime dataInicio, DateTime dataTermino) : base(id)
    {
        Nome = nome;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
        Inscricoes = [];
    }

    public DateTime DataInicio { get; private set; }

    public DateTime DataTermino { get; private set; }

    // Propriedade de navegação para Inscricoes
    public ICollection<Inscricao> Inscricoes { get; private set; }

    public string Nome { get; private set; }

    // Método para atualizar os dados do Processo Seletivo
    public void AtualizarProcessoSeletivo(string nome, DateTime dataInicio, DateTime dataTermino)
    {
        Nome = nome;
        DataInicio = dataInicio;
        DataTermino = dataTermino;
    }
}