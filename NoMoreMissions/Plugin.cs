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
        public const string PluginVer = "1.0.1";

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

        [HarmonyPatch(typeof(Scene_Titles))]
        public static class Scene_Titles_Patch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(Scene_Titles.Start))]
            public static void Start_Patch()
            {
                if (!Plugin.loaded)
                {
                    Plugin.loaded = true;
                    for (int i = 1; i <= Characters.no_chars; i++)
                    {
                        if (Characters.c[i].promo >= 501 && Characters.c[i].promo < 600)
                            Characters.c[i].promo = 0;
                    }
                }
            }
        }
        [HarmonyPatch(typeof(FMHJNNGPMKG))]
        public static class FMHJNNGPMKG_Patch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(FMHJNNGPMKG.KLEAPOMKONL))]
            public static void KLEAPOMKONL_Patch()
            {
                Character character2 = Characters.c[FMHJNNGPMKG.GHCBKFNBGGN];
                if (character2.promo >= 501 && character2.promo < 600)
                {
                    character2.promo = 0;
                }
                if (FMHJNNGPMKG.BGHNMCBLGMF >= 501 && FMHJNNGPMKG.BGHNMCBLGMF < 600)
                {
                    FMHJNNGPMKG.BGHNMCBLGMF = 0;
                }

            }
            [HarmonyPostfix]
            [HarmonyPatch(nameof(FMHJNNGPMKG.AEFLNJDIKGE))]
            public static void AEFLNJDIKGE_Patch(ref int __result)
            {
                if (FMHJNNGPMKG.BGHNMCBLGMF >= 501 && FMHJNNGPMKG.BGHNMCBLGMF < 600)
                {
                    FMHJNNGPMKG.BGHNMCBLGMF = 0;
                    __result = 0;
                }

            }
        }
    }

}