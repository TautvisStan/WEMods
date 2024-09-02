// scan whole plugins folder

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
        public const string PluginVer = "0.9.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        public static string PluginPath;


        public static int CardWidth { get; set; } = 719;
        public static int CardHeight { get; set; } = 1000;

        public static int CardsMenuButton { get; set; }
        public static int CardsMenuButtonCareer { get; set; }
        public static int CardsMenuPage { get; set; } = 741;
        public static int EntryMenu { get; set; } = 0;
        public static string CardsDirectory { get; set; } = Path.Combine(Application.persistentDataPath, "Cards");
        private static bool ShouldAwardCard { get; set; } = false;
        public static Plugin ThisPlugin { get; set; }
        private void Awake()
        {
            ThisPlugin = this;
            Plugin.Log = base.Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);

            Background.GetShaderPrefab();
            OverlaytxtFileParser.LoadSigFont();
            OverlaytxtFileParser.LoadCardFont();
            CanvasController.LoadShaders();
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
            if (LIPNHOMGGHF.FAKHAFKOBPB == 20)
            {
                if (NAEEIFNFBBO.CBMHGKFFHJE == 1)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Collectible Cards", 0f, -125f, 1f, 1f);
                    CardsMenuButtonCareer = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
            }
        }
        //Setting up the main menu button redirect
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch(Scene_Titles __instance)
        {
            if (LIPNHOMGGHF.PIEMLEPEDFN >= 5 && LIPNHOMGGHF.ODOAPLMOJPD == 0 && NAEEIFNFBBO.EKFJCKLKELN != 1)
            {
                if (LIPNHOMGGHF.NNMDEFLLNBF == CardsMenuButton)
                {
                    LIPNHOMGGHF.ODOAPLMOJPD = CardsMenuPage;
                    EntryMenu = 1;
                    LIPNHOMGGHF.ICGNAJFLAHL(0);
                }
            }
        }
        //Career -> Cards open menu
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Start))]
        [HarmonyPostfix]
        public static void Scene_Titles_Start_Patch()
        {
            if(EntryMenu == 2)
            {
                LIPNHOMGGHF.ODOAPLMOJPD = CardsMenuPage;
                LIPNHOMGGHF.ICGNAJFLAHL(0);
            }
        }
        //Setting up the calendar menu button redirect
        [HarmonyPatch(typeof(Scene_Calendar), nameof(Scene_Calendar.Update))]
        [HarmonyPostfix]
        public static void Scene_Calendar_Update_Patch()
        {
            if (LIPNHOMGGHF.PIEMLEPEDFN >= 5)
            {
                if (LIPNHOMGGHF.NNMDEFLLNBF == CardsMenuButtonCareer)
                {
                   
                    LIPNHOMGGHF.PMIIOCMHEAE(1);
                    EntryMenu = 2;
                }
            }
        }
        //Preparing to award a card on career win
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.BAGEPNPJPLD))]
        [HarmonyPostfix]
        public static void FFCEGMEAIBP_BAGEPNPJPLD_Patch(int KJELLNJFNGO)
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == 1)
            {
                int playerindex = 0;
                for(int i = 1; i < NJBJIIIACEP.OAAMGFLINOB.Length; i++)
                {
                    if (NJBJIIIACEP.OAAMGFLINOB[i].GOOKPABIPBC == Characters.wrestler)
                    {
                        playerindex = i;
                        break;
                    }
                }
                if ((FFCEGMEAIBP.OLJFOJOLLOM > 0 && NJBJIIIACEP.OAAMGFLINOB[KJELLNJFNGO].LBCFAJGDKJP == NJBJIIIACEP.OAAMGFLINOB[playerindex].LBCFAJGDKJP) || NJBJIIIACEP.OAAMGFLINOB[KJELLNJFNGO].GOOKPABIPBC == Characters.wrestler)
                {
                    ShouldAwardCard = true;
                }
            }
        }
        //The match got restarted
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.EAAIHKLJFCM))]
        [HarmonyPostfix]
        public static void LIPNHOMGGHF_EAAIHKLJFCM_Patch()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == 1)
            {
                ShouldAwardCard = false;
            }
        }

        [HarmonyPatch(typeof(Scene_Finances), nameof(Scene_Finances.Start))]
        [HarmonyPostfix]
        public static void Scene_Finances_Start_Patch()
        {
            if (ShouldAwardCard == true)
            {
                GenerateSingleCard((action) => { });
                ShouldAwardCard = false;
            }
        }
        [HarmonyPatch(typeof(Scene_NextWeek), nameof(Scene_NextWeek.Start))]
        [HarmonyPostfix]
        public static void Scene_NextWeek_Start_Patch()
        {
            if (ShouldAwardCard == true)
            {
                GenerateSingleCard((action) => { });
                ShouldAwardCard = false;
            }
        }

    }
}