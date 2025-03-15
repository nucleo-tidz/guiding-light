using infrastructure.Constants;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Helper
{
    public interface IChatHistoryManager
    {
        Task<ChatHistory> GetChatHistory(string userId, string sessionId, PersonaType personaType);
        Task<ChatHistory> Append(string message, string userId, string sessionId, ChatHistory chatHistory, AuthorRole authorRole);
    }
}
