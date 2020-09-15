using System.Text.Json.Serialization;

namespace TradingViewUdfProvider.Models
{
    public class TvSymbolInfo
    {
        /// <summary>
        /// It's the name of the symbol. It is a string that your users will be able to see. Also, it will be used for data requests if you are not using tickers.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// It's an unique identifier for this particular symbol in your symbology.
        /// If you specify this property then its value will be used for all data requests for this symbol.
        /// Ticker will be treated the same as name if not specified explicitly.
        /// </summary>
        [JsonPropertyName("ticker")]
        public string Ticker { get; set; }

        /// <summary>
        /// Description of a symbol. Will be displayed in the chart legend for this symbol.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Optional type of the instrument.
        /// Possible types are:
        /// stock 
        /// index 
        /// forex 
        /// futures 
        /// bitcoin 
        /// expression 
        /// spread 
        /// cfd 
        /// or another string value. 
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Trading hours for this symbol. See the Trading Sessions article to learn more details.
        /// https://github.com/tradingview/charting_library/wiki/Trading-Sessions
        /// </summary>
        [JsonPropertyName("session")]
        public string Session { get; set; }

        /// <summary>
        /// List of holidays for this symbol. These dates are not displayed on the chart. It's a string in the following format: 
        /// YYYYMMDD[,YYYYMMDD]. 
        /// Example: 20181105,20181107,20181112.
        /// </summary>
        [JsonPropertyName("holidays")]
        public string Holidays { get; set; }

        /// <summary>
        /// List of corrections for this symbol. Corrections are days with specific trading sessions. They can be applied to holidays as well. It's a string in the following format: 
        /// SESSION:YYYYMMDD[,YYYYMMDD][;SESSION:YYYYMMDD[,YYYYMMDD]]. 
        /// Where SESSION has the same format as Trading Sessions. 
        /// Example: 1900F4-2350F4,1000-1845:20181113;1000-1400:20181114. 
        /// </summary>
        [JsonPropertyName("corrections")]
        public string Corrections { get; set; }

        /// <summary>
        /// Expected to have a short name of the exchange where this symbol is traded.
        /// The name will be displayed in the chart legend for this symbol.
        /// </summary>
        [JsonPropertyName("exchange-traded")]
        public string ExchangeTraded { get; set; }

        /// <summary>
        /// Expected to have a short name of the exchange where this symbol is traded.
        /// The name will be displayed in the chart legend for this symbol.
        /// </summary>
        [JsonPropertyName("exchange-listed")]
        public string ExchangeListed { get; set; }

        /// <summary>
        /// Timezone of the exchange for this symbol. We expect to get the name of the time zone in olsondb format.
        /// Supported timezones are (only example):
        /// Etc/UTC
        /// Europe/Warsaw
        /// America/New_York
        /// </summary>
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = "Etc/UTC";

        /// <summary>
        /// Format of displaying labels on the price scale: 
        /// 'price' - formats decimal or fractional numbers based on minmov, pricescale, minmove2 and fractional values  
        /// 'volume' - formats decimal numbers in thousands, millions or billions 
        /// </summary>
        [JsonPropertyName("format")]
        public string Format { get; set; }

        /// <summary>
        /// Minmov is the amount of price precision steps for 1 tick.
        /// For example, since the tick size for U.S. equities is 0.01, minmov is 1.
        /// But the price of the E-mini SP500 futures contract moves upward or downward by 0.25 increments, so the minmov is 25.
        /// </summary>
        [JsonPropertyName("minmov")]
        public double? MinMov { get; set; }

        /// <summary>
        /// PriceScale defines the number of decimal places. It is 10^number-of-decimal-places.
        /// If a price is displayed as 1.01, pricescale is 100; 
        /// If it is displayed as 1.005, pricescale is 1000. 
        /// </summary>
        [JsonPropertyName("pricescale")]
        public double? PriceScale { get; set; }

        /// <summary>
        /// Minmov2 for common prices is 0 or it can be skipped.
        /// </summary>
        [JsonPropertyName("minmov2")]
        public double? MinMov2 { get; set; }

        /// <summary>
        /// Fractional for common prices is false or it can be skipped.
        /// When true, fractional prices are displayed 2 different forms: 1) xx'yy (for example, 133'21) 2) xx'yy'zz (for example, 133'21'5). 
        /// xx is an integer part. 
        /// minmov/pricescale is a Fraction. 
        /// minmove2 is used in form 2. 
        /// fractional is true 
        /// </summary>
        [JsonPropertyName("fractional")]
        public bool? Fractional { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        [JsonPropertyName("pointvalue")]
        public double? PointValue { get; set; }

        /// <summary>
        /// Default: false 
        /// Boolean value showing whether the symbol includes intraday (minutes) historical data. 
        /// If it's false then all buttons for intraday resolutions will be disabled for this particular symbol. 
        /// If it is set to true, all resolutions that are supplied directly by the datafeed must be provided in intraday_multipliers array. 
        /// </summary>
        [JsonPropertyName("has_intraday")]
        public bool? HasIntraday { get; set; }


        /// <summary>
        /// An array of resolutions which should be enabled for this symbol. 
        /// Each item of an array is expected to be a string. Format is described in another article. 
        /// If one changes the symbol and new symbol does not support the selected resolution then resolution will be switched to the first available one in the list.
        ///
        /// In case of absence of supported_resolutions in a symbol info all DWM resolutions will be available. Intraday resolutions will be available if has_intraday is true.
        /// Supported resolutions affect available timeframes too. The timeframe will not be available if it requires the resolution that is not supported.
        /// </summary>
        [JsonPropertyName("supported_resolutions")]
        public string[] SupportedResolutions { get; set; }

        /// <summary>
        /// Default: [] 
        /// Array of resolutions (in minutes) supported directly by the data feed. 
        /// Each such resolution may be passed to, and should be implemented by, getBars. 
        /// The default of [] means that the data feed supports aggregating by any number of minutes. 
        /// 
        /// If the data feed only supports certain minute resolutions but not the requested resolution,
        /// getBars will be called (repeatedly if needed) with a higher resolution as a parameter, in order to build the requested resolution. 
        /// 
        /// For example, if the data feed only supports minute resolution, set intraday_multipliers to ['1']. 
        /// When the user wants to see 5-minute data, getBars will be called with the resolution set to 1 until the library builds all the 5-minute resolution by itself. 
        /// </summary>
        [JsonPropertyName("intraday_multipliers")]
        public string[] IntradayMultipliers { get; set; }

        /// <summary>
        /// Default: false 
        /// The boolean value showing whether data feed has its own daily resolution bars or not. 
        /// If has_daily = false then Charting Library will build the respective resolutions using 1-minute bars by itself. If not, then it will request those bars from the data feed. 
        /// </summary>
        [JsonPropertyName("has_daily")]
        public bool? HasDaily { get; set; }

        /// <summary>
        /// Default: false 
        /// Boolean value showing whether the symbol includes seconds in the historical data. 
        /// If it's false then all buttons for resolutions that include seconds will be disabled for this particular symbol. 
        /// If it is set to true, all resolutions that are supplied directly by the data feed must be provided in seconds_multipliers array. 
        /// </summary>
        [JsonPropertyName("has_seconds")]
        public bool? HasSeconds { get; set; }

        /// <summary>
        /// Default: [] 
        /// It is an array containing resolutions that include seconds (excluding postfix) that the data feed provides. 
        /// E.g., if the data feed supports resolutions such as ["1S", "5S", "15S"], but has 1-second bars for some symbols 
        /// then you should set seconds_multipliers of this symbol to [1]. This will make Charting Library build 5S and 15S resolutions by itself. 
        /// </summary>
        [JsonPropertyName("seconds_multipliers")]
        public string[] SecondsMultipliers { get; set; }

        /// <summary>
        /// Default: false 
        /// The boolean value showing whether data feed has its own weekly and monthly resolution bars or not. 
        /// If has_weekly_and_monthly = false then Charting Library will build the respective resolutions using daily bars by itself. If not, then it will request those bars from the data feed. 
        /// </summary>
        [JsonPropertyName("has_weekly_and_monthly")]
        public bool? HasWeeklyAndMonthly { get; set; }

        /// <summary>
        /// Default: false 
        /// The boolean value showing whether the library should generate empty bars in the session when there is no data from the data feed for this particular time. 
        /// I.e., if your session is 0900-1600 and your data has gaps between 11:00 and 12:00 and your has_empty_bars is true, then the Library will fill the gaps with bars for this time. 
        /// </summary>
        [JsonPropertyName("has_empty_bars")]
        public bool? HasEmptyBars { get; set; }

        /// <summary>
        /// Default: true 
        /// The boolean value showing whether the library should filter bars using the current trading session. 
        /// If false, bars will be filtered only when the library builds data from another resolution or if has_empty_bars was set to true. 
        /// If true, then the Library will remove bars that don't belong to the trading session from your data. 
        /// </summary>
        [JsonPropertyName("force_session_rebuild")]
        public bool? ForceSessionRebuild { get; set; }

        /// <summary>
        /// Default: false 
        /// Boolean showing whether the symbol includes volume data or not. 
        /// </summary>
        [JsonPropertyName("has_no_volume")]
        public bool? HasNoVolume { get; set; }

        /// <summary>
        /// Default: 0 
        /// Integer showing typical volume value decimal places for a particular symbol. 
        /// 0 means volume is always an integer. 
        /// 1 means that there might be 1 numeric character after the comma. 
        /// </summary>
        [JsonPropertyName("volume_precision")]
        public int? VolumePrecision { get; set; }

        /// <summary>
        /// The status code of a series with this symbol. The status is shown in the upper right corner of a chart. 
        /// Supported statuses are: 
        /// streaming 
        /// endofday 
        /// pulsed 
        /// delayed_streaming 
        /// </summary>
        [JsonPropertyName("data_status")]
        public string DataStatus { get; set; }

        /// <summary>
        /// Default: false 
        /// Boolean value showing whether this symbol is an expired futures contract or not. 
        /// </summary>
        [JsonPropertyName("expired")]
        public bool? Expired { get; set; }

        /// <summary>
        /// Unix timestamp of the expiration date. One must set this value when expired = true. 
        /// Charting Library will request data for this symbol starting from that time point. 
        /// </summary>
        [JsonPropertyName("expiration_date")]
        public string ExpirationDate { get; set; }

        /// <summary>
        /// Sector for stocks to be displayed in the Symbol Info.
        /// </summary>
        [JsonPropertyName("sector")]
        public string Sector { get; set; }

        /// <summary>
        /// Industry for stocks to be displayed in the Symbol Info. 
        /// </summary>
        [JsonPropertyName("industry")]
        public string Industry { get; set; }

        /// <summary>
        /// The currency in which the instrument is traded.
        /// </summary>
        [JsonPropertyName("original_currency_code")]
        public string OriginalCurrencyCode { get; set; }

        /// <summary>
        /// The currency in which the instrument is traded or some other currency if currency conversion is enabled.
        /// It is displayed in the Symbol Info dialog and on the price axes.
        /// </summary>
        [JsonPropertyName("currency_code")]
        public string CurrencyCode { get; set; }
    }
}
