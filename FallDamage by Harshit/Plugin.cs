using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;

namespace IncreasedFallDamage
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.IncreasedFallDamage";
        public const string PluginName = "IncreasedFallDamage";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<int> multiplier;
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            multiplier = Config.Bind("General", "Additional gravity multiplier", 500, "Additional gravity multiplier");
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
        [HarmonyPostfix]
        [HarmonyPatch(typeof(DFOGOCNBECG),nameof(DFOGOCNBECG.KKJINKDNBMA))]
        static void DFOGOCNBECG_KKJINKDNBMA_Postfix(DFOGOCNBECG __instance, float __0)
        {
            if (__instance.FNNBCDPJBIO >= 0f && __instance.FNNBCDPJBIO < 100f)
            {
                if (__instance.ENCGDNBNGLP(__instance.NJDGEELLAKG, __instance.FNNBCDPJBIO, __instance.BMFDFFLPBOJ) > 0 || __instance.NLDPMDNKGIC == 644)
                {
                    int num2 = 0;
                    if (__instance.NLDPMDNKGIC >= 1311 && __instance.NLDPMDNKGIC <= 1312 && __instance.FDIDJMFDFBO < 10f)
                    {
                        num2 = 1;
                    }
                    if (__instance.FNNBCDPJBIO <= __instance.EKOHAKPAOGN && num2 == 0)
                    {
                        if (__instance.KPEFINJBJLJ[0] == 0 && __instance.NLDPMDNKGIC != 1400 && LIPNHOMGGHF.FAKHAFKOBPB != 60)
                        {
                            float damage = multiplier.Value * -__instance.EGAGEEIJFOE;
                            __instance.HLGALFAGDGC -= damage;
                            __instance.KCFNOONDGKE(0, damage);

                        }
                    }
                }
            }
        }

    }
}