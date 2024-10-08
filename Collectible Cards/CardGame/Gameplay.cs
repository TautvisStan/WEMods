using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Runtime.CompilerServices;

namespace CardGame
{
    public static class Gameplay
    {
        public static int players = 2;
        public static PlayableCard[] Cards = null;
        public static int P1Score = 0;
        public static int P2Score = 0;
        public static void Reset()
        {
            for (int i = 0; i < players; i++) 
            {
                Cards[i] = null;
            }
            P1Score = 0;
            P2Score = 0;
        }
        public static void ReceiveCard(PlayableCard card, int playerIndex)
        {
            if(Cards == null)
            {
                Cards = new PlayableCard[players];
            }
            Cards[playerIndex] = card;
            if (Cards[0] != null && Cards[1] != null) 
            {
                CompareCards(Cards[0], Cards[1]);
                Reset();
            }
        }
        public static void CompareCards(PlayableCard card1,  PlayableCard card2)
        {
            Debug.LogWarning("Comparing cards...");
            Debug.LogWarning($"Card 1: {card1.WrestlerName}");
            Debug.LogWarning($"Card 2: {card2.WrestlerName}");

            CompareStat("Popularity", card1.Popularity, card2.Popularity);
            CompareStat("Strength", card1.Strength, card2.Strength);
            CompareStat("Skill", card1.Skill, card2.Skill);
            CompareStat("Agility", card1.Agility, card2.Agility);
            CompareStat("Stamina", card1.Stamina, card2.Stamina);
            CompareStat("Attitude", card1.Attitude, card2.Attitude);
            if(P1Score == P2Score)
            {
                Debug.LogWarning("Bonus!");
                CompareStat("Overall", card1.CalculateOverall(), card2.CalculateOverall());
            }
            if(P1Score == P2Score) 
            {
                Debug.LogWarning("THIS ROUND WAS A DRAW!");
            }
            else if (P1Score > P2Score)
            {
                Debug.LogWarning("P1 Wins!");
            }
            else
            {
                Debug.LogWarning("P2 Wins!");
            }
        }
        public static void CompareStat(string stat, float card1stat, float card2stat)
        {
            Debug.LogWarning($"Comparing {stat}: {card1stat} vs {card2stat}");
            int result = CompareSingleNumbers(card1stat, card2stat);
            if (result < 0)
            {
                Debug.LogWarning("Card 1 wins.");
                P1Score++;
            }
            else if (result > 0)
            {
                Debug.LogWarning("Card 2 wins.");
                P2Score++;
            }
            else
            {
                Debug.LogWarning("Draw!");
            }
            Debug.LogWarning($"Score: {P1Score}-{P2Score}");
        }
        public static int CompareSingleNumbers(float a, float b)
        {
            if (a > b) { return -1; }
            else if (a < b) { return 1; }
            else { return 0; }
        }
    }
}
