using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UniApiTest.Helpers;
using Refit;
using Test.API.Connector;
using Test.API.Models;

namespace UniApiTest.Handlers;

public class MessageHandler
{
    private readonly TelegramBotClient _client;

    public MessageHandler(TelegramBotClient client)
    {
        _client = client;
    }

    public async Task OnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        var messageText = "";
        

    if (!string.IsNullOrEmpty(message.Text)) messageText = message.Text;
        else if (!string.IsNullOrEmpty(message.Caption)) messageText = message.Caption;
        else return;
        
        var action = messageText.Split(' ')[0] switch
        {
            "/reply" => HandleUserMessageAsync(_client, message, cancellationToken),
            "/меню" => MainMenu(_client, message, cancellationToken),
            "/command" => SendCommandToApi(message),
            _ => null
        };
        
        if (action != null)
        {
            await action;
        }
    }

    private async Task HandleUserMessageAsync(ITelegramBotClient _client, Message message,
        CancellationToken cancellationToken)
    {
        var userMessage = message.Text;

        if (userMessage.Contains("Hello World"))
        {
            await _client.SendTextMessageAsync(message.Chat.Id, "Hello");
        }
        else if (userMessage.Contains("Ping"))
        {
            await _client.SendTextMessageAsync(message.Chat.Id, "Pong");
        }
        else if (userMessage.Contains(null))
        {
            await _client.SendTextMessageAsync(message.Chat.Id, "Enter Hello World/Ping");
        }
    }

    private async Task<Message> MainMenu(ITelegramBotClient _client, Message message,
        CancellationToken cancellationToken)
    {

        await _client.SendChatActionAsync(
            chatId: message.Chat.Id,
            chatAction: ChatAction.Typing,
            cancellationToken: cancellationToken);

        return await _client.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Основное меню",
            replyMarkup: KeyboardHelper.GetMainMenuKeyboard(message.From.Id),
            cancellationToken: cancellationToken);
    }

    private async Task<ProcessCommandModel> ProcessComandModel(Message message)
    {
        var splittedMsg = (string.IsNullOrEmpty(message.Text) ? message.Caption : message.Text).Split(' ').ToList();
        splittedMsg.RemoveAt(0);
        var text = string.Join(" ", splittedMsg);
        ProcessCommandModel commandModel = new ProcessCommandModel()
        {
            Text = text,
            UserId = message.From.Id
        };
        return commandModel;
    }
    
    private async Task<Message> SendCommandToApi(Message message)
    {
        try
        {
            var commandModel = await ProcessComandModel(message);
            var commandApi = RestService.For<ICommandApi>("http://localhost:5285/api/");
            var response = await commandApi.ProcessCommandAsync(commandModel);
            
            return await _client.SendTextMessageAsync(message.From.Id, response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending command to API: {ex}");
            throw;
        }
    }
}