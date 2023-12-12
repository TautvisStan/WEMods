using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JoinSpectators
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.JoinSpectators";
        public const string PluginName = "JoinSpectators";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);
        public static ConfigEntry<KeyCode> CPUButton;
        internal static string PluginPath;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            CPUButton = Config.Bind("Controls",
             "CPU Button",
             KeyCode.CapsLock,
             "Press this button to let CPU take control of your character.");
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


        private void Update()
        {
            if(Input.GetKeyDown(CPUButton.Value))
            {
                //   for (int j = 0; j <= HKJOAJOKOIJ.NGCNKGDDKGF; j++)
                for (int j = HKJOAJOKOIJ.NGCNKGDDKGF; j >= 0; j--)
                {
                    if (HKJOAJOKOIJ.NAADDLFFIHG[j].AHBNKMMMGFI > 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].BPJFLJPKKJK >= 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP > 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP <= NJBJIIIACEP.NBBBLJDBLNM)
                    {
                        int num = HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP;
                        NJBJIIIACEP.OAAMGFLINOB[num].NLOOBNDGIKO.FOAPDJMIFGP = 0;
                        NJBJIIIACEP.OAAMGFLINOB[num].OJAJENJLBMF = -1;
                        NJBJIIIACEP.OAAMGFLINOB[num].NLOOBNDGIKO = HKJOAJOKOIJ.IPDFOJEMPMM;
                        break;
                    }
                }
            }
        }

    }

}