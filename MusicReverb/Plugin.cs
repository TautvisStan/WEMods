using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MusicReverb
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.MusicReverb";
        public const string PluginName = "MusicReverb";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static AudioReverbFilter Reverb = null;

        public static ConfigEntry<string> ConfigPreset;
        public static string[] PresetValues;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            PresetValues = Enum.GetNames(typeof(AudioReverbPreset));
            ConfigPreset = Config.Bind("Preset", "Reverb Preset", nameof(AudioReverbPreset.Arena), new ConfigDescription("Reverb Preset", new AcceptableValueList<string>(PresetValues)));
        }
        [HarmonyPatch(typeof(CHLPMKEGJBJ),nameof(CHLPMKEGJBJ.GOLIDGCCCPC))]
        [HarmonyPostfix]
        static void ReverbSet()
        {
            if (Reverb == null)
            {
                Reverb = CHLPMKEGJBJ.JDDFOINNPMO.AddComponent<AudioReverbFilter>();
                Reverb.enabled = false;
            }
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
        [HarmonyPatch(typeof(CHLPMKEGJBJ), nameof(CHLPMKEGJBJ.DGAGLLKPNJB))]
        [HarmonyPostfix]
        static void CHLPMKEGJBJ_DGAGLLKPNJB_Postfix(int JJNMLJBGPAH, float MCJHGEHEPMD, float CDNNGHGFALM)
        {
            if (Reverb != null)
            {
                if (ConfigPreset.Value != nameof(Reverb.reverbPreset)) Reverb.reverbPreset = (AudioReverbPreset)Enum.Parse(typeof(AudioReverbPreset), ConfigPreset.Value);
                if (SceneManager.GetActiveScene().name == "Game" && World.location <= 1 && JJNMLJBGPAH > 0)
                {
                    Reverb.enabled = true;
                }
                else
                {
                    Reverb.enabled = false;
                }
            }
        }
    }
}