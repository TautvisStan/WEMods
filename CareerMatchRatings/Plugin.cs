using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Diagnostics;
using System.IO;
using UnityEngine.SceneManagement;

namespace CareerMatchRatings
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CareerMatchRatings";
        public const string PluginName = "CareerMatchRatings";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        

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



     /*   private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                //Debug.Log("CORRECT SCENE");
                if (JJDCNALMPCI.FCNKGIEMNGM == 0)
                {
                    if (PHECEOMIMND.IPAFPBPKIKP >= 2 && PHECEOMIMND.FPNMMEOLGKP <= 0)
                    {
                        if (PHECEOMIMND.PIIDOKJHMEN == null)
                        {
                            PHECEOMIMND.PHANDLABCEP();
                            PHECEOMIMND.JBOPJLCBCNN(3);
                        }
                        PHECEOMIMND.OECDBBMAOHJ();
                    }
                }
            }
        }*/
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Start))]  //creating ratings game object
        [HarmonyPostfix]
        static void Scene_Game_StartPostfix()
        {
            if (PHECEOMIMND.IPAFPBPKIKP != 0 && PHECEOMIMND.FPNMMEOLGKP <= 0)
            {
                PHECEOMIMND.PHANDLABCEP();
            }
        }
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Update))]  //updating ratings
        [HarmonyPostfix]
        static void Scene_Game_UpdatePostfix()
        {
            if (JJDCNALMPCI.FCNKGIEMNGM == 0)
            {
                if (PHECEOMIMND.IPAFPBPKIKP >= 2 && PHECEOMIMND.FPNMMEOLGKP <= 0)
                {
                    if (PHECEOMIMND.PIIDOKJHMEN != null)
                    {
                        PHECEOMIMND.OECDBBMAOHJ();
                    }
                }
            }
        }
        [HarmonyPatch(typeof(PHECEOMIMND), nameof(PHECEOMIMND.NHPJFOBBCEF))]  //Match start
        [HarmonyPostfix]
        static void PHECEOMIMND_NHPJFOBBCEFPostfix()
        {
            if (PHECEOMIMND.FPNMMEOLGKP <= 0)
            {
                PHECEOMIMND.JBOPJLCBCNN(LFNJDEGJLLJ.FBHGHEHIFOK);
            }
        }
        [HarmonyPatch(typeof(PHECEOMIMND), nameof(PHECEOMIMND.JLHIGPPGGDG))]  //Match restart
        [HarmonyPostfix]
        static void PHECEOMIMND_JLHIGPPGGDGPostfix()
        {
            if (PHECEOMIMND.FPNMMEOLGKP <= 0)
            {
                PHECEOMIMND.FGMJCPFIIEE = PHECEOMIMND.ONBIIDFHLPH * 0.75f * PHECEOMIMND.GNLJKMOOECN;
                PHECEOMIMND.HBBLNFPONMG = PHECEOMIMND.AOHFPCPNEFJ * 0.75f * PHECEOMIMND.GNLJKMOOECN;
                PHECEOMIMND.JBOPJLCBCNN(LFNJDEGJLLJ.FBHGHEHIFOK);
            }
        }
        [HarmonyPatch(typeof(PHECEOMIMND), nameof(PHECEOMIMND.HDKPGMAKCLO))]  //Match end
        [HarmonyPostfix]
        static void PHECEOMIMND_HDKPGMAKCLOPostfix()
        {
            if (PHECEOMIMND.FPNMMEOLGKP <= 0)
            {
                PHECEOMIMND.OECDBBMAOHJ();
                PHECEOMIMND.JBOPJLCBCNN(1);
                PHECEOMIMND.FHGELGGEKBL(PHECEOMIMND.HHNMPIGANJO, PHECEOMIMND.ONBIIDFHLPH, 5500f, 1);
            }
        }

}

}