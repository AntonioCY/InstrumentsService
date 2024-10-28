namespace InstrumentsService.Application.Interfaces;

public interface IDataProvider
{
    Task<double> GetPrice(string instrument);
}
