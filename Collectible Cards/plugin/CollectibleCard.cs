using System;
using System.Collections.Generic;
using System.Text;

namespace CollectibleCards2
{
    public class CollectibleCard
    {
        public byte[] CardBytes { get; set; }
        public Dictionary<string, string> Metadata { get; set; } = new()
        {
                { "CharID", "" },
                { "Name", "" },
                { "FedName", "" },
                { "Border", "" },
                { "Foil", "" },
                { "Signature", "" },
                { "CustomGenerated", "" }
        };
        public CollectibleCard(byte[] cardBytes, Dictionary<string, string> metadata)
        {
            CardBytes = cardBytes;
            Metadata = metadata;
        }
    }
}
