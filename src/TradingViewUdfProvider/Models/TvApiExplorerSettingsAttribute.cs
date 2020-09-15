using Microsoft.AspNetCore.Mvc;

namespace TradingViewUdfProvider.Models
{
    /// <summary>
    /// ApiExplorerSettings for TradingView controller
    /// </summary>
    internal class TvApiExplorerSettingsAttribute : ApiExplorerSettingsAttribute
    {
        /// <summary>
        /// Internal global variable, because of attribute
        /// </summary>
        internal static bool DisplayEndpoints { get; set; } = false;

        public TvApiExplorerSettingsAttribute()
        {
            IgnoreApi = !DisplayEndpoints;
        }
    }
}
