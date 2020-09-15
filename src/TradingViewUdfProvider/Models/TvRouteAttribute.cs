using Microsoft.AspNetCore.Mvc;

namespace TradingViewUdfProvider.Models
{
    internal class TvRouteAttribute : RouteAttribute
    {
        /// <summary>
        /// Internal global variable, because of attribute
        /// </summary>
        internal static string BaseUrl { get; set; } = "api/trading-view/udf";

        public TvRouteAttribute() : base(BaseUrl)
        {
        }
    }
}
