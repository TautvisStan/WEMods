using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace UnlimitedParticles
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.UnlimitedParticles";
        public const string PluginName = "UnlimitedParticles";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);
        public static ConfigEntry<int> configSize;
        internal static string PluginPath;
        public static Dictionary<int, float> _overrideTimers = new Dictionary<int, float>();
        public static float PuddleSpeed = 0;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            configSize = Config.Bind("General",
             "New Limit",
             10000,
             "New particles limit. Too many particles on screen might cause performance issues");
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

        [HarmonyPatch(typeof(ALIGLHEIAGO), nameof(ALIGLHEIAGO.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ALIGLHEIAGO_ICGNAJFLAHL()
        {
            var testarray = new MEHJAJJNHLL[configSize.Value + 1];
            for (int m = 0; m <= configSize.Value; m++)
            {
                testarray[m] = new MEHJAJJNHLL();
                testarray[m].PLFGKLGCOMD = m;
            }
            Array.Copy(ALIGLHEIAGO.HGLADILOIJA, testarray, ALIGLHEIAGO.AABNMPIJODF + 1);
            ALIGLHEIAGO.HGLADILOIJA = testarray;
            ALIGLHEIAGO.AABNMPIJODF = configSize.Value;
        }

    /*    [HarmonyPatch(typeof(MEHJAJJNHLL), nameof(MEHJAJJNHLL.BLJNNEDCMEI))]
        [HarmonyPostfix]
        public static void NAEEIFNFBBO_NFKGHIGAMEI(MEHJAJJNHLL __instance, int CHMHJJNEMKB, Color OLMMBNEKBAJ, float EDCMPAAEHFO, GameObject JKCAPDICGGA, float DPBNKMPJJOJ, float NKEMECHAEEJ, float KLNFFHLCNKF, float EDEIGDNEICF = 0f, float LNJHLMLIHAI = 0f, float CMIKKPKKOBP = 0f, int CCELDFBKACJ = 1)
        {
            __instance.IMJHCHECCED = 1000;
        }*/

        [HarmonyPatch(typeof(MEHJAJJNHLL), nameof(MEHJAJJNHLL.DIJBHIAAIOF))]
        [HarmonyPostfix]
        public static void ALIGLHEIAGOO_DIJBHIAAIOF(MEHJAJJNHLL __instance)
        {
            if (__instance != null)
            {
                bool flag = __instance.BPJFLJPKKJK >= 0;
                if (!flag)
                {
                    if (__instance.DDLDBDJEPJA <= 0) return;
                    if (__instance.CAILAODIPDO <= 0) return;
                    if (__instance.GKAKINIMLGA <= 0) return;
                    __instance.JPMOFJPKINC = 1;
                    float mcjhgehepmd = MBLIOKEDHHB.MCJHGEHEPMD;
                    int plfgklgcomd = __instance.PLFGKLGCOMD;
                    bool flag2 = !_overrideTimers.ContainsKey(plfgklgcomd);
                    if (flag2)
                    {
                        _overrideTimers.Add(plfgklgcomd, 0f);
                    }
                    bool flag3 = __instance.IMJHCHECCED + 2f < _overrideTimers[plfgklgcomd];
                    if (flag3)
                    {
                        _overrideTimers[plfgklgcomd] = __instance.IMJHCHECCED;
                    }
                    Dictionary<int, float> overrideTimers = _overrideTimers;
                    int num = plfgklgcomd;
                    overrideTimers[num] += mcjhgehepmd * PuddleSpeed;
                    bool flag4 = overrideTimers[plfgklgcomd] == 0f;
                    if (flag4)
                    {
                        overrideTimers[plfgklgcomd] = 1f;
                    }
                    __instance.IMJHCHECCED = (float)((int)overrideTimers[plfgklgcomd]);
                }
            }
        }
    }
}