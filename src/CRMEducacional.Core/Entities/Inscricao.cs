namespace CRMEducacional.Core.Entities;

public class Inscricao : BaseEntity
{
    [JsonConstructor]
    public Inscricao()
    {
        NumeroInscricao = string.Empty;
        Data = DateTime.Now;
        Status = string.Empty;
        Lead = new Lead();
        ProcessoSeletivo = new ProcessoSeletivo();
        Oferta = new Oferta();
    }

    public Inscricao(string numeroInscricao, DateTime data, string status, int leadId, int processoSeletivoId, int ofertaId)
    {
        NumeroInscricao = numeroInscricao;
        Data = data;
        Status = status;
        LeadId = leadId;
        ProcessoSeletivoId = processoSeletivoId;
        OfertaId = ofertaId;
        Lead = new Lead();
        ProcessoSeletivo = new ProcessoSeletivo();
        Oferta = new Oferta();
    }

    public Inscricao(int id, string numeroInscricao, DateTime data, string status, int leadId, int processoSeletivoId, int ofertaId) : base(id)
    {
        NumeroInscricao = numeroInscricao;
        Data = data;
        Status = status;
        LeadId = leadId;
        ProcessoSeletivoId = processoSeletivoId;
        OfertaId = ofertaId;
        Lead = new Lead();
        ProcessoSeletivo = new ProcessoSeletivo();
        Oferta = new Oferta();
    }

    public DateTime Data { get; private set; }

    public Lead Lead { get; private set; }

    public int LeadId { get; private set; }

    public string NumeroInscricao { get; private set; }

    public Oferta Oferta { get; private set; }

    public int OfertaId { get; private set; }

    public ProcessoSeletivo ProcessoSeletivo { get; private set; }

    public int ProcessoSeletivoId { get; private set; }

    public string Status { get; private set; }

    public void AtualizarInscricao(string numeroInscricao, DateTime data, string status, int leadId, int processoSeletivoId, int ofertaId)
    {
        NumeroInscricao = numeroInscricao;
        Data = data;
        Status = status;
        LeadId = leadId;
        ProcessoSeletivoId = processoSeletivoId;
        OfertaId = ofertaId;
    }

    public void SetLead(Lead lead)
    {
        Lead = lead ?? throw new ArgumentNullException(nameof(lead));
        LeadId = lead.Id;
    }

    public void SetOferta(Oferta oferta)
    {
        Oferta = oferta ?? throw new ArgumentNullException(nameof(oferta));
        OfertaId = oferta.Id;
    }

    public void SetProcessoSeletivo(ProcessoSeletivo processoSeletivo)
    {
        ProcessoSeletivo = processoSeletivo ?? throw new ArgumentNullException(nameof(processoSeletivo));
        ProcessoSeletivoId = processoSeletivo.Id;
    }
}