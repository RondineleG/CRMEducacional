namespace CRMEducacional.Application.Dtos.Lead.Request;

public class LeadUpdateRequestDto
{
    public int Id { get; set; }

    public LeadRequestDto Lead { get; set; } = new LeadRequestDto();
}