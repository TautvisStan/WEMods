using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using BepInEx.Configuration;

namespace CasualEntranceCostumes
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CasualEntranceCostumes";
        public const string PluginName = "CasualEntranceCostumes";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<KeyCode> ChangeClothesButton;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            ChangeClothesButton = Config.Bind("General", "Costume change keybind", KeyCode.None, "Changes the current entrants' costume from casual to wrestling");
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
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Start))]
        [HarmonyPostfix]
        public static void Scene_Game_Start()
        {
            if (FFCEGMEAIBP.LOBDMDPMFLK == 1 && FFCEGMEAIBP.CBIPLGLDCAG != 1)
            {
                for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                {
                    DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[i];
                    if (dfogocnbecg.FIEMGOLBHIO == 1 && dfogocnbecg.PCNHIIPBNEK[0] != null)
                    {
                        dfogocnbecg.OEGJEBDBGJA = dfogocnbecg.EMDMDLNJFKP.costume[2];
                        dfogocnbecg.ABHDOPBDDPB();
                    }
                }
            }
        }
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Update))]
        [HarmonyPostfix]
        public static void Scene_Game_Update() 
        {
            if (Input.GetKeyDown(ChangeClothesButton.Value))
            {
                if (FFCEGMEAIBP.LOBDMDPMFLK == 1)
                {
                    for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                    {
                        DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[i];
                        if (dfogocnbecg.FIEMGOLBHIO == 1 && FFCEGMEAIBP.LPBCEGPJNMF == dfogocnbecg.PLFGKLGCOMD)
                        {
                            dfogocnbecg.OEGJEBDBGJA = dfogocnbecg.EMDMDLNJFKP.costume[1];
                            dfogocnbecg.ABHDOPBDDPB();
                        }
                    }
                }
            }
        }

        /*    //change costumes to casual on character spawn
            //todo only wrestlers on entrance
            [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.ICGNAJFLAHL))]
            [HarmonyPostfix]
            public static void DFOGOCNBECG_ICGNAJFLAHL_Postfix(DFOGOCNBECG __instance, int APCLJHNGGFM, int IMDNPIKCPMD, int LGKGPDCGGCG = 0)
            {
                Debug.LogWarning(FFCEGMEAIBP.LOBDMDPMFLK);
                __instance.OEGJEBDBGJA = __instance.EMDMDLNJFKP.costume[2];
                __instance.ABHDOPBDDPB();
            }*/
        //change it back to wrestling on match start
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.DMJFCHKLEFH))]
        [HarmonyPostfix]
        public static void FFCEGMEAIBP_DMJFCHKLEFH_Postfix()
        {
            if (FFCEGMEAIBP.CBIPLGLDCAG != 1 && FFCEGMEAIBP.LOBDMDPMFLK == 2)
            {
                for (int i = 1; i <= NJBJIIIACEP.NBBBLJDBLNM; i++)
                {
                    DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[i];
                    if (dfogocnbecg.FIEMGOLBHIO == 1 && dfogocnbecg.PCNHIIPBNEK[0] != null)
                    {
                        dfogocnbecg.OEGJEBDBGJA = dfogocnbecg.EMDMDLNJFKP.costume[1];
                        dfogocnbecg.ABHDOPBDDPB();
                    }
                }
            }
        }
    }
}