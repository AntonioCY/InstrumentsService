namespace InstrumentsService.Domain.Entities.Binance;

public class BinancePriceResponse
{
    public string Id { get; set; } = null!;
    public int Status { get; set; }
    public BinanceResult Result { get; set; }
    public List<RateLimit>? RateLimits { get; set; }
}

public class BinanceResult
{
    public string Symbol { get; set; } = null!;
    public double Price { get; set; }
    public Int64 Time { get; set; }
}

public class RateLimit
{
    public string RateLimitType { get; set; } = null!;
    public string Interval { get; set; } = null!;
    public int IntervalNum { get; set; }
    public int Count { get; set; }
}
