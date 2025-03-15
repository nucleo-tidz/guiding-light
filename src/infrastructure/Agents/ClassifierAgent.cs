using infrastructure.Constants;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace infrastructure.Agents
{
    public class ClassifierAgent(Kernel kernel) : IClassifierAgent
    {
        private ChatCompletionAgent Create()
        {
            Kernel agentKernel = kernel.Clone();
            return
                new ChatCompletionAgent()
                {
                    Name = "ClassifierAgent",
                    Instructions = Persona.Classifier,
                    Kernel = agentKernel,
                };
        }
        public async Task<string> Classify(string query)
        {
            var tempHistory = new ChatHistory();
            tempHistory.AddUserMessage(query);
            var agent = Create();
            await foreach (var message in agent.InvokeAsync(tempHistory))
            {
                return message.Content;
            }
            return "0";
        }
    }
}
