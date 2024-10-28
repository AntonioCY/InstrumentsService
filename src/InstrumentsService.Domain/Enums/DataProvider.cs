using System.ComponentModel.DataAnnotations;

namespace InstrumentsService.Domain.Enums;

public enum DataProvider
{
    [Display(Name = "Tiingo")]
    Tiingo,
    [Display(Name = "Binance")]
    Binance
}
