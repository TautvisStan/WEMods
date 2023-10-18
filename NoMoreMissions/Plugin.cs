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
        public const string PluginVer = "1.0.2";

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
        [HarmonyPatch(typeof(NEGAFEHECNL))]
        public static class NEGAFEHECNL_Patch
        {
            [HarmonyPostfix]
            [HarmonyPatch(nameof(NEGAFEHECNL.OILBBIFDBDP))]
            public static void OILBBIFDBDP_Patch()
            {
                Character character2 = Characters.c[NEGAFEHECNL.FEAIGHFCIBK];
                if (character2.promo >= 501 && character2.promo < 600)
                {
                    character2.promo = 0;
                }
                if (NEGAFEHECNL.GOEACIHJCCJ >= 501 && NEGAFEHECNL.GOEACIHJCCJ < 600)
                {
                    NEGAFEHECNL.GOEACIHJCCJ = 0;
                }

            }
            [HarmonyPostfix]
            [HarmonyPatch(nameof(NEGAFEHECNL.CICDFJKBOIM))]
            public static void CICDFJKBOIM_Patch(ref int __result)
            {
                if (NEGAFEHECNL.GOEACIHJCCJ >= 501 && NEGAFEHECNL.GOEACIHJCCJ < 600)
                {
                    NEGAFEHECNL.GOEACIHJCCJ = 0;
                    __result = 0;
                }

            }
        }
    }

}