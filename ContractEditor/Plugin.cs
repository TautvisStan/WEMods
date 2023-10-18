using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContractEditor
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.ContractEditor";
        public const string PluginName = "ContractEditor";
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static ConfigEntry<KeyCode> loadContractButton;
        public struct ContractStruct
        {
            public int? salary { get; set; } = null;
            public int? weeks { get; set; } = null;
            public int? clause { get; set; } = null;

            public ContractStruct()
            {

            }
        }
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            loadContractButton = Config.Bind("Controls", 
             "Load custom contract",
             KeyCode.KeypadMinus,
             "Button that loads the custom contract");
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
            if(Input.GetKeyDown(loadContractButton.Value))
            {
                    if (SceneManager.GetActiveScene().name == "Select_Char")
                    {
                        Logger.LogInfo("Loading custom contract.");
                        try
                        {
                            LoadCard();
                        }
                        catch (Exception e)
                        {
                            Logger.LogError("Error while loading custom contract: ");
                            Logger.LogError(e);
                        }
                        finally
                        {
                            Logger.LogInfo("Done loading custom contract.");
                        }
                    }
                    else
                    {
                        Logger.LogInfo("Didn't load, not in the character select menu.");
                    }

            }    
        }
        public void LoadCard()
        {
            ContractStruct contract = new ContractStruct();
            string[] lines = File.ReadAllLines(Path.Combine(PluginPath, "CustomContract.txt"));
            foreach (string line in lines)
            {
                if (line.Trim().Length == 0)
                {
                    continue;
                }
                
                if (line.ToLower().StartsWith("salary:"))
                {
                    contract.salary = int.Parse(line.Substring(7).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("weeks:"))
                {
                    contract.weeks = int.Parse(line.Substring(6).Trim());
                    continue;
                }
                if (line.ToLower().StartsWith("clause:"))
                {
                    contract.clause = int.Parse(line.Substring(7).Trim());
                    continue;
                }
            }
            if (contract.weeks != null)
            {
                Logger.LogInfo("setting weeks " + contract.weeks);
                Characters.c[Characters.foc].contract = (int)contract.weeks;
                
            }
            if (contract.salary != null)
            {
                Logger.LogInfo("setting salary " + contract.salary);
                Characters.c[Characters.foc].salary = (int)contract.salary;

            }
            if (contract.clause != null)
            {
                Logger.LogInfo("setting clause " + contract.clause);
                Characters.c[Characters.foc].clause = (int)contract.clause;

            }

        }
        
    }
}

