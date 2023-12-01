using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ExhibitionClones
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.ExhibitionClones";
        public const string PluginName = "ExhibitionClones";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);
        internal static string PluginPath;
        public static ConfigEntry<bool> AllowClonesInBooker;
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            AllowClonesInBooker = Config.Bind("Controls",
             "Allow clones in booker mode",
             false,
             "Allow selecting clones in the booker mode");

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

        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.CADGOLBBAEC))]
        [HarmonyPostfix]
        private static void FFCEGMEAIBP_CADGOLBBAEC_Postfix(ref int __result, int GOOKPABIPBC)
        {
            if (AllowClonesInBooker.Value == false)
            {
                if (NAEEIFNFBBO.CBMHGKFFHJE == 0)
                {
                    if (SceneManager.GetActiveScene().name == "Select_Char")
                        __result = 0;
                }
            }
            else
            {
                if (NAEEIFNFBBO.CBMHGKFFHJE == 0 || NAEEIFNFBBO.CBMHGKFFHJE == 2)
                {
                    if (SceneManager.GetActiveScene().name == "Select_Char")
                        __result = 0;
                }
            }

        }



    }
}