using Microsoft.Extensions.Configuration;
using OpenAI_API.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Chatgpt
{
    public class Zhixie : IExcutor
    {
        ChatApi chatApi;
        IConfigurationRoot configuration;
        Queue<string> queue = new Queue<string>();
        string src = "data";
        PubHelper pubHelper = new PubHelper();
        public Zhixie()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Path.Combine(AppContext.BaseDirectory))
           .AddJsonFile("config/config.json", optional: true, reloadOnChange: false);
            configuration = builder.Build();
            var key = configuration.GetSection("key").Value;
            var org = configuration.GetSection("org").Value;
            chatApi = new ChatApi(key, org);
        }
        public async Task Go()
        {
            var files = Directory.GetFiles(src).ToList();
            List<string> lines;


            foreach (var file in files)
            {
                Console.WriteLine("读取文件" + file);
                lines = File.ReadAllLines(file).ToList();
                foreach (var line in lines)
                {
                    queue.Enqueue(line);
                    Console.WriteLine(line + "写入队列");
                }
            }

            await Chat();
            Console.WriteLine("运行完成");

        }

        private async Task Chat()
        {
            var cate = int.Parse(configuration.GetSection("category").Value);
            var author = int.Parse(configuration.GetSection("author").Value);
            while (queue.Count > 0)
            {
                Console.WriteLine($"还剩下{queue.Count()}行");
                var line = queue.Dequeue();
                if (string.IsNullOrEmpty(line))
                {
                    Console.WriteLine("关键词为空");
                }
                else
                {
                    try
                    {
                        List<ChatMessage> messages = new List<ChatMessage>
                        {
                           new ChatMessage(ChatMessageRole.System, "根据给定的标题写一篇文章"),
                           new ChatMessage(ChatMessageRole.User,
                                        line),
                           new ChatMessage(ChatMessageRole.Assistant,
                                        "不少于2000字")
                        };
                        var reply = await chatApi.Call(0.5, 1024, messages);
                        await pubHelper.Post(line, reply.Content.Trim(), cate, author,
                            new List<string>(), new Dictionary<string, string>());
                        Console.WriteLine(reply.Content.Trim());
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.ToString());
                        queue.Enqueue(line);
                    }
                    Thread.Sleep(1000);
                }
            }

        }
    }
}
