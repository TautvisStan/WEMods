using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace EpicArenaInBooker
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.EpicArenaInBooker";
        public const string PluginName = "EpicArenaInBooker";
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
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Start))]
        [HarmonyPostfix]
        static void Scene_Match_Setup_Postfix()
        {
            int i = -2;  //Epic Arena
            if (!World.library.Contains(i))
            {
                int[] array = new int[World.library.Length + 1];
                World.library.CopyTo(array, 0);
                World.library = array;
                World.library[World.library.Length - 1] = i;
                if (i == World.location)
                {
                    World.libraryFoc = World.library.Length - 1;
                }
            }
        }
    }
}