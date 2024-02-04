using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace NoCameraPanningDuringMatches
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.NoCameraPanningDuringMatches";
        public const string PluginName = "NoCameraPanningDuringMatches";
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
        [HarmonyPatch(typeof(BLNKDHIGFAN))]
        public static class BLNKDHIGFANPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(BLNKDHIGFAN.GMGLHLHLDGM))]
            public static bool GMGLHLHLDGMPatch()
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK == 2)
                {
                    return false;
                }
                return true;
            }
        }
    }
}