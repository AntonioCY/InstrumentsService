using InstrumentsService.Application.Dto;
using InstrumentsService.Application.Helpers;
using InstrumentsService.Application.Interfaces;
using InstrumentsService.Domain.Constants;
using InstrumentsService.Domain.Enums;

namespace InstrumentsService.Application.Services
{
    public class FinancialInstrumentService(DataProviderResolver providerResolver) : IFinancialInstrumentService
    {
        private readonly DataProviderResolver _providerResolver = providerResolver;

        public List<InstrumentDto> GetAvailableInstruments()
        {
            var instruments = new List<InstrumentDto>();

            foreach (var symbol in InstrumentsConstants.Symbols)
            {
                instruments.Add(new InstrumentDto
                {
                    Symbol = symbol
                });
            }

            return instruments;
        }

        public List<ProviderDto> GetAvailableProviders()
        {
            var providers = new List<ProviderDto>();

            foreach (DataProvider provider in Enum.GetValues(typeof(DataProvider)))
            {
                providers.Add(new ProviderDto
                {
                    Id = (int)provider,
                    Name = provider.ToString()
                });
            }

            return providers;
        }

        public async Task<InstrumentDetailsDto> GetCurrentPrice(string instrument, int providerTypeId)
        {
            var dataProvider = _providerResolver.GetDataProviderByEnum(providerTypeId);
            var price = await dataProvider.GetPrice(instrument);

            return new InstrumentDetailsDto
            {
                Symbol = instrument,
                Price = price
            };
        }
    }
}
