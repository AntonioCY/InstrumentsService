using InstrumentsService.Application.Interfaces;
using InstrumentsService.Application.Services;
using InstrumentsService.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace InstrumentsService.Application.Helpers;

public class DataProviderResolver(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IDataProvider GetDataProviderByEnum(int providerTypeId)
    {
        if (!Enum.TryParse(providerTypeId.ToString(), out DataProvider dataProviderEnum))
        {
            return _serviceProvider.GetRequiredService<TiingoDataProvider>();
        }

        return dataProviderEnum switch
        {
            DataProvider.Tiingo => _serviceProvider.GetRequiredService<TiingoDataProvider>(),
            DataProvider.Binance => _serviceProvider.GetRequiredService<BinanceDataProvider>(),
            _ => throw new NotImplementedException()
        };
    }
};

