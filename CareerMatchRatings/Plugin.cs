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
        public const string PluginVer = "1.0.1";

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
                if (LIPNHOMGGHF.GCJKOBOBIGA == 0)
                {
                    if (FFCEGMEAIBP.LOBDMDPMFLK >= 2 && FFCEGMEAIBP.AEKLGCEFIHM <= 0)
                    {
                        if (FFCEGMEAIBP.MONJNNCCICI == null)
                        {
                            FFCEGMEAIBP.MFIOAPKFAEF();
                            FFCEGMEAIBP.KGFCDMHGJHL(3);
                        }
                        FFCEGMEAIBP.FLOHFJIGHNK();
                    }
                }
            }
        }*/
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Start))]  //creating ratings game object
        [HarmonyPostfix]
        static void Scene_Game_StartPostfix()
        {
            if (FFCEGMEAIBP.LOBDMDPMFLK != 0 && FFCEGMEAIBP.AEKLGCEFIHM <= 0)
            {
                FFCEGMEAIBP.MFIOAPKFAEF();
            }
        }
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Update))]  //updating ratings
        [HarmonyPostfix]
        static void Scene_Game_UpdatePostfix()
        {
            if (LIPNHOMGGHF.GCJKOBOBIGA == 0)
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK >= 2 && FFCEGMEAIBP.AEKLGCEFIHM <= 0)
                {
                    if (FFCEGMEAIBP.MONJNNCCICI != null)
                    {
                        FFCEGMEAIBP.FLOHFJIGHNK();
                    }
                }
            }
        }
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.DMJFCHKLEFH))]  //Match start
        [HarmonyPostfix]
        static void FFCEGMEAIBP_DMJFCHKLEFHPostfix()
        {
            if (FFCEGMEAIBP.AEKLGCEFIHM <= 0)
            {
                FFCEGMEAIBP.KGFCDMHGJHL(NAEEIFNFBBO.CGOHLKMAFOB);
            }
        }
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.EAAIHKLJFCM))]  //Match restart
        [HarmonyPostfix]
        static void FFCEGMEAIBP_EAAIHKLJFCMPostfix()
        {
            if (FFCEGMEAIBP.AEKLGCEFIHM <= 0)
            {
                FFCEGMEAIBP.MBJFIEPNHPP = FFCEGMEAIBP.LPCIJOCKGHB * 0.75f * FFCEGMEAIBP.HFHBNLJKNJI;
                FFCEGMEAIBP.IJOIMLGJION = FFCEGMEAIBP.MANDMCIEJBK * 0.75f * FFCEGMEAIBP.HFHBNLJKNJI;
                FFCEGMEAIBP.KGFCDMHGJHL(NAEEIFNFBBO.CGOHLKMAFOB);
            }
        }
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.BAGEPNPJPLD))]  //Match end
        [HarmonyPostfix]
        static void FFCEGMEAIBP_BAGEPNPJPLDPostfix()
        {
            if (FFCEGMEAIBP.AEKLGCEFIHM <= 0)
            {
                FFCEGMEAIBP.FLOHFJIGHNK();
                FFCEGMEAIBP.KGFCDMHGJHL(1);
                FFCEGMEAIBP.GADAJGJBMIB(FFCEGMEAIBP.FMBDEENKKJG, FFCEGMEAIBP.LPCIJOCKGHB, 5500f, 1);
            }
        }

}

}