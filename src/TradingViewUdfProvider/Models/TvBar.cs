using System;

namespace TradingViewUdfProvider.Models
{
    public class TvBar
    {
        /// <summary>
        /// Bar time. Unix timestamp (UTC)
        /// </summary>
        public DateTime Timestamp {get ; set; }

        /// <summary>
        /// Closing price
        /// </summary>
        public double Close { get; set; }

        /// <summary>
        /// Opening price (optional)
        /// </summary>
        public double? Open { get; set; }

        /// <summary>
        /// High price (optional)
        /// </summary>
        public double? High { get; set; }

        /// <summary>
        /// Low price (optional)
        /// </summary>
        public double? Low { get; set; }

        
        /// <summary>
        /// Volume (optional)
        /// </summary>
        public double? Volume { get; set; }
    }
}
