using Microsoft.AspNetCore.Mvc;
using Parser.API.Models;
using Parser.API.Services;

namespace Parser.API.Controllers
{
    [ApiController]
    [Route("api/parser")]
    public class ParserController : Controller
    {
        private readonly IParserService _service;

        public ParserController(IParserService service)
        {
            _service = service;
        }

        [HttpPost("parse")]
        [Consumes("text/plain")]
        public async Task<IActionResult> Parse()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var input = await reader.ReadToEndAsync();

                var result = _service.Parse(input);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
