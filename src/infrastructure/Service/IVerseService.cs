using infrastructure.Constants;

namespace infrastructure.Service
{
    public interface IVerseService
    {
        public Task<(string, bool)> GetVerse(string query, AgentType agentType);
        Task<(string, bool)> Ask(string query, AgentType agentType);
    }
}
