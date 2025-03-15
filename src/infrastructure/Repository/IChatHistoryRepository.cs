using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using model;

namespace infrastructure.Repository
{
    public interface IChatHistoryRepository
    {
        Task SaveChatMessageAsync(UserChatHistory userChatHistory);
        Task<IEnumerable<UserChatHistory>> GetChats(string userid, string sessionid);
    }
}
