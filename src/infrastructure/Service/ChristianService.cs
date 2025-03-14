using infrastructure.Agents;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace infrastructure.Service
{
#pragma warning disable SKEXP0110
    public class ChristianService : IPastorService
    {
        ChatCompletionAgent pastorAgent;
        ChatCompletionAgent bibleAgent;
        ChatCompletionAgent classifierAgent;
        AgentGroupChat chat;
        Kernel _kernel;
        ChatHistory chatHistory;
        IServiceProvider _serviceProvider;
        public ChristianService(Kernel kernel, IServiceProvider serviceProvider)
        {
            chatHistory = new();
            _serviceProvider = serviceProvider;
            _kernel = kernel;
            pastorAgent = new PastorAgentFactory().Create(_kernel, _serviceProvider);
            bibleAgent = new BibleAgentFactory().Create(kernel, _serviceProvider);
            classifierAgent = new ClassifierAgent().Create(kernel);
            chatHistory.Add(new ChatMessageContent { Role = AuthorRole.System, Content = pastorAgent.Instructions });

        }

        public async Task<ChatMessageContent> GetReponse(string query)
        {
            var history = new ChatHistory();
            history.AddUserMessage(query);
            chatHistory.AddMessage(authorRole: AuthorRole.User, query);
            await foreach (var message in classifierAgent.InvokeAsync(history))
            {
                if (message.Content.Contains("1"))
                {
                    chatHistory.AddMessage(authorRole: AuthorRole.Assistant, await GetVesre());
                }
            }
            string finalResponse = await Gethelp(history);
            chatHistory.AddMessage(authorRole: AuthorRole.Assistant, finalResponse);
            return null;
        }

        private async Task<string> Gethelp(ChatHistory history)
        {
            StringBuilder stringBuilder = new StringBuilder();
            await foreach (var message in pastorAgent.InvokeAsync(history))
            {
                stringBuilder.Append(message.Content);
            }
            return stringBuilder.ToString();
        }


        private async Task<string> GetVesre()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Response from Bible Expert");
            await foreach (var vesre in bibleAgent.InvokeAsync(chatHistory))
            {
                stringBuilder.Append(vesre.Content);
            }
            return stringBuilder.ToString();
        }
        //https://medium.com/@akshaykokane09/step-by-step-guide-to-develop-ai-multi-agent-system-using-microsoft-semantic-kernel-and-gpt-4o-f5991af40ea6

    }
#pragma warning restore SKEXP0110 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}
