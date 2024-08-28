//todo injured taunt?; menu with cards; left right button + slider; text info below card; awarding cards for each career win; back arrow fix;
//menu reachable from titles menu + career calendar; optimize card scanning?;
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
        public const string PluginVer = "0.8.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        public static string PluginPath;


        public static int CardWidth { get; set; } = 719;
        public static int CardHeight { get; set; } = 1000;

        public static ConfigEntry<int> CameraMode { get; set; }

        public static int CardsMenuButton { get; set; }
        public static int CardsMenuPage { get; set; } = 741;
        public static string CardsDirectory { get; set; } = Path.Combine(Application.persistentDataPath, "Cards");
        public static Plugin ThisPlugin { get; set; }
        private void Awake()
        {
            ThisPlugin = this;
            Plugin.Log = base.Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);

            Background.GetShaderPrefab();
            OverlaytxtFileParser.LoadSigFont();
            OverlaytxtFileParser.LoadCardFont();

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
                if (LIPNHOMGGHF.NNMDEFLLNBF == CardsMenuButton)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = CardsMenuPage;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
                }
            }
        }

    }
}