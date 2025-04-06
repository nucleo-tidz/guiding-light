using infrastructure.Constants;
using Microsoft.SemanticKernel.ChatCompletion;

namespace infrastructure.Helper
{
    public interface IChatHistoryManager
    {
        Task<ChatHistory> GetChatHistory(string userId, string sessionId, AgentType agentType,string? ragOutput=null);
        Task<ChatHistory> Append(string message, string userId, string sessionId, ChatHistory chatHistory, AuthorRole authorRole);
    }
}
