using infrastructure.Agents;
using infrastructure.Constants;
using infrastructure.Helper;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace infrastructure.Service
{
#pragma warning disable SKEXP0110
    public class PastorService : IPastorService
    {

        private readonly IBibleService _bibleService;
        private readonly IChatHistoryManager _chatHistorymanager;
        private readonly ILogger<PastorService> _logger;
        private readonly IClassifierAgent _classifierAgent;
        private readonly IChatCompletionService _chatCompletionService;

        public PastorService(Kernel kernel, IBibleService bibleService, IChatHistoryManager chatHistorymanager, ILogger<PastorService> logger, IClassifierAgent classifierAgent)
        {
            _bibleService = bibleService ?? throw new ArgumentNullException(nameof(bibleService));
            _chatHistorymanager = chatHistorymanager ?? throw new ArgumentNullException(nameof(chatHistorymanager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _classifierAgent = classifierAgent;
            _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        }

        public async Task<ChatMessageContent> GetReponse(string query, string userId, string sessionId)
        {
            try
            {
                ChatHistory chatHistory = await _chatHistorymanager.GetChatHistory(userId, sessionId, PersonaType.Pastor);
               
                if ((await _classifierAgent.Classify(query)).Contains( "1"))
                {
                    var verse = await _bibleService.GetVerse(query);
                    if (!verse.Item2)
                    {
                        await _chatHistorymanager.Append(verse.Item1, userId, sessionId, chatHistory, AuthorRole.Assistant);
                    }
                }
                await _chatHistorymanager.Append(query, userId, sessionId, chatHistory, AuthorRole.User);
                var chatMessageContent = await _chatCompletionService.GetChatMessageContentAsync(chatHistory);
                await _chatHistorymanager.Append(chatMessageContent.Content, userId, sessionId, chatHistory, AuthorRole.Assistant);
                return chatMessageContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating response");
                throw;
            }
        }
    }
#pragma warning restore SKEXP0110
}
