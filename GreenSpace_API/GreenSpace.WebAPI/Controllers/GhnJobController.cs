using GreenSpace.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GreenSpace.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GhnJobController : BaseController
    {
        private readonly GhnJobService _ghnJobService;

        public GhnJobController(GhnJobService ghnJobService)
        {
            _ghnJobService = ghnJobService;
        }

        [HttpPost("run")]
        public async Task<IActionResult> Run()
        {
            await _ghnJobService.FetchGhnOrder();
            return Ok("Done");
        }
    }
}
