using System;
using System.Diagnostics;
using CsvHelper.Configuration.Attributes;
using TradingViewUdfProvider.Utils;

namespace TradingViewUdfProvider.Example.Data
{
    [DebuggerDisplay("Bar {Index} {TimestampDate} {CurrentPrice}")]
    public class RangeBarModel
    {
        private DateTime? _date;

        public double Timestamp { get; set; }

        public DateTime? Date
        {
            get => _date;
            set
            {
                if (value == null)
                    return;

                _date = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
                Timestamp = _date.Value.ToUnixSeconds();
            }
        }

        public double? Mid { get; set; }

        public double? Bid { get; set; }
        public double? Ask { get; set; }

        public double? Open { get; set; }
        public double? High { get; set; }
        public double? Low { get; set; }
        public double? Close { get; set; }
        public double? Volume { get; set; }

        [Ignore] 
        public double CurrentPrice => Close ?? Mid ?? 0;

        [Ignore]
        public double InitialPrice => Open ?? Mid ?? 0;

        [Ignore]
        public int Index { get; set; }

        [Ignore]
        public DateTime TimestampDate => Date ?? TvDateTimeConverter.ConvertFromUnixSeconds(Timestamp);
    }
}
