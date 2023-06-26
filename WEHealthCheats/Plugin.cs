using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace HealthCheats
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.HealthCheats";
        public const string PluginName = "HealthCheats";
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
        private void Update()
        {
            try
            {
                if (Input.GetKey(KeyCode.LeftAlt)) //target
                {
                    if (Input.GetKey(KeyCode.Q)) //health max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].LPEIDPCNMGK = int.MaxValue;
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].JAENLEMFEIC = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.W)) //health 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].LPEIDPCNMGK = 0;
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].JAENLEMFEIC = 0;
                    }
                    if (Input.GetKey(KeyCode.E)) //stun max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].CIKIJABJGOL = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.R)) //stun 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].CIKIJABJGOL = 0;
                    }
                    if (Input.GetKey(KeyCode.T)) //blindness max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].DEGCIAJNOMC = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.Y)) //blindness 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].DEGCIAJNOMC = 0;
                    }
                    if (Input.GetKey(KeyCode.D)) //adrenaline max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].AEICBDHMALN = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.F)) //adrenaline 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].AEICBDHMALN = 0;
                    }
                    if (Input.GetKey(KeyCode.G)) //adr timer max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].LPJAPJBIIFF = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.H)) //adr timer 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].LPJAPJBIIFF = 1;
                    }

                }
                else //player
                {
                    if (Input.GetKey(KeyCode.Q)) //health max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].LPEIDPCNMGK = int.MaxValue;
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].JAENLEMFEIC = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.W)) //health 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].LPEIDPCNMGK = 0;
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].JAENLEMFEIC = 0;
                    }
                    if (Input.GetKey(KeyCode.E)) //stun max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].CIKIJABJGOL = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.R)) //stun 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].CIKIJABJGOL = 0;
                    }
                    if (Input.GetKey(KeyCode.T)) //blindness max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].DEGCIAJNOMC = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.Y)) //blindness 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].DEGCIAJNOMC = 0;
                    }
                    if (Input.GetKey(KeyCode.D)) //adrenaline max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].AEICBDHMALN = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.F)) //adrenaline 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].AEICBDHMALN = 0;
                    }
                    if (Input.GetKey(KeyCode.G)) //adr timer max
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].LPJAPJBIIFF = int.MaxValue;
                    }
                    if (Input.GetKey(KeyCode.H)) //adr timer 0
                    {
                        AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].LPJAPJBIIFF = 1;
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
//target                    AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].PDMDFGNJCPN].
//self                     AMJONEKIAID.NCPIJJFEDFL[AMJONEKIAID.LKBPOJLFICP[1].PDMDFGNJCPN].