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
using Steamworks;

namespace CardGame
{
    [HarmonyPatch]
    public static class Gameplay
    {
        
        public static int Players { get; set; } = 2;
        public static bool[] PlayersReady = new bool[Players];
        
        public static List<PlayableCard> Deck { get; set; } = new();
        public static List<Texture2D> DeckCardTexture { get; set; } = new();

        public static int P1Total { get; set; } = 0;
        public static int P2Total { get; set; } = 0;
        public static GameObject ScoreText { get; set;} = null;
        public static PlayableCardDisplay[] DeckCardElements { get; set; } = new PlayableCardDisplay[3];

        public static int ContinueButton { get; set; }

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
                rectTransform.anchoredPosition = new Vector2(Position.x, Position.y - 125);
            }
            public void HandleClicks()
            {
                if (HKJOAJOKOIJ.EOOBMIDCKIF == 1f && HKJOAJOKOIJ.LMADDGDMBGB == 0f)
                {
                    if (Mathf.Abs(HKJOAJOKOIJ.GMCCPOAIBHC - CardObject.transform.position.x) < 70f * NAEEIFNFBBO.IADPBBEPJKF && Mathf.Abs(HKJOAJOKOIJ.MINFPCEENFN - CardObject.transform.position.y) < 100f * NAEEIFNFBBO.PNHIGOBEEBB && CardObject.activeSelf)
                    {
                        if (DisplayIndex != -1)
                        {
                            Debug.LogWarning("CLICKED ON CARD " + DisplayIndex);
                            Plugin.steamNetworking.SEND_CARD(Card);
                            Plugin.steamNetworking.SendFULLTextureToPlayers(texture2D);
                    //        SingleRound.ReceiveCard(Card, Plugin.steamLobby.SteamLobbyMemberIndex);
                            SingleRound.ReceiveCardTexture(texture2D.EncodeToPNG(), Plugin.steamLobby.SteamLobbyMemberIndex);

                      //      SingleRound.ReceiveCard(Card, Plugin.steamLobby.SteamLobbyMemberIndex+1);
                     //       SingleRound.ReceiveCardTexture(texture2D.EncodeToPNG(), Plugin.steamLobby.SteamLobbyMemberIndex+1);


                            RemoveCardFromDeck(DisplayIndex);
                            HideHand();
                            ShowPlayed();
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
        public static void DisplayStatusText()
        {
            if (ScoreText == null)
            {
                ScoreText = new GameObject("Status");
                ScoreText.transform.SetParent(LIPNHOMGGHF.JPABICKOAEO.transform, false);
                ScoreText.AddComponent<Text>().font = CardMenu.VanillaFont;
                ScoreText.AddComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
                ScoreText.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
                ScoreText.AddComponent<Shadow>().effectDistance = new Vector2(3, -3);


            }
            Text text = ScoreText.GetComponent<Text>();
            text.text = $"Score: {P1Total}-{P2Total}";
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperCenter;
            text.fontSize = 30;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.anchoredPosition = new Vector2(0, 300);
        }
        public static void HideHand()
        {
            foreach (PlayableCardDisplay card in DeckCardElements)
            {
                card.CardObject?.SetActive(false);
                card.CardStats?.SetActive(false);
            }
        }
        public static void ShowHand()
        {
            foreach (PlayableCardDisplay card in DeckCardElements)
            {
                card.CardObject?.SetActive(true);
                card.CardStats?.SetActive(true);
            }
        }
        public static void HidePlayed()
        {
            foreach (SingleRound.PlayableCardDisplay card in SingleRound.PlayedCardElements)
            {
                card.CardObject?.SetActive(false);
                card.CardStats?.SetActive(false);
            }
        }
        public static void ShowPlayed()
        {
            foreach (SingleRound.PlayableCardDisplay card in SingleRound.PlayedCardElements)
            {
                card.CardObject?.SetActive(true);
                card.CardStats?.SetActive(true);
            }
        }
        public static void RandomizeDeck()
        {
            int n = Deck.Count;
            while (n > 1)
            {
                int k = UnityEngine.Random.Range(0, n--);
                Debug.LogWarning(n + " " + k);
                (Deck[n], Deck[k]) = (Deck[k], Deck[n]);
            }
            List<PlayableCard> ProperDeck = new();
            for (int i = 0; i < 10; i++)
            {
                Debug.LogWarning(i);
                ProperDeck.Add(Deck[i]);
            }
            Deck = ProperDeck;
        }
        public static void RemoveCardFromDeck(int index)
        {
            Deck.RemoveAt(index);
            GameObject.Destroy(DeckCardTexture[index]);
            DeckCardTexture.RemoveAt(index);
            for (int i = 0; i < 3; i++)
            {
                if (i < Deck.Count)
                {
                    DeckCardElements[i].Card = Deck[i];
                    DeckCardElements[i].DisplayCard();
                }
                else
                {
                    DeckCardElements[i].Cleanup();
                }
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
                    y = 100;
                    x = -350 + (350 * col);
                    DeckCardElements[i].Position = new Vector2(x, y);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                if (SingleRound.PlayedCardElements[i] == null)
                {
                    SingleRound.PlayedCardElements[i] = new();
                    SingleRound.PlayedCardElements[i].DisplayIndex = i;
                    int col = i;
                    int x;
                    int y;
                    y = 0;
                    x = -400 + (800 * col);
                    SingleRound.PlayedCardElements[i].Position = new Vector2(x, y);

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
                    RandomizeDeck();
                    FillupDeckTextures();
                    PlaceCards();
                    SetupCards();
                    DisplayStatusText();
                    for (int i = 0; i < PlayersReady.Length; i++)
                    {
                        PlayersReady[i] = false;
                    }

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Click a Card to Play", 0f, -250f, 1.5f, 1.5f);
                    ContinueButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].AHBNKMMMGFI = 0;
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
            if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.GameplayPage)
            {
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[ContinueButton].CLMDCNDEBGD != 0)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[ContinueButton].NKEDCLBOOMJ = "Waiting For Players";
                    Plugin.steamNetworking.SEND_READY();
                  //  SingleRound.ReceiveReady(Plugin.steamLobby.SteamLobbyMemberIndex + 1);
                    LIPNHOMGGHF.FKANHDIMMBJ[ContinueButton].AHBNKMMMGFI = 0;
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN > 5)
                {
                    LIPNHOMGGHF.PIEMLEPEDFN = 0;
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN <= -5)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = Plugin.MPLobbyPage;
                    Plugin.steamLobby.LeaveLobby();
                    FillupDeck();
                    foreach (PlayableCardDisplay card in DeckCardElements)
                    {
                        card.Cleanup();
                    }
                    foreach (SingleRound.PlayableCardDisplay card in SingleRound.PlayedCardElements)
                    {
                        card.Cleanup();
                    }
                    GameObject.Destroy(ScoreText);
                    ScoreText = null;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
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
            Debug.LogWarning("TRYING TO FILL DECK TEXTURES, DECK SIZE " + Deck.Count);
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
                Debug.LogWarning(i + " " + Deck[i].LocalIndex);
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

    }
}
