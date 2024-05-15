using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BetterPlayersOnlyCamera
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.BetterPlayersOnlyCamera";
        public const string PluginName = "BetterPlayersOnlyCamera";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;


        public static int oldScope = -1;


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
        private void Update()
        {
            if (BLNKDHIGFAN.GMJKGKDFHOH == 0 && SceneManager.GetActiveScene().name == "Game" && FFCEGMEAIBP.LOBDMDPMFLK != 1)
            {
                int playernum = GetMainPlayerNum();
                if (BLNKDHIGFAN.JCKCCDKDEKP != playernum)
                {
                    BLNKDHIGFAN.JCKCCDKDEKP = playernum;
                }
            }
            if(oldScope == 0 && BLNKDHIGFAN.GMJKGKDFHOH != 0)
            {
                BLNKDHIGFAN.JCKCCDKDEKP = 0;
            }
            oldScope = BLNKDHIGFAN.GMJKGKDFHOH;
        }
        public int GetMainPlayerNum()
        {
            for (int j = 0; j <= HKJOAJOKOIJ.NGCNKGDDKGF; j++)
            {
                if (HKJOAJOKOIJ.NAADDLFFIHG[j].AHBNKMMMGFI > 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].BPJFLJPKKJK >= 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP > 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP <= NJBJIIIACEP.NBBBLJDBLNM)
                {
                    return HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP;
                }
            }
            return 0;
        }
        
    }
}