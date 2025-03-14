using DocumentFormat.OpenXml.Wordprocessing;
using infrastructure.Agents;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace infrastructure.Service
{
#pragma warning disable SKEXP0110
    public class PastorService : IPastorService
    {
        Kernel _kernel;
        ChatHistory chatHistory;
        IBibleService _bibleService;
        public PastorService(Kernel kernel, IBibleService bibleService, IKernelMemory kernelMemory)
        {
            chatHistory = new();
            _kernel = kernel;
            chatHistory.Add(new ChatMessageContent { Role = AuthorRole.System, Content = "You are an AI Pastor, offering compassionate biblical guidance and pastoral counseling. Your role is to listen with empathy, as a caring pastor would during a confession, and provide thoughtful, scripture-based advice. Respond with wisdom, kindness, and encouragement, ensuring your guidance aligns with biblical teachings. If present, refer relevant Bible verses provided to you in the history to support your response, but always prioritize a compassionate and understanding tone. Your goal is to comfort, guide, and uplift the user, addressing their concerns with faith-based wisdom." });
            _bibleService = bibleService;

        }

        public async Task<ChatMessageContent> GetReponse(string query)
        {
            ChatCompletionAgent classifierAgent = new ClassifierAgent().Create(_kernel);
            var history = new ChatHistory();
            history.AddUserMessage(query);
            chatHistory.AddMessage(authorRole: AuthorRole.User, query);
            await foreach (var message in classifierAgent.InvokeAsync(history))
            {
                if (message.Content.Contains("1"))
                {
                    var classiferRespone = await GetVesre(query);
                    if (!classiferRespone.Item2)
                    {
                        chatHistory.AddMessage(authorRole: AuthorRole.Assistant, "Relevant Bible verse - "+classiferRespone.Item1);
                    };
                }
            }
            var chat = _kernel.GetRequiredService<IChatCompletionService>();
            var response = await chat.GetChatMessageContentAsync(chatHistory);
            chatHistory.AddMessage(authorRole: AuthorRole.Assistant,response.Content);
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var reducer = new ChatHistorySummarizationReducer(chat, 3);
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            chatHistory= await chatHistory.ReduceAsync(reducer,CancellationToken.None);
            return response;
        }



        private async Task<(string, bool)> GetVesre(string query)
        {
            return await _bibleService.GetVerse(query);
        }
        //https://medium.com/@akshaykokane09/step-by-step-guide-to-develop-ai-multi-agent-system-using-microsoft-semantic-kernel-and-gpt-4o-f5991af40ea6

    }
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
