using System.Text.Json.Serialization;

namespace TradingViewUdfProvider.Models
{
    public class TvConfiguration
    {
        [JsonPropertyName("supported_resolutions")]
        public string[] SupportedResolutions { get; set; } = {"1", "5", "15", "30", "60", "1D", "1W", "1M"};

        /// <summary>
        /// Set it to true if your data feed provides full information on symbol group only and is not able to perform symbol search or individual symbol resolve.
        /// Either supports_search or supports_group_request should be set to true.
        /// </summary>
        [JsonPropertyName("supports_group_request")]
        public bool SupportGroupRequest { get; set; }

        [JsonPropertyName("supports_marks")]
        public bool SupportMarks { get; set; }

        /// <summary>
        /// Set it to true if your data feed supports symbol search and individual symbol resolve logic.
        /// Either supports_search or supports_group_request should be set to true.
        /// </summary>
        [JsonPropertyName("supports_search")]
        public bool SupportSearch { get; set; }

        [JsonPropertyName("supports_timescale_marks")]
        public bool SupportTimeScaleMarks { get; set; }
    }
}
