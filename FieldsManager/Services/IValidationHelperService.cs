using FluentValidation;

namespace FieldsManager.Services;

public interface IValidationHelperService
{
    List<string> Validate(string nomeCampo, string validacao, object valor);
    Dictionary<string, List<string>> AdicionarRegra(string validacao, InlineValidator<object> validator, string nomeCampo);
}