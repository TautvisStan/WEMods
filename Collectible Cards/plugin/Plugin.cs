//todo costumes based on character role; injured taunt?; menu with cards; left right button + slider; text info below card; fix the text overflow (sig, char id 154);
// custom card generator separate project; add char pos (+camera?) to meta.txt;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CollectibleCards2
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CollectibleCards";
        public const string PluginName = "CollectibleCards";
        public const string PluginVer = "0.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        public static string PluginPath;


        public static int CardWidth { get; set; } = 719;
        public static int CardHeight { get; set; } = 1000;

        public static ConfigEntry<float> CharXPos { get; set; }
        public static ConfigEntry<float> CharYPos { get; set; }
        public static ConfigEntry<float> CharSize { get; set; }

        public static ConfigEntry<int> CameraMode { get; set; }

        public static int CardsMenuButton { get; set; }
        public static int CardsMenuPage { get; set; } = 741;

        public static Plugin ThisPlugin { get; set; }
        private void Awake()
        {
            ThisPlugin = this;
            Plugin.Log = base.Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);

            Background.GetShaderPrefab();
            OverlaytxtFileParser.LoadSigFont();
            OverlaytxtFileParser.LoadCardFont();

            CharXPos = Config.Bind("General",
             "Character x position",
             0f,
             "Character x position");
            CharYPos = Config.Bind("General",
             "Character y position",
             0f,
             "Character y position");
            CharSize = Config.Bind("General",
             "Character scale",
             1f,
             "Character scale");

            CameraMode = Config.Bind("General",
             "Camera mode",
             0,
             "0 - perspective, 1 - orthographic");
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


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                GenerateSingleCard((action) => { }, preset:"preset_1");
            }
        }
        public static void GenerateSingleCard(Action<string> onGenerated, int CharID = -1, string preset = "", int borderRarity = -1, int foilRarity = -1, int signatureRarity = -1, bool customGenerated = false)
        {
            ThisPlugin.StartCoroutine(CollectibleCardGenerator.GenerateCollectibleCard(onGenerated, CharID, preset, borderRarity, foilRarity, signatureRarity, customGenerated));
        }

        //adding new button to the main menu
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ICGNAJFLAHL_Patch()
        {
            if (LIPNHOMGGHF.FAKHAFKOBPB == 1)
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == 0)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Collectible Cards", 200f, -300f, 1.5f, 1.5f);
                    CardsMenuButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
            }
        }
        //Setting up the main menu button redirect
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.PIEMLEPEDFN >= 5 && LIPNHOMGGHF.ODOAPLMOJPD == 0 && NAEEIFNFBBO.EKFJCKLKELN != 1)
            {
                Debug.LogWarning("TEST");
                if (LIPNHOMGGHF.NNMDEFLLNBF == CardsMenuButton)
                {

                    LIPNHOMGGHF.ODOAPLMOJPD = CardsMenuPage;
                    Debug.LogWarning(LIPNHOMGGHF.FAKHAFKOBPB);
                    Debug.LogWarning("TESTAS");
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
                }
            }
        }
    }
}