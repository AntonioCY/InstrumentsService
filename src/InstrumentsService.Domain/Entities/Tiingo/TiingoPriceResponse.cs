namespace InstrumentsService.Domain.Entities.Tiingo;

public class TingoPriceResponse
{
    public string Ticker { get; set; } = null!;
    public string QuoteTimeStamp { get; set; } = null!;
    public double BidPrice { get; set; }
    public double BidSize { get; set; }
    public double AskPrice { get; set; }
    public double AskSize { get; set; }
    public double MidAskSize { get; set; }
}
