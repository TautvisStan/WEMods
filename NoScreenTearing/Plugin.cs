using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace NoScreenTearing
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.NoScreenTearing";
        public const string PluginName = "NoScreenTearing";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<int> vsyncLevel;
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            vsyncLevel = Config.Bind("General", "vSync level", 1, new ConfigDescription("Sets the vsync level. Use it if the game is running too fast.", new AcceptableValueRange<int>(1, 3)));
           
        }
        private void Start()
        {
             QualitySettings.vSyncCount = vsyncLevel.Value;
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
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.NMGIGLPKPCM))]
        [HarmonyPostfix]
        static void NMGIGLPKPCM_Postfix()
        {
            QualitySettings.vSyncCount = vsyncLevel.Value;
        }
        [HarmonyPatch(typeof(NAEEIFNFBBO), nameof(NAEEIFNFBBO.EBIFMMHNGKL))]
        [HarmonyPostfix]
        static void EBIFMMHNGKL_Postfix()
        {
            QualitySettings.vSyncCount = vsyncLevel.Value;
        }
        [HarmonyPatch(typeof(NAEEIFNFBBO), nameof(NAEEIFNFBBO.LKLGAIACOLB))]
        [HarmonyPostfix]
        static void LKLGAIACOLB_Postfix()
        {
            QualitySettings.vSyncCount = vsyncLevel.Value;
        }
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Update))]
        [HarmonyPostfix]
        static void SceneGameUpdate_Postfix()
        {
            QualitySettings.vSyncCount = vsyncLevel.Value;
        }
    }
}