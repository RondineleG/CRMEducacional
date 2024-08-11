using CRMEducacional.Application.Services;
using CRMEducacional.Core.Interfaces.Repositories;

namespace CRMEducacional.API.Extensions;

public static class ServicesAndRepositoriesServicesExtensions
{
    public static void RegisterServicesAndRepositoriesServices(this IServiceCollection services)
    {
        #region Respositories

        services.AddScoped<IInscricaoRepositorio, InscricaoRepositorio>();
        services.AddScoped<ILeadRepositorio, LeadRepositorio>();
        services.AddScoped<IProcessoSeletivoRepositorio, ProcessoSeletivoRepositorio>();
        services.AddScoped<IOfertaRepositorio, OfertaRepositorio>();

        #endregion Respositories

        #region Services

        services.AddScoped<IInscricaoServico, InscricaoServico>();
        services.AddScoped<ILeadServico, LeadServico>();
        services.AddScoped<IProcessoSeletivoServico, ProcessoSeletivoServico>();
        services.AddScoped<IOfertaServico, OfertaServico>();

        #endregion Services

        #region Validations

        services.AddScoped<IValidacaoService, ValidacaoService>();

        #endregion Validations
    }
}