using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Service
{
    public interface IPastorService
    {
        Task<ChatMessageContent> GetReponse(string query, string userId, string sessionId);
    }
}
