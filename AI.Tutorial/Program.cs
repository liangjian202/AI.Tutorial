
using System.Text;
using System.Xml;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddChatClient(
    new OllamaChatClient(new Uri("http://localhost:11434"), "llama3"));

var app = builder.Build();


var chatClient = app.Services.GetRequiredService<IChatClient>();

var chatHistory = new List<ChatMessage>();

while (true)
{
    Console.WriteLine("Your prompt:");    
    var userInput = Console.ReadLine();
    if (string.IsNullOrEmpty(userInput))
    {
        continue;   
    }
    chatHistory.Add(new ChatMessage(ChatRole.User, userInput));

    Console.WriteLine("AI Response:");

    var chatResponseText = new StringBuilder();
    var response = await chatClient.GetResponseAsync(chatHistory);
    foreach (var message in response.Messages)
    {
        Console.WriteLine(message.Text);
        chatResponseText.Append(message.Text);
    }
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, chatResponseText.ToString()));
    Console.WriteLine();
}
