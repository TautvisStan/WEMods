using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using CollectibleCards2;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CustomCardGenerator
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CardGen";
        public const string PluginName = "CustomCardGenerator";
        public const string PluginVer = "1.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        private static RawImage rawImage { get; set; }
        private static Texture2D texture2D { get; set; }
        public static int CardsGenPage { get; set; } = CollectibleCards2.Plugin.CardsMenuPage-1;
        public static int CardsGenButton { get; set; }
        public static List<DirectoryInfo> Presets { get; set; } = new();
        public static GameObject CardObject { get; set; } = null;
        public static int CharButton { get; set; }
        public static int CharID { get; set; } = 0;
        public static int CostumeButton { get; set; }
        public static int Costume { get; set; } = 0;
        public static int PresetButton { get; set; }
        public static int Preset { get; set; } = -1;
        public static int RarityButton { get; set; }
        public static int Rarity { get; set; } = -1;
        public static int FoilButton { get; set; }
        public static int Foil { get; set; } = -1;
        public static int SignatureButton { get; set; }
        public static int Signature { get; set; } = -1;
        public static int GenerateButton { get; set; }

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }
        public static void ScanPresets()
        {

            Presets = new();
            foreach (string modPath in Directory.GetDirectories(Path.Combine(Paths.BepInExRootPath, "plugins")))
            {
                ScanFolder(modPath);
            }
        }
        static void ScanFolder(string path)
        {
            DirectoryInfo directoryInfo = new(path);
            FileInfo[] files = directoryInfo.GetFiles("CardDesign.txt");
            if (files.Length != 0)
            {
                Presets.Add(directoryInfo);
            }
            else
            {
                foreach (string subDir in Directory.GetDirectories(path))
                {
                    ScanFolder(subDir);
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
                if (LIPNHOMGGHF.ODOAPLMOJPD == CollectibleCards2.Plugin.CardsMenuPage)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Card Generator", 350f, 250f, 1.5f, 1.5f);
                    CardsGenButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
                if (LIPNHOMGGHF.ODOAPLMOJPD == CardsGenPage)
                {
                    ScanPresets();

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(3, "Character", -100f, 250f, 2.5f, 2.5f);
                    CharButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Preset", 375f, 125f, 1.25f, 1.25f);
                    PresetButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Costume", 375f, 50f, 1.25f, 1.25f);
                    CostumeButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Rarity", 375f, -25f, 1.25f, 1.25f);
                    RarityButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Foil", 375f, -100f, 1.25f, 1.25f);
                    FoilButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Signature", 375f, -175f, 1.25f, 1.25f);
                    SignatureButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Generate", 375f, -250f, 1.25f, 1.25f);
                    GenerateButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
                else
                {
                    if(CardObject != null)
                    {
                        UnityEngine.Object.Destroy(CardObject);
                        CardObject = null;
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
        }

        //handling buttons
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.ODOAPLMOJPD == CollectibleCards2.Plugin.CardsMenuPage)
            {
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[CardsGenButton].CLMDCNDEBGD != 0)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = CardsGenPage;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
                }
            }
            if (LIPNHOMGGHF.ODOAPLMOJPD == CardsGenPage)
            {
                CharID = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[CharButton].ODONMLDCHHF(CharID, 1f, 10f, -11f, Characters.no_chars, 0));
                if (CharID < 0)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[CharButton].FFCNPGPALPD = $"Random from {Characters.fedData[(CharID*-1)-1].name}";
                }
                else if (CharID == 0)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[CharButton].FFCNPGPALPD = "Random";
                }
                else
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[CharButton].FFCNPGPALPD = $"[{CharID}] {Characters.c[CharID].name}";
                }
                Costume = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[CostumeButton].ODONMLDCHHF(Costume, 1f, 10f, 0f, 3, 0));
                switch (Costume)
                {
                    case 0:
                        LIPNHOMGGHF.FKANHDIMMBJ[CostumeButton].FFCNPGPALPD = "Random, based on role";
                        break;
                    case 1:
                        LIPNHOMGGHF.FKANHDIMMBJ[CostumeButton].FFCNPGPALPD = "Wrestling";
                        break;
                    case 2:
                        LIPNHOMGGHF.FKANHDIMMBJ[CostumeButton].FFCNPGPALPD = "Casual";
                        break;
                    case 3:
                        LIPNHOMGGHF.FKANHDIMMBJ[CostumeButton].FFCNPGPALPD = "Referee";
                        break;
                }
                Preset = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[PresetButton].ODONMLDCHHF(Preset, 1f, 10f, -1f, Presets.Count-1, 0));
                if (Preset == -1)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[PresetButton].FFCNPGPALPD = "Base";
                }
                else
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[PresetButton].FFCNPGPALPD = Presets[Preset].Name;
                }
                Rarity = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[RarityButton].ODONMLDCHHF(Rarity, 1f, 10f, -1f, 3, 0));
                switch (Rarity)
                {
                    case -1:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityButton].FFCNPGPALPD = "Random";
                        break;
                    case 0:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityButton].FFCNPGPALPD = "Base";
                        break;
                    case 1:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityButton].FFCNPGPALPD = "Bronze";
                        break;
                    case 2:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityButton].FFCNPGPALPD = "Silver";
                        break;
                    case 3:
                        LIPNHOMGGHF.FKANHDIMMBJ[RarityButton].FFCNPGPALPD = "Gold";
                        break;
                }
                Foil = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[FoilButton].ODONMLDCHHF(Foil, 1f, 10f, -1f, 1, 0));
                if (Foil == -1)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[FoilButton].FFCNPGPALPD = "Random";
                }
                Signature = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[SignatureButton].ODONMLDCHHF(Signature, 1f, 10f, -1f, 1, 0));
                if (Signature == -1)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[SignatureButton].FFCNPGPALPD = "Random";
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN == 5 && LIPNHOMGGHF.FKANHDIMMBJ[GenerateButton].CLMDCNDEBGD != 0)
                {

                    int charid = -1;
                    if (CharID < 0)
                    {
                        int fednum = (CharID * -1) - 1;
                        charid = Characters.fedData[fednum].roster[UnityEngine.Random.Range(1, Characters.fedData[fednum].size + 1)];                        
                    }
                    else if (CharID == 0)
                    {
                        charid = -1;
                    }
                    else
                    {
                        charid = CharID;
                    }
                    string preset = Preset == -1 ? "" : Presets[Preset].FullName;
                    int rarity = Rarity == -1 ? -1 : Rarity;
                    CollectibleCards2.Plugin.GenerateSingleCard((fileName) =>
                    {
                        DisplayCard(fileName);
                    }, CharID:charid, costume:Costume, preset:preset, borderRarity:rarity, foilRarity:Foil, signatureRarity:Signature, customGenerated:true);
                    
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN > 5 || LIPNHOMGGHF.FKANHDIMMBJ[CharButton].CLMDCNDEBGD != 0)
                {
                    LIPNHOMGGHF.PIEMLEPEDFN = 0;
                }


                if (LIPNHOMGGHF.PIEMLEPEDFN <= -5)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = CollectibleCards2.Plugin.CardsMenuPage;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
                }
            }
        }
        public static void DisplayCard(string fileName)
        {
            byte[] array2 = File.ReadAllBytes(fileName);
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
                rectTransform.sizeDelta = new Vector2(texture2D.width/2, texture2D.height/2);
                rectTransform.anchoredPosition = new Vector2(-200, -100);
                rawImage.transform.SetAsLastSibling();
            }
        }
        //disabling annoying audio
        [HarmonyPatch(typeof(CHLPMKEGJBJ), nameof(CHLPMKEGJBJ.DNNPEAOCDOG))]
        [HarmonyPrefix]
        public static bool CHLPMKEGJBJ_DNNPEAOCDOG_Prefix(AudioClip GGMBIAAEMKO, float ELJKCOHGBBD = 0f, float CDNNGHGFALM = 1f)
        {
            if (GGMBIAAEMKO == CHLPMKEGJBJ.PAJJMPLBDPL && LIPNHOMGGHF.ODOAPLMOJPD == CardsGenPage)
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