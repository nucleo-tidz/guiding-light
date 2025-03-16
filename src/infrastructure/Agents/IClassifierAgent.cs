using infrastructure.Constants;

namespace infrastructure.Agents
{
    public interface IClassifierAgent
    {        Task<string> Classify(string query, AgentType agent);
    }
}
