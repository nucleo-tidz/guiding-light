using infrastructure.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.KernelMemory;

namespace api.Controllers
{
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController(IKernelMemory kernelMemory, IExpertService pastor) : ControllerBase
    {

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, string index, string tags)
        {
            TagCollection tagcollection = new TagCollection();
            foreach (var item in tags.Split(','))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    var finalTags = item.Split(":");
                    tagcollection.Add(finalTags[0], finalTags[1]);
                }
            }
            await kernelMemory.ImportDocumentAsync(file.OpenReadStream(), fileName: file.FileName, index: index, tags: tagcollection);
            return Created();
        }
    }
}

#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.