namespace CRMEducacional.Core.Interfaces.Validations;

public interface IValidacaoService
{
    void AdicionarErroSeInvalido(CustomValidationResult resultado, string contexto, CustomResult response);

    void Validar<T>(
        T entidade,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade,
        CustomResult response
    );

    void Validar<T>(
        IEnumerable<T> entidades,
        Func<T, CustomValidationResult> funcValidacao,
        string nomeEntidade,
        CustomResult response
    );

    CustomValidationResult ValidarCEP(string cep);

    CustomValidationResult ValidarCPF(string cpf);

    CustomValidationResult ValidarDocumento(string documento);

    CustomValidationResult ValidarEmail(string email);

    CustomValidationResult ValidarInscricao(Inscricao inscricao);

    CustomValidationResult ValidarLead(Lead lead);

    CustomValidationResult ValidarOferta(Oferta oferta);

    CustomValidationResult ValidarProcessoSeletivo(ProcessoSeletivo processoSeletivo);

    CustomValidationResult ValidarRG(string rg);
}