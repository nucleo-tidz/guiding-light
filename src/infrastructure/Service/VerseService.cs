using infrastructure.Constants;
using Microsoft.KernelMemory;

namespace infrastructure.Service
{
    public class VerseService(IKernelMemory kernelMemory) : IVerseService
    {
        public async Task<(string, bool)> GetVerse(string query, AgentType agentType)
        {
            MemoryAnswer searchResult = agentType switch
            {
                AgentType.IslamicScholar => await kernelMemory.AskAsync($"Give very breif answer limit it to 100 words only for the user query ,iclude citations, user query - {query}", index: "quran"),
                _ => await kernelMemory.AskAsync($"Give very breif answer limit it to 100 words only for the user query ,iclude citations, user query - {query}", index: "holybible")
            };

            return (searchResult.Result, searchResult.NoResult);
        }

        public async Task<(string, bool)> Ask(string query, AgentType agentType)
        {
            MemoryAnswer searchResult = agentType switch
            {
                AgentType.IslamicScholar => await kernelMemory.AskAsync(query, index: "quran"),
                _ => await kernelMemory.AskAsync(query, index: "holybible")
            };

            return (searchResult.Result, searchResult.NoResult);
        }
    }
}
