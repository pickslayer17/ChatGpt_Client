using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGpt_Client.Models
{
    public class UserChatMessage : ChatMessage
    {
        public UserChatMessage(string message) : base(message)
        {
            Role = "user";
        }
    }
}
