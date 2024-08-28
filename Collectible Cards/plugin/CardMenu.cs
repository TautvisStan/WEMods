using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace CollectibleCards2
{
    [HarmonyPatch]
    public static class CardMenu
    {
        public static int TopSliderButton { get; set; }
        public static int TopLoadCard { get; set; }
        public static int LeftButton { get; set; }
        public static int RightButton { get; set; }
        private static RawImage rawImage { get; set; }
        private static Texture2D texture2D { get; set; }
        public static GameObject CardObject { get; set; } = null;
        public static List<CollectibleCard> Cards { get; set; } = new();
        public static int CardIndex { get; set; } = 0;
        public static void ScanCards()
        {
            Debug.LogWarning("SCANNING FOR CARDS");
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
                    { "CustomGenerated", "" }
                };
                PngUtils.GetFullMetadata(file.FullName, metadata);
                Debug.LogWarning("FOUND CARD " + file.FullName);
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
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(3, "Card index", -200f, 250f, 1.5f, 1.5f);
                    TopSliderButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Load card", 200f, 250f, 1.5f, 1.5f);
                    TopLoadCard = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "<-", -300f, 0f, 1f, 1f);
                    LeftButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "->", 300f, 0f, 1f, 1f);
                    RightButton = LIPNHOMGGHF.HOAOLPGEBKJ;
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
                CardIndex = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[TopSliderButton].ODONMLDCHHF(CardIndex, 1f, 10f, 0f, Cards.Count, 0));


                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.NNMDEFLLNBF == TopLoadCard)
                {
                    DisplayCard(Cards[CardIndex].CardBytes);
                }


                if (LIPNHOMGGHF.PIEMLEPEDFN > 5 || LIPNHOMGGHF.FKANHDIMMBJ[TopSliderButton].CLMDCNDEBGD != 0)
                {
                    LIPNHOMGGHF.PIEMLEPEDFN = 0;
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
                rectTransform.anchoredPosition = new Vector2(0, -100);
                rawImage.transform.SetAsLastSibling();
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

    }
}
