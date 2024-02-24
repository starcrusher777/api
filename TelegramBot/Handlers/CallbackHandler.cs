using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using UniApiTest.Helpers;

namespace UniApiTest.Handlers;

public class CallbackHandler
{
    private readonly TelegramBotClient _client;

    public CallbackHandler(TelegramBotClient client)
    {
        _client = client;
    }

    public async Task OnCallbackReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        var action = callbackQuery.Data.Split(' ')[0] switch
        {
            "mainMenu" => MainMenu(_client, callbackQuery),
            _ => HandleUnrecognizedCallback(_client, callbackQuery)
        };

        await action;
    }

    private async Task<Message> MainMenu(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        return await botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: "Основное меню",
            replyMarkup: KeyboardHelper.GetMainMenuKeyboard(callbackQuery.From.Id));
    }

    private async Task<Message> HandleUnrecognizedCallback(ITelegramBotClient botClient, CallbackQuery callbackQuery)
    {
        // Handle or log the fact that the callback data is not recognized.
        // You might want to inform the user or take other appropriate actions.

        return await botClient.SendTextMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            text: "Unrecognized callback data");
    }
}