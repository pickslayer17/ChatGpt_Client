using ChatGpt_Client.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ChatGpt_Client.Helpers
{
    public class ChatGptClientHelper
    {
        private static readonly string apiUrl = "https://api.openai.com/v1/chat/completions";

        public static async Task<string> SendMessageWithContext(string apiKey, ChatGptContext context, string userMessage, Action<double>? onRateLimitDetected = null)
        {
            context.AddMessage(new UserChatMessage(userMessage));
            var requestData = GetRequestDataFromContext(context);
            var response = await GetChatGptResponse(apiKey, requestData, onRateLimitDetected);
            context.AddMessage(new AssistantChatMessage(response));

            return response;
        }

        private static object GetRequestDataFromContext(ChatGptContext context) => new
        {
            model = context.model,
            messages = context.messages.Select(msg => new { role = msg.Role, content = msg.Content }).ToArray(),
            max_tokens = context.max_tokens
        };

        private static async Task<string> GetChatGptResponse(string apiKey, object requestData, Action<double>? onRateLimitDetected = null)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                string json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseJson = await response.Content.ReadAsStringAsync();
                dynamic responseObject = JsonConvert.DeserializeObject(responseJson);

                if (responseObject == null)
                    return "Пустой ответ от сервера.";

                if (responseObject.error != null)
                {
                    string message = responseObject.error.message?.ToString() ?? "Неизвестная ошибка";
                    if (message.Contains("Rate limit"))
                    {
                        var match = Regex.Match(message, @"try again in ([\d.]+)s");
                        if (match.Success && double.TryParse(match.Groups[1].Value, out double seconds))
                        {
                            int milliseconds = (int)(seconds * 1000);
                            onRateLimitDetected?.Invoke(seconds);
                            await Task.Delay(milliseconds);

                            return await GetChatGptResponse(apiKey, requestData, onRateLimitDetected);
                        }
                        
                    }
                    return $"Ошибка: {message}";
                }

                if (responseObject.choices == null || responseObject.choices.Count == 0)
                    return "Ответ не содержит результатов.";

                return responseObject?.choices[0]?.message?.content ?? "Ошибка ответа от сервера.";
            }
        }
    }
}
