using System.Text.Json.Serialization;

namespace TradingViewUdfProvider.Models
{
    public class TvSymbolSearch
    {
        /// <summary>
        /// Short symbol name
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Full symbol name, // e.g. BTCE:BTCUSD
        /// </summary>
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// Symbol ticker name, optional
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Symbol description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Symbol exchange name
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// "stock" or "futures" or "bitcoin" or "forex" or "index"
        /// </summary>
        public string Type { get; set; }
        
    }
}
