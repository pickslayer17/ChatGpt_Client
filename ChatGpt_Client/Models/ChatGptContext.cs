using System.Windows.Forms;

namespace ChatGpt_Client.Models
{
    public class ChatGptContext
    {
        public string Name;
        public string model;
        public LinkedList<ChatMessage> messages = new LinkedList<ChatMessage>();
        public int max_tokens;

        public void AddMessage(ChatMessage message) => messages.AddLast(message);

        public void ClearMesssages()
        {
            while (messages.Count > 1)
            {
                messages.RemoveLast();
            }
        }
    public void ReplaceLastMessage(string message) => messages.Last().Content = message;
    }
}
