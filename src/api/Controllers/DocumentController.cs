using infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.DocumentStorage;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Memory;

namespace api.Controllers
{
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController(IKernelMemory kernelMemory, IPastorService pastor) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Query(string query)
        {
            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, string denominationName)
        {
            await kernelMemory.ImportDocumentAsync(file.OpenReadStream(), fileName: file.FileName, index: denominationName);
            return Created();
        }
    }
}

#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.