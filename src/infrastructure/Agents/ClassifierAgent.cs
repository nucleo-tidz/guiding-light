using infrastructure.Constants;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace infrastructure.Agents
{
    public class ClassifierAgent(Kernel kernel) : IClassifierAgent
    {
        private ChatCompletionAgent Create(AgentType agent)
        {
            Kernel agentKernel = kernel.Clone();
            return
                new ChatCompletionAgent()
                {
                    Name = "ClassifierAgent",
                    Instructions = agent == AgentType.IslamicScholar ? Persona.IslamicScholar : Persona.Classifier,
                    Kernel = agentKernel,
                };
        }
        public async Task<string> Classify(string query, AgentType agent)
        {
            var tempHistory = new ChatHistory();
            tempHistory.AddUserMessage(query);
            var classifierAgent = Create(agent);
            await foreach (var message in classifierAgent.InvokeAsync(tempHistory))
            {
                return message.Content;
            }
            return "0";
        }
    }
}
