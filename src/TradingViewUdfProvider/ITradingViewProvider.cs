using System;
using System.Threading.Tasks;
using TradingViewUdfProvider.Models;

namespace TradingViewUdfProvider
{
    /// <summary>
    /// TradingView data provider
    /// </summary>
    public interface ITradingViewProvider
    {
        /// <summary>
        /// Get initial configuration
        /// </summary>
        Task<TvConfiguration> GetConfiguration();

        /// <summary>
        /// Get info for single symbol
        /// </summary>
        Task<TvSymbolInfo> GetSymbol(string symbol);

        /// <summary>
        /// Find available symbols by specified values
        /// </summary>
        Task<TvSymbolSearch[]> FindSymbols(string query, string type, string exchange, int? limit);

        /// <summary>
        /// Get historical data for specified time range.
        /// 1. Bar time for daily bars should be 00:00 UTC and is expected to be a trading day (not a day when the session starts).
        ///    Charting Library aligns the time according to the Session from SymbolInfo.  
        /// 2. Bar time for monthly bars should be 00:00 UTC and is the first trading day of the month.  
        /// 3. Prices should be passed as numbers and not as strings in quotation marks.  
        /// </summary>
        Task<TvBarResponse> GetHistory(DateTime from, DateTime to, string symbol, string resolution);

        /// <summary>
        /// Get marks inside specified time range.  
        /// This call will be requested if your data feed sent supports_marks: true in the configuration data.
        /// </summary>
        Task<TvMark[]> GetMarks(DateTime from, DateTime to, string symbol, string resolution);
    }
}
