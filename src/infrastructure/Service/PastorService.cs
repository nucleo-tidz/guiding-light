using infrastructure.Agents;
using infrastructure.Repository;
using Microsoft.Extensions.AI;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using StackExchange.Redis;

namespace infrastructure.Service
{
#pragma warning disable SKEXP0110
    public class PastorService : IPastorService
    {
        Kernel _kernel;
        IBibleService _bibleService;
        IChatHistoryRepository _chatHistoryRepository;
        public PastorService(Kernel kernel, IBibleService bibleService, IKernelMemory kernelMemory,IChatHistoryRepository chatHistoryRepository)
        {
            
            _kernel = kernel;
            _bibleService = bibleService;
            _chatHistoryRepository = chatHistoryRepository;

        }

        public async Task<ChatMessageContent> GetReponse(string query, string userId, string sessionId)
        {
            var chat = _kernel.GetRequiredService<IChatCompletionService>();
            ChatHistory chatHistory = await _chatHistoryRepository.GetChats(userId, sessionId);
            if(chatHistory is null || !chatHistory.Any(_=>_.Role==AuthorRole.System))
            {
                var persona = SetPersona();
                chatHistory.Add(persona);
            }

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var reducer = new ChatHistorySummarizationReducer(chat, 3);
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var newhistory = await chatHistory.ReduceAsync(reducer, CancellationToken.None);


            ChatCompletionAgent classifierAgent = new ClassifierAgent().Create(_kernel);
            var history = new ChatHistory();
            history.AddUserMessage(query);
            await _chatHistoryRepository.SaveChatMessageAsync(new model.UserChatHistory().ConvertToEntity(SetMessage(query,AuthorRole.User), userId, sessionId));
            chatHistory.Add(SetMessage(query, AuthorRole.User));
            await foreach (var message in classifierAgent.InvokeAsync(history))
            {
                if (message.Content.Contains("1"))
                {
                    var classiferRespone = await GetVesre(query);
                    if (!classiferRespone.Item2)
                    {                      
                        chatHistory.Add(SetMessage("Relevant Bible verse - " + classiferRespone.Item1, AuthorRole.Assistant));
                        await _chatHistoryRepository.SaveChatMessageAsync(new model.UserChatHistory().ConvertToEntity(SetMessage("Relevant Bible verse - " + classiferRespone.Item1, AuthorRole.Assistant), userId, sessionId));
                    };
                }
            }
          
            var response = await chat.GetChatMessageContentAsync(chatHistory);
            chatHistory.Add(SetMessage(response.Content,AuthorRole.Assistant));
            await _chatHistoryRepository.SaveChatMessageAsync(new model.UserChatHistory().ConvertToEntity(SetMessage(response.Content, AuthorRole.Assistant), userId, sessionId));
          


            return response;
        }
        public ChatMessageContent SetPersona()=> new ChatMessageContent { Role = AuthorRole.System, Content = "You are an AI Pastor, offering compassionate biblical guidance and pastoral counseling. Your role is to listen with empathy, as a caring pastor would during a confession, and provide thoughtful, scripture-based advice. Respond with wisdom, kindness, and encouragement, ensuring your guidance aligns with biblical teachings. If present, refer relevant Bible verses provided to you in the history to support your response, but always prioritize a compassionate and understanding tone. Your goal is to comfort, guide, and uplift the user, addressing their concerns with faith-based wisdom." };

        public ChatMessageContent SetMessage(string message,AuthorRole role)=> new ChatMessageContent { Role= role, Content = message };
        private async Task<(string, bool)> GetVesre(string query)
        {
            return await _bibleService.GetVerse(query);
        }
    }
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
