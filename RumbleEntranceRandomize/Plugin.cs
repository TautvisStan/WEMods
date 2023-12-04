using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RumbleEntranceRandomize
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.RumbleEntranceRandomize";
        public const string PluginName = "RumbleEntranceRandomize";
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
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.OOMNLEIFFHM))]  //
        [HarmonyPostfix]
        private static void FFCEGMEAIBP_OOMNLEIFFHM_Postfix()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == 1)
            {
                if(FFCEGMEAIBP.OLJFOJOLLOM < 0)
                {
                    var rng = new System.Random();
                    rng.ShuffleCharacters();
                    FFCEGMEAIBP.NJPKCMBLMLG = 0; //removing promo
                }
            }
        }
    }
    static class RandomExtensions
    {
        public static void ShuffleCharacters(this System.Random rng)
        {
            int n = FFCEGMEAIBP.EHIDHAPMAKG+1;
            while (n > 2)
            {
                int k = rng.Next(n--);
                while (k == 0) k = rng.Next(n);
                FFCEGMEAIBP.DKBKBEHNCEN(n, k);
            }
            for(int i = 1; i <= FFCEGMEAIBP.EHIDHAPMAKG; i++)
            {
                if(FFCEGMEAIBP.NMMABDGIJNC[i] == Characters.wrestler)
                {
                    Plugin.Log.LogInfo(Characters.c[FFCEGMEAIBP.NMMABDGIJNC[i]].name + " new position is " + i + " (not counting surprise entrants)");
                    if(i > 2)
                    {
                        Plugin.Log.LogInfo("Keep an eye for " + Characters.c[FFCEGMEAIBP.NMMABDGIJNC[i-1]].name + ", you might go out after them.");
                    }
                    break;
                }
                //Debug.LogWarning("CHAR " + i + " " + Characters.c[FFCEGMEAIBP.NMMABDGIJNC[i]].name);
            }
        }
    }

}