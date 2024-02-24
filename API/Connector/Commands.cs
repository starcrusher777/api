namespace Test.API.Connector;

public class Commands
{
    [Headers("Accept: text/plain, application/json, text/json")]
    [Post("/CreateUser")]
    Task<UserModel> CreateUser([Query] string username, [Query] long? cafeId, [Query] string password,
        [Query] string firstName, [Query] string middleName, [Query] string lastName,
        [Query] System.DateTimeOffset? dateBirth, [Query] string phone, [Query] string email, [Query] long? discordId,
        [Query] long? telegramId, [Query] long? vkontakteId, [Query] long? steam, [Query] string instagram);
}