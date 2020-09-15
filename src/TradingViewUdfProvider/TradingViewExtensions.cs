using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TradingViewUdfProvider.Models;

namespace TradingViewUdfProvider
{
    /// <summary>
    /// Startup extensions to easily setup TradingView provider
    /// </summary>
    public static class TradingViewExtensions
    {
        /// <summary>
        /// Integrate TradingView provider into solution.
        /// Register your data provider into DI under ITradingViewProvider interface. 
        /// </summary>
        /// <typeparam name="TProvider">Your data provider</typeparam>
        public static void AddTradingViewProvider<TProvider>(this IServiceCollection services) where TProvider : class, ITradingViewProvider
        {
            services.AddTransient<ITradingViewProvider, TProvider>();
            TvApiExplorerSettingsAttribute.DisplayEndpoints = true;
        }

        /// <summary>
        /// Configure TradingView provider. Optional!
        /// </summary>
        public static void UseTradingViewProvider(this IApplicationBuilder app, TradingViewSettings config)
        {
            if (config == null)
                return;

            TvApiExplorerSettingsAttribute.DisplayEndpoints = !config.HideEndpoints;
            //TvRouteAttribute.BaseUrl = config.BaseUrl;
        }
    }
}
