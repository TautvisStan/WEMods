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
        public const string PluginVer = "1.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static GameObject[] SpecialBarBase = new GameObject[5];
        public static GameObject[] SpecialBar = new GameObject[5];
        public static Color SpecialColor = new Color(1f, 1f, 1f);
        public static Color SpecialColorBreakdown = new Color(0.1f, 0.1f, 0.1f);
        public static float[] SpecialDisplay = new[] {1f, 1f, 1f, 1f, 1f};
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
                SpecialBarBase[__instance.PLFGKLGCOMD] = GameObject.Instantiate(__instance.JBCKJGIJFGK.transform.Find("Spirit Base").gameObject, __instance.JBCKJGIJFGK.transform);
                SpecialBar[__instance.PLFGKLGCOMD] = SpecialBarBase[__instance.PLFGKLGCOMD].transform.Find("Spirit Meter").gameObject;


                if (__instance.PLFGKLGCOMD == 1 || __instance.PLFGKLGCOMD == 3)
                {
                    baseMeter.transform.localPosition = new Vector3(128, 0, 0);
                    score.transform.localPosition = new Vector3(163, -25, 0);
                    baseMeter.transform.localScale = new Vector3(2f, 1f, 1f);


                    SpecialBarBase[__instance.PLFGKLGCOMD].transform.localPosition = new Vector3(256, -17, 0);
                    SpecialBarBase[__instance.PLFGKLGCOMD].transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else
                {
                    baseMeter.transform.localScale = new Vector3(-2f, 1f, 1f);
                    baseMeter.transform.localPosition = new Vector3(-128, 0, 0);
                    score.transform.localPosition = new Vector3(-163, -25, 0);


                    SpecialBarBase[__instance.PLFGKLGCOMD].transform.localPosition = new Vector3(-256, -17, 0);
                    SpecialBarBase[__instance.PLFGKLGCOMD].transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                if(__instance.PLFGKLGCOMD == 1)
                {
                    float y = FFCEGMEAIBP.OGIOFKKEFHK.transform.parent.Find("Meter01").transform.localPosition.y;
                    FFCEGMEAIBP.OGIOFKKEFHK.transform.localPosition = new Vector3(0, y, 0);
                }
            }
            [HarmonyPrefix]
            [HarmonyPatch(nameof(DLNHHGFNIIG.DIJBHIAAIOF))]
            public static void DIJBHIAAIOFPatch(DLNHHGFNIIG __instance, int IDHFOGNOIFC = 0)
            {
                GameObject score = __instance.DPIEHJIEEMM;
                DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[__instance.NNMDEFLLNBF];
                float num5 = 0;
                if (dfogocnbecg.LLGHFGNMCGF > 0)
                {
                    num5 = dfogocnbecg.LLGHFGNMCGF / 750f;
                    MCDCDEBALPI.BBLJCJMDDLO(SpecialBar[__instance.PLFGKLGCOMD], SpecialColor.r, SpecialColor.g, SpecialColor.b);
                }
                if (dfogocnbecg.LLGHFGNMCGF < 0)
                {
                    num5 = -dfogocnbecg.LLGHFGNMCGF / 900f;
                    MCDCDEBALPI.BBLJCJMDDLO(SpecialBar[__instance.PLFGKLGCOMD], SpecialColorBreakdown.r, SpecialColorBreakdown.g, SpecialColorBreakdown.b);
                }
                SpecialDisplay[__instance.PLFGKLGCOMD] = NAEEIFNFBBO.IFDOICDHAFC(SpecialDisplay[__instance.PLFGKLGCOMD], num5, 0.1f, 0.001f);
                if (__instance.NNMDEFLLNBF != __instance.PGKOMOIMNJN || IDHFOGNOIFC > 0)
                {
                    SpecialDisplay[__instance.PLFGKLGCOMD] = num5;
                }

                SpecialBar[__instance.PLFGKLGCOMD].transform.localScale = new Vector3(SpecialDisplay[__instance.PLFGKLGCOMD], 1f, 1f);
                SpecialBar[__instance.PLFGKLGCOMD].GetComponent<RectTransform>().anchoredPosition = new Vector2(2f - SpecialDisplay[__instance.PLFGKLGCOMD] * 2f, 0f);

                if (SpecialDisplay[__instance.PLFGKLGCOMD] == 0)
                {
                    SpecialBarBase[__instance.PLFGKLGCOMD].SetActive(false);
                    if (__instance.PLFGKLGCOMD == 1 || __instance.PLFGKLGCOMD == 3)
                    {
                        score.transform.localPosition = new Vector3(163, -25, 0);
                    }
                    else
                    {
                        score.transform.localPosition = new Vector3(-163, -25, 0);
                    }
                }
                else
                {
                    SpecialBarBase[__instance.PLFGKLGCOMD].SetActive(true);
                    if (__instance.PLFGKLGCOMD == 1 || __instance.PLFGKLGCOMD == 3)
                    {
                        score.transform.localPosition = new Vector3(163, -34, 0);
                    }
                    else
                    {
                        score.transform.localPosition = new Vector3(-163, -34, 0);
                    }
                }
            }

        }
    }
}