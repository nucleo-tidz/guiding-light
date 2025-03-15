using Microsoft.SemanticKernel;
using System.Runtime.CompilerServices;
namespace model
{
    
    public class UserChatHistory
    {
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string Role { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public  UserChatHistory ConvertToEntity(ChatMessageContent chatMessageContent,string UserId,string SessionId)
        {
            this.UserId=UserId; this.SessionId=SessionId;Role = chatMessageContent.Role.ToString();Message = chatMessageContent.Content;
            return this;
        }
    }
}
