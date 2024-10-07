using CollectibleCards2;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGame
{
    public class PlayableCard
    {
        public string WrestlerName { get; set; }
        public int Popularity { get; set; }
        public int Strength { get; set; }
        public int Skill { get; set; }
        public int Agility { get; set; }
        public int Stamina { get; set; }
        public int Attitude { get; set; }
        public PlayableCard(CollectibleCard card)
        {
            WrestlerName = card.GetName();
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
            return (Popularity + Strength + Skill + Agility + Stamina + Attitude) / 6;
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
