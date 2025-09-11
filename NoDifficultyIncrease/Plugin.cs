using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;

namespace NoDifficultyIncrease
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.NoDifficultyIncrease";
        public const string PluginName = "NoDifficultyIncrease";
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

        [HarmonyPatch(typeof(Progress), nameof(Progress.EEANPLJLLMA))]
        [HarmonyTranspiler]  //Removes the pre-match promo
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            int promo = -1125;
            foreach (var instruction in codes)
            {
                if (instruction.opcode == OpCodes.Ldc_I4 && (int)instruction.operand == promo)
                {
                    instruction.operand = 0;
                    break;
                }
            }
            return codes;
        }

        [HarmonyPatch(typeof(NEGAFEHECNL), nameof(NEGAFEHECNL.BLJNNEDCMEI))]
        [HarmonyPrefix]  //Removes the post-match promo
        public static bool NEGAFEHECNL_BLJNNEDCMEI_Prefix(int JDFNNPLMLKC, int KJELLNJFNGO = 0, int GKNIAFAOLJK = 0, int GNILJHAGLME = 0)
        {
            if (JDFNNPLMLKC == 1262)
            {
                NEGAFEHECNL.IMJHCHECCED += 30f;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Start))]
        [HarmonyPostfix]  //Removes the post-match promo
        public static void Scene_Titles_Start_Postfix()
        {
            if (Progress.promo != null && Progress.promo[Progress.date] == -1125)
            {
                Progress.promo[Progress.date] = 0;
            }
        }
    }
}