using Microsoft.SemanticKernel.ChatCompletion;
using model;

namespace infrastructure.Helper
{

    public static class ChatHistoryConverter
    {
        public static ChatHistory ToChatHistory(this List<UserChatHistory> userChatHistories)
        {
            ChatHistory history = new ChatHistory();
            foreach (var userHistory in userChatHistories)
            {
                history.Add(new Microsoft.SemanticKernel.ChatMessageContent { Content = userHistory.Message, Role = new AuthorRole(userHistory.Role) });
            }
            return history;
        }
    }
}
