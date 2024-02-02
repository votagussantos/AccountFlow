namespace FieldsManager.Model;

public class ValidationContract
{
    public ValidationContract(string key, List<Campo> campos)
    {
        Key = key;
        Campos = campos;
    }

    public string Key { get; set; }
    public List<Campo> Campos { get; set; }
}