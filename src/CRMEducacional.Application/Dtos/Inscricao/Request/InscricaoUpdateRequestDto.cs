namespace CRMEducacional.Application.Dtos.Inscricao.Request;

public class InscricaoUpdateRequestDto
{
    public int Id { get; set; }

    public InscricaoRequestDto Inscricao { get; set; } = new InscricaoRequestDto();
}