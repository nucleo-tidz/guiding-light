using infrastructure.Plugin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI;


namespace infrastructure.Agents
{
    public class BibleAgentFactory
    {
        public ChatCompletionAgent Create(Kernel _kernel,IServiceProvider serviceProvider)
        {
            Kernel agentKernel = _kernel.Clone();
            agentKernel.Plugins.AddFromObject(new BibleSearchPlugin(serviceProvider.GetRequiredService<IKernelMemory>()), "BibleSearchPlugin");
            return
                new ChatCompletionAgent()
                {
                    Name = "BibleExpertAgent",
                    Instructions = "You are a Bible expert. Your primary role is to invoke the BibleSearchPlugin to retrieve relevant Bible verses based on the user's query or confession. These verses will assist the Pastor agent in crafting a meaningful response.",
                    Kernel = agentKernel,
                    Arguments = new KernelArguments(
                        new AzureOpenAIPromptExecutionSettings()
                        {
                            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),

                        })
                };
        }
    }
}
