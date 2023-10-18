using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using UnityEngine;

namespace IncreasedSliderLimits
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.IncreasedSliderLimits";
        public const string PluginName = "IncreasedSliderLimits";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<float> minStat;
        public static ConfigEntry<float> maxStat;
        public static ConfigEntry<float> minHeight;
        public static ConfigEntry<float> maxHeight;
        public static ConfigEntry<bool> KeepPopAttLocked;
        public static ConfigEntry<float> minMusic;
        public static ConfigEntry<float> maxMusic;

        private void Awake()
        {
           
            Plugin.Log = base.Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);
            minStat = Config.Bind("General",
             "Min Stat",
             50f,
             "The min stat limit");
            maxStat = Config.Bind("General",
             "Max Stat",
             99f,
             "The max stat limit");
            KeepPopAttLocked = Config.Bind("General",
             "Keep Popularity and attitude limits",
             true,
             "Keep default popularity and attitude limits (may prevent some issues)");
            minHeight = Config.Bind("General",
             "Min Height",
             0.8f,
             "The min height limit (by the game units)");
            maxHeight = Config.Bind("General",
             "Max Height",
             1.2f,
             "The max height limit (by the game units)");
            minMusic = Config.Bind("General",
             "Min Music speed",
             0.7f,
             "The min theme song speed limit");
            maxMusic = Config.Bind("General",
             "Max Music speed",
             1.2f,
             "The max theme song speed limit");

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

        [HarmonyPatch(typeof(AKFIIKOMPLL), nameof(AKFIIKOMPLL.ODONMLDCHHF))]
        [HarmonyPrefix]
        static bool Prefix(AKFIIKOMPLL __instance, float __0, float __1, float __2, ref float __3, ref float __4, int __5)
        {
            if (__1 == 1f && __2 == 5f && __3 == 50f && __4 == 99f && __5 == 0 && (__instance == LIPNHOMGGHF.FKANHDIMMBJ[1] || __instance == LIPNHOMGGHF.FKANHDIMMBJ[2] || __instance == LIPNHOMGGHF.FKANHDIMMBJ[3] || __instance == LIPNHOMGGHF.FKANHDIMMBJ[4] || __instance == LIPNHOMGGHF.FKANHDIMMBJ[5] || __instance == LIPNHOMGGHF.FKANHDIMMBJ[6]) && LIPNHOMGGHF.CHLJMEPFJOK == 1)
            {
                __3 = Plugin.minStat.Value;
                __4 = Plugin.maxStat.Value;
                if ((__instance == LIPNHOMGGHF.FKANHDIMMBJ[1] || __instance == LIPNHOMGGHF.FKANHDIMMBJ[6]) && KeepPopAttLocked.Value)
                {
                    __3 = 50f;
                    __4 = 99f;
                }
            }
            if (__1 == 0.01f && __2 == 10f && __3 == 0.8f && __4 == 1.2f && __5 == 0 && (__instance == LIPNHOMGGHF.FKANHDIMMBJ[3]) && LIPNHOMGGHF.CHLJMEPFJOK == 2)
            {
                __3 = Plugin.minHeight.Value;
                __4 = Plugin.maxHeight.Value;
            }
            if (__1 == 0.01f && __2 == 10f && __3 == 0.7f && __4 == 1.2f && __5 == 0 && (__instance == LIPNHOMGGHF.FKANHDIMMBJ[9]) && LIPNHOMGGHF.CHLJMEPFJOK == 1)
            {
                __3 = Plugin.minMusic.Value;
                __4 = Plugin.maxMusic.Value;
            }
            return true;
        }
        [HarmonyPatch(typeof(Character), nameof(Character.IMMIIDECGCF))] //!!!!
        [HarmonyPrefix]
        static bool CharacterIMMIIDECGCF(Character __instance, int LGLHGGDPNPD, ref float OEGLNPMNEOE)
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE > 0)
            {
                OEGLNPMNEOE *= MBLIOKEDHHB.MCJHGEHEPMD;
                int num = Mathf.RoundToInt(__instance.stat[LGLHGGDPNPD]);
                if (LGLHGGDPNPD == 6 && OEGLNPMNEOE > 0f)
                {
                    OEGLNPMNEOE *= (150f - __instance.stat[LGLHGGDPNPD]) / 100f;
                }
                __instance.stat[LGLHGGDPNPD] += OEGLNPMNEOE;
                if ((LGLHGGDPNPD == 1 || LGLHGGDPNPD == 6) && KeepPopAttLocked.Value)
                {
                    if (__instance.stat[LGLHGGDPNPD] < 50f)
                    {
                        __instance.stat[LGLHGGDPNPD] = 50f;
                    }
                    if (__instance.stat[LGLHGGDPNPD] > 99f)
                    {
                        __instance.stat[LGLHGGDPNPD] = 99f;
                    }
                }
                else
                {
                    if (__instance.stat[LGLHGGDPNPD] < Plugin.minStat.Value)
                    {
                        __instance.stat[LGLHGGDPNPD] = Plugin.minStat.Value;
                    }
                    if (__instance.stat[LGLHGGDPNPD] > Plugin.maxStat.Value)
                    {
                        __instance.stat[LGLHGGDPNPD] = Plugin.maxStat.Value;
                    }
                }
                if (LIPNHOMGGHF.FAKHAFKOBPB == 50 && FFCEGMEAIBP.LOBDMDPMFLK == 0 && __instance.id == Characters.star && NJBJIIIACEP.DCAFAIGGFCC != null && NJBJIIIACEP.DCAFAIGGFCC[1].GHGLMKCECOI == 0f)
                {
                    if (Mathf.RoundToInt(__instance.stat[LGLHGGDPNPD]) < num)
                    {
                        CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.AEJEOKACNBJ, 1f, 1f);
                        NJBJIIIACEP.DCAFAIGGFCC[1].BFBKLJHKKGE(Characters.statName[LGLHGGDPNPD] + ": " + Mathf.RoundToInt(__instance.stat[LGLHGGDPNPD]).ToString("0") + "%", new Color(0.9f, 0.1f, 0.1f));
                    }
                    if (Mathf.RoundToInt(__instance.stat[LGLHGGDPNPD]) > num)
                    {
                        CHLPMKEGJBJ.DNNPEAOCDOG(CHLPMKEGJBJ.MBEMMCMOJFF, 1f, 1f);
                        NJBJIIIACEP.DCAFAIGGFCC[1].BFBKLJHKKGE(Characters.statName[LGLHGGDPNPD] + ": " + Mathf.RoundToInt(__instance.stat[LGLHGGDPNPD]).ToString("0") + "%", new Color(0.1f, 0.9f, 0.1f));
                    }
                }
            }
            return false;
        }
        [HarmonyPatch(typeof(Character), nameof(Character.LLJKACBKKJM))]  //!!!!
        [HarmonyPrefix]
        static bool CharacterLLJKACBKKJM(Character __instance)
        {
            {
                if (__instance.health < 0f)
                {
                    __instance.health = 0f;
                }
                if (__instance.health > 100f)
                {
                    __instance.health = 100f;
                }
                if (__instance.oldStat[0] < 0f)
                {
                    __instance.oldStat[0] = 0f;
                }
                if (__instance.oldStat[0] > 100f)
                {
                    __instance.oldStat[0] = 100f;
                }
                if (__instance.newStat[0] < 0f)
                {
                    __instance.newStat[0] = 0f;
                }
                if (__instance.newStat[0] > 100f)
                {
                    __instance.newStat[0] = 100f;
                }
                for (int i = 1; i <= 6; i++)
                {
                    if ((i == 1 || i == 6) && Plugin.KeepPopAttLocked.Value)
                    {
                        if (__instance.stat[i] < 50f)
                        {
                            __instance.stat[i] = 50f;
                        }
                        if (__instance.stat[i] > 99f)
                        {
                            __instance.stat[i] = 99f;
                        }
                        if (__instance.oldStat[i] < 50f)
                        {
                            __instance.oldStat[i] = 50f;
                        }
                        if (__instance.oldStat[i] > 99f)
                        {
                            __instance.oldStat[i] = 99f;
                        }
                        if (__instance.newStat[i] < 50f)
                        {
                            __instance.newStat[i] = 50f;
                        }
                        if (__instance.newStat[i] > 99f)
                        {
                            __instance.newStat[i] = 99f;
                        }
                    }
                    else
                    {
                        if (__instance.stat[i] < Plugin.minStat.Value)
                        {
                            __instance.stat[i] = Plugin.minStat.Value;
                        }
                        if (__instance.stat[i] > Plugin.maxStat.Value)
                        {
                            __instance.stat[i] = Plugin.maxStat.Value;
                        }
                        if (__instance.oldStat[i] < Plugin.minStat.Value)
                        {
                            __instance.oldStat[i] = Plugin.minStat.Value;
                        }
                        if (__instance.oldStat[i] > Plugin.maxStat.Value)
                        {
                            __instance.oldStat[i] = Plugin.maxStat.Value;
                        }
                        if (__instance.newStat[i] < Plugin.minStat.Value)
                        {
                            __instance.newStat[i] = Plugin.minStat.Value;
                        }
                        if (__instance.newStat[i] > Plugin.maxStat.Value)
                        {
                            __instance.newStat[i] = Plugin.maxStat.Value;
                        }
                    }
                }
                if (__instance.id == Characters.fraud)
                {
                    __instance.stat[1] = 110f;
                    __instance.oldStat[1] = __instance.stat[1];
                    __instance.newStat[1] = __instance.stat[1];
                }
            }
            return false;
        }
    }
}