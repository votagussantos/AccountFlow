using FieldsManager.Model;
using FieldsManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FieldsManager.Controllers;

[ApiController]
[Route("[controller]")]
public class ValidationManagerController : ControllerBase
{
    private readonly IValidationFieldService _validationFieldService;

    public ValidationManagerController(IValidationFieldService validationFieldService)
    {
        _validationFieldService = validationFieldService;
    }

    [HttpPost("SaveContract")]
    public async Task<IActionResult> SaveContract([FromBody] ValidationContract validator)
    {
        await _validationFieldService.SaveContract(validator);
        return Ok();
    }

    [HttpPost("ValidationRequest")]
    public async Task<IActionResult> ValidationRequest([FromBody] Dictionary<string, string> request)
    {
        var result = await _validationFieldService.ValidationRequest(request);

        if (result.Any())
        {
            return new JsonResult(result) { StatusCode = 400 };
        }

        return Ok();
    }
}
