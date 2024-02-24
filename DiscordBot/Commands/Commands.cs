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
    
    [SlashCommand("process", "enter command to process")]
    public async Task ProcessCommand(SocketUser user, SocketTextChannel textChannel, string text)
    {
        try
        {
            var command = text;
            var userId = user.Id.ToString();
            var userIdLong = long.Parse(userId);
            var channelId = GetChannelIdFromTextChannel(textChannel);
            var commandModel = new ProcessCommandModel()
            {
                Text = command,
                UserId = userIdLong
            };
            await RespondAsync($"**{command}**");
            await SendCommandToApi(commandModel, channelId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing command: {ex}");
            throw;
        }
    }

    private async Task SendCommandToApi(ProcessCommandModel commandModel, ulong channelId)
    {
        try
        {
            var commandApi = RestService.For<ICommandApi>("http://localhost:5285/api/");
            var commandJson = JsonConvert.SerializeObject(commandModel);
            var channel = _client.GetChannel(channelId) as ISocketMessageChannel;
            if (channel != null)
            {
                var response = await commandApi.ProcessCommandAsync(commandJson);
                
                await channel.SendMessageAsync(response);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending command to API: {ex}");
            throw;
        }
    }
}