using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Runtime.CompilerServices;
using CollectibleCards2;
using System.Reflection;
using HarmonyLib;
using UnityEngine.UI;
using UnityEngine.XR;

namespace CardGame
{
    [HarmonyPatch]
    public static class Gameplay
    {
        public static int Players { get; set; } = 2;
        public static PlayableCard[] PlayedCards { get; set; } = null;
        public static List<PlayableCard> Deck { get; set; } = new();
        public static List<Texture2D> DeckCardTexture { get; set; } = new();
        public static int P1Score { get; set; } = 0;
        public static int P2Score { get; set; } = 0;
        public static GameObject ScoreText { get; set;} = null;
        public static PlayableCardDisplay[] DeckCardElements = new PlayableCardDisplay[3];

        public static PlayableCardDisplay[] PlayedCardElements = new PlayableCardDisplay[2];
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
                        if(texture2D == null)
                            texture2D = new Texture2D(1, 1);
                    }
                    else
                    {
                        rawImage = CardObject.GetComponent<RawImage>();
                      //  texture2D = (Texture2D)rawImage.texture;
                    }
                    if (DisplayIndex != -1)
                    {
                        texture2D = DeckCardTexture[DisplayIndex];
                    }
                    rawImage.texture = texture2D;
                    RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(texture2D.width/2, texture2D.height/2);
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
                text.text = $"{Card.WrestlerName}\n{Card.GetStatsString()}";
                text.horizontalOverflow = HorizontalWrapMode.Wrap;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.alignment = TextAnchor.UpperCenter;
                text.fontSize = 30;
                RectTransform rectTransform = text.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(300, 0);
                if (DisplayIndex != -1)
                {
                    rectTransform.anchoredPosition = new Vector2(Position.x, Position.y - 150);
                }
                else
                {
                    rectTransform.anchoredPosition = new Vector2(Position.x, Position.y+50);
                }
            }
            public void HandleClicks()
            {
                if (HKJOAJOKOIJ.EOOBMIDCKIF == 1f && HKJOAJOKOIJ.LMADDGDMBGB == 0f)
                {
                    if (Mathf.Abs(HKJOAJOKOIJ.GMCCPOAIBHC - CardObject.transform.position.x) < 70f * NAEEIFNFBBO.IADPBBEPJKF && Mathf.Abs(HKJOAJOKOIJ.MINFPCEENFN - CardObject.transform.position.y) < 100f * NAEEIFNFBBO.PNHIGOBEEBB)
                    {
                        if (DisplayIndex != -1)
                        {
                            Debug.LogWarning("CLICKED ON CARD " + DisplayIndex);
                            Plugin.steamNetworking.SEND_CARD(Card);
                            Plugin.steamNetworking.SendFULLTextureToPlayers(texture2D);
                            PlayedCardElements[Plugin.steamLobby.SteamLobbyMemberIndex].Card = Card;
                            PlayedCardElements[Plugin.steamLobby.SteamLobbyMemberIndex].texture2D = texture2D;
                            PlayedCardElements[Plugin.steamLobby.SteamLobbyMemberIndex].DisplayCard();
                            
                        }
                    }
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
            }
        }
        public static void SetupCards()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Deck[i] != null)
                {
                    DeckCardElements[i].Card = Deck[i];
                    DeckCardElements[i].DisplayCard();
                    continue;
                }
                DeckCardElements[i].Cleanup();
            }
        }

        public static void PlaceCards()
        {
            for (int i = 0; i < 3; i++)
            {
                if (DeckCardElements[i] == null)
                {
                    DeckCardElements[i] = new();
                    DeckCardElements[i].DisplayIndex = i;
                    int col = i;
                    int x;
                    int y;
                    y = 200;
                    x = -350 + (350 * col);
                    DeckCardElements[i].Position = new Vector2(x, y);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                if (PlayedCardElements[i] == null)
                {
                    PlayedCardElements[i] = new();
                    PlayedCardElements[i].DisplayIndex = -1;
                    int col = i;
                    int x;
                    int y;
                    y = -200;
                    x = -350 + (700 * col);
                    PlayedCardElements[i].Position = new Vector2(x, y);

                }
            }
        }
        //adding buttons
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ICGNAJFLAHL_Patch()
        {
            if (LIPNHOMGGHF.FAKHAFKOBPB == 1)
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.GameplayPage)
                {
                    FillupDeck();
                    PlaceCards();
                    SetupCards();

                }
            }
        }
        //handling buttons
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.GameplayPage)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (DeckCardElements[i].Card != null && DeckCardElements[i].CardObject != null)
                    {
                        DeckCardElements[i].HandleClicks();
                    }
                }
            }
        }
        //disabling annoying audio
        [HarmonyPatch(typeof(CHLPMKEGJBJ), nameof(CHLPMKEGJBJ.DNNPEAOCDOG))]
        [HarmonyPrefix]
        public static bool CHLPMKEGJBJ_DNNPEAOCDOG_Prefix(AudioClip GGMBIAAEMKO, float ELJKCOHGBBD = 0f, float CDNNGHGFALM = 1f)
        {
            if (GGMBIAAEMKO == CHLPMKEGJBJ.PAJJMPLBDPL && LIPNHOMGGHF.ODOAPLMOJPD == Plugin.GameplayPage)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void ResetPlayedCards()
        {
            for (int i = 0; i < Players; i++) 
            {
                PlayedCards[i] = null;
            }
        }
        public static void FillupDeck()
        {
            Deck.Clear();


            foreach (CollectibleCard card in CollectibleCards2.CardMenu.Cards)
            {
                if (PlayableCard.VerifyCard(card)) Deck.Add(new PlayableCard(card));
            }
        }
        public static void FillupDeckTextures()
        {
            for (int i = 0; i < DeckCardTexture.Count; i++)
            {
                if (DeckCardTexture[i] != null)
                {
                    GameObject.Destroy(DeckCardTexture[i]);
                    DeckCardTexture[i] = null;
                }
            }
            DeckCardTexture = new();
            for (int i = 0; i < 10; i++)
            {
                AddResizedTexture(CardMenu.Cards[Deck[i].LocalIndex]);
            }
        }
        public static void AddResizedTexture(CollectibleCard card)
        {
            byte[] array = card.GetCardBytes();
            Texture2D texture2D = new Texture2D(1, 1);
            ImageConversion.LoadImage(texture2D, array);
            DeckCardTexture.Add(PngUtils.ResizeTexture(texture2D, texture2D.width /2, texture2D.height/2));
            GameObject.Destroy(texture2D);
            texture2D = null;
        }
        public static void ReceiveCard(PlayableCard card, int playerIndex)
        {
            if(PlayedCards == null)
            {
                PlayedCards = new PlayableCard[Players];
            }
            PlayedCards[playerIndex] = card;
            PlayedCardElements[playerIndex].Card = card;
            if (PlayedCards[0] != null && PlayedCards[1] != null) 
            {
                CompareCards(PlayedCards[0], PlayedCards[1]);
                ResetPlayedCards();
            }
        }
        public static void ReceiveCardTexture(byte[] bytes, int playerIndex)
        {
            if (PlayedCardElements[playerIndex].texture2D == null) PlayedCardElements[playerIndex].texture2D = new Texture2D(1, 1);
            ImageConversion.LoadImage(PlayedCardElements[playerIndex].texture2D, bytes);
            PlayedCardElements[playerIndex].DisplayCard();
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
            P1Score = 0;
            P2Score = 0;
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
