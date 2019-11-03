using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxLibrary
{
    public class BankJSON
    {
        [JsonProperty(nameof(Date))]
        public string Date { get; set; }

        [JsonProperty(nameof(PreviousDate))]
        public string PreviousDate { get; set; }

        [JsonProperty(nameof(PreviousURL))]
        public string PreviousURL { get; set; }

        [JsonProperty(nameof(Timestamp))]
        public string Timestamp { get; set; }

        [JsonProperty(nameof(Valute))]
        public Dictionary<string, Valute> Valute { get; set; }
    }

    public class Valute
    {
        [JsonProperty(nameof(ID))]
        public string ID { get; set; }

        [JsonProperty(nameof(NumCode))]
        public int NumCode { get; set; }

        [JsonProperty(nameof(CharCode))]
        public string CharCode { get; set; }

        [JsonProperty(nameof(Nominal))]
        public int Nominal { get; set; }

        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Value))]
        public decimal Value { get; set; }

        [JsonProperty(nameof(Previous))]
        public decimal Previous { get; set; }
    }

}
