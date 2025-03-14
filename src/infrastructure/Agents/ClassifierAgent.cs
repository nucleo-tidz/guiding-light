using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace infrastructure.Agents
{
    public class ClassifierAgent
    {
        public ChatCompletionAgent Create(Kernel _kernel)
        {
            Kernel agentKernel = _kernel.Clone();

            return
                new ChatCompletionAgent()
                {
                    Name = "ClassifierAgent",
                    Instructions = "You are a classifier that determines whether an input is a confession or a question requiring a Bible verse for an answer, or if it is a normal question or a follow-up in an ongoing conversation. Respond with '1' if the input requires a Bible verse and '0' if it does not. Provide no additional text",
                    Kernel = agentKernel,
                };
        }
    }
}
