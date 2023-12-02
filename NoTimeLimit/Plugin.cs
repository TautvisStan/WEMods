using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;

namespace NoTimeLimits
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.NoTimeLimits";
        public const string PluginName = "NoTimeLimits";
        public const string PluginVer = "1.0.2";

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


        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.DMJFCHKLEFH))]  //Match start
        [HarmonyPostfix]
        static void FFCEGMEAIBP_DMJFCHKLEFHPostfix()
        {
            //check wrestler mode
            if (NAEEIFNFBBO.CBMHGKFFHJE == 1)
            {
                //check not allowed stipulations
                if (FFCEGMEAIBP.MINKNAEGMIH != 8 && FFCEGMEAIBP.BPJFLJPKKJK != 3 && FFCEGMEAIBP.BPJFLJPKKJK != 4 && FFCEGMEAIBP.OLJFOJOLLOM >= 0)
                {
                    FFCEGMEAIBP.NBAFIEALMHN = 0;
                }
            }
        }

    }

}