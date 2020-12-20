using System.Threading.Tasks;
using JobSityChat.Core.Repository.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace JobSityChat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IUserMessageRepository _repository;

        public MessageController(IUserMessageRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _repository.GetMessages();
            return Ok(messages);
        }

        [HttpGet("GetLast50Messages")]
        public async Task<IActionResult> GetLast50Messages()
        {
            var messages = await _repository.GetLast50Messages();
            return Ok(messages);
        }
    }
}
