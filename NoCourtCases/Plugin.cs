using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;

namespace NoCourtCases
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.NoCourtCases";
        public const string PluginName = "NoCourtCases";
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

        [HarmonyPatch(typeof(NEGAFEHECNL), nameof(NEGAFEHECNL.ICKAKILAKBB))]  //matches
        [HarmonyPrefix]
        static bool NEGAFEHECNL_ICKAKILAKBB(ref int __result, int HAAJHJFGODL, int OGPEJMGFEJJ, int HJDMCALMAOC, int MKEBAFANECN, int KPJANCMHICM)
        {
            __result = 0;
            return false;
        }
    }
}