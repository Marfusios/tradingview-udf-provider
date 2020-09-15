using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using TradingViewUdfProvider.Example.Data;
using TradingViewUdfProvider.Models;

namespace TradingViewUdfProvider.Example
{
    public class MyTvProvider : ITradingViewProvider
    {
        private static readonly Dictionary<string, TvBar[]> _bars = new Dictionary<string, TvBar[]>();

        public Task<TvConfiguration> GetConfiguration()
        {
            var config = new TvConfiguration
            {
                SupportedResolutions = new[] {"60","120","240","D","2D","3D","W","3W","M","6M"},
                SupportGroupRequest = false,
                SupportMarks = true,
                SupportSearch = true,
                SupportTimeScaleMarks = false
            };
            return Task.FromResult(config);
        }

        public Task<TvSymbolInfo> GetSymbol(string symbol)
        {
            var symbolSafe = (symbol ?? string.Empty)
                .Replace("Crypto", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace(":", string.Empty);

            var sym = new TvSymbolInfo()
            {
                Name = symbolSafe,
                Ticker = symbolSafe,
                Description = "Bitcoin",
                Type = "bitcoin",
                ExchangeTraded = "Crypto",
                ExchangeListed = "Crypto",
                //Timezone = "America/New_York",
                MinMov = 1,
                MinMov2 = 0,
                PriceScale = 100,
                //PointValue = 1,
                //Session = "0930-1630",
                Session = "24x7",
                HasIntraday = true,
                IntradayMultipliers = new []{ "60" },
                HasNoVolume = false,
                SupportedResolutions = new []{"60","120","240","D","2D","3D","W","3W","M","6M"},
                CurrencyCode = "USD",
                OriginalCurrencyCode = "USD",
                VolumePrecision = 2

            };
            return Task.FromResult(sym);
        }

        public Task<TvSymbolSearch[]> FindSymbols(string query, string type, string exchange, int? limit)
        {
            var querySafe = query ?? string.Empty;
            var symbols = new[]
            {
                new TvSymbolSearch {Symbol = "AAPL", FullName = "Apple", Description = "Apple Inc", Type = "stock"},
                new TvSymbolSearch {Symbol = "MSFT", FullName = "Microsoft", Description = "Microsoft Inc", Type = "stock"},
                new TvSymbolSearch {Symbol = "BTC", FullName = "Bitcoin", Description = "Only Bitcoin!", Type = "bitcoin"},
            };
            var found = symbols
                .Where(x => x.Symbol.Contains(querySafe, StringComparison.InvariantCultureIgnoreCase) ||
                            x.FullName.Contains(querySafe, StringComparison.InvariantCultureIgnoreCase) ||
                            x.Description.Contains(querySafe, StringComparison.InvariantCultureIgnoreCase))
                .Take(limit ?? 100)
                .ToArray();
            return Task.FromResult(found);
        }

        public async Task<TvBarResponse> GetHistory(DateTime @from, DateTime to, string symbol, string resolution)
        {
            var key = $"{symbol}__{resolution}";
            if (_bars.ContainsKey(key))
            {
                var bars = _bars[key];
                return FindBars(from, to, bars);
            }

            RangeBarModel[] loaded = new RangeBarModel[0];

            if (resolution.Equals("d", StringComparison.InvariantCultureIgnoreCase) ||
                resolution.Equals("1d", StringComparison.InvariantCultureIgnoreCase))
            {
                loaded = LoadBars("Data\\bitfinex_btcusd_ohlcv_1d_2017_2020.csv");
                //loaded = LoadBars("Data\\bitflyer_btcusd_ohlcv_1d_2019.csv", "ms");
            }

            if (resolution.Equals("h", StringComparison.InvariantCultureIgnoreCase) ||
                resolution.Equals("1h", StringComparison.InvariantCultureIgnoreCase) ||
                resolution.Equals("60", StringComparison.InvariantCultureIgnoreCase))
            {
                loaded = LoadBars("Data\\bitstamp_btcusd_ohlcv_1h_2020.csv", "ms");
            }

            var converted = ConvertBars(loaded);
            _bars[key] = converted;
            return FindBars(from, to, converted);
        }

        public Task<TvMark[]> GetMarks(DateTime @from, DateTime to, string symbol, string resolution)
        {
            var key = $"{symbol}__{resolution}";
            if (!_bars.ContainsKey(key))
                return Task.FromResult(new TvMark[0]);

            var bars = _bars[key].OrderByDescending(x => x.Timestamp).ToArray();
            var first = bars.Skip(10).FirstOrDefault();
            var last = bars.Skip(2).FirstOrDefault();

            var marks = new[]
            {
                new TvMark {Id = 1, Color = "red", Label = "S", LabelFontColor = "black", MinSize = 15, Text = "Sell", Timestamp = last?.Timestamp ?? DateTime.UtcNow},
                new TvMark {Id = 2, Color = "blue", Label = "B", LabelFontColor = "black", MinSize = 10, Text = "Buy", Timestamp = first?.Timestamp ?? DateTime.UtcNow},
            };

            var foundMarks = marks
                .Where(x => x.Timestamp >= from && x.Timestamp <= to)
                .OrderBy(x => x.Timestamp)
                .ToArray();

            return Task.FromResult(foundMarks);
        }

        private TvBar[] ConvertBars(RangeBarModel[] loaded)
        {
            return loaded
                .Select(ConvertBar)
                .ToArray();
        }

        private TvBar ConvertBar(RangeBarModel arg)
        {
            return new TvBar()
            {
                Timestamp = arg.TimestampDate,
                Close = arg.Close ?? 0,
                Open = arg.Open,
                High = arg.High,
                Low = arg.Low,
                Volume = arg.Volume
            };
        }

        private TvBarResponse FindBars(DateTime @from, DateTime to, TvBar[] bars)
        {
            var foundBars = bars
                .Where(x => x.Timestamp >= RemoveTime(from) && x.Timestamp <= RemoveTime(to))
                .OrderBy(x => x.Timestamp)
                .ToArray();
            var before = bars
                .OrderBy(x => x.Timestamp)
                .LastOrDefault(x => x.Timestamp < RemoveTime(@from));
            return new TvBarResponse()
            {
                Bars = foundBars,
                Status = foundBars.Any() ? TvBarStatus.Ok : TvBarStatus.NoData,
                NextTime = before?.Timestamp
            };
        }

        private DateTime RemoveTime(in DateTime timestamp)
        {
            return timestamp;
            //return new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, 
            //    0, 0, 0, DateTimeKind.Utc);
        }


        private static RangeBarModel[] LoadBars(string file, string timestampType = null)
        {
            using var reader = new StreamReader(file);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Configuration.PrepareHeaderForMatch = (header, index) => header.ToLower();
            csv.Configuration.HeaderValidated = null;
            csv.Configuration.MissingFieldFound = null;
            var bars = csv.GetRecords<RangeBarModel>().ToArray();
            FixTimestamp(bars, timestampType);
            var ordered = bars.OrderBy(x => x.TimestampDate).ToArray();
            return ordered;
        }

        private static void FixTimestamp(RangeBarModel[] bars,  string timestampType = null)
        {
            if (string.IsNullOrWhiteSpace(timestampType) || 
                timestampType == "unix-sec" ||
                timestampType == "date")
            {
                // default valid timestamp format, do nothing
                return;
            }

            foreach (var bar in bars)
            {
                var t = bar.Timestamp;
                var d = 3;
                var converted = t;

                switch (timestampType)
                {
                    case "unix-ms":
                    case "ms":
                        converted = t / (Math.Pow(10, d));
                        break;
                }

                bar.Timestamp = converted;
            }
        }
    }
}

// {"name":"AAPL","exchange-traded":"NasdaqNM","exchange-listed":"NasdaqNM","timezone":"America/New_York","minmov":1,"minmov2":0,"pointvalue":1,
// "session":"0930-1630","has_intraday":false,"has_no_volume":false,"description":"Apple Inc.","type":"stock",
// "supported_resolutions":["D","2D","3D","W","3W","M","6M"],"pricescale":100,"ticker":"AAPL"}


// {"supports_search":true,"supports_group_request":false,"supports_marks":true,"supports_timescale_marks":true,"supports_time":true,
// "exchanges":[{"value":"","name":"All Exchanges","desc":""},{"value":"NasdaqNM","name":"NasdaqNM","desc":"NasdaqNM"},
// {"value":"NYSE","name":"NYSE","desc":"NYSE"},{"value":"NCM","name":"NCM","desc":"NCM"},{"value":"NGM","name":"NGM","desc":"NGM"}],
// "symbols_types":[{"name":"All types","value":""},{"name":"Stock","value":"stock"},{"name":"Index","value":"index"}],"supported_resolutions":["D","2D","3D","W","3W","M","6M"]}