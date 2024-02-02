using FieldsManager.Model;
using Microsoft.AspNetCore.Mvc;

namespace FieldsManager.Services;

public interface IValidationFieldService
{
    Task SaveContract(ValidationContract contract);
    Task<List<Campo>> FindCogitntract(string key);
    Task<List<ProblemDetails>> ValidationRequest(dynamic? request);
    object GetPropertyValue(dynamic obj, string propertyName);
}
