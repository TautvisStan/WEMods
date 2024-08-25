//todo back button; scan meta on opening the menu; rename rarity button text (base, bronze, silver, gold, etc)

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using CollectibleCards2;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CardGen
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CardGen";
        public const string PluginName = "CardGen";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        private static RawImage rawImage;
        private static Texture2D texture2D;

        public static int CardsMenuPage { get; set; } = CollectibleCards2.Plugin.CardsMenuPage;
        public static List<string> Presets { get; set; } = new();
        public static GameObject CardObject { get; set; } = null;
        public static int CharButton { get; set; }
        public static int CharID { get; set; } = -1;
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

        void Start()
        {
            Debug.LogWarning("STARTING");
            ScanPresets();
        }
        public static void ScanPresets()
        {
            string path = CollectibleCards2.Plugin.PluginPath;
            ScanFolder(path);
        }
        static void ScanFolder(string path)
        {
            Debug.LogWarning("SCANNING " + path);
            DirectoryInfo directoryInfo = new(path);
            FileInfo[] files = directoryInfo.GetFiles("meta.txt");
            if (files.Length != 0)
            {
                Debug.LogWarning("FOUND " + directoryInfo.Name);
                Presets.Add(directoryInfo.Name);
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
                Debug.LogWarning(LIPNHOMGGHF.ODOAPLMOJPD);
                if (LIPNHOMGGHF.ODOAPLMOJPD == CardsMenuPage)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(3, "Character", 375f, 250f, 1.5f, 1.5f);
                    CharButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Preset", 375f, 150f, 1.5f, 1.5f);
                    PresetButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Rarity", 375f, 50f, 1.5f, 1.5f);
                    RarityButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Foil", 375f, -50f, 1.5f, 1.5f);
                    FoilButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Signature", 375f, -150f, 1.5f, 1.5f);
                    SignatureButton = LIPNHOMGGHF.HOAOLPGEBKJ;

                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Generate", 375f, -250f, 1.5f, 1.5f);
                    GenerateButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
            }
        }

        //handling buttons
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.ODOAPLMOJPD == CardsMenuPage)
            {
                CharID = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[CharButton].ODONMLDCHHF(CharID, 1f, 10f, 0f, Characters.no_chars, 0));
                if (CharID == 0)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[CharButton].FFCNPGPALPD = "Random";
                }
                Preset = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[PresetButton].ODONMLDCHHF(Preset, 1f, 10f, -1f, Presets.Count-1, 0));
                if (Preset == -1)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[PresetButton].FFCNPGPALPD = "Base";
                }
                else
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[PresetButton].FFCNPGPALPD = Presets[Preset];
                }
                Rarity = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[RarityButton].ODONMLDCHHF(Rarity, 1f, 10f, -1f, 3, 0));
                if (Rarity == -1)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[RarityButton].FFCNPGPALPD = "Random";
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
                    int charid = CharID == 0 ? -1 : CharID;
                    string preset = Preset == -1 ? "" : Presets[Preset];
                    int rarity = Rarity == 0 ? -1 : Rarity;
                    CollectibleCards2.Plugin.GenerateSingleCard((fileName) =>
                    {
                        Debug.LogWarning("filename is " + fileName);
                        DisplayCard(fileName);
                    }, charid, preset, rarity, Foil, Signature, true);
                    
                }
                if (LIPNHOMGGHF.PIEMLEPEDFN > 5 || LIPNHOMGGHF.FKANHDIMMBJ[CharButton].CLMDCNDEBGD != 0)
                {
                    LIPNHOMGGHF.PIEMLEPEDFN = 0;
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
                rectTransform.anchoredPosition = new Vector2(-200, 0);
                rawImage.transform.SetAsLastSibling();
            }
        }
    }
}