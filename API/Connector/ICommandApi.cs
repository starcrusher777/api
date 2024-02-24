using Refit;
using Test.API.Models;

namespace Test.API.Connector;


public interface ICommandApi
{
    [Headers("Accept: text/plain, application/json, text/json")]
    [Post("/ProcessCommand")]
    Task<string> ProcessCommandAsync([Body] ProcessCommandModel commandModel);

    //Task<string> ProcessCommandAsync(ProcessCommandModel commandModel);
    Task<string> ProcessCommandAsync(string commandJson);
}