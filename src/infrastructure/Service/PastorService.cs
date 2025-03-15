using infrastructure.Agents;
using infrastructure.Repository;
using Microsoft.Extensions.AI;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;

namespace infrastructure.Service
{
#pragma warning disable SKEXP0110
    public class PastorService : IPastorService
    {
        private readonly Kernel _kernel;
        private readonly IBibleService _bibleService;
        private readonly IChatHistoryRepository _chatHistoryRepository;
        private readonly ILogger<PastorService> _logger;

        public PastorService(Kernel kernel, IBibleService bibleService, IChatHistoryRepository chatHistoryRepository, ILogger<PastorService> logger)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _bibleService = bibleService ?? throw new ArgumentNullException(nameof(bibleService));
            _chatHistoryRepository = chatHistoryRepository ?? throw new ArgumentNullException(nameof(chatHistoryRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ChatMessageContent> GetReponse(string query, string userId, string sessionId)
        {
            try
            {
                var chat = _kernel.GetRequiredService<IChatCompletionService>();
                var chatHistory = await _chatHistoryRepository.GetChats(userId, sessionId) ?? new ChatHistory();
                await EnsureSystemPersona(chatHistory, userId, sessionId);

                var isRelevant = await CheckForRelevantBibleVerse(query, chatHistory, userId, sessionId);

                if (!isRelevant)
                {
                    _logger.LogInformation("No relevant Bible verse found for query: {Query}", query);
                }
                var summarizedHistory = await SummarizeChatHistory(chat, chatHistory);
                var response = await chat.GetChatMessageContentAsync(summarizedHistory);
                await SaveChatMessage(response.Content, AuthorRole.Assistant, userId, sessionId, chatHistory);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while generating response");
                throw;
            }
        }

        private async Task EnsureSystemPersona(ChatHistory chatHistory, string userId, string sessionId)
        {
            if (!chatHistory.Any(msg => msg.Role == AuthorRole.System))
            {
                ChatMessageContent persona = SetPersona();
                chatHistory.Add(persona);
                await _chatHistoryRepository.SaveChatMessageAsync(new model.UserChatHistory().ConvertToEntity(persona, userId, sessionId));
            }
        }

        private async Task<ChatHistory> SummarizeChatHistory(IChatCompletionService chat, ChatHistory chatHistory)
        {
#pragma warning disable SKEXP0001
            var reducer = new ChatHistorySummarizationReducer(chat, 5, 2);
#pragma warning restore SKEXP0001
            return await chatHistory.ReduceAsync(reducer, CancellationToken.None);
        }

        private async Task<bool> CheckForRelevantBibleVerse(string query, ChatHistory chatHistory, string userId, string sessionId)
        {
            var classifierAgent = new ClassifierAgent().Create(_kernel);
            var tempHistory = new ChatHistory { SetMessage(query, AuthorRole.User) };

            await SaveChatMessage(query, AuthorRole.User, userId, sessionId, chatHistory);

            await foreach (var message in classifierAgent.InvokeAsync(tempHistory))
            {
                if (message.Content.Contains("1"))
                {
                    var (verse, notfound) = await _bibleService.GetVerse(query);
                    if (!notfound)
                    {
                        var verseMessage = $"Relevant Bible verse - {verse}";
                        chatHistory.Add(SetMessage(verseMessage, AuthorRole.Assistant));
                        await SaveChatMessage(verseMessage, AuthorRole.Assistant, userId, sessionId, chatHistory);
                        return true;
                    }
                }
            }
            return false;
        }

        private async Task SaveChatMessage(string message, AuthorRole role, string userId, string sessionId, ChatHistory chatHistory)
        {
            var chatMessage = SetMessage(message, role);
            chatHistory.Add(chatMessage);
            await _chatHistoryRepository.SaveChatMessageAsync(new model.UserChatHistory().ConvertToEntity(chatMessage, userId, sessionId));
        }

        public ChatMessageContent SetPersona() => new()
        {
            Role = AuthorRole.System,
            Content = "You are an AI Pastor, offering compassionate biblical guidance and pastoral counseling. Your role is to listen with empathy, as a caring pastor would during a confession, and provide thoughtful, scripture-based advice. Respond with wisdom, kindness, and encouragement, ensuring your guidance aligns with biblical teachings. If present, refer relevant Bible verses provided to you in the history to support your response, but always prioritize a compassionate and understanding tone. Your goal is to comfort, guide, and uplift the user, addressing their concerns with faith-based wisdom."
        };

        public ChatMessageContent SetMessage(string message, AuthorRole role) => new()
        {
            Role = role,
            Content = message
        };
    }
#pragma warning restore SKEXP0110
}
