using Microsoft.KernelMemory;

namespace infrastructure.Service
{
    public class BibleService(IKernelMemory kernelMemory) : IBibleService
    {
        public async Task<(string,bool)> GetVerse(string query)
        {
            var searchResult = await kernelMemory.AskAsync($"Find only the verse names from bible to help answer , do not include any decsription of the vesre - {query}", index: "holybible");
            return (searchResult.Result,searchResult.NoResult);
        }
    }
}
