using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TradingViewUdfProvider.Models;
using TradingViewUdfProvider.Utils;

namespace TradingViewUdfProvider.Controllers
{
    /// <summary>
    ///  Controller that implements TradingView compatible data provider
    /// </summary>
    [TvRoute]
    [TvApiExplorerSettings]
    public class TradingViewController : Controller
    {
        private readonly ITradingViewProvider _provider;

        public TradingViewController(ITradingViewProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider), 
                            "TradingView provider is not set. Please call AddTradingViewProvider() in Startup.cs");
        }

        /// <summary>
        /// Get initial configuration
        /// </summary>
        [Route("config")]
        [HttpGet]
        public Task<TvConfiguration> GetConfig()
        {
            return _provider.GetConfiguration();
        }

        /// <summary>
        /// Get info for single symbol
        /// </summary>
        [Route("symbols")]
        [HttpGet]
        public Task<TvSymbolInfo> GetSymbol([FromQuery] string symbol = null)
        {
            return _provider.GetSymbol(symbol);
        }

        /// <summary>
        /// Find available symbols by specified values
        /// </summary>
        [Route("search")]
        [HttpGet]
        public Task<TvSymbolSearch[]> FindSymbols(
            [FromQuery] string query = null, [FromQuery] string type = null,
            [FromQuery] string exchange = null, [FromQuery] int? limit = null)
        {
            return _provider.FindSymbols(query, type, exchange, limit);
        }

        [Route("history")]
        [HttpGet]
        public async Task<OkObjectResult> GetHistory([FromQuery] long from, [FromQuery] long to, 
            [FromQuery] string symbol, [FromQuery] string resolution)
        {
            var fromDate = TvDateTimeConverter.ConvertFromUnixSeconds(from);
            var toDate = TvDateTimeConverter.ConvertFromUnixSeconds(to);

            var response = await _provider.GetHistory(fromDate, toDate, symbol, resolution);
            var bars = response?.Bars;

            var nextTime = response?.NextTime?.ToUnixSeconds();
            var nextTimeLong = (long?) nextTime;

            if (response?.Bars == null)
            {
                if(nextTimeLong != null)
                    return Ok(
                        new
                        {
                            status = "no_data",
                            nextTime = nextTimeLong
                        });
                return Ok(
                    new
                    {
                        status = "error",
                        errmsg = "missing data"
                    });
            }

            var status = GetStatus(response);
            var timestamps = bars.Select(q => q.Timestamp.ToUnixSeconds()).ToArray();
            var closing = bars.Select(q => q.Close).ToArray();
            var opening = bars.Select(q => q.Open).ToArray();
            var high = bars.Select(q => q.High).ToArray();
            var low = bars.Select(q => q.Low).ToArray();
            var volume = bars.Select(q => q.Volume).ToArray();

            return Ok(new
            {
                s = status,
                t = timestamps,
                c = closing,
                o = ArrayOrNull(opening),
                h = ArrayOrNull(high),
                l = ArrayOrNull(low),
                v = ArrayOrNull(volume),
                errmsg = response.ErrorMessage,
                nextTime = nextTimeLong
            });
        }

        [Route("marks")]
        [HttpGet]
        public async Task<OkObjectResult> GetMarks([FromQuery] long from, [FromQuery] long to, 
            [FromQuery] string symbol, [FromQuery] string resolution)
        {
            var fromDate = TvDateTimeConverter.ConvertFromUnixSeconds(from);
            var toDate = TvDateTimeConverter.ConvertFromUnixSeconds(to);

            var response = await _provider.GetMarks(fromDate, toDate, symbol, resolution);
            var marks = response ?? new TvMark[0];
          
            var timestamps = marks.Select(q => q.Timestamp.ToUnixSeconds()).ToArray();
            var ids = marks.Select(q => q.Id).ToArray();
            var labels = marks.Select(q => q.Label).ToArray();
            var labelFonts = marks.Select(q => q.LabelFontColor).ToArray();
            var colors = marks.Select(q => q.Color).ToArray();
            var texts = marks.Select(q => q.Text).ToArray();
            var sizes = marks.Select(q => q.MinSize).ToArray();

            return Ok(new
            {
                id = ids,
                time = timestamps,
                label = labels,
                labelFontColor = labelFonts,
                text = texts,
                color = colors,
                minSize = sizes,
            });
        }

        private static string GetStatus(TvBarResponse response)
        {
            switch (response.Status)
            {
                case TvBarStatus.Error:
                    return "error";
                case TvBarStatus.NoData:
                    return "no_data";
            }

            return "ok";
        }

        private double?[] ArrayOrNull(double?[] values)
        {
            if (values == null)
                return null;
            if (values.Any(x => x > 0))
                return values;
            return null;
        }
    }
}
