using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using WECCL.API;

namespace WrestlerMatchEditor
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.WrestlerMatchEditor";
        public const string PluginName = "WrestlerMatchEditor";
        public const string PluginVer = "1.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static ConfigEntry<string> showType;
        public static ConfigEntry<string> size;
        public static ConfigEntry<string> gimmick;
        public static ConfigEntry<string> opponent;
        public static ConfigEntry<string> match;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            Buttons.RegisterCustomButton(this, "Load custom match", () =>
            {
                LoadCard();
                return "Custom match loaded!";
            });

            Buttons.RegisterCustomButton(this, "Setup world title match", () =>
            {
                LoadWorldTitle();
                return "Custom match loaded!";
            });

            Buttons.RegisterCustomButton(this, "Setup inter title match", () =>
            {
                LoadInterTitle();
                return "Custom match loaded!";
            });

            Buttons.RegisterCustomButton(this, "Setup women title match", () =>
            {
                LoadWomenTitle();
                return "Custom match loaded!";
            });

            Buttons.RegisterCustomButton(this, "Setup tag team title match", () =>
            {
                LoadTagTitle();
                return "Custom match loaded!";
            });
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
        public void LoadWorldTitle()
        {
            Progress.gimmick[Progress.date] = 0;
            Progress.match[Progress.date] = 2;
            Progress.matchSize[Progress.date] = 2;
            int opp = Characters.fedData[Characters.c[Characters.wrestler].fed].champ[1, 1];
            Progress.opponent[Progress.date] = opp;
            FFCEGMEAIBP.JMBGHDFADHN = 1;

        }
        public void LoadInterTitle()
        {
            Progress.gimmick[Progress.date] = 0;
            Progress.match[Progress.date] = 2;
            Progress.matchSize[Progress.date] = 2;
            int opp = Characters.fedData[Characters.c[Characters.wrestler].fed].champ[2, 1];
            Progress.opponent[Progress.date] = opp;
            FFCEGMEAIBP.JMBGHDFADHN = 2;

        }
        public void LoadWomenTitle()
        {
            Progress.gimmick[Progress.date] = 0;
            Progress.match[Progress.date] = 2;
            Progress.matchSize[Progress.date] = 2;
            int opp = Characters.fedData[Characters.c[Characters.wrestler].fed].champ[3, 1];
            Progress.opponent[Progress.date] = opp;
            FFCEGMEAIBP.JMBGHDFADHN = 3;

        }
        public void LoadTagTitle()
        {
            Progress.gimmick[Progress.date] = 0;
            Progress.match[Progress.date] = 14;
            Progress.matchSize[Progress.date] = 4;
            int opp = Characters.fedData[Characters.c[Characters.wrestler].fed].champ[4, 1];
            Progress.opponent[Progress.date] = opp;
            FFCEGMEAIBP.JMBGHDFADHN = 4;

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
        public void LoadCard()
        {
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

