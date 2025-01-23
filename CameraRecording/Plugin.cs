using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

namespace CameraRecording
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CameraRecording";
        public const string PluginName = "CameraRecording";
        public const string PluginVer = "0.1.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<int> width;
        public static ConfigEntry<int> height;
        public static ConfigEntry<int> frameRate;
        public static ConfigEntry<int> crf;

        public static ConfigEntry<KeyCode> RecordingToggle;

        public static List<GameplayRecorder> recorders = new();

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            width = Config.Bind("General",
             "Width",
             1920,
             "Screen width");

            height = Config.Bind("General",
             "Height",
             1080,
             "Screen height");

            frameRate = Config.Bind("General",
             "Frame rate",
             30,
             "Video frame rate");

            crf = Config.Bind("General",
             "Constant Rate Factor",
             28,
             "Constant Rate Factor (18-28 is good range, lower = better quality)");

            RecordingToggle = Config.Bind("Constant Rate Factor",
             "Recording toggle keybind",
             KeyCode.None,
             "Keycode to start/stop recording");
        }
        private void Update()
        { 
            if(Input.GetKeyDown(RecordingToggle.Value))
            {
                for (int i = 0; i < 4; i++)
                {
                    Debug.LogWarning("I " + i);
                    if(recorders.Count <= i)
                    {
                        recorders.Add(new GameplayRecorder());
                        Debug.LogWarning("ADDED");
                    }
                    recorders[i].ToggleRecording();
                    Debug.LogWarning("A");
                    recorders[i].test.recordingCamera.gameObject.transform.position = WEFreeCamera.FreeCameraPlugin.savedPositions[i+1].Value;
                    Debug.LogWarning("B");
                    recorders[i].test.recordingCamera.gameObject.transform.rotation = WEFreeCamera.FreeCameraPlugin.savedRotations[i+1].Value;
                    Debug.LogWarning("C");
                }
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
    }
}