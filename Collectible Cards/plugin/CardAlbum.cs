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
        public static int LeftPageButton { get; set; }
        public static int RightPageButton { get; set; }
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
                        LIPNHOMGGHF.ODOAPLMOJPD = Plugin.CardsMenuPage;
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
                    RarityFilterButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Foil Filter", 0f, 300f, 1.25f, 1.25f);
                    FoilFilterButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Signature Filter", 250f, 300f, 1.25f, 1.25f);
                    SignatureFilterButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Apply Filters", 500f, 300f, 1.25f, 1.25f);
                    ApplyFilterButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "<--", -500f, 0f, 1f, 1f);
                    LeftPageButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "-->", 500f, 0f, 1f, 1f);
                    RightPageButton = LIPNHOMGGHF.HOAOLPGEBKJ;

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
            if (Page > MaxPages) Page = MaxPages;
            if (Page == 0 && MaxPages != 0) Page = 1;
            if (MaxPages < 2)
            {
                LIPNHOMGGHF.FKANHDIMMBJ[LeftPageButton].AHBNKMMMGFI = 0;
                LIPNHOMGGHF.FKANHDIMMBJ[RightPageButton].AHBNKMMMGFI = 0;
            }
            else
            {
                LIPNHOMGGHF.FKANHDIMMBJ[LeftPageButton].AHBNKMMMGFI = 1;
                LIPNHOMGGHF.FKANHDIMMBJ[RightPageButton].AHBNKMMMGFI = 1;
            }
        }
        public static void SetupCards()
        {
            for (int i = 0; i < 10; i++)
            {
                int listIndex = (10 * (Page-1)) + i;
                if (listIndex >= 0)
                {
                    if (listIndex < FilteredCards.Count && FilteredCards[listIndex] != null)
                    {
                        CardElements[i].Card = FilteredCards[listIndex];
                        CardElements[i].DisplayCard();
                        continue;
                    }
                }
                CardElements[i].Cleanup();
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
                x = -350 + (175 * col);
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
                    FilteredCards = FilteredCards.FindAll(x => x.IsSigned() == false);
                    break;
                case 1:
                    FilteredCards = FilteredCards.FindAll(x => x.IsSigned() == true);
                    break;
            }

        }
        //disabling annoying audio
        [HarmonyPatch(typeof(CHLPMKEGJBJ), nameof(CHLPMKEGJBJ.DNNPEAOCDOG))]
        [HarmonyPrefix]
        public static bool CHLPMKEGJBJ_DNNPEAOCDOG_Prefix(AudioClip GGMBIAAEMKO, float ELJKCOHGBBD = 0f, float CDNNGHGFALM = 1f)
        {
            if (GGMBIAAEMKO == CHLPMKEGJBJ.PAJJMPLBDPL && LIPNHOMGGHF.ODOAPLMOJPD == CardAlbumPage)
            {
                return false;
            }
            else
            {
                return true;
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
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.NNMDEFLLNBF == LeftPageButton)
                {
                    Page--;
                    if (Page == 0)
                    {
                        Page = MaxPages;
                    }
                    SetupCards();
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.NNMDEFLLNBF == RightPageButton)
                {
                    Page++;
                    if (Page > MaxPages)
                    {
                        Page = 1;
                    }
                    SetupCards();
                }
                RarityFilter = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[RarityFilterButton].ODONMLDCHHF(RarityFilter, 1f, 10f, -1f, 3, 0));
                switch (RarityFilter)
                {
                    case -1:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityFilterButton].FFCNPGPALPD = "No Filter";
                        break;
                    case 0:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityFilterButton].FFCNPGPALPD = "Base";
                        break;
                    case 1:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityFilterButton].FFCNPGPALPD = "Bronze";
                        break;
                    case 2:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityFilterButton].FFCNPGPALPD = "Silver";
                        break;
                    case 3:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityFilterButton].FFCNPGPALPD = "Gold";
                        break;
                }
                FoilFilter = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[FoilFilterButton].ODONMLDCHHF(FoilFilter, 1f, 10f, -1f, 1, 0));
                if (FoilFilter == -1)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[FoilFilterButton].FFCNPGPALPD = "No Filter";
                }
                SignatureFilter = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[SignatureFilterButton].ODONMLDCHHF(SignatureFilter, 1f, 10f, -1f, 1, 0));
                if (SignatureFilter == -1)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[SignatureFilterButton].FFCNPGPALPD = "No Filter";
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[ApplyFilterButton].CLMDCNDEBGD != 0)
                {
                    FilterCards();
                    CalculatePages();
                    SetupCards();
                }

                if (LIPNHOMGGHF.PIEMLEPEDFN > 5)
                {
                    LIPNHOMGGHF.PIEMLEPEDFN = 0;
                }


                if (LIPNHOMGGHF.PIEMLEPEDFN <= -5)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = Plugin.CardsMenuPage;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
                }

            }
        }
    }
}
