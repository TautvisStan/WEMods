using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using UnityEngine;

namespace ViolentEntrances
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.ViolentEntrances";
        public const string PluginName = "ViolentEntrances";
        public const string PluginVer = "2.0.0";

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

        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.JCAKMBCFLCK))]
        [HarmonyTranspiler]  //Enable attacks during entrance
        public static IEnumerable<CodeInstruction> DFOGOCNBECG_JCAKMBCFLCK_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {

                if (i + 2 < codes.Count &&
                    codes[i].opcode == OpCodes.Ldsfld && codes[i].operand.ToString() == "System.Int32 LOBDMDPMFLK" &&
                    codes[i + 1].opcode == OpCodes.Ldc_I4_1 &&
                    codes[i + 2].opcode == OpCodes.Bne_Un)
                {
                    CodeInstruction instruction = new CodeInstruction(OpCodes.Ldc_I4_0);
                    instruction.labels.AddRange(codes[i].labels);
                    codes[i] = instruction;  
                }
                yield return codes[i];

            }
        }
        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.POCGHMGBHFD))]
        [HarmonyTranspiler]  //Make AI fight back during entrance (except for current entrant)
        public static IEnumerable<CodeInstruction> DFOGOCNBECG_POCGHMGBHFD_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {

                if (i + 2 < codes.Count &&
                    codes[i].opcode == OpCodes.Ldsfld && codes[i].operand.ToString() == "System.Int32 LOBDMDPMFLK" &&
                    codes[i + 1].opcode == OpCodes.Ldc_I4_1 &&
                    codes[i + 2].opcode == OpCodes.Bne_Un)
                {
                    CodeInstruction instruction = new CodeInstruction(OpCodes.Ldc_I4_0);
                    instruction.labels.AddRange(codes[i].labels);
                    codes[i] = instruction; 
                }
                yield return codes[i];

            }
        }

        //Changing entrant AI state
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.LCAJNIOJAPG))]
        static void Prefix_LCAJNIOJAPG(DFOGOCNBECG __instance)
        {
            if (FFCEGMEAIBP.LOBDMDPMFLK == 1 && __instance.AHBNKMMMGFI == 2) __instance.AHBNKMMMGFI = 3;
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