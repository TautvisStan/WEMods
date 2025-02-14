using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace CameraRecording
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CameraRecording";
        public const string PluginName = "CameraRecording";
        public const string PluginVer = "0.6.2";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<string> Mode;
        public static ConfigEntry<int> width;
        public static ConfigEntry<int> height;
        public static ConfigEntry<int> frameRate;
        public static ConfigEntry<int> crf;
        public static ConfigEntry<int> CameraCount;
        public static ConfigEntry<KeyCode> RecordingToggle;

        public static List<GameplayRecorder> recorders = new();

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            Mode = Config.Bind("General",
             "Recording camera mode",
             "MainCam",
             new ConfigDescription("MainCam = main game camera will be used. FreeCam = free cameras will be used", new AcceptableValueList<string>("MainCam", "FreeCam")));

            width = Config.Bind("General",
             "(FreeCam) Width",
             1920,
             "Screen width");

            height = Config.Bind("General",
             "(FreeCam) Height",
             1080,
             "Screen height");

            frameRate = Config.Bind("General",
             "Frame rate",
             30,
             "Video frame rate");

            CameraCount = Config.Bind("General",
             "(FreeCam) Recording cameras count",
             1,
             new ConfigDescription("Number of cameras recording gameplay at once", new AcceptableValueRange<int>(1, 10)));

            crf = Config.Bind("General",
             "Constant Rate Factor",
             28,
             "Constant Rate Factor (18-28 is good range, lower = better quality but bigger file size)");

            RecordingToggle = Config.Bind("Constant Rate Factor",
             "Recording toggle keybind",
             KeyCode.None,
             "Keycode to start/stop recording");
        }


        private void Update()
        { 
            if(Input.GetKeyDown(RecordingToggle.Value))
            {
                if (Mode.Value == "FreeCam")
                {
                    for (int i = 0; i < CameraCount.Value; i++)
                    {
                        Debug.LogWarning("I " + i);
                        if (recorders.Count <= i)
                        {
                            GameObject testobj = GameObject.Instantiate(Camera.main.gameObject);

                            GameObject.Destroy(testobj.GetComponent<AudioListener>());
                            GameplayRecorder test = testobj.AddComponent<CameraRecording.GameplayRecorder>();
                            recorders.Add(test);
                            Debug.LogWarning("ADDED");
                        }
                        if (recorders[i] == null)
                        {
                            Debug.LogWarning("ADDING IN NULL");
                            GameObject testobj = GameObject.Instantiate(Camera.main.gameObject);

                            GameObject.Destroy(testobj.GetComponent<AudioListener>());

                            GameplayRecorder test = testobj.AddComponent<CameraRecording.GameplayRecorder>();
                            recorders[i] = test;
                            Debug.LogWarning("ADDED");
                        }
                        ToggleRecording(recorders[i]);
                        recorders[i].gameObject.transform.position = WEFreeCamera.FreeCameraPlugin.savedPositions[i + 1].Value;
                        recorders[i].gameObject.transform.rotation = WEFreeCamera.FreeCameraPlugin.savedRotations[i + 1].Value;
                        recorders[i].GetComponent<Camera>().fieldOfView = WEFreeCamera.FreeCameraPlugin.savedFoVs[i + 1].Value;
                    }
                }
                else
                {
                    Debug.LogWarning("ADDING MAIN CAM");
                    if (recorders.Count == 0 || recorders[0] == null)
                    {
                        GameplayRecorder test = Camera.main.gameObject.AddComponent<CameraRecording.GameplayRecorder>();
                        if (recorders.Count == 0)
                        {
                            recorders.Add(test);
                            Debug.LogWarning("FIRST TIME");

                        }
                        else
                        {
                            recorders[0] = test;
                            Debug.LogWarning("Already existed before");
                        }
                    }
                    ToggleRecording(recorders[0]);
                }
            }
        }
        public void ToggleRecording(GameplayRecorder recorder)
        {
            UnityEngine.Debug.LogWarning("-2");
            if (!recorder.isRecording)
            {
                UnityEngine.Debug.LogWarning("-1");
                recorder.Setup();
                recorder.StartRecording();
                recorder.isRecording = true;
            }
            else
            {
                Debug.LogWarning("STOPPING");
                recorder.StopRecording();
                if (Mode.Value == "FreeCam")
                {
                    Destroy(recorder.gameObject);
                }
                else
                {
                    Destroy(recorder);
                }
                recorder = null;
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