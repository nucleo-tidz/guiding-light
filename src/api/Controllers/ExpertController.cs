﻿using api.Request;
using infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using System.Collections.Generic;
using System.Text;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpertController(IExpertService expertService,IVerseService verseService) : ControllerBase
    {
        [HttpPost("chat")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> chat([FromBody] ExpertChatRequest expertChatRequest)
        {
           ChatMessageContent resposne= await expertService.GetReponse(expertChatRequest.message, expertChatRequest.userid, expertChatRequest.sessionid, expertChatRequest.agent);
            return Ok(resposne.Content);
        }
        [HttpPost("ask-quran")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ask([FromBody] ExpertChatRequest expertChatRequest)
        {
            var response = await verseService.Ask(expertChatRequest.message, expertChatRequest.agent);
            if(!response.Item2) 
                return Ok(response.Item1);
            return Ok("Sorry i could not find the answer of your quetion");
           
        }
        [HttpPost("chat-stream")]   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces("text/plain")]
        public async Task Chat([FromBody] ExpertChatRequest expertChatRequest)
        {
            Response.ContentType = "text/plain"; 
            await foreach (var chunk in expertService.GetStreamingResponse(expertChatRequest.message, expertChatRequest.userid, expertChatRequest.sessionid, expertChatRequest.agent))
            {
                await Response.WriteAsync(chunk + " ");
                await Response.Body.FlushAsync();
            }
        }
    }
}
