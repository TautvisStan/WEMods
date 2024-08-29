using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

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
        public static void ScanCards()
        {
            Cards = new();
            foreach (FileInfo file in new DirectoryInfo(Plugin.CardsDirectory).EnumerateFiles("*.png"))
            {
                byte[] array = File.ReadAllBytes(file.FullName);
                Dictionary<string, string> metadata  = new()
                {
                    { "CharID", "" },
                    { "Name", "" },
                    { "FedName", "" },
                    { "Border", "" },
                    { "Foil", "" },
                    { "Signature", "" },
                    { "CustomGenerated", "" },
                    { "Popularity", "" },
                    { "Strength", "" },
                    { "Skill", "" },
                    { "Agility", "" },
                    { "Stamina", "" },
                    { "Attitude", "" },
                    { "FrontFinisher", "" },
                    { "BackFinisher", "" },

                };
                metadata = PngUtils.GetFullMetadata(file.FullName, metadata);
                Cards.Add(new CollectibleCard(array, metadata));
            }
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
                    ScanCards();
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(3, "Card index", 0f, 280f, 1.5f, 1.5f);
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
                        DisplayCard(Cards[DisplayedCardIndex-1].CardBytes);
                    }
                }
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
                    DisplayCard(Cards[DisplayedCardIndex - 1].CardBytes);
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.NNMDEFLLNBF == LeftButton)
                {
                    DisplayedCardIndex--;
                    if(DisplayedCardIndex == 0)
                    {
                        DisplayedCardIndex = Cards.Count;
                    }
                    DisplayCard(Cards[DisplayedCardIndex - 1].CardBytes);
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.NNMDEFLLNBF == RightButton)
                {
                    DisplayedCardIndex++;
                    if (DisplayedCardIndex == Cards.Count+1)
                    {
                        DisplayedCardIndex = 1;
                    }
                    DisplayCard(Cards[DisplayedCardIndex - 1].CardBytes);
                }



                if (LIPNHOMGGHF.PIEMLEPEDFN > 5 || LIPNHOMGGHF.FKANHDIMMBJ[TopSliderButton].CLMDCNDEBGD != 0)
                {
                    LIPNHOMGGHF.PIEMLEPEDFN = 0;
                }
                OldIndex = DisplayedCardIndex;


                if (LIPNHOMGGHF.PIEMLEPEDFN <= -5)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = 0;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
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
            }
        }

        public static void DisplayCard(byte[] array2)
        {
           // byte[] array2 = File.ReadAllBytes(fileName);
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
                DisplayCardInfo();
            }
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
                CardDescription.AddComponent<Text>().font = OverlaytxtFileParser.CardFont;
                
            }
            Text text = CardDescription.GetComponent<Text>();
            text.text = Cards[DisplayedCardIndex - 1].GetDescription();
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.alignment = TextAnchor.UpperCenter;
            text.fontSize = 48;
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(Screen.width * 0.9f, 0);
            rectTransform.anchoredPosition = new Vector2(0, -250);


        }

    }
}
