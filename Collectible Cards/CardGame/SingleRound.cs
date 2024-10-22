using CollectibleCards2;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace CardGame
{
    public static class SingleRound
    {
        public static PlayableCard[] PlayedCards { get; set; } = null;
        public static int P1Score { get; set; } = 0;
        public static int P2Score { get; set; } = 0;
        public static PlayableCardDisplay[] PlayedCardElements { get; set; } = new PlayableCardDisplay[2];
        public static GameObject RoundText { get; set; } = null;
        public class PlayableCardDisplay
        {
            public int DisplayIndex;
            private RawImage rawImage { get; set; }
            public Texture2D texture2D { get; set; } = null;
            public GameObject CardObject { get; set; } = null;
            public PlayableCard Card { get; set; } = null;
            public Vector2 Position { get; set; } = new();
            public GameObject CardStats { get; set; } = null;

            public void DisplayCard()
            {
                if (Card != null)
                {
                    if (CardObject == null)
                    {
                        CardObject = new("card");
                        CardObject.transform.SetParent(LIPNHOMGGHF.JPABICKOAEO.transform, false);
                        rawImage = CardObject.AddComponent<RawImage>();

                    }
                    else
                    {
                        rawImage = CardObject.GetComponent<RawImage>();
                        //  texture2D = (Texture2D)rawImage.texture;
                    }
                    if (texture2D == null)
                        texture2D = new Texture2D(1, 1);
                    rawImage.texture = texture2D;
                    RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(texture2D.width / 2, texture2D.height / 2);
                    rectTransform.anchoredPosition = Position;
                    rawImage.transform.SetAsLastSibling();
                    DisplayCardInfo();
                }

            }
            public void DisplayCardInfo()
            {
                if (CardStats == null)
                {
                    CardStats = new GameObject("Description");
                    CardStats.transform.SetParent(LIPNHOMGGHF.JPABICKOAEO.transform, false);
                    CardStats.AddComponent<Text>().font = CardMenu.VanillaFont;
                    CardStats.AddComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
                    CardStats.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
                    CardStats.AddComponent<Shadow>().effectDistance = new Vector2(3, -3);


                }
                Text text = CardStats.GetComponent<Text>();
                text.text = $"{Card.WrestlerName}\nPop: {Card.Popularity}\nStr: {Card.Strength}\nSkl: {Card.Skill}\nAgl: {Card.Agility}\nSta: {Card.Stamina}\nAtt: {Card.Attitude}\nOvr: {Card.CalculateOverall()}";
                text.horizontalOverflow = HorizontalWrapMode.Wrap;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.alignment = TextAnchor.MiddleCenter;
                text.fontSize = 30;
                RectTransform rectTransform = text.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(250, 0);
                if (DisplayIndex == 0)
                {
                    rectTransform.anchoredPosition = new Vector2(Position.x + 200, Position.y);
                } 
                else
                {
                    rectTransform.anchoredPosition = new Vector2(Position.x - 200, Position.y);
                }
            }
            public void Cleanup()
            {
                if (CardObject != null)
                {
                    UnityEngine.Object.Destroy(CardObject);
                    CardObject = null;
                }
                if (CardStats != null)
                {
                    UnityEngine.Object.Destroy(CardStats);
                    CardStats = null;
                }
                if (rawImage != null)
                {
                    UnityEngine.Object.Destroy(rawImage);
                    rawImage = null;
                }
                if (texture2D != null)
                {
                    Debug.LogWarning("Destroying texture " + texture2D.name);
                    UnityEngine.Object.Destroy(texture2D);
                    texture2D = null;
                }
                Card = null;
            }
        }
        public static void DisplayRoundText()
        {
            if (RoundText == null)
            {
                RoundText = new GameObject("Round");
                RoundText.transform.SetParent(LIPNHOMGGHF.JPABICKOAEO.transform, false);
                RoundText.AddComponent<Text>().font = CardMenu.VanillaFont;
                RoundText.AddComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
                RoundText.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
                RoundText.AddComponent<Shadow>().effectDistance = new Vector2(3, -3);

            }
            Text text = RoundText.GetComponent<Text>();
            text.text = $"Round Score: {P1Score}-{P2Score}";
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperCenter;
            text.fontSize = 30;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.anchoredPosition = new Vector2(0, -200);
        }
        public static void ReceiveCard(PlayableCard card, int playerIndex)
        {
            if (PlayedCards == null)
            {
                PlayedCards = new PlayableCard[Gameplay.Players];
            }
            PlayedCards[playerIndex] = card;
            PlayedCardElements[playerIndex].Card = card;
            PlayedCardElements[playerIndex].DisplayCard();
            if (PlayedCards[0] != null && PlayedCards[1] != null)
            {
                CompareCards(PlayedCards[0], PlayedCards[1]);
                ResetPlayedCards();
            }
        }
        public static void ReceiveReady(int playerIndex)
        {
            Gameplay.PlayersReady[playerIndex] = true;
            if (Gameplay.PlayersReady[0] && Gameplay.PlayersReady[1])
            {
                Gameplay.HidePlayed();
                Gameplay.ShowHand();
                Gameplay.PlayersReady[0] = false;
                Gameplay.PlayersReady[1] = false;
                PlayedCardElements[0].Card = new();
                PlayedCardElements[1].Card = new();
                if (PlayedCardElements[0].texture2D != null)
                {
                    Debug.LogWarning("Destroying texture " + PlayedCardElements[0].texture2D.name);
                    UnityEngine.Object.Destroy(PlayedCardElements[0].texture2D);
                    PlayedCardElements[0].texture2D = null;
                }
                if (PlayedCardElements[1].texture2D != null)
                {
                    Debug.LogWarning("Destroying texture " + PlayedCardElements[1].texture2D.name);
                    UnityEngine.Object.Destroy(PlayedCardElements[1].texture2D);
                    PlayedCardElements[1].texture2D = null;
                }
                PlayedCardElements[0].DisplayCard();
                PlayedCardElements[1].DisplayCard();
                LIPNHOMGGHF.FKANHDIMMBJ[Gameplay.ContinueButton].NKEDCLBOOMJ = "Click a Card to Play";
            }
        }
        public static void ReceiveCardTexture(byte[] bytes, int playerIndex)
        {
            if (PlayedCardElements[playerIndex].texture2D != null)
            {
                GameObject.Destroy(PlayedCardElements[playerIndex].texture2D);
            }
            PlayedCardElements[playerIndex].texture2D = new Texture2D(1, 1);
            ImageConversion.LoadImage(PlayedCardElements[playerIndex].texture2D, bytes);
            PlayedCardElements[playerIndex].DisplayCard();
        }
        public static void CompareCards(PlayableCard card1, PlayableCard card2)
        {
            P1Score = 0;
            P2Score = 0;
            Debug.LogWarning("Comparing cards...");
            Debug.LogWarning($"Card 1: {card1.WrestlerName}");
            Debug.LogWarning($"Card 2: {card2.WrestlerName}");

            CompareStat("Popularity", card1.Popularity, card2.Popularity);
            CompareStat("Strength", card1.Strength, card2.Strength);
            CompareStat("Skill", card1.Skill, card2.Skill);
            CompareStat("Agility", card1.Agility, card2.Agility);
            CompareStat("Stamina", card1.Stamina, card2.Stamina);
            CompareStat("Attitude", card1.Attitude, card2.Attitude);
            CompareStat("Overall", card1.CalculateOverall(), card2.CalculateOverall());
            if (P1Score == P2Score)
            {
                Debug.LogWarning("THIS ROUND WAS A DRAW!");
                Gameplay.P1Streak = 0;
                Gameplay.P2Streak = 0;
            }
            else if (P1Score > P2Score)
            {
                Debug.LogWarning("P1 Wins!");
                Gameplay.P1Total++;
                Gameplay.P1Streak++;
                Gameplay.P2Streak = 0;
            }
            else
            {
                Debug.LogWarning("P2 Wins!");
                Gameplay.P2Total++;
                Gameplay.P2Streak++;
                Gameplay.P1Streak = 0;
            }
            Gameplay.ScoreText.GetComponent<Text>().text = $"Total Score: {Gameplay.P1Total}({Gameplay.P1Streak})-{Gameplay.P2Total}({Gameplay.P2Streak})";
            LIPNHOMGGHF.FKANHDIMMBJ[Gameplay.ContinueButton].AHBNKMMMGFI = 1;
            LIPNHOMGGHF.FKANHDIMMBJ[Gameplay.ContinueButton].NKEDCLBOOMJ = "Click to continue";
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
        public static void ResetPlayedCards()
        {
            for (int i = 0; i < Gameplay.Players; i++)
            {
                PlayedCards[i] = null;
            }
        }
    }
}
