using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;

namespace WrestlingEmpireTemplateMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.GiantThumbtacks";
        public const string PluginName = "GiantThumbtacks";
        public const string PluginVer = "1.0.2";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);
        public static ConfigEntry<float> configSize;
        internal static string PluginPath;
        

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            configSize = Config.Bind("General",
             "TacksSize",
             7.5f,
             "The size of thumbtacks");
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

        [HarmonyPatch(typeof(MEHJAJJNHLL), nameof(MEHJAJJNHLL.BLJNNEDCMEI))]
        [HarmonyPrefix]
        public static bool MEHJAJJNHLL_BLJNNEDCMEI(MEHJAJJNHLL __instance, int __0, UnityEngine.Color __1, float __2, UnityEngine.GameObject __3, float __4, float __5, float __6, float __7, float __8, float __9, int __10)
        {
            if (__0 == -69 && __2 < configSize.Value)
            {
                __instance.BLJNNEDCMEI(__0, __1, configSize.Value, __3, __4, __5, __6, __7, __8, __9, __10);
                return false;
            }
            return true;
        }
    }
}