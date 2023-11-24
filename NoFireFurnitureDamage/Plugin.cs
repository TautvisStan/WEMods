using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;

namespace WrestlingEmpireTemplateMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.NoFireFurnitureDamage";
        public const string PluginName = "NoFireFurnitureDamage";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static float? oldHealth;


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
        [HarmonyPatch(typeof(GGKBLABCJFN), nameof(GGKBLABCJFN.ABOPBNILIAK))]
        [HarmonyPrefix]
        private static void GGKBLABCJFN_ABOPBNILIAKPrefix(GGKBLABCJFN __instance)
        {
            if (__instance.OFEFGOADEBM != null && __instance.HBCKJOKMGFN > 0 && NAEEIFNFBBO.ECGKBJEKPPJ > 0)
            {
                if (__instance.HLGALFAGDGC > 0f)
                {
                    Plugin.oldHealth = new float?(__instance.HLGALFAGDGC);
                }
            }
        }
        [HarmonyPatch(typeof(GGKBLABCJFN), nameof(GGKBLABCJFN.ABOPBNILIAK))]
        [HarmonyPostfix]
        private static void GGKBLABCJFN_ABOPBNILIAKPostfix(GGKBLABCJFN __instance)
        {
            if (__instance.OFEFGOADEBM != null && __instance.HBCKJOKMGFN > 0 && NAEEIFNFBBO.ECGKBJEKPPJ > 0)
            {
                if (Plugin.oldHealth != null)
                {
                    __instance.HLGALFAGDGC = Plugin.oldHealth.Value;
                    Plugin.oldHealth = null;
                }
            }
        }
    }
}