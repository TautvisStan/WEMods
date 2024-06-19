using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueliteMode
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.RogueliteMode";
        public const string PluginName = "RogueliteMode";
        public const string PluginVer = "0.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        static int RogueliteButton;
        static int RogueliteModeNum = 123654;
        static int SelectedChar = 0;


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

        //adding new button to the main menu
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ICGNAJFLAHL_Patch()
        {
            if (LIPNHOMGGHF.FAKHAFKOBPB == 1)
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == 1)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Roguelite Mode", 0f, 0f, 1.5f, 1.5f);
                    RogueliteButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
            }
        }


        //Setting up the main menu button redirect
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.PIEMLEPEDFN >= 5 && LIPNHOMGGHF.ODOAPLMOJPD == 1 && NAEEIFNFBBO.EKFJCKLKELN != 1)
            {
                if (LIPNHOMGGHF.NNMDEFLLNBF == RogueliteButton)
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = RogueliteModeNum;
                    LIPNHOMGGHF.BCKLOCJPIMD = RogueliteModeNum;
                    LIPNHOMGGHF.PMIIOCMHEAE(11);
                }
            }
        }




        //Setting up the char select back button redirect
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.Update))]
        [HarmonyPostfix]
        public static void Scene_Select_Char_Update_Patch()
        {
            if (LIPNHOMGGHF.PIEMLEPEDFN <= -5)
            {
                if (LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
                {
                    LIPNHOMGGHF.PMIIOCMHEAE(1);
                }
            }
        }


        //ending after player loses
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.NCAAOLGAGCG))]
        [HarmonyPostfix]
        public static void FFCEGMEAIBP_NCAAOLGAGCG_Patch(int KJELLNJFNGO, int GKNIAFAOLJK)
        {
            if(NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum || LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
            {
                if(GKNIAFAOLJK == 1) //TODO Replace with selected char id
                {
                    FFCEGMEAIBP.BAGEPNPJPLD(KJELLNJFNGO);
                }
            }
        }

        //????
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.PMIIOCMHEAE))]
        [HarmonyPrefix]
        public static void LIPNHOMGGHF_PMIIOCMHEAE_Patch(ref int KBEAJEIMNMI)
        {
            //Doing stuff when going match setup -> game
            if(SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum && KBEAJEIMNMI == 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = RogueliteModeNum;
                return;
            }

            //Doing stuff when exiting match setup
            if (SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum && KBEAJEIMNMI != 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = RogueliteModeNum;
                KBEAJEIMNMI = 1;
                return;
            }

            //Doing stuff when exiting match
            if (SceneManager.GetActiveScene().name == "Game" && LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = RogueliteModeNum;
                LIPNHOMGGHF.BCKLOCJPIMD = 0;
                KBEAJEIMNMI = 1;
                return;
            }
        }

        //disable interference and simulate buttons;
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.OPBBKBAJJHA))]
        [HarmonyPostfix]
        public static void LIPNHOMGGHF_OPBBKBAJJHA_Patch()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == RogueliteModeNum || LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
            {
                LIPNHOMGGHF.FKANHDIMMBJ[3].AHBNKMMMGFI = 0;
                LIPNHOMGGHF.FKANHDIMMBJ[4].AHBNKMMMGFI = 0;
            }
        }
        //disable tabs in the match setup

        //Set up the selected character

        //set up opponents



        //Setting up the char selection button redirect
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.EGPFEGLDMJM))]
        [HarmonyPostfix]
        public static void Scene_Select_Char_EGPFEGLDMJM_Patch(Scene_Select_Char __instance, int GOOKPABIPBC)
        {
            if (LIPNHOMGGHF.BCKLOCJPIMD == RogueliteModeNum)
            {
                SelectedChar = GOOKPABIPBC;

                FFCEGMEAIBP.JELMGJMKKEK(22);


                   FFCEGMEAIBP.LCCCCENGFOK = 4;
                   FFCEGMEAIBP.JPBHIEOKODO = 0;
                   FFCEGMEAIBP.BPJFLJPKKJK = 5;
                   FFCEGMEAIBP.CADLONHABMC = 2;
                   FFCEGMEAIBP.OLJFOJOLLOM = -1;
                   FFCEGMEAIBP.LGHMLHICAFL = 2;
                   FFCEGMEAIBP.DOLNEDHNKMM = 0;
                   FFCEGMEAIBP.GDKCEGBINCM = 2;
                   FFCEGMEAIBP.NBAFIEALMHN = 0;

                FFCEGMEAIBP.OHBEGHIIHJB = 0;
                FFCEGMEAIBP.LOBDMDPMFLK = 1;

              //  NAEEIFNFBBO.CBMHGKFFHJE = 0;//?????????
                FFCEGMEAIBP.EBMPAEBEMNE = 0;
                FFCEGMEAIBP.AEKLGCEFIHM = 0;


                
                LIPNHOMGGHF.PMIIOCMHEAE(14);
            }
        }
    }
}