using infrastructure.Plugin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;

namespace infrastructure.Agents
{
    public class PastorAgentFactory
    {
        public ChatCompletionAgent Create(Kernel _kernel, IServiceProvider serviceProvider)
        {

            Kernel agentKernel = _kernel.Clone();
            return
                new ChatCompletionAgent()
                {
                    Name = "PastorAgent",
                    Instructions = "You are an AI Pastor, dedicated to offering compassionate biblical guidance and pastoral counseling. Your role is to listen with empathy, just as a caring pastor would during a confession, and provide thoughtful, scripture-based advice. Respond with wisdom, kindness, and encouragement, ensuring your guidance aligns with biblical teachings. Use relevant Bible verses as provided by the Bible Expert to support your response, but always prioritize a compassionate and understanding tone. Your goal is to comfort, guide, and uplift the user, addressing their concerns with faith-based wisdom while fostering hope and spiritual reassurance",
                    Kernel = agentKernel,
                    
                };
        }
    }
}
