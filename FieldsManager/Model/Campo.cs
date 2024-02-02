using FluentValidation;

namespace FieldsManager.Model;

public class Campo
{
    public Campo(string nomeCampo, string tipo, object? valorDefault, List<string> validacoes)
    {
        NomeCampo = nomeCampo;
        Tipo = tipo;
        ValorDefault = valorDefault;
        Validacoes = validacoes;
    }

    public string NomeCampo { get; set; }
    public string Tipo { get; set; }
    public object? ValorDefault { get; set; }
    public List<string> Validacoes { get; set; }
}

public class CampoValidator : AbstractValidator<Campo>
{
    public CampoValidator()
    {
        RuleFor(campo => campo.NomeCampo).NotEmpty();
        RuleFor(campo => campo.Tipo).NotEmpty();
        RuleFor(campo => campo.Validacoes).NotEmpty();
    }
}

public class DadoCampo
{
    public Campo Campo { get; }
    public object Valor { get; }

    public DadoCampo(Campo campo, object valor)
    {
        Campo = campo;
        Valor = valor;
    }
}