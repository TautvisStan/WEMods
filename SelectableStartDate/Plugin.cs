using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace PromotionEditorUnlock
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.SelectableStartDate";
        public const string PluginName = "SelectableStartDate";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<int> StartDate;
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            StartDate = Config.Bind("General",
             "Starting week",
             1,
             "New starting week number (1-48).");
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
        [HarmonyPatch(typeof(Progress))]
        public static class ProgressPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(Progress.HEPPCLJCKHM))]
            public static void Progress_ICNGJHENMEN_Patch()
            {
                Progress.date = StartDate.Value;
            }
        }
    }
}