using System;

namespace TradingViewUdfProvider.Models
{
    public class TvBarResponse
    {
        /// <summary>
        /// Status code. Expected values: ok | error | no_data
        /// </summary>
        public TvBarStatus Status { get; set; }

        /// <summary>
        /// Error message. Should be present only when status = 'error'
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Bars for selected time range
        /// </summary>
        public TvBar[] Bars { get; set; }

        /// <summary>
        /// Should be the time of the closest available bar in the past if there is no data (status code is no_data) in the requested period (optional).
        /// </summary>
        public DateTime? NextTime { get; set; }
    }
}
