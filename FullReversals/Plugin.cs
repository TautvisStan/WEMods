using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;

namespace WrestlingEmpireTemplateMod
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.FullReversals";
        public const string PluginName = "FullReversals";
        public const string PluginVer = "0.1.0";

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

        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.AHOHHCDBLMA))]
        [HarmonyPrefix]  //risk counter
        static void DFOGOCNBECG_AHOHHCDBLMAPrefix(DFOGOCNBECG __instance, DFOGOCNBECG ELPIOHCPOIJ, ref int DGJAEIBKLJO, int GBEJFLOIBOH, float DILPLMPNICB, float KMNBIPKAJAE = 0f)
        {
            if (ELPIOHCPOIJ.EMDMDLNJFKP.id != Characters.wrestler) return;
            DGJAEIBKLJO = int.MaxValue;
           // UnityEngine.Debug.LogWarning("RISK COUNTER");
        }
        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.PCHGFNPPOEM))] 
        [HarmonyPrefix]  //risk reversal
        static void DFOGOCNBECG_PCHGFNPPOEMPrefix(DFOGOCNBECG __instance, DFOGOCNBECG ELPIOHCPOIJ, ref int DGJAEIBKLJO, float NIIEGJGDAAM = -1f)
        {
            if (ELPIOHCPOIJ.EMDMDLNJFKP.id != Characters.wrestler) return;
            DGJAEIBKLJO = int.MaxValue;
            //UnityEngine.Debug.LogWarning("RISK REVERSAL");
        }

        /*[HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.JPPFMOKHMNE))] 
        [HarmonyPrefix]  //break  moves ?????
        static void DFOGOCNBECG_JPPFMOKHMNEPrefix(DFOGOCNBECG __instance, int FFFFLHENONC, ref int DGJAEIBKLJO)
        {
			if (__instance.ELPIOHCPOIJ.EMDMDLNJFKP.id != Characters.wrestler) return;
            DGJAEIBKLJO = int.MaxValue;
            UnityEngine.Debug.LogWarning("BREAK MOVE");
        }*/
        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.GEJEKGNJGFG))] 
        [HarmonyPrefix]  //break submissions
        static void DFOGOCNBECG_GEJEKGNJGFGPrefix(DFOGOCNBECG __instance, DFOGOCNBECG ELPIOHCPOIJ, ref float DGJAEIBKLJO)
        {
            if (ELPIOHCPOIJ.EMDMDLNJFKP.id != Characters.wrestler) return;
            DGJAEIBKLJO = 0;
            //UnityEngine.Debug.LogWarning("BREAK SUB");
        }

    }
}
//AHOHHCDBLMA
//PCHGFNPPOEM
//GOIDLMAMLJI?
//OBGMJPODDBB???
//GEJEKGNJGFG+++
//JPPFMOKHMNE?