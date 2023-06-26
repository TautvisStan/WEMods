using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;

namespace NoMoreMissions
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.NoMoreMissions";
        public const string PluginName = "NoMoreMissions";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);
        public static bool loaded = false;

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
    }
    [HarmonyPatch(typeof(Scene_Titles))]
    public static class Scene_Titles_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch("Start")]
        public static void Start_Patch()
        {
            if(!Plugin.loaded)
            {
                Plugin.loaded = true;
                for (int i = 1; i <= Characters.no_chars; i++)
                {
                    if(Characters.c[i].promo >= 501 && Characters.c[i].promo < 600)
                        Characters.c[i].promo = 0;
                }
            }
        }
    }
    [HarmonyPatch(typeof(DNMADBBLNDC))]
    public static class DNMADBBLNDC_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch("EFGMHNHMMME")]
        public static void EFGMHNHMMME_Patch()
        {
            Character character2 = Characters.c[DNMADBBLNDC.EFNLJOAIMHB];
            if(character2.promo >= 501 && character2.promo < 600)
            {
                character2.promo = 0;
            }
            if (DNMADBBLNDC.NKNDDJEALNN >= 501 && DNMADBBLNDC.NKNDDJEALNN < 600)
            {
                DNMADBBLNDC.NKNDDJEALNN = 0;
            }

        }
        [HarmonyPostfix]
        [HarmonyPatch("OGCDDEOKIFB")]
        public static void OGCDDEOKIFB_Patch(ref int __result)
        {
            if (DNMADBBLNDC.NKNDDJEALNN >= 501 && DNMADBBLNDC.NKNDDJEALNN < 600)
            {
                DNMADBBLNDC.NKNDDJEALNN = 0;
                __result = 0;
            }

        }
    }
}