
using System.Security.AccessControl;
using System.Text;
using System.Xml;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.Services.AddChatClient(
    new OllamaChatClient(new Uri("http://localhost:11434"), "llava"));

var app = builder.Build();


var chatClient = app.Services.GetRequiredService<IChatClient>();

var message = new ChatMessage(ChatRole.User, "请返回图片中的文本内容");
message.Contents.Add(new DataContent(File.ReadAllBytes("images/img1.png"),"image/png"));

var response = await chatClient.GetResponseAsync(message);
Console.WriteLine(response.Text);


