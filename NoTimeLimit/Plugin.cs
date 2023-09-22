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
        public const string PluginName = "CareerMatchRatings";
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


        [HarmonyPatch(typeof(PHECEOMIMND), nameof(PHECEOMIMND.NHPJFOBBCEF))]  //Match start
        [HarmonyPostfix]
        static void PHECEOMIMND_NHPJFOBBCEFPostfix()
        {
            //check wrestler mode
            if (LFNJDEGJLLJ.NHDABIOCLFH == 1)
            {
                //check not allowed stipulations
                if (PHECEOMIMND.NEECMNDFAFF != 8 && PHECEOMIMND.FNIDHNNCLBB != 3 && PHECEOMIMND.FNIDHNNCLBB != 4)
                {
                    PHECEOMIMND.CCPMCEMAKBJ = 0;
                }
            }
        }

    }

}