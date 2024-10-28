namespace InstrumentsService.Domain.Configurations;

public class DataProviderConfiguration
{
    public TiingoConfiguration Tiingo { get; set; } = null!;
    public BinaceConfiguration Binace { get; set; } = null!;
    public string DeafultProvider { get; set; } = null!;
}
