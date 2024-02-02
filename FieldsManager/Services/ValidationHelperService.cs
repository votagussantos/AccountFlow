using FluentValidation;

namespace FieldsManager.Services;

public class ValidationHelperService : IValidationHelperService
{
    public List<string> Validate(string nomeCampo, string validacao, object valor)
    {
        var validator = new InlineValidator<object>();
        AdicionarRegra(validacao, validator, nomeCampo);
        var resultadoValidacao = validator.Validate(valor);
        return resultadoValidacao.Errors.Select(x => x.ErrorMessage).ToList();
    }

    public Dictionary<string, List<string>> AdicionarRegra(string validacao, InlineValidator<object> validator, string nomeCampo)
    {
        var problemasEDetalhes = new Dictionary<string, List<string>>();
        var partes = validacao.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
        var nomeMetodo = partes[0];
        string? parametro = null;

        if (partes.Length > 1)
        {
            parametro = partes[1];
        }

        switch (nomeMetodo.ToLower())
        {
            case "maximumlength":
                int maxLenght = int.Parse(parametro ?? "0f");
                if (maxLenght > 0)
                    validator.RuleFor(x => (string)x).MaximumLength(maxLenght).WithName(nomeCampo);
                break;
            case "minimumlength":
                int minLenght = int.Parse(parametro ?? "0f");
                if (minLenght > 0)
                    validator.RuleFor(x => (string)x).MinimumLength(minLenght).WithName(nomeCampo);
                break;
            case "notempty":
                validator.RuleFor(x => (string)x).NotEmpty().WithName(nomeCampo);
                break;
            case "notnull":
                validator.RuleFor(x => (string)x).NotNull().WithName(nomeCampo);
                break;
            case "emailaddress":
                validator.RuleFor(x => (string)x).EmailAddress().WithName(nomeCampo);
                break;
            case "notequal":
                validator.RuleFor(x => (string)x).NotEqual(parametro).WithName(nomeCampo);
                break;
            case "equal":
                validator.RuleFor(x => (string)x).Equal(parametro).WithName(nomeCampo);
                break;
            case "lessthan":
                validator.RuleFor(x => (string)x).LessThan(parametro).WithName(nomeCampo);
                break;
            case "lessthanorequalto":
                validator.RuleFor(x => (string)x).LessThanOrEqualTo(parametro).WithName(nomeCampo);
                break;
            case "greaterthan":
                validator.RuleFor(x => (string)x).GreaterThan(parametro).WithName(nomeCampo);
                break;
            case "greaterthanorequalto":
                validator.RuleFor(x => (string)x).GreaterThanOrEqualTo(parametro).WithName(nomeCampo);
                break;
            case "regex":
                validator.RuleFor(x => (string)x).Matches(parametro).WithName(nomeCampo);
                break;
            default:
                throw new NotSupportedException($"Validação '{nomeMetodo}' não reconhecida.");
        }

        return problemasEDetalhes;
    }
}
