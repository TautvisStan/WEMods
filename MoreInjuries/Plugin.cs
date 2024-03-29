using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace MoreInjuries
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.MoreInjuries";
        public const string PluginName = "MoreInjuries";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<int> configIncreasedTimes;
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);


            configIncreasedTimes = Config.Bind("General",
             "Increased times",
             50,
             new ConfigDescription("How many times injuries are likely"));
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
        [HarmonyPatch(typeof(DFOGOCNBECG))]
        public static class ProgressPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(DFOGOCNBECG.KCFNOONDGKE))]
            public static void KCFNOONDGKEPatch(DFOGOCNBECG __instance, int __result, int CNHJJIPILFK,ref float DGJAEIBKLJO)
            {
                DGJAEIBKLJO *= configIncreasedTimes.Value;
            }
        }
    }
}