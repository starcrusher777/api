using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Services;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Refit;
using Test.API.Connector;
using Test.API.Models;

namespace DiscordBot.Commands;

public class ExampleCommands : InteractionModuleBase<SocketInteractionContext>
{
    // dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
    public InteractionService Commands { get; set; }
    private static CommandHandler _handler;
    private DiscordSocketClient _client;
    
    public ExampleCommands (CommandHandler handler)
    {
        _handler = handler;
    }
    
    private async Task MessageReceived(SocketMessage message)
    {
        ulong userId = message.Author.Id;
    }

    //command!
    [SlashCommand("8ball", "find your answer!")]
    public async Task EightBall(string question)
    {
        
        var replies = new List<string>();

        
        replies.Add("yes");
        replies.Add("no");
        replies.Add("maybe");
        replies.Add("hazzzzy....");

        // get the answer
        var answer = replies[new Random().Next(replies.Count - 1)];

        // reply with the answer
        await RespondAsync($"You asked: [**{question}**], and your answer is: [**{answer}**]");
    }
    
    [SlashCommand("test", "1234")]
    public async Task PingCommand(string text)
    {
        string? answer;
        
        if (text == "Ping")
        {
            answer = "Pong";
        }
        else
        {
            answer = "Unknown Command";
        }
        
        await RespondAsync($"**{answer}**");
    }
    
    public ulong GetChannelIdFromTextChannel(SocketTextChannel textChannel)
    {
        return textChannel.Id;
    }
    
    public ulong GetUserIdFromUser(SocketUser user)
    {
        return user.Id;
    }

    public async Task<ProcessCommandModel> ProcessCommand(SocketUser user, string text)
    {
        try
        {
            var command = text;
            var userId = user.Id.ToString();
            var userIdLong = ulong.Parse(userId);
            var commandModel = new ProcessCommandModel()
            {
                Text = command,
                UserId = userIdLong
            };
            return commandModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing command: {ex}");
            throw;
        }
        
        [SlashCommand("process", "enter command to process")]
        async Task SendCommandToApi(SocketUser user, SocketTextChannel textChannel, string text)
        {
            try
            {
                var commandModel = await ProcessCommand(user, text);
                var commandApi = RestService.For<ICommandApi>("http://localhost:5285/api/");
                var channel = GetChannelIdFromTextChannel(textChannel);
                if (channel != null)
                {
                    var response = await commandApi.ProcessCommandAsync(commandModel);

                    await RespondAsync($"{response}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending command to API: {ex}");
                throw;
            }
        }
    }
}