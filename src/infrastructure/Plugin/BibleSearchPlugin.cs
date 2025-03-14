using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace infrastructure.Plugin
{
    public class BibleSearchPlugin(IKernelMemory kernelMemory)
    {
        [KernelFunction, Description("Search the Bible for relevant verses that help answer this question.Return only the most relevant Bible verse as it appears in the Bible, without explanation, summary, or additional text.do not include multiple verses or multiline responses.")]
        public async Task<string> SearchVerses([Description("The query to search for.")] string query)
        {
            var searchResult = await kernelMemory.AskAsync($"Find only the verse names from bible to help answer - {query}", index: "holybible");
            return searchResult.Result;
        }
    }
}
