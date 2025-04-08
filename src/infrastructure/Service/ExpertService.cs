using infrastructure.Agents;
using infrastructure.Constants;
using infrastructure.Helper;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace infrastructure.Service
{
#pragma warning disable SKEXP0110
    public class ExpertService : IExpertService
    {

        private readonly IVerseService _verseService;
        private readonly IChatHistoryManager _chatHistorymanager;
        private readonly ILogger<ExpertService> _logger;
        private readonly IClassifierAgent _classifierAgent;
        private readonly IChatCompletionService _chatCompletionService;

        public ExpertService(Kernel kernel, IVerseService verseService, IChatHistoryManager chatHistorymanager, ILogger<ExpertService> logger, IClassifierAgent classifierAgent)
        {
            _verseService = verseService ?? throw new ArgumentNullException(nameof(verseService));
            _chatHistorymanager = chatHistorymanager ?? throw new ArgumentNullException(nameof(chatHistorymanager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _classifierAgent = classifierAgent;
            _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        }
        public async IAsyncEnumerable<string> GetStreamingResponse(string query, string userId, string sessionId, AgentType agentType)
        {
            string RagOutput=string.Empty;
            //if ((await _classifierAgent.Classify(query, agentType)).Contains("1"))
            //{
            //    var verse = await _verseService.GetVerse(query, agentType);
            //    if (!verse.Item2)
            //    {
            //        RagOutput = verse.Item1;
            //    }
            //}
            ChatHistory chatHistory = await _chatHistorymanager.GetChatHistory(userId, sessionId, agentType, RagOutput);
            await _chatHistorymanager.Append(query, userId, sessionId, chatHistory, AuthorRole.User);
           
            StringBuilder assistantResponseBuilder = new();

            await foreach (var chunk in _chatCompletionService.GetStreamingChatMessageContentsAsync(chatHistory))
            {

                assistantResponseBuilder.Append(chunk.Content);
                yield return chunk.Content;

            }
            await _chatHistorymanager.Append(assistantResponseBuilder.ToString(), userId, sessionId, chatHistory, AuthorRole.Assistant);

        }

        public async Task<ChatMessageContent> GetReponse(string query, string userId, string sessionId, AgentType agentType)
        {
            try
            {
                ChatHistory chatHistory = await _chatHistorymanager.GetChatHistory(userId, sessionId, agentType);
                await _chatHistorymanager.Append(query, userId, sessionId, chatHistory, AuthorRole.User);

                //if ((await _classifierAgent.Classify(query, agentType)).Contains( "1"))
                //{
                //    var verse = await _verseService.GetVerse(query, agentType);
                //    if (!verse.Item2)
                //    {
                //        await _chatHistorymanager.Append(verse.Item1, userId, sessionId, chatHistory, AuthorRole.Assistant);
                //    }
                //}

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
