using Dm;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using OpenAI_API.Moderation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chatgpt
{
    public class ChatApi
    {
        OpenAIAPI api;
        string key;
        string org;
        public ChatApi(string key, string org)
        {
            this.key = key;
            this.org = org;
            api = this.Instance;
        }

        public OpenAIAPI Instance
        {
            get
            {
                return new OpenAIAPI(new APIAuthentication(key, org));
            }
        }

        public async Task<ChatMessage> Call(double temperaturem, int max_tokens,
            List<ChatMessage> message)
        {
            var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo,
                Temperature = temperaturem,
                MaxTokens = max_tokens,
                Messages = message
            });

            var reply = result.Choices[0].Message;
            return reply;
        }

        
    }
}
