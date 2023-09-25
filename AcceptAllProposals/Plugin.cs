using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AcceptAllProposals
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.AcceptAllProposals";
        public const string PluginName = "AcceptAllProposals";
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

        [HarmonyPatch(typeof(FMHJNNGPMKG), nameof(FMHJNNGPMKG.HBNENABOFLI))]  //matches
        [HarmonyPrefix]
        static bool FMHJNNGPMKG_HBNENABOFLI(ref int __result, int JCEJPDHEDFD)
        {
            if(Input.GetKey(KeyCode.N))
            {
                __result = 1;
                return false;
            }
            return true;
        }
        [HarmonyPatch(typeof(FMHJNNGPMKG), nameof(FMHJNNGPMKG.HLEALPKBHCB))]  //manager
        [HarmonyPrefix]
        static bool FMHJNNGPMKG_HLEALPKBHCB(ref int __result, int JCEJPDHEDFD)
        {
            if (Input.GetKey(KeyCode.N))
            {
                __result = 1;
                return false;
            }
            return true;
        }
        [HarmonyPatch(typeof(FMHJNNGPMKG), nameof(FMHJNNGPMKG.CDNBJOGEAME))]  //partner
        [HarmonyPrefix]
        static bool FMHJNNGPMKG_CDNBJOGEAME(ref int __result, int JCEJPDHEDFD)
        {
            if (Input.GetKey(KeyCode.N))
            {
                __result = 1;
                return false;
            }
            return true;
        }
    }
}