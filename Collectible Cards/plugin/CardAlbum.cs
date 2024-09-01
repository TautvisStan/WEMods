using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace CollectibleCards2
{
    [HarmonyPatch]
    public static class CardAlbum
    {
        public static GameObject[] CardPage { get; set; } = new GameObject[10];
        private static RawImage[] rawImage { get; set; } = new RawImage[10];
        private static Texture2D[] texture2D { get; set; } = new Texture2D[10];
        public static int CardAlbumPage { get; set; } = Plugin.CardsMenuPage + 1;
        public static int AlbumButton { get; set; }
        public static AlbumDisplayElement[] CardElements = new AlbumDisplayElement[10];
        public static List<CollectibleCard> FilteredCards { get; set; } = null;
        public static int Page { get; set; } = 1;
        public static int MaxPages { get; set; } = 1;
        public static int RarityFilter { get; set; } = -1;
        public static int RarityFilterButton { get; set; }
        public static int FoilFilter { get; set; } = -1;
        public static int FoilFilterButton { get; set; }
        public static int SignatureFilter { get; set; } = -1;
        public static int SignatureFilterButton { get; set; }
        public static int ApplyFilterButton { get; set; }
        private static bool SetUp = false;
        public class AlbumDisplayElement
        {
            private RawImage rawImage { get; set; }
            private Texture2D texture2D { get; set; }
            public GameObject CardObject { get; set; } = null;
            public CollectibleCard Card { get; set; } = null;
            public Vector2 Position { get; set; } = new();
            public GameObject CardIndex { get; set; } = null;

            public void DisplayCard()
            {
                if (Card != null)
                {
                    byte[] array2 = Card.GetCardBytes();
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
                        rectTransform.sizeDelta = new Vector2(texture2D.width / 5, texture2D.height / 5);
                        rectTransform.anchoredPosition = Position;
                        rawImage.transform.SetAsLastSibling();
                        DisplayCardInfo();
                    }
                }

            }
            public void DisplayCardInfo()
            {
                if (CardIndex == null)
                {
                    CardIndex = new GameObject("Description");
                    CardIndex.transform.SetParent(LIPNHOMGGHF.JPABICKOAEO.transform, false);
                    CardIndex.AddComponent<Text>().font = CardMenu.VanillaFont;
                    CardIndex.AddComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
                    CardIndex.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
                    CardIndex.AddComponent<Shadow>().effectDistance = new Vector2(3, -3);


                }
                Text text = CardIndex.GetComponent<Text>();
                text.text = Card.Index.ToString();
                text.horizontalOverflow = HorizontalWrapMode.Wrap;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.alignment = TextAnchor.UpperCenter;
                text.fontSize = 48;
                RectTransform rectTransform = text.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(Screen.width * 0.9f, 0);
                rectTransform.anchoredPosition = new Vector2(Position.x, Position.y-100);
            }
            public void HandleClicks()
            {
                if (HKJOAJOKOIJ.EOOBMIDCKIF == 1f && HKJOAJOKOIJ.LMADDGDMBGB == 0f)
                {
                    if (Mathf.Abs(HKJOAJOKOIJ.GMCCPOAIBHC - CardObject.transform.position.x) < 70f * NAEEIFNFBBO.IADPBBEPJKF && Mathf.Abs(HKJOAJOKOIJ.MINFPCEENFN - CardObject.transform.position.y) < 100f * NAEEIFNFBBO.PNHIGOBEEBB)
                    {
                        UnityEngine.Debug.LogWarning("CLICKED" + Card.Index);
                        LIPNHOMGGHF.ODOAPLMOJPD = CollectibleCards2.Plugin.CardsMenuPage;
                        LIPNHOMGGHF.ICGNAJFLAHL(0);
                        CardMenu.DisplayedCardIndex = Card.Index;
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
                if (CardIndex != null)
                {
                    UnityEngine.Object.Destroy(CardIndex);
                    CardIndex = null;
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
        //adding buttons
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ICGNAJFLAHL_Patch()
        {
            if (LIPNHOMGGHF.FAKHAFKOBPB == 1)
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.CardsMenuPage)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Card Album", 350f, 175f, 1.5f, 1.5f);
                    AlbumButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
                if (LIPNHOMGGHF.ODOAPLMOJPD == CardAlbumPage)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Rarity Filter", -250f, 300f, 1.25f, 1.25f);
                    AlbumButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Foil Filter", 0f, 300f, 1.25f, 1.25f);
                    AlbumButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Border Filter", 250f, 300f, 1.25f, 1.25f);
                    AlbumButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Apply Filters", 500f, 300f, 1.25f, 1.25f);
                    AlbumButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    PlaceCards();
                    FilterCards();
                    CalculatePages();
                    SetupCards();
                    SetUp = true;
                }
                else
                {
                    if(SetUp == true)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            CardElements[i].Cleanup();
                        }
                        SetUp = false;
                    }
                }
            }
        }
        public static void CalculatePages()
        {
            MaxPages = Mathf.CeilToInt(FilteredCards.Count / 10f);
        }
        public static void SetupCards()
        {
            for (int i = 0; i < 10; i++)
            {
                int listIndex = (10 * (Page-1)) + i;
                Debug.LogWarning(listIndex);
                if (listIndex >= 0)
                {
                    if (listIndex < FilteredCards.Count && FilteredCards[listIndex] != null)
                    {
                        CardElements[i].Card = FilteredCards[listIndex];
                        CardElements[i].DisplayCard();
                    }
                }
            }
        }

        public static void PlaceCards()
        {
            for (int i = 0; i < 10; i++)
            {
                CardElements[i] = new();
                int col = i;
                int x;
                int y;
                if (i < 5)
                {
                    y = 150;
                }
                else
                {
                    y = -150;
                    col -= 5;
                }
                x = -400 + (200 * col);
                CardElements[i].Position = new Vector2(x, y);
            }
        }
        public static void FilterCards()
        {
            FilteredCards = new List<CollectibleCard>(CardMenu.Cards);
            switch (RarityFilter)
            {
                case 0:
                    FilteredCards = FilteredCards.FindAll(x => x.GetBorder().Trim() == "");
                    break;
                case 1:
                    FilteredCards = FilteredCards.FindAll(x => x.GetBorder().Trim() == "Bronze");
                    break;
                case 2:
                    FilteredCards = FilteredCards.FindAll(x => x.GetBorder().Trim() == "Silver");
                    break;
                case 3:
                    FilteredCards = FilteredCards.FindAll(x => x.GetBorder().Trim() == "Gold");
                    break;
            }
            switch (FoilFilter)
            {
                case 0:
                    FilteredCards = FilteredCards.FindAll(x => x.IsFoil() == false);
                    break;
                case 1:
                    FilteredCards = FilteredCards.FindAll(x => x.IsFoil() == true);
                    break;
            }
            switch (SignatureFilter)
            {
                case 0:
                    FilteredCards = FilteredCards.FindAll(x => x.IsSigned() == true);
                    break;
                case 1:
                    FilteredCards = FilteredCards.FindAll(x => x.IsSigned() == false);
                    break;
            }

        }
        //handling buttons
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.ODOAPLMOJPD == Plugin.CardsMenuPage)
            {
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[AlbumButton].CLMDCNDEBGD != 0)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = CardAlbumPage;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
                }
            }
            if (LIPNHOMGGHF.ODOAPLMOJPD == CardAlbumPage)
            {
                for (int i = 0; i < 10; i++)
                {
                    if(CardElements[i].Card != null && CardElements[i].CardObject != null)
                    {
                        CardElements[i].HandleClicks();
                    }
                }
            }
        }
    }
}
