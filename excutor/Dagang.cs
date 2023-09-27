using MD5Hash;
using Microsoft.Extensions.Configuration;
using OpenAI_API.Chat;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace Chatgpt
{
    public class Dagang : IExcutor
    {
        IConfigurationRoot configuration;
        Queue<string> queue = new Queue<string>();
        string src = "data";
        int max_tokens;
        double temperature;
        ChatApi chatApi;
        Regex regex = new Regex(@"共包含([1-9一二三四五六七八九十]+)个部分");
        PubHelper pubHelper = new PubHelper();

        public Dagang()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Path.Combine(AppContext.BaseDirectory))
           .AddJsonFile("config/config.json", optional: true, reloadOnChange: false);
            configuration = builder.Build();
            max_tokens = int.Parse(configuration.GetSection("max_tokens").Value);
            temperature = double.Parse(configuration.GetSection("temperature").Value);
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
                            new ChatMessage(ChatMessageRole.System, "用我给出的题目写出大纲，然后根据大纲写文章")
                        };
                        messages.Add(new ChatMessage(ChatMessageRole.User,
                            $"我想用{line}为题用中文写一篇文章，请帮我列出大纲"));
                        messages.Add(new ChatMessage(ChatMessageRole.Assistant,
                            "用“共包含n个部分”的语句形式说一下大纲一共包含几部分（n为阿拉伯数字）"));                       

                        var reply = await chatApi.Call(temperature, 1024,messages);
                        Console.WriteLine(reply.Content.Trim());
                        messages.Add( //将chat回复的内容添加进列表
                            new ChatMessage(ChatMessageRole.Assistant, reply.Content.Trim()));
                        var content = await Outline(messages, reply.Content.Trim());
                        await pubHelper.Post(line, content, cate, author,
                            new List<string>(), new Dictionary<string, string>());
                        Console.WriteLine("成功发布文章");
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

        private async Task<string> Outline(List<ChatMessage> messages, string text)
        {
            Match match = regex.Match(text);
            string value = "0";
            if (match.Success)
            {
                value = match.Groups[1].Value;
                Console.WriteLine("提取的数字是: " + value);
            }
            var number = Helper.ConvertChineseToArabic(value);
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= number; i++)
            {
                messages.Add(new ChatMessage(ChatMessageRole.User,
                    $"根据大纲的第{i}部分写文章"));
                var reply = await chatApi.Call(temperature, 2048, messages);
                messages.Add( //将chat回复的内容添加进列表
                            new ChatMessage(ChatMessageRole.Assistant, reply.Content.Trim()));
                sb.AppendLine(reply.Content.Trim());
                Console.WriteLine(reply.Content.Trim());
            }
            return sb.ToString();
        }
    }
}
