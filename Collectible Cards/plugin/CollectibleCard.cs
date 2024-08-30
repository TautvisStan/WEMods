using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CollectibleCards2
{
    public class CollectibleCard
    {
        public string FileName { get; set; }
        public byte[] CardBytes { get; set; }
        public string CharID { get; set; }
        public string Name { get; set; }
        public string FedName { get; set; }
        public string Border { get; set; }
        public string Foil { get; set; }
        public string Signature { get; set; }
        public string CustomGenerated { get; set; }
        public string Popularity { get; set; }
        public string Strength { get; set; }
        public string Skill { get; set; }
        public string Agility { get; set; }
        public string Stamina { get; set; }
        public string Attitude { get; set; }
        public string FrontFinisher { get; set; }
        public string BackFinisher { get; set; }

        public CollectibleCard(string fileName, byte[] cardBytes, Dictionary<string, string> metadata)
        {
            FileName = fileName;
            CardBytes = cardBytes;
            CharID = metadata["CharID"];
            Name = metadata["Name"];
            FedName = metadata["FedName"];
            Border = metadata["Border"];
            Foil = metadata["Foil"];
            Signature = metadata["Signature"];
            CustomGenerated = metadata["CustomGenerated"];
            Popularity = metadata["Popularity"];
            Strength = metadata["Strength"];
            Skill = metadata["Skill"];
            Agility = metadata["Agility"];
            Stamina = metadata["Stamina"];
            Attitude = metadata["Attitude"];
            FrontFinisher = metadata["FrontFinisher"];
            BackFinisher = metadata["BackFinisher"];
        }

        public string GetDescription()
        {
            string text = "";
            if (IsCustom()) text += " Custom Generated";
            if (IsSigned()) text += " Signed";
            text += GetBorder();
            if (IsFoil()) text += " Foil";
            text += GetID();
            text += GetName();
            text += GetFed();
            return text.Trim();
        }
        public bool IsCustom()
        {
            if (CustomGenerated.ToLower() == "true") return true;
            else return false;
        }
        public bool IsSigned()
        {
            if (Signature != "" && Signature.ToLower() != "0") return true;
            else return false;
        }
        public string GetBorder()
        {
            if (Border == "" || Border == "0") return "";
            if (Border == "1") return " Bronze";
            if (Border == "2") return " Silver";
            if (Border == "3") return " Gold";
            return "";
        }
        public bool IsFoil()
        {
            if (Foil != "" && Foil.ToLower() != "0") return true;
            else return false;
        }
        public string GetID()
        {
            if (CharID != "") return $" [{CharID}]";
            return "";
        }
        public string GetName()
        {
            if (Name != "") return $" {Name}";
            return "";
        }
        public string GetFed()
        {
            if (FedName != "") return $" ({FedName})";
            return "";
        }
    }
}
