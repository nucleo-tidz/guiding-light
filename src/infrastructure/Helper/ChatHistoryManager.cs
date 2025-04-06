using infrastructure.Constants;
using infrastructure.Repository;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace infrastructure.Helper
{
    public class ChatHistoryManager(IChatHistoryRepository _chatHistoryRepository,Kernel kernel ) : IChatHistoryManager
    {
        public async Task<ChatHistory> GetChatHistory(string userId, string sessionId, AgentType agentType, string? ragOutput = null)
        {
            ChatHistory chatHistory = new ChatHistory();
            if (agentType == AgentType.Pastor)
            {
                chatHistory.Add(new Microsoft.SemanticKernel.ChatMessageContent { Role = AuthorRole.System, Content = Persona.Pastor });
            }
            else if (agentType == AgentType.IslamicScholar)
            {
                chatHistory.Add(new Microsoft.SemanticKernel.ChatMessageContent { Role = AuthorRole.System, Content = string.IsNullOrEmpty(ragOutput) ? Persona.IslamicScholar : Persona.IslamicScholar + $", Use the provided ragoutput to support your answer ,ragoutput  - {ragOutput}" });
            }

            var savedHistory = await _chatHistoryRepository.GetChats(userId, sessionId);
            foreach (var history in savedHistory)
            {
                chatHistory.Add(new Microsoft.SemanticKernel.ChatMessageContent { Role = new AuthorRole(history.Role), Content = history.Message });
            }
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var reducer = new ChatHistorySummarizationReducer(kernel.GetRequiredService<IChatCompletionService>(), 5, 2);
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return await chatHistory.ReduceAsync(reducer,CancellationToken.None);
        }
        public async Task<ChatHistory> Append(string message, string userId, string sessionId, ChatHistory chatHistory, AuthorRole authorRole)
        {
            var chatMessageContent = new Microsoft.SemanticKernel.ChatMessageContent { Role = authorRole, Content = message };
            chatHistory.Add(chatMessageContent);
            await _chatHistoryRepository.SaveChatMessageAsync(new model.UserChatHistory().ConvertToEntity(chatMessageContent, userId, sessionId));
            return chatHistory;
        }        
    }

}
