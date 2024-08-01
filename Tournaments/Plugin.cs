//TODO auto set up the wrestlers; scene nav in the match; ending the match saves the results and sets the winners further

using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tournaments
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.Tournaments";
        public const string PluginName = "Tournaments";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        static int TournamentButton;
        static readonly int TournamentNum = -789987;
        public static CustomCard TournamentCard { get; set; } = null;

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

        public static bool InCustomMode()
        {
            return NAEEIFNFBBO.CBMHGKFFHJE == TournamentNum || LIPNHOMGGHF.BCKLOCJPIMD == TournamentNum;
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
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Tournament", 0f, -0f, 1.5f, 1.5f);
                    TournamentButton = LIPNHOMGGHF.HOAOLPGEBKJ;
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
                if (LIPNHOMGGHF.NNMDEFLLNBF == TournamentButton)
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = TournamentNum;
                    LIPNHOMGGHF.BCKLOCJPIMD = TournamentNum;
                    Progress.date = 0;
                    Progress.focDate = 0;

                    LIPNHOMGGHF.PMIIOCMHEAE(12); //card
                }
            }
        }
        //Scene nav stuff
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.PMIIOCMHEAE))]
        [HarmonyPrefix]
        public static void LIPNHOMGGHF_PMIIOCMHEAE_Patch(ref int KBEAJEIMNMI)
        {
            //Doing stuff when going match setup -> game
            if (SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == TournamentNum && KBEAJEIMNMI == 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = TournamentNum;
                FFCEGMEAIBP.EBMPAEBEMNE = 0;
                return;
            }

            //Doing stuff when exiting match setup
            if (SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == TournamentNum && KBEAJEIMNMI != 50)
            {
                if (KBEAJEIMNMI != 11)
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = TournamentNum;
                    LIPNHOMGGHF.BCKLOCJPIMD = TournamentNum;
                    KBEAJEIMNMI = 12;   //card
                }
                else
                {

                }
                return;
            }
            //Doing stuff when exiting card
            if (SceneManager.GetActiveScene().name == "Card" && NAEEIFNFBBO.CBMHGKFFHJE == TournamentNum && KBEAJEIMNMI != 14)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = 0;
                KBEAJEIMNMI = 1;   //titles
                return;
            }

            /*     //Doing stuff when exiting match
                 if (SceneManager.GetActiveScene().name == "Game" && LIPNHOMGGHF.BCKLOCJPIMD == RogueliteNum)
                 {
                     if (!Won)
                     {
                         NAEEIFNFBBO.CBMHGKFFHJE = RogueliteNum;
                         LIPNHOMGGHF.BCKLOCJPIMD = RogueliteNum;
                         KBEAJEIMNMI = 14;  //match setup redirect
                     }
                     else
                     {
                         Won = false;
                         NAEEIFNFBBO.CBMHGKFFHJE = 0;
                         LIPNHOMGGHF.BCKLOCJPIMD = 0;
                         KBEAJEIMNMI = 1;   //titles
                     }
                     return;
                 }*/

        }


        //replacing the vanilla game card class with my own custom class
        [HarmonyPatch(typeof(Card), nameof(Card.PIMGMPBCODM))]
        [HarmonyPrefix]
        public static bool Card_PIMGMPBCODM(ref Card __instance, int DCODOPDDJOA, int EDCMPAAEHFO = 6)
        {
            if (InCustomMode())
            {
                if (TournamentCard == null)
                {
                    __instance = new CustomCard();
                }
                else
                {
                    __instance = TournamentCard;
                }
                TournamentCard = (CustomCard)__instance;
                FFCEGMEAIBP.IOBFNECDOIH = __instance;
                return false;
            }
            return true;
        }
        //Removing the unneeded button
        [HarmonyPatch(typeof(Scene_Card), nameof(Scene_Card.Start))]
        [HarmonyPostfix]
        public static void Scene_Card_Start(Scene_Card __instance)
        {
            if (InCustomMode())
            {
                __instance.gManual.SetActive(false);
                FFCEGMEAIBP.EJBBMLKPNFK[11].SetActive(false);
            }
        }
        //Redirecting to the match setup
        [HarmonyPatch(typeof(Scene_Card), nameof(Scene_Card.Update))]
        [HarmonyPostfix]
        public static void Scene_Card_Update(Scene_Card __instance)
        {
            if (InCustomMode())
            {
                if(LIPNHOMGGHF.PIEMLEPEDFN >= 15)
                {
                    TournamentCard.CurrentSegment = LIPNHOMGGHF.NNMDEFLLNBF;
                    LIPNHOMGGHF.PMIIOCMHEAE(14);
                }
            }
        }
        //Renaming the card name
        [HarmonyPatch(typeof(Scene_Card), nameof(Scene_Card.DescribeContext))]
        [HarmonyPostfix]
        public static void Scene_Card_DescribeContext(Scene_Card __instance, ref string __result)
        {
            if (InCustomMode())
            {
                __result = "Tournament Card:";
            }
        }
        //Renaming the card segments
        [HarmonyPatch(typeof(Card), nameof(Card.LLGKMFDLEJC))]
        [HarmonyPostfix]
        public static void Card_LLGKMFDLEJC(Card __instance, ref string __result, int KJELLNJFNGO)
        {
            if (InCustomMode())
            {
                switch (KJELLNJFNGO)
                {
                    case 1:
                        __result = "Finals";
                        break;
                    case 2:
                    case 3:
                        __result = "Semi Finals";
                        break;
                    case 4:
                    case 5:
                    case 6:
                        __result = "?????";
                        break;
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                        __result = "Quarter Finals";
                        break;
                }
            }
        }

    }
}