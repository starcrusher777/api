using Refit;
using Test.API.Models;

namespace Test.API.Connector;


public interface ICommandApi
{
    [Headers("Accept: text/plain, application/json, text/json")]
    [Post("/ProcessCommand")]
    Task<string> ProcessCommandAsync<T>([Body] T commandModel);
}