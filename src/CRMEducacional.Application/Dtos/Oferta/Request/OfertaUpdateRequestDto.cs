using CRMEducacional.Application.Dtos.Oferta.Request;

namespace CRMEducacional.Application.Dtos.Inscricao.Request;

public class OfertaUpdateRequestDto
{
    public int Id { get; set; }

    public OfertaRequestDto Oferta { get; set; } = new OfertaRequestDto();
}