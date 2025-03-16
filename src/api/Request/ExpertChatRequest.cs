using infrastructure.Constants;

namespace api.Request
{
    public class ExpertChatRequest
    {
        public string message { get; set; }
        public AgentType agent { get; set; }
        public string userid { get; set; }
        public string sessionid { get; set; }
    }
}
