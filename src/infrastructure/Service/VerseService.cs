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
                AgentType.IslamicScholar => await kernelMemory.AskAsync($"Find only the verse names from the holy quran to help answer user query , do not include any decsription of the vesre , user query - {query}", index: "quran"),
                _ => await kernelMemory.AskAsync($"Find only the verse names from bible to help answer , do not include any decsription of the vesre - {query}", index: "holybible")
            };

            return ("Relevant Verses - " + searchResult.Result, searchResult.NoResult);
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
