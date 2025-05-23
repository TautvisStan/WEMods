﻿using System;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

namespace CollectibleCards2
{
    [HarmonyPatch]
    public static class CardMenu
    {
        public static int TopSliderButton { get; set; }
        public static int LeftButton { get; set; }
        public static int RightButton { get; set; }
        private static RawImage rawImage { get; set; }
        private static Texture2D texture2D { get; set; }
        public static GameObject CardObject { get; set; } = null;
        public static List<CollectibleCard> Cards { get; set; } = new();
        public static int DisplayedCardIndex { get; set; } = 1;
        public static int OldIndex { get; set; } = 1;
        public static GameObject CardDescription { get; set; } = null;
        public static GameObject CardStats { get; set; } = null;
        public static Font VanillaFont { get; set; } = null;
        public static GameObject LogoObject { get; set; } = null;
        public static List<CollectibleCard> ScanCards()
        {
            ConcurrentBag<CollectibleCard> cards = new();
            Parallel.ForEach(new DirectoryInfo(Plugin.CardsDirectory).EnumerateFiles("*.png"), file =>
            {
                var metadata = PngUtils.GetCardMetadata(file.FullName);
                cards.Add(new CollectibleCard(file.Name, metadata));
            });
            List<CollectibleCard> cardsList = cards.ToList();
            cardsList.Sort((card1, card2) => string.Compare(card1.FileName, card2.FileName, StringComparison.Ordinal));
            for (int i = 0; i < cardsList.Count; i++)
            {
                cardsList[i].Index = i+1;
            }
            return cardsList;
        }
        //adding buttons
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ICGNAJFLAHL_Patch()
        {
            if (LIPNHOMGGHF.FAKHAFKOBPB == 1)
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.CardsMenuPage)
                {
                    if (LogoObject == null)
                    {
                        LogoObject = GameObject.Find("Logo");
                        LogoObject.SetActive(false);
                    }
                    Cards.Clear();
                    Cards = ScanCards();
                    Cards.Sort((card1, card2) => string.Compare(card1.FileName, card2.FileName, StringComparison.Ordinal));
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(3, "Card index", 0f, 300f, 1.5f, 1.5f);
                    TopSliderButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "<--", -350f, 0f, 1.5f, 1.5f);
                    LeftButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "-->", 350f, 0f, 1.5f, 1.5f);
                    RightButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    if(Cards.Count < 2)
                    {
                        LIPNHOMGGHF.FKANHDIMMBJ[TopSliderButton].AHBNKMMMGFI = 0;
                        LIPNHOMGGHF.FKANHDIMMBJ[LeftButton].AHBNKMMMGFI = 0;
                        LIPNHOMGGHF.FKANHDIMMBJ[RightButton].AHBNKMMMGFI = 0;
                    }
                    if(Cards.Count != 0)
                    {
                        if(DisplayedCardIndex > Cards.Count)
                        {
                            DisplayedCardIndex = Cards.Count;
                            OldIndex = Cards.Count;
                            
                        }
                        if (DisplayedCardIndex < 1) DisplayedCardIndex = 1;
                        DisplayCard(Cards[DisplayedCardIndex-1]);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Start))]
        [HarmonyPostfix]
        public static void Scene_Titles_Start_Patch()
        {
            if(VanillaFont == null)
            {
                VanillaFont = MCDCDEBALPI.IMPJPDIEKDF[1].GetComponentInChildren<Text>().font;
            }
        }
        //handling buttons
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.CardsMenuPage)
            {
                DisplayedCardIndex = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[TopSliderButton].ODONMLDCHHF(DisplayedCardIndex, 1f, 10f, 1f, Cards.Count, 0));
                if(OldIndex != DisplayedCardIndex && DisplayedCardIndex != 0 && DisplayedCardIndex <= Cards.Count)
                {
                    DisplayCard(Cards[DisplayedCardIndex - 1]);
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.NNMDEFLLNBF == LeftButton)
                {
                    DisplayedCardIndex--;
                    if(DisplayedCardIndex == 0)
                    {
                        DisplayedCardIndex = Cards.Count;
                    }
                    DisplayCard(Cards[DisplayedCardIndex - 1]);
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.NNMDEFLLNBF == RightButton)
                {
                    DisplayedCardIndex++;
                    if (DisplayedCardIndex == Cards.Count+1)
                    {
                        DisplayedCardIndex = 1;
                    }
                    DisplayCard(Cards[DisplayedCardIndex - 1]);
                }



                if (LIPNHOMGGHF.PIEMLEPEDFN > 5 || LIPNHOMGGHF.FKANHDIMMBJ[TopSliderButton].CLMDCNDEBGD != 0)
                {
                    LIPNHOMGGHF.PIEMLEPEDFN = 0;
                }
                OldIndex = DisplayedCardIndex;


                if (LIPNHOMGGHF.PIEMLEPEDFN <= -5)
                {
                    if (LogoObject != null)
                    {
                        LogoObject.SetActive(true);
                        LogoObject = null;
                    }
                    LIPNHOMGGHF.ODOAPLMOJPD = 0;
                    if (Plugin.EntryMenu == 2)
                    {
                        Plugin.EntryMenu = 0;
                        LIPNHOMGGHF.PMIIOCMHEAE(20);
                    }
                    else
                    {
                        Plugin.EntryMenu = 0;
                        LIPNHOMGGHF.ICGNAJFLAHL(0);
                    }
                }
            }
            else
            {
                if (CardObject != null)
                {
                    UnityEngine.Object.Destroy(CardObject);
                    CardObject = null;
                }
                if (CardDescription != null)
                {
                    UnityEngine.Object.Destroy(CardDescription);
                    CardDescription = null;
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
                    UnityEngine.Object.Destroy(texture2D);
                    texture2D = null;
                }
            }
        }

        public static void DisplayCard(CollectibleCard card)
        {
            byte[] array2 = card.GetCardBytes();
            if (array2 != null)
            {
                if (CardObject == null)
                {
                    CardObject = new("card");
                    CardObject.transform.SetParent(LIPNHOMGGHF.JPABICKOAEO.transform, false);
                    rawImage = CardObject.AddComponent<RawImage>();
                    texture2D = new Texture2D(1, 1);
                }
                else
                {
                    rawImage = CardObject.GetComponent<RawImage>();
                    texture2D = (Texture2D)rawImage.texture;
                }
                ImageConversion.LoadImage(texture2D, array2);
                rawImage.texture = texture2D;
                RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(texture2D.width / 2, texture2D.height / 2);
                rectTransform.anchoredPosition = new Vector2(0, 0);
                rawImage.transform.SetAsLastSibling();
            }
            DisplayCardInfo();
            DisplayCardStats();
        }
        //disabling annoying audio
        [HarmonyPatch(typeof(CHLPMKEGJBJ), nameof(CHLPMKEGJBJ.DNNPEAOCDOG))]
        [HarmonyPrefix]
        public static bool CHLPMKEGJBJ_DNNPEAOCDOG_Prefix(AudioClip GGMBIAAEMKO, float ELJKCOHGBBD = 0f, float CDNNGHGFALM = 1f)
        {
            if (GGMBIAAEMKO == CHLPMKEGJBJ.PAJJMPLBDPL && LIPNHOMGGHF.ODOAPLMOJPD == Plugin.CardsMenuPage)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static void DisplayCardInfo()
        {
            if(CardDescription == null)
            {
                CardDescription = new GameObject("Description");
                CardDescription.transform.SetParent(LIPNHOMGGHF.JPABICKOAEO.transform, false);
                CardDescription.AddComponent<Text>().font = VanillaFont;
                CardDescription.AddComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
                CardDescription.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
                CardDescription.AddComponent<Shadow>().effectDistance = new Vector2(3, -3);
            }
            Text text = CardDescription.GetComponent<Text>();
            text.text = Cards[DisplayedCardIndex - 1].GetDescription();
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperCenter;
            text.fontSize = 40;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(1250, 0);
            rectTransform.anchoredPosition = new Vector2(0, -250);
        }
        public static void DisplayCardStats()
        {
            if (CardStats == null)
            {
                CardStats = new GameObject("Stats");
                CardStats.transform.SetParent(LIPNHOMGGHF.JPABICKOAEO.transform, false);
                CardStats.AddComponent<Text>().font = VanillaFont;
                CardStats.AddComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
                CardStats.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
                CardStats.AddComponent<Shadow>().effectDistance = new Vector2(3, -3);
            }
            Text text = CardStats.GetComponent<Text>();
            text.text = Cards[DisplayedCardIndex - 1].GetStatsString();
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperRight;
            text.fontSize = 30;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(300, 0);
            rectTransform.anchoredPosition = new Vector2(-350, -75);
        }

    }
}