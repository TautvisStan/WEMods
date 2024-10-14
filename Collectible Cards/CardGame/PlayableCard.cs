using CollectibleCards2;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public class PlayableCard
    {
        public string WrestlerName { get; set; }
        internal int LocalIndex { get; set; }
        public int Popularity { get; set; }
        public int Strength { get; set; }
        public int Skill { get; set; }
        public int Agility { get; set; }
        public int Stamina { get; set; }
        public int Attitude { get; set; }
        public PlayableCard(CollectibleCard card)
        {
            WrestlerName = card.GetName();
            LocalIndex = card.Index-1;
            Popularity = int.Parse(card.Popularity);
            Strength = int.Parse(card.Strength);
            Skill = int.Parse(card.Skill);
            Agility = int.Parse(card.Agility);
            Stamina = int.Parse(card.Stamina);
            Attitude = int.Parse(card.Attitude);
        }
        public PlayableCard()
        {

        }
        public float CalculateOverall()
        {
            float stats = (Popularity + Strength + Skill + Agility + Stamina + Attitude) / 6f;
            return (float)Math.Round(stats, 2);
        }
        public string GetStatsString()
        {
            string text = "";
            text = $"Pop: {Popularity}; Str: {Strength}; Skl: {Skill}; Agl: {Agility}; Sta: {Stamina}; Att: {Attitude}; Ovr: {CalculateOverall()}";
            return text.Trim();
        }
        public static bool VerifyCard(CollectibleCard card)
        {
            if (card.GetName() == "") return false;
            if (card.Popularity == "") return false;
            if (card.Strength == "") return false;
            if (card.Skill == "") return false;
            if (card.Agility == "") return false;
            if (card.Stamina == "") return false;
            if (card.Attitude == "") return false;
            return true;
        }
    }
}
