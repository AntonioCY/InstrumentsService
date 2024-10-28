using InstrumentsService.Application.Dto;

namespace InstrumentsService.Application.Interfaces;

public interface IFinancialInstrumentService
{
    List<InstrumentDto> GetAvailableInstruments();
    List<ProviderDto> GetAvailableProviders();
    Task<InstrumentDetailsDto> GetCurrentPrice(string instrument, int providerTypeId);
}
