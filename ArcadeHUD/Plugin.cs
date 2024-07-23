using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace ArcadeHUD
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.ArcadeHUD";
        public const string PluginName = "ArcadeHUD";
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
        [HarmonyPatch(typeof(DLNHHGFNIIG))]
        public static class DLNHHGFNIIGPatch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(DLNHHGFNIIG.ICGNAJFLAHL))]
            public static void ICGNAJFLAHLPatch(DLNHHGFNIIG __instance, int APCLJHNGGFM, float DPBNKMPJJOJ, float NKEMECHAEEJ, float JCPCBMOMLBD)
            {
                GameObject baseMeter = __instance.JBCKJGIJFGK.transform.Find("Health Base").gameObject;
                GameObject score = __instance.DPIEHJIEEMM;
                
                if (__instance.PLFGKLGCOMD == 1 || __instance.PLFGKLGCOMD == 3)
                {
                    baseMeter.transform.localPosition = new Vector3(125, 0, 0);
                    score.transform.localPosition = new Vector3(163, -25, 0);
                    baseMeter.transform.localScale = new Vector3(2f, 1f, 1f);
                }
                else
                {
                    baseMeter.transform.localScale = new Vector3(-2f, 1f, 1f);
                    baseMeter.transform.localPosition = new Vector3(-125, 0, 0);
                    score.transform.localPosition = new Vector3(-163, -25, 0);
                }
                if(__instance.PLFGKLGCOMD == 1)
                {
                    float y = FFCEGMEAIBP.OGIOFKKEFHK.transform.parent.Find("Meter01").transform.localPosition.y;
                    FFCEGMEAIBP.OGIOFKKEFHK.transform.localPosition = new Vector3(0, y, 0);
                }
            }
        }
    }
}