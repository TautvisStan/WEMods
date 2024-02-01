using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using UnityEngine;

namespace MoreRingSizes
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.MoreRingSizes";
        public const string PluginName = "MoreRingSizes";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;


        public static float? ringSize = null;


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
        [HarmonyPatch(typeof(World), nameof(World.MNCIAPLCFDM))]
        [HarmonyPrefix]
        private static bool World_MNCIAPLCFDMPrefix(ref float __result)
        {
            if (World.location <= 1)
            {
                __result = 1.65f;
                return false;
            }
            return true;
        }
        [HarmonyPatch(typeof(AKFIIKOMPLL), nameof(AKFIIKOMPLL.ODONMLDCHHF))]
        [HarmonyPrefix]
        private static bool AKFIIKOMPLL_ODONMLDCHHFPrefix(AKFIIKOMPLL __instance, ref float __result, ref float CAAJBNHEFJJ, float OEGLNPMNEOE, float NOGFHHECJBM, ref float JBPAELFIDOP, float LKBOHHGFJFO, int LKJHMOHMKCM)
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE != 1)
            {
                if (LIPNHOMGGHF.CHLJMEPFJOK == 1 && LIPNHOMGGHF.ODOAPLMOJPD == 1)
                {
                    if (__instance == LIPNHOMGGHF.FKANHDIMMBJ[8])
                    {
                        JBPAELFIDOP = 0.2f;
                    }
                }
            }
            return true;
        }
        [HarmonyPatch(typeof(AKFIIKOMPLL), nameof(AKFIIKOMPLL.ODONMLDCHHF))]
        [HarmonyPostfix]
        private static void AKFIIKOMPLL_ODONMLDCHHFPostfix(AKFIIKOMPLL __instance, ref float __result, float CAAJBNHEFJJ, float OEGLNPMNEOE, float NOGFHHECJBM, ref float JBPAELFIDOP, float LKBOHHGFJFO, int LKJHMOHMKCM)
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE != 1)
            {
                if (LIPNHOMGGHF.CHLJMEPFJOK == 1 && LIPNHOMGGHF.ODOAPLMOJPD == 1)
                {
                    if (__instance == LIPNHOMGGHF.FKANHDIMMBJ[8])
                    {
                        ringSize = __result;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Update))]
        [HarmonyPostfix]
        private static void Scene_Match_Setup_Update(Scene_Match_Setup __instance)
        {
            if (World.location == __instance.oldArena)
            {
                if (World.MNCIAPLCFDM() > 0f)
                {
                    if (ringSize != null)
                    {
                        if (World.ringShape > 0 && World.gRing != null)
                        {
                            World.ringSize = (float)ringSize;
                            World.KJKAGJNFCEG();
                            GIMNNPMAKNJ.NALPMNNGKAE();
                        }
                    }
                }
            }
        }
        [HarmonyPatch(typeof(World), nameof(World.DBKOAJKLBIF))]
        [HarmonyPostfix]
        private static void World_DBKOAJKLBIF(int EJDHFNIJFHI)
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE != 1)
            {
                World.gRing.transform.localScale = new Vector3(16.67f * World.ringSize, 16.67f, 16.67f * World.ringSize);
            }
        }
        [HarmonyPatch(typeof(World), nameof(World.BNIAGHOEDHA))]
        [HarmonyPrefix]
        private static void World_BNIAGHOEDHA()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE != 1)
            {
                if (ringSize != null && World.ringSize != ringSize)
                {
                    World.ringSize = (float)ringSize;
                }
            }
        }
    }
}