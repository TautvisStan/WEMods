using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace ViolentEntrances
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.ViolentEntrances";
        public const string PluginName = "ViolentEntrances";
        public const string PluginVer = "1.0.3";

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

        static int matchstate1 = 0;
        static int matchstate2 = 0;
        private static bool entrance = false;
        private static float oldvalue;
        [HarmonyPatch(typeof(DFOGOCNBECG))]
        public static class DFOGOCNBECGPatch
        {
            //Enable attacks during entrance
            [HarmonyPrefix]
            [HarmonyPatch(nameof(DFOGOCNBECG.JCAKMBCFLCK))]   
            static void Prefix_JCAKMBCFLCK(DFOGOCNBECG __instance)
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK == 1)
                {
                    Plugin.entrance = true;
                }
                if (FFCEGMEAIBP.LOBDMDPMFLK == 1 || FFCEGMEAIBP.LOBDMDPMFLK == 2)
                {
                    if (Plugin.entrance)
                    {
                        Plugin.oldvalue = __instance.OOFFPCOALKB;
                        __instance.OOFFPCOALKB = 1f;
                    }
                    Plugin.matchstate1 = FFCEGMEAIBP.LOBDMDPMFLK;
                    FFCEGMEAIBP.LOBDMDPMFLK = 2;
                }
            }
            [HarmonyPostfix]
            [HarmonyPatch(nameof(DFOGOCNBECG.JCAKMBCFLCK))]
            static void Postfix_JCAKMBCFLCK(DFOGOCNBECG __instance)
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK == 1 || FFCEGMEAIBP.LOBDMDPMFLK == 2)
                {
                    FFCEGMEAIBP.LOBDMDPMFLK = matchstate1;
                }
                if (Plugin.entrance)
                {
                    __instance.OOFFPCOALKB = Plugin.oldvalue;
                }
                Plugin.entrance = false;
            }
            //Make AI fight back during entrance (except for current entrant)
            [HarmonyPrefix]
            [HarmonyPatch(nameof(DFOGOCNBECG.POCGHMGBHFD))]
            static void Prefix_POCGHMGBHFD()
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK == 1 || FFCEGMEAIBP.LOBDMDPMFLK == 2)
                {
                    Plugin.matchstate2 = FFCEGMEAIBP.LOBDMDPMFLK;
                    FFCEGMEAIBP.LOBDMDPMFLK = 2;
                }
            }
            [HarmonyPostfix]
            [HarmonyPatch(nameof(DFOGOCNBECG.POCGHMGBHFD))]
            static void Postfix_POCGHMGBHFD(DFOGOCNBECG __instance,ref int __result, int GKNIAFAOLJK)
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK == 1 || FFCEGMEAIBP.LOBDMDPMFLK == 2)
                {
                    FFCEGMEAIBP.LOBDMDPMFLK = matchstate2;
                }
            }

            //Changing entrant AI state
            [HarmonyPrefix]
            [HarmonyPatch(nameof(DFOGOCNBECG.LCAJNIOJAPG))]
            static void Prefix_LCAJNIOJAPG(DFOGOCNBECG __instance)
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK == 1 && __instance.AHBNKMMMGFI == 2) __instance.AHBNKMMMGFI = 3;
            }

        }
        //Entrance run in when pressing Taunt
        [HarmonyPatch(typeof(BJMGCKGNCHO), nameof(BJMGCKGNCHO.NCOEPCFFBJA))]
        [HarmonyPostfix]
        static void Postfix_NCOEPCFFBJA(BJMGCKGNCHO __instance)
        {
            if (__instance.IOIJFFLMBCH != null && NJBJIIIACEP.OAAMGFLINOB != null)
            {
                if (__instance.IOIJFFLMBCH[5] == 1)
                {
                    if (NJBJIIIACEP.OAAMGFLINOB[__instance.FOAPDJMIFGP].AHBNKMMMGFI == 0)
                    {
                        FFCEGMEAIBP.LPBCEGPJNMF = __instance.FOAPDJMIFGP;
                    }
                }
            }
        }
    }
}