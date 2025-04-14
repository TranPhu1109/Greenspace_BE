using GreenSpace.Application.Features.Bills.Commands;
using GreenSpace.Application.Features.Blogs.Queries;
using GreenSpace.Application.Features.Dashboard.Queries;
using GreenSpace.Application.ViewModels.Bills;
using GreenSpace.Application.ViewModels.Report;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GreenSpace.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Queries
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> Get()
     => Ok(await _mediator.Send(new GetAllRevenueQuery()));


        [HttpGet("filter")]
        [ProducesResponseType(typeof(ReportFillterViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetFilteredRevenue([FromQuery] DateTime? date, 
                                                            [FromQuery] int? month, 
                                                            [FromQuery] int? year)
           => Ok(await _mediator.Send(new GetRevenueByFilterQuery
           {
               Date = date,
               Month = month,
               Year = year
           }));

        #endregion
    }

}
