using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace infrastructure.Plugin
{
    public class HolyQuranPlugin(IKernelMemory kernelMemory)
    {
        [KernelFunction, Description("Search the holy quran for relevant verses that help answer this question.Return only the most relevant quran verse , without explanation, summary, or additional text.do not include multiple verses or multiline responses.")]
        public async Task<string> SearchVerses([Description("The query to search for.")] string query)
        {
            var searchResult = await kernelMemory.AskAsync($"Find only the verse names and small description not exceeding 50 words from quran to help answer - {query}", index: "quran");
            return searchResult.NoResult ? string.Empty : searchResult.Result;
        }
    }
}
