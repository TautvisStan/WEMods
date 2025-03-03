using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using WEFreeCamera;
using static WEFreeCamera.FreeCameraPlugin;

namespace CameraRecording
{
    [BepInDependency("GeeEm.WrestlingEmpire.WEFreeCamera", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CameraRecording";
        public const string PluginName = "CameraRecording";
        public const string PluginVer = "1.0.2";  //#TODO maincam mode without hud; action targeting?; fix MainCam rec + freecam mode
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
        public static ConfigEntry<bool> freecamSmoothering;
        public static ConfigEntry<bool> maincamVisibleUI;

        public static List<GameplayRecorder> recorders = new();
        public static bool isRecording = false;
        private void Awake()
        {
            FreeCameraPlugin.savedPositions.ToString(); //test to see if freecam is enabled

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

            freecamSmoothering = Config.Bind("General",
             "(FreeCam) lag smoothering",
             false,
             "In theory, should make the lag smoother by adding a small delay to each cam (I'm not sure if it actually has an impact)");

            maincamVisibleUI = Config.Bind("General",
             "(MainCam) visible UI",   
             true,
             "Should the UI be visible in main camera recordings");

            RecordingToggle = Config.Bind("Constant Rate Factor",  //messed up the name & if I fix it users' setting will be reset
             "Recording toggle keybind",
             KeyCode.None,
             "Keycode to start/stop recording");

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
            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (Input.GetKeyDown(RecordingToggle.Value))
                {
                    if (!isRecording)
                    {
                        Log.LogInfo("Starting recording");
                        if (Mode.Value == "FreeCam")
                        {
                            Log.LogInfo($"Recording {CameraCount.Value} freecams");
                            for (int i = 0; i < CameraCount.Value; i++)
                            {
                                GameObject recordingCamera = GameObject.Instantiate(Camera.main.gameObject);
                                GameObject.Destroy(recordingCamera.GetComponent<AudioListener>());
                                WEFreeCamera.CustomCamera customCamera = null;
                                recordingCamera.TryGetComponent<WEFreeCamera.CustomCamera>(out customCamera);
                                if (customCamera != null)
                                    GameObject.Destroy(customCamera);
                                GameplayRecorder gameplayRecorder = recordingCamera.AddComponent<CameraRecording.GameplayRecorder>();
                                gameplayRecorder.camNumber = (i + 1) % 10;
                                gameplayRecorder.frameDelay = CameraCount.Value;
                                if (recorders.Count <= i)
                                {
                                    recorders.Add(gameplayRecorder);
                                }
                                else
                                {
                                    recorders[i] = gameplayRecorder;
                                }
                                StartRecording(ref gameplayRecorder);
                                gameplayRecorder.gameObject.transform.position = WEFreeCamera.FreeCameraPlugin.savedPositions[(i + 1) % 10].Value;
                                gameplayRecorder.gameObject.transform.rotation = WEFreeCamera.FreeCameraPlugin.savedRotations[(i + 1) % 10].Value;
                                gameplayRecorder.GetComponent<Camera>().fieldOfView = WEFreeCamera.FreeCameraPlugin.savedFoVs[(i + 1) % 10].Value;
                                gameplayRecorder.recordingCamera.enabled = false;
                            }
                        }
                        else  //MainCam
                        {
                            Log.LogInfo("Recording main camera");
                            GameplayRecorder gameplayRecorder = Camera.main.gameObject.AddComponent<CameraRecording.GameplayRecorder>();

                            gameplayRecorder.camNumber = 1;
                            gameplayRecorder.frameDelay = 1;
                            if (recorders.Count == 0)
                            {
                                recorders.Add(gameplayRecorder);
                            }
                            else
                            {
                                recorders[0] = gameplayRecorder;
                            }
                            StartRecording(ref gameplayRecorder);
                        }
                        isRecording = true;
                    }
                    else
                    {
                        Log.LogInfo("Stopping recording");
                        StopRecordingAll();
                    }
                }
            }
        }
        public static void StartRecording(ref GameplayRecorder recorder)
        {
            if (!recorder.isRecording)
            {
                recorder.Setup();
                recorder.StartRecording();
                recorder.isRecording = true;
            }
        }
        public static void StopRecording(ref GameplayRecorder recorder)
        {
            recorder.StopRecording();
            if (Mode.Value == "FreeCam")
            {
                Destroy(recorder.gameObject);
                Destroy(recorder);
            }
            else
            {
                Destroy(recorder);
            }
            recorder = null;
        }
        public static void StopRecordingAll()
        {
            for (int i = 0; i < recorders.Count; i++)
            {
                if (recorders[i] != null && recorders[i].isRecording)
                {
                    GameplayRecorder recorder = recorders[i];
                    StopRecording(ref recorder);
                }
            }
            isRecording = false;
        }
        //disabling on scene switch
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.PMIIOCMHEAE))]
        public static void LIPNHOMGGHF_PMIIOCMHEAE_Prefix()
        {
            if (isRecording)
            {
                Log.LogInfo("Stopping recording");
                StopRecordingAll();
            }
        }

    }
}