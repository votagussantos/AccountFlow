using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using FieldsManager.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FieldsManager.Services;

public class ValidationFieldService : IValidationFieldService
{
    private readonly AmazonDynamoDBClient _dynamoDBClient;
    private readonly IValidationHelperService _validationHelperService;

    public ValidationFieldService(IValidationHelperService validationHelperService)
    {
        _validationHelperService = validationHelperService;
        _dynamoDBClient = new AmazonDynamoDBClient(
            new AmazonDynamoDBConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.SAEast1
            }
        );
    }

    public ValidationFieldService(
        AmazonDynamoDBClient dynamoDBClient,
        IValidationHelperService validationHelperService
    )
    {
        _dynamoDBClient = dynamoDBClient;
        _validationHelperService = validationHelperService;
    }

    public async Task<List<ProblemDetails>> ValidationRequest(dynamic? request)
    {
        try
        {
            var key = GetPropertyValue(request, "Key");
            if (key == null)
                return new();

            var contract = await FindContract(key);

            var problemas = new List<ProblemDetails>();

            foreach (var fields in contract)
            {
                var valor = GetPropertyValue(request, fields.NomeCampo);
                if (valor != null)
                {
                    foreach (var validacao in fields.Validacoes)
                    {
                        List<string> resultado = _validationHelperService.Validate(fields.NomeCampo, validacao, valor);

                        if (resultado.Any())
                        {
                            foreach (var mensagem in resultado)
                            {
                                problemas.Add(
                                    new ProblemDetails
                                    {
                                        Detail = mensagem,
                                        Status = 400,
                                    }
                                );
                            }
                        }
                    }
                }
            }

            return problemas;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public object GetPropertyValue(dynamic obj, string propertyName)
    {
        return obj[propertyName];
    }

    public async Task SaveContract(ValidationContract validation)
    {
        try
        {
            var table = Table.LoadTable(_dynamoDBClient, "DecisorManager");

            var document = Document.FromJson(JsonConvert.SerializeObject(validation));
            await table.PutItemAsync(document);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Campo>> FindContract(string key)
    {
        var table = Table.LoadTable(_dynamoDBClient, "DecisorManager");
        var document = await table.GetItemAsync(key);

        if (document == null || !document.ContainsKey("Campos"))
            return new();

        var camposJson = document["Campos"].AsListOfDocument().ToJson();
        return JsonConvert.DeserializeObject<List<Campo>>(camposJson) ?? new();
    }
}
