using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using model;

namespace infrastructure.Repository
{
    public interface IChatHistoryRepository
    {
        Task SaveChatMessageAsync(UserChatHistory userChatHistory);
        Task<ChatHistory> GetChats(string userid, string sessionid);
    }
}
