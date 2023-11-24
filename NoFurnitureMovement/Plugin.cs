using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace NoFurnitureMovement
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.NoFurnitureMovement";
        public const string PluginName = "NoFurnitureMovement";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static List<int> MoveableFurniture = new List<int>();

        public static ConfigEntry<string> MoveableFurnitureConfig;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            MoveableFurnitureConfig = Config.Bind("General",
             "Moveable Furniture IDs",
             "2 7 8 9 10 14 16 18 33",
             "Furniture IDs that will be ignored by this mod and will get moved on contact");

            string[] IDs = MoveableFurnitureConfig.Value.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string ID in IDs)
            {
                MoveableFurniture.Add(int.Parse(ID));
            }

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


        [HarmonyPatch(typeof(GGKBLABCJFN), nameof(GGKBLABCJFN.IPDDNOLDFLE))]  
        [HarmonyPrefix]
        static bool GGKBLABCJFN_IPDDNOLDFLEPrefix(GGKBLABCJFN __instance)
        {
            if (MoveableFurniture.Contains(__instance.BPJFLJPKKJK)) return true;
            return false;
        }

    }

}