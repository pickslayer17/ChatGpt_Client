using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGpt_Client.Models
{
    public class SystemChatMessage : ChatMessage
    {
        public SystemChatMessage(string message) : base(message)
        {
            Role = "system";
        }
    }
}
