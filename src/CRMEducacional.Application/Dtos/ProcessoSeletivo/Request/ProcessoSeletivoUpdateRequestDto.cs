namespace CRMEducacional.Application.Dtos.ProcessoSeletivo.Request;

public class ProcessoSeletivoUpdateRequestDto
{
    public int Id { get; set; }

    public ProcessoSeletivoRequestDto ProcessoSeletivo { get; set; } = new ProcessoSeletivoRequestDto();
}