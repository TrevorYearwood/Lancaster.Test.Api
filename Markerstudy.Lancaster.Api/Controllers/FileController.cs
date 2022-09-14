using Markerstudy.Lancaster.Application.Features.File.Commands.CreateFile;
using Markerstudy.Lancaster.Application.Features.Valuation.Queries.FindValuations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Markerstudy.Lancaster.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FileController> _logger;

        public FileController(IMediator mediator, ILogger<FileController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("all", Name = "FindAllValuations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> Get(string? filePath)
        {
            var result = await _mediator.Send(new FindValuationsQuery { Filename = filePath });

            return Ok(result);
        }

        [HttpPost(Name = "AddFile")]
        public async Task<ActionResult<string>> Create([FromBody] CreateFileCommand createFileCommand)
        {
            var id = await _mediator.Send(createFileCommand);

            var result = await _mediator.Send(new FindValuationsQuery { Filename = createFileCommand.Filename });

            return Ok(result);
        }
    }
}