//todo costumes based on character role; injured taunt?; menu with cards; left right button + slider; text info below card;
// custom card generator separate project; lock in character pos & camera mode; multiple presets + custom; 
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CollectibleCards2
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CollectibleCards";
        public const string PluginName = "CollectibleCards";
        public const string PluginVer = "0.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;


        public static int CardWidth { get; set; } = 719;
        public static int CardHeight { get; set; } = 1000;

        public static ConfigEntry<float> CharXPos { get; set; }
        public static ConfigEntry<float> CharYPos { get; set; }
        public static ConfigEntry<float> CharSize { get; set; }

        public static ConfigEntry<int> CameraMode { get; set; }

        private void Awake()
        {
            Plugin.Log = base.Logger;
            PluginPath = Path.GetDirectoryName(Info.Location);

            Background.GetShaderPrefab();
            OverlaytxtFileParser.LoadSigFont();
            OverlaytxtFileParser.LoadCardFont();

            CharXPos = Config.Bind("General",
             "Character x position",
             0f,
             "Character x position");
            CharYPos = Config.Bind("General",
             "Character y position",
             0f,
             "Character y position");
            CharSize = Config.Bind("General",
             "Character scale",
             1f,
             "Character scale");

            CameraMode = Config.Bind("General",
             "Camera mode",
             0,
             "0 - perspective, 1 - orthographic");
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
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                StartCoroutine(CollectibleCardGenerator.GenerateCollectibleCard(preset:"1"));
            }
        }
    }
}