using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bots.Http;
using UniApiTest.Handlers;
using static Telegram.Bot.Types.Enums.UpdateType;
using CallbackQuery = Telegram.Bot.Types.CallbackQuery;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Args;

namespace UniApiTest
{
    internal class Program
    {
        private static TelegramBotClient _botClient;
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var messageHandler = new MessageHandler(_botClient);
            var callbackHandler = new CallbackHandler(_botClient);

            var handler = update switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                { Message: { } message }                       => messageHandler.OnMessageReceived(message, cancellationToken),
                //{ EditedMessage: { } message }                 => BotOnMessageReceived(message, cancellationToken),
                { CallbackQuery: { } callbackQuery }           => callbackHandler.OnCallbackReceived(callbackQuery, cancellationToken),
                _                                              => Unknown()
            };

            try
            {
                await handler;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        private static Task Unknown()
        {
            return Task.CompletedTask;
        }
        

        public static async Task Main(string[] args)
        {
            var botToken = "token";
            
            _botClient = new TelegramBotClient(botToken);
            
            Console.WriteLine("Starting..... " + _botClient.GetMeAsync().Result.FirstName);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = {  }, 
            };
            
            _botClient.StartReceiving(
                (client, update, cancellationToken) => HandleUpdateAsync(client, update, cancellationToken),
                (client, exception, arg3) => HandleErrorAsync(client, exception, arg3),
                receiverOptions,
                cancellationToken
            );

            Console.ReadLine();
            
        }
        private static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }
    }
}
