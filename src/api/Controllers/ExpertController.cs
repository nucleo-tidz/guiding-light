using api.Request;
using infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertController(IExpertService expertService) : ControllerBase
    {
        [HttpPost("chat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> chat([FromBody] ExpertChatRequest expertChatRequest)
        {
           ChatMessageContent resposne= await expertService.GetReponse(expertChatRequest.message, expertChatRequest.userid, expertChatRequest.sessionid, expertChatRequest.agent);
            return Ok(resposne.Content);
        }

    }
}
