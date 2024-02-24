using Microsoft.AspNetCore.Mvc;
using Test.API.Models;

namespace Test.API.Controllers;

[Route("api/[controller]")]

public class ProcessCommandController : ControllerBase
{
    [HttpPost]
    public string ProcessCommandAsync([FromBody] ProcessCommandModel commandModel)
    {
        string response;

        switch (commandModel.Text.ToLower())
        {
            case "ping":
                response = "Pong";
                break;
            case "hello":
                response = "World";
                break;
            default:
                response = "Unknown command";
                break;
        }

        return response;
    }
}