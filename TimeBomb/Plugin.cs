using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using WECCL.API;

namespace TimeBomb
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.TimeBomb";
        public const string PluginName = "TimeBomb";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<float> configSize;
        public static ConfigEntry<int> configBurstSize;
        public static ConfigEntry<float> configTimeInBurst;
        //public static ConfigEntry<bool> configRepeat;
        public static ConfigEntry<int> configSeconds;
        public static ConfigEntry<bool> configMatch;
        public static int LastExplosion = 0;

        public static int? TimeBombID = null;
        public static bool active = false;
        public static int time;
        public static int oldtime;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            configSize = Config.Bind("General",
             "ExplosionSize",
             30.5f,
             "Base size of an explosion");
            configBurstSize = Config.Bind("General",
             "BurstSize",
             2,
             "Amount of spawned explosions in the burst");
            configTimeInBurst = Config.Bind("General",
             "BurstExplosionTime",
             0.5f,
             "Time between explosions in a single burst");
            configRepeat = Config.Bind("General",
             "Repeat",
             true,
             "Repeat explosions on the interval");
            configSeconds = Config.Bind("General",
             "ExplosionInterval",
             10,
             "Time in seconds between explosions");
        }
        private void Start()
        {
            TimeBombID = WECCL.API.CustomMatch.RegisterCustomPreset("TimeBomb", true);
            if (TimeBombID == null)
            {
                Logger.LogError("Failed to connect to WECCL! Disabling mod.");
                this.enabled = false;
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
       // [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Update))]
       // [HarmonyPostfix]
        private void Update()
        {
            if (FFCEGMEAIBP.CBIPLGLDCAG != TimeBombID) return;
            if (FFCEGMEAIBP.LOBDMDPMFLK != 2) return;
            //FFCEGMEAIBP.LOBDMDPMFLK match state, 2 - in match
            int minutes = FFCEGMEAIBP.IBGAIDBHGED;
            int seconds = FFCEGMEAIBP.LCLHNINHLHO;
            time = minutes * 60 + seconds;
            if (time % configSeconds.Value == 0 && time != 0)
            {
                if (time != oldtime)
                {
                    StartCoroutine(MakeExplosions());
                    if (!configRepeat.Value)
                    {
                        active = false;
                    }
                }
            }
            oldtime = time;
        }
        IEnumerator MakeExplosions()
        {
            for (int i = 0; i < configBurstSize.Value; i++)
            {
                ALIGLHEIAGO.MDFJMAEDJMG(1, 2, new Color(1f, 1f, 1f), Plugin.configSize.Value * World.ringSize, null, 0f, 15f, 0f, 0f, 0f, 0f, 1);
                yield return new WaitForSeconds(configTimeInBurst.Value);
            }
        }
    }
}