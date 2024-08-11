namespace CRMEducacional.Application.Validation;

public sealed class ValidacaoService : IValidacaoService
{
    public void AdicionarErroSeInvalido(CustomValidationResult resultado, string contexto, CustomResult response)
    {
        if (!resultado.IsValid)
        {
            foreach (var erro in resultado.Errors)
            {
                response.AddEntityError(contexto, erro);
            }
        }
    }

    public void Validar<T>(T entidade, Func<T, CustomValidationResult> funcValidacao, string nomeEntidade, CustomResult response)
    {
        var resultado = funcValidacao(entidade);
        AdicionarErroSeInvalido(resultado, nomeEntidade, response);
    }

    public void Validar<T>(IEnumerable<T> entidades, Func<T, CustomValidationResult> funcValidacao, string nomeEntidade, CustomResult response)
    {
        foreach (var entidade in entidades)
        {
            if (entidade is null) continue;

            var resultado = funcValidacao(entidade);
            var idValor = ObterIdValor(entidade);
            AdicionarErroSeInvalido(resultado, $"{nomeEntidade} {idValor}", response);
        }
    }

    public CustomValidationResult ValidarCampos(string valor, string pattern, string nomeCampo)
    {
        var resultado = new CustomValidationResult();

        if (string.IsNullOrWhiteSpace(valor))
        {
            resultado.AddError($"{nomeCampo} não pode ser vazio ou nulo.", nomeCampo);
            return resultado;
        }

        if (!Regex.IsMatch(valor, pattern))
        {
            resultado.AddError($"{nomeCampo} inválido.", nomeCampo);
        }

        return resultado;
    }

    public CustomValidationResult ValidarCEP(string cep) => ValidarCampos(cep, RegexPatterns.CEP, "CEP");

    public CustomValidationResult ValidarCPF(string cpf)
    {
        cpf = cpf.Trim().Replace(".", string.Empty).Replace("-", string.Empty);
        var resultado = new CustomValidationResult();

        if (cpf.Length != 11 || cpf.All(c => c == cpf[0]) || !ValidarDigitosCPF(cpf))
        {
            resultado.AddError("CPF inválido.", "CPF");
        }

        return resultado;
    }

    public CustomValidationResult ValidarDataInicioFim(DateTime dataInicio, DateTime dataTermino)
    {
        var resultado = new CustomValidationResult();

        if (dataInicio == DateTime.MinValue)
        {
            resultado.AddError("DataInicio", "A data de início não pode ser a data mínima.");
        }

        if (dataTermino == DateTime.MinValue)
        {
            resultado.AddError("DataTermino", "A data de término não pode ser a data mínima.");
        }

        if (dataTermino < dataInicio)
        {
            resultado.AddError("DataTermino", "A data de término não pode ser anterior à data de início.");
        }

        return resultado;
    }

    public CustomValidationResult ValidarDescricao(string descricao)
    {
        var resultado = new CustomValidationResult();

        if (string.IsNullOrWhiteSpace(descricao))
        {
            resultado.AddError("Descricao", "A descrição não pode ser vazia.");
        }
        else if (descricao.Length > 500)
        {
            resultado.AddError("Descricao", "A descrição não pode ter mais que 500 caracteres.");
        }

        return resultado;
    }

    public CustomValidationResult ValidarDocumento(string documento)
    {
        documento = documento.Trim().Replace(".", string.Empty).Replace("-", string.Empty);

        if (documento.Length == 11)
        {
            return ValidarDocumentoComFormato(documento, 11, ValidarDigitosCPF, "CPF inválido.");
        }

        if (documento.Length == 14)
        {
            return ValidarDocumentoComFormato(documento, 14, ValidarDigitosCNPJ, "CNPJ inválido.");
        }

        var resultado = new CustomValidationResult();
        return resultado.AddError("Documento deve ter 11 dígitos (CPF) ou 14 dígitos (CNPJ).", "CpfCnpj");
    }

    public CustomValidationResult ValidarEmail(string email) => ValidarCampos(email, RegexPatterns.Email, "Email");

    public CustomValidationResult ValidarInscricao(Inscricao inscricao)
    {
        var resultado = new CustomValidationResult();

        if (string.IsNullOrWhiteSpace(inscricao.NumeroInscricao))
        {
            resultado.AddError("NumeroInscricao", "O número da inscrição não pode ser vazio.");
        }

        if (inscricao.Data == DateTime.MinValue)
        {
            resultado.AddError("Data", "A data da inscrição não pode ser a data mínima.");
        }

        resultado.Merge(ValidarStatus(inscricao.Status));

        return resultado;
    }

    public CustomValidationResult ValidarLead(Lead lead)
    {
        var resultado = new CustomValidationResult();

        resultado.Merge(ValidarCPF(lead.CPF));
        resultado.Merge(ValidarEmail(lead.Email));
        resultado.Merge(ValidarTelefone(lead.Telefone));
        resultado.Merge(ValidarNome(lead.Nome));

        return resultado;
    }

    public CustomValidationResult ValidarNome(string nome)
    {
        var resultado = new CustomValidationResult();

        if (string.IsNullOrWhiteSpace(nome))
        {
            resultado.AddError("Nome", "O nome não pode ser vazio.");
        }
        else if (nome.Length < 2)
        {
            resultado.AddError("Nome", "O nome deve ter pelo menos 2 caracteres.");
        }
        else if (nome.Length > 100)
        {
            resultado.AddError("Nome", "O nome não pode ter mais que 100 caracteres.");
        }

        return resultado;
    }

    public CustomValidationResult ValidarNome(string nome, int minLength, int maxLength)
    {
        var resultado = new CustomValidationResult();

        if (string.IsNullOrWhiteSpace(nome))
        {
            resultado.AddError("Nome", "O nome não pode ser vazio.");
        }
        else if (nome.Length < minLength)
        {
            resultado.AddError("Nome", $"O nome deve ter pelo menos {minLength} caracteres.");
        }
        else if (nome.Length > maxLength)
        {
            resultado.AddError("Nome", $"O nome não pode ter mais que {maxLength} caracteres.");
        }

        return resultado;
    }

    public CustomValidationResult ValidarOferta(Oferta oferta)
    {
        var resultado = new CustomValidationResult();

        if (string.IsNullOrWhiteSpace(oferta.Nome))
        {
            resultado.AddError("Nome", "O nome da oferta não pode ser vazio.");
        }

        resultado.Merge(ValidarDescricao(oferta.Descricao));
        resultado.Merge(ValidarVagasDisponiveis(oferta.VagasDisponiveis));

        return resultado;
    }

    public CustomValidationResult ValidarProcessoSeletivo(ProcessoSeletivo processoSeletivo)
    {
        var resultado = new CustomValidationResult();

        if (string.IsNullOrWhiteSpace(processoSeletivo.Nome))
        {
            resultado.AddError("Nome", "O nome do processo seletivo não pode ser vazio.");
        }

        resultado.Merge(ValidarDataInicioFim(processoSeletivo.DataInicio, processoSeletivo.DataTermino));
        return resultado;
    }

    public CustomValidationResult ValidarRG(string rg) => ValidarCampos(rg, RegexPatterns.RG, "RG");

    public CustomValidationResult ValidarStatus(string status)
    {
        var resultado = new CustomValidationResult();

        if (string.IsNullOrWhiteSpace(status))
        {
            resultado.AddError("Status", "O status não pode ser vazio.");
        }

        return resultado;
    }

    public CustomValidationResult ValidarTelefone(string telefone)
    {
        var resultado = new CustomValidationResult();

        if (string.IsNullOrWhiteSpace(telefone))
        {
            resultado.AddError("Telefone", "Telefone não pode ser vazio ou nulo.");
            return resultado;
        }

        telefone = new string(telefone.Where(char.IsDigit).ToArray());

        if (telefone.Length is not 10 and not 11)
        {
            resultado.AddError("Telefone", "Número de telefone deve ter 10 para fixo ou 11 dígitos para celular.");
            return resultado;
        }

        if (!int.TryParse(telefone.Substring(0, 2), out var ddd) || ddd < 11 || ddd > 99 || ddd % 10 == 0)
        {
            resultado.AddError("Telefone", "DDD inválido.");
            return resultado;
        }

        var pattern = telefone.Length == 11 && telefone[2] == '9'
            ? RegexPatterns.Celular
            : RegexPatterns.ResidencialComercial;

        resultado.Merge(ValidarCampos(telefone, pattern, "Telefone"));
        return resultado;
    }

    public CustomValidationResult ValidarVagasDisponiveis(int vagasDisponiveis)
    {
        var resultado = new CustomValidationResult();

        if (vagasDisponiveis <= 0)
        {
            resultado.AddError("VagasDisponiveis", "O número de vagas disponíveis deve ser maior que zero.");
        }

        return resultado;
    }

    private static string ObterIdValor<T>(T entidade)
    {
        if (entidade is null)
        {
            return string.Empty;
        }
        var propriedadeId = entidade.GetType().GetProperty("Id");
        return propriedadeId?.GetValue(entidade)?.ToString() ?? "Desconhecido";
    }

    private static bool ValidarDigitosCNPJ(string cnpj)
    {
        int[] multiplicadores1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicadores2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

        if (cnpj.Length != 14)
        {
            return false;
        }

        var tempCnpj = cnpj[..12];
        var soma = multiplicadores1.Select((m, i) => int.Parse(tempCnpj[i].ToString()) * m).Sum();
        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        tempCnpj += digito1;
        soma = multiplicadores2.Select((m, i) => int.Parse(tempCnpj[i].ToString()) * m).Sum();
        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return cnpj.EndsWith($"{digito1}{digito2}");
    }

    private static bool ValidarDigitosCPF(string cpf)
    {
        int[] multiplicadores1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] multiplicadores2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

        cpf = new string(cpf.Where(char.IsDigit).ToArray());

        if (cpf.Length != 11)
        {
            return false;
        }

        var tempCpf = cpf[..9];
        var soma = multiplicadores1.Select((m, i) => int.Parse(tempCpf[i].ToString()) * m).Sum();
        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito1;
        soma = multiplicadores2.Select((m, i) => int.Parse(tempCpf[i].ToString()) * m).Sum();
        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{digito1}{digito2}");
    }

    private static CustomValidationResult ValidarDocumentoComFormato(string documento, int tamanho, Func<string, bool> validarDigitos, string mensagemErro)
    {
        var resultado = new CustomValidationResult();

        if (documento.Length != tamanho || !validarDigitos(documento))
        {
            resultado.AddError(mensagemErro, "CpfCnpj");
        }
        return resultado;
    }
}