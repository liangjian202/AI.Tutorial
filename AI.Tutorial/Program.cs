
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddChatClient(
    new OllamaChatClient(new Uri("http://localhost:11434"), "llama3"));

var app = builder.Build();


var chatClient = app.Services.GetRequiredService<IChatClient>();

var messages = new List<ChatMessage>
{
    new ChatMessage(ChatRole.User, "波多野结衣是谁？简单介绍一下她."),
    // new ChatMessage(ChatRole.Assistant, "Hi there! How can I help you today?"),
    // new ChatMessage(ChatRole.User, "What is the capital of France?")
};

var response = await chatClient.GetResponseAsync(messages);
Console.WriteLine(response.Text);
