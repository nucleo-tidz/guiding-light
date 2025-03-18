using infrastructure.Constants;
using Microsoft.SemanticKernel;

namespace infrastructure.Service
{
    public interface IExpertService
    {
        Task<ChatMessageContent> GetReponse(string query, string userId, string sessionId, AgentType agentType);
        IAsyncEnumerable<string> GetStreamingResponse(string query, string userId, string sessionId, AgentType agentType);
    }
}
