using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WrestlerMatchEditor
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.WrestlerMatchEditor";
        public const string PluginName = "WrestlerMatchEditor";
        public const string PluginVer = "1.0.3";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static ConfigEntry<KeyCode> loadMatchButton;
        public static ConfigEntry<string> showType;
        public static ConfigEntry<string> size;
        public static ConfigEntry<string> gimmick;
        public static ConfigEntry<string> opponent;
        public static ConfigEntry<string> match;
   /*     public struct CardStruct
        {
            public int date { get; set; } = Progress.date;
            public int type { get; set; } = -1;
            public int size { get; set; } = -1;
            public int gimmick { get; set; } = -1;
            public int opponent { get; set; } = -1;
            public int match { get; set; } = -1;

            public CardStruct()
            {

            }
        }*/
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            loadMatchButton = Config.Bind("Controls",
             "Load custom match",
             KeyCode.KeypadPlus,
             "Button that loads the custom match");

            showType = Config.Bind("Match",
             "Show type",
             "",
             "Show type");
            size = Config.Bind("Match",
             "Size",
             "",
             "Match size");
            match = Config.Bind("Match",
             "Match type",
             "",
             "Match type");
            gimmick = Config.Bind("Match",
             "Gimmick",
             "",
             "Match gimmick");
            opponent = Config.Bind("Match",
             "Opponent",
             "",
             "Opponent ID");
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
            if(Input.GetKeyDown(loadMatchButton.Value))
            {
              //  Logger.LogInfo("Load custom match button has been pressed");
                if (NAEEIFNFBBO.CBMHGKFFHJE == 1)
                {
                    if (SceneManager.GetActiveScene().name == "Calendar")
                    {
                        Logger.LogInfo("Loading custom match.");
                        try
                        {
                            LoadCard();
                        }
                        catch (Exception e)
                        {
                            Logger.LogError("Error while loading custom match: ");
                            Logger.LogError(e);
                        }
                        finally
                        {
                            Logger.LogInfo("Done loading custom match.");
                        }
                    }
                    else
                    {
                        Logger.LogInfo("Didn't load, not in the calendar menu.");
                    }
                }
                else
                {
                   // Logger.LogInfo("Didn't load, not in the wrestler mode.");
                }

            }    
        }
        public void LoadCard()
        {
            /*    CardStruct card = new CardStruct();
                string[] lines = File.ReadAllLines(Path.Combine(PluginPath, "CustomMatch.txt"));
                foreach (string line in lines)
                {
                    if (line.Trim().Length == 0)
                    {
                        continue;
                    }
                    if (line.ToLower().StartsWith("date:"))
                    {
                        if(line.Substring(5).Trim().ToLower() == "today")
                        {
                            card.date = Progress.date;
                            continue;
                        }
                        if(line.Substring(5).Trim().ToLower() == "highlight")
                        {
                            card.date = Progress.focDate;
                            continue;
                        }
                        if(int.TryParse(line.Substring(5).Trim().ToLower(), out int date))
                        {
                            if (date >= 1 && date <= 48)
                            {
                                card.date = date;
                                continue;
                            }
                        }
                        continue;
                    }

                    if (line.ToLower().StartsWith("show_type:"))
                    {
                        card.type = int.Parse(line.Substring(10).Trim());
                        continue;
                    }
                    if (line.ToLower().StartsWith("match_type:"))
                    {
                        card.match = int.Parse(line.Substring(11).Trim());
                        continue;
                    }
                    if (line.ToLower().StartsWith("match_size:"))
                    {
                        card.size = int.Parse(line.Substring(11).Trim());
                        continue;
                    }
                    if (line.ToLower().StartsWith("gimmick:"))  
                    {
                        card.gimmick = int.Parse(line.Substring(8).Trim());
                        continue;
                    }
                    if (line.ToLower().StartsWith("opponent:"))
                    {
                        card.opponent = int.Parse(line.Substring(9).Trim());
                        continue;
                    }
                }
                if (card.type != -1)
                {
                    Logger.LogInfo("setting show_type " + card.type);
                    Progress.show[card.date] = card.type;

                }
                if (card.match != -1)
                {
                    Logger.LogInfo("setting match_type " + card.match);
                    Progress.match[card.date] = card.match;

                }
                if (card.size != -1)
                {
                    Logger.LogInfo("setting match_size " + card.size);
                    Progress.matchSize[card.date] = card.size;

                }
                if (card.gimmick != -1)
                {
                    Logger.LogInfo("setting gimmick " + card.gimmick);
                    Progress.gimmick[card.date] = card.gimmick;

                }
                if (card.opponent != -1)
                {
                    Logger.LogInfo("setting opponent " + card.opponent);
                    Progress.opponent[card.date] = card.opponent;

                }*/
            if (showType.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting show type " + int.Parse(showType.Value));
                    Progress.show[Progress.date] = int.Parse(showType.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse show type: " + showType.Value);
                    Logger.LogError(e);
                }

            }
            if (match.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting match type " + int.Parse(match.Value));
                    Progress.match[Progress.date] = int.Parse(match.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse match type: " + match.Value);
                    Logger.LogError(e);
                }

            }
            if (size.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting size " + size.Value);
                    Progress.matchSize[Progress.date] = int.Parse(size.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse size: " + size.Value);
                    Logger.LogError(e);
                }

            }
            if (gimmick.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting gimmick " + gimmick.Value);
                    Progress.gimmick[Progress.date] = int.Parse(gimmick.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse gimmick: " + gimmick.Value);
                    Logger.LogError(e);
                }

            }
            if (opponent.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting opponent " + opponent.Value);
                    Progress.opponent[Progress.date] = int.Parse(opponent.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse opponent: " + opponent.Value);
                    Logger.LogError(e);
                }

            }

        }

    }
}

