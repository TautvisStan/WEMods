using System;
using System.Collections.Generic;
using System.Text;

namespace CollectibleCards2
{
    public class CollectibleCard
    {
        public string CardName { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new()
        {
            { "CharID", "" },
            { "CharName", "" },
            { "FedName", "" },
            { "BorderRarity", "" },
            { "FoilRarity", "" },
            { "SignatureRarity", "" }
        };
    }
}
