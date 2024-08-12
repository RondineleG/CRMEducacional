namespace CRMEducacional.Application.Dtos.Oferta.Request;

public class OfertaUpdateRequestDto
{
    public int Id { get; set; }

    public OfertaRequestDto Oferta { get; set; } = new OfertaRequestDto();
}