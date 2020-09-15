namespace TradingViewUdfProvider
{
    public class TradingViewSettings
    {
        // TODO: fix it
        ///// <summary>
        ///// Base relative TradingView provider url, default: 'api/trading-view/udf'
        ///// </summary>
        //public string BaseUrl { get; set; } = "api/trading-view/udf";

        /// <summary>
        /// Hide TradingView provider endpoints from api explorer (Swagger, etc)
        /// </summary>
        public bool HideEndpoints { get; set; }
    }
}
