
using System.Security.AccessControl;
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

foreach (var filePath in Directory.GetFiles("posts", "*.txt"))
{
    string prompt = $$"""
                      您将收到一个输入文本和所需的输出格式。
                      您需要分析文本并生成所需的输出格式。
                      不允许更改代码、文本或其他引用。

                      #期望的响应

                      仅按照此格式提供符合RFC8259的JSON响应，不得有任何偏差。

                      {
                      “title”：“标题取自正文部分”，
                      “summary”：“以不超过100字的篇幅总结文章”
                      }

                      #文章内容：

                      {{File.ReadAllText(filePath)}}
                      """;

    var msg = new ChatMessage(ChatRole.User, prompt);
    // var response = await chatClient.GetResponseAsync(msg);
    var response = await chatClient.GetResponseAsync<PostCategory>(msg);
    
    Console.WriteLine(response.Text);
    Console.WriteLine();
}

record PostCategory(string Title, string Summary);

/*
{
"title": "C#：微软生态的核心编程语言",
"summary": "C# 是由微软开发的一种现代、通用的编程语言，自 2000 年发布以来，它已成为 .NET 平台的核心语言之一。"
}

{
"title": "**JavaScript：Web 时代的编程语言**",
"summary": "JavaScript 是一种动态、弱类型的编程语言，自 1995 年由 Brendan Eich 创造以来，它已成为 Web 开发的核心技术之一。作为浏览器的原生脚本语言，JavaScript 赋予了网页动态交互的能力，同时随着 Node.js 的兴起，它也扩展到了服务器端开发领域。"
}

{
"title": "**Python：现代编程的瑞士军刀**",
"summary": "Python 是一种高级编程语言，以其简洁、易读和强大的功能而闻名。自 1991 年由 Guido van Rossum 发布以来，Python 已经发展成为全球最受欢迎的编程语言之一。无论是初学者还是经验丰富的开发者，Python 都能提供高效的工具和丰富的资源，满足各 种编程需求。"
}

/*
{
"title": "C#：微软生态的核心编程语言",
"summary": "C# 是微软开发的一种现代、通用的编程语言，适用于开发桌面应用、Web 应用、移动应用以及游戏等。文章将深入探讨 C# 的特点、应用领域及其未来发展。"
}

{
"title": "**JavaScript：Web 时代的编程语言**",
"summary": "JavaScript 是一种动态、弱类型的编程语言，自 1995 年由 Brendan Eich 创造以来，它已成为 Web 开发的核心技术之一。作为浏览器的原生脚本语言，JavaScript 赋予了网页动态交互的能力，同时随着 Node.js 的兴起，它也扩展到了服务器端开发领域。"
}

{
"title": "**Python：现代编程的瑞士军刀**",
"summary": "Python 是一种高级编程语言，以其简洁、易读和强大的功能而闻名。自 1991 年由 Guido van Rossum 发布以来，Python 已经发展成为全球最受欢迎的编程语言之一。无论是初学者还是经验丰富的开发者，Python 都能提供高效的工具和丰富的资源，满足各 种编程需求。本文将探讨 Python 的特点、应用领域以及其未来的发展趋势。"
}
*/