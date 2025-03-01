using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using WEFreeCamera;

namespace CameraRecording
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CameraRecording";
        public const string PluginName = "CameraRecording";
        public const string PluginVer = "0.8.0";  //#TODO maincam mode without hud; action targeting?; realtime getting freecam pos; cleanup;
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
        Vector3[] savedPositions = new Vector3[10];
        Quaternion[] savedRotations = new Quaternion[10];
        float[] savedFoVs = new float[10];
        public static bool isRecording = false;
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
        private void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                savedPositions[i] = FreeCameraPlugin.savedPositions[i].Value;
                savedRotations[i] = FreeCameraPlugin.savedRotations[i].Value;
                savedFoVs[i] = FreeCameraPlugin.savedFoVs[i].Value;
            }
        }

        private void Update()
        { 
            if(Input.GetKeyDown(RecordingToggle.Value))
            {
                if (!isRecording)
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
                                test.camNumber = (i + 1) % 10;
                                recorders.Add(test);
                                Debug.LogWarning("ADDED");
                            }
                            Debug.LogWarning(recorders.Count);
                            Debug.LogWarning(i);
                            GameplayRecorder recorder = recorders[i];
                            Debug.LogWarning(recorders[i] == null);
                            if (recorder == null)
                            {
                                Debug.LogWarning("ADDING IN NULL");
                                GameObject testobj = GameObject.Instantiate(Camera.main.gameObject);

                                GameObject.Destroy(testobj.GetComponent<AudioListener>());

                                GameplayRecorder test = testobj.AddComponent<CameraRecording.GameplayRecorder>();
                                test.camNumber = (i + 1) % 10;
                                recorder = test;
                                Debug.LogWarning("ADDED");
                                recorders[i] = recorder;
                            }
                            Debug.LogWarning(recorders[i] == null);
                            StartRecording(ref recorder);
                            recorder.gameObject.transform.position = savedPositions[(i + 1) % 10];
                            recorder.gameObject.transform.rotation = savedRotations[(i + 1) % 10];
                            recorder.GetComponent<Camera>().fieldOfView = savedFoVs[(i + 1) % 10];
                        }
                    }
                    else
                    {
                        Debug.LogWarning("ADDING MAIN CAM");
                        if (recorders.Count == 0 || recorders[0] == null)
                        {
                            GameplayRecorder test = Camera.main.gameObject.AddComponent<CameraRecording.GameplayRecorder>();
                            test.camNumber = 1;
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
                        GameplayRecorder recorder = recorders[0];
                        StartRecording(ref recorder);
                    }
                    isRecording = true;
                }
                else
                {
                    StopRecordingAll();
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
            Debug.LogWarning("STOPPING");
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
                Debug.LogWarning("stopping I " + i);
                Debug.LogWarning(recorders[i] == null);
                Debug.LogWarning(recorders[i].isRecording);
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
            StopRecordingAll();
        }

    }
}