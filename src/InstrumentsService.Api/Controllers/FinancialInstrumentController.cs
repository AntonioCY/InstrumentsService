using InstrumentsService.Application.Dto;
using InstrumentsService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InstrumentsService.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FinancialInstrumentController(IFinancialInstrumentService service,
        ILogger<FinancialInstrumentController> logger) : ControllerBase
    {
        private readonly IFinancialInstrumentService _service = service;
        private readonly ILogger<FinancialInstrumentController> _logger = logger;

        [HttpGet("instruments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<InstrumentDto>))]
        public IActionResult GetInstruments()
        {
            var instruments = _service.GetAvailableInstruments();
            return Ok(instruments);
        }
        
        [HttpGet("providers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProviderDto>))]
        public IActionResult GetProviders()
        {
            var providers = _service.GetAvailableProviders();
            return Ok(providers);
        }

        [HttpGet("price/{instrument}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InstrumentDetailsDto))]
        public async Task<IActionResult> GetPrice(string instrument, int providerId)
        {
            var price = await _service.GetCurrentPrice(instrument, providerId);
            return Ok(price);
        }
    }
}
