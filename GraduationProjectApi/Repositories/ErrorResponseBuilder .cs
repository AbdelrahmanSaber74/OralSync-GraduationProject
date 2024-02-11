using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharedClassLibrary.Contracts;

public class ErrorResponseBuilder : IErrorResponseBuilder
{
   
    Task IErrorResponseBuilder.BuildBadRequestAsync(int statusCode, string message)
    {

        IActionResult result = new BadRequestObjectResult(new { StatusCode = statusCode, Message = message });
        return Task.FromResult<IActionResult>(result);
    }

    Task IErrorResponseBuilder.BuildNotFoundAsync(int statusCode, string message)
    {
        IActionResult result = new NotFoundObjectResult(new { StatusCode = statusCode, Message = message });
        return Task.FromResult<IActionResult>(result);
    }
}
