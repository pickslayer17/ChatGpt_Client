namespace ChatGpt_Client.Models
{
    public class ChatMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }

        public ChatMessage(string message)
        {
            Content = message;
        }
    }
}
