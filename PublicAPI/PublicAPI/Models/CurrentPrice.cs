using System;

namespace PublicAPI.Models
{
    public class CurrentPrice
    {
        public Timeinfo time { get; set; }
        public string disclaimer { get; set; }
        public string chartName { get; set; }
        public Bpi bpi { get; set; }
    }

    public class Bpi
    {
        public CurrencyInfo USD { get; set; }
        public CurrencyInfo GBP { get; set; }
        public CurrencyInfo EUR { get; set; }
    }

    public class CurrencyInfo
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Rate { get; set; }
        public string Description { get; set; }
        public float Rate_Float { get; set; }
    }

    public class Timeinfo
    {
        public string Updated { get; set; }
        public DateTime UpdatedISO { get; set; }
        public string Updateduk { get; set; }  // Corrected property name
    }
}
