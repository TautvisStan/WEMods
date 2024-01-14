using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using WECCL.API;

namespace BookerCardEditor
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.BookerCardEditor";
        public const string PluginName = "BookerCardEditor";
        public const string PluginVer = "1.0.4";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static ConfigEntry<bool> ResetCard;
        public static ConfigEntry<string> showType;
        public static ConfigEntry<string> size;
        public static ConfigEntry<string> rivalFed;
        public static ConfigEntry<string> attendance;
        public static ConfigEntry<string> territory;

        public static ConfigEntry<string> SegmentID;
        public static ConfigEntry<string> SegmentMatch;
        public static ConfigEntry<string> SegmentLeftID;
        public static ConfigEntry<string> SegmentRightID;
        public static ConfigEntry<string> SegmentLeftName;
        public static ConfigEntry<string> SegmentRightName;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            Buttons.RegisterCustomButton(this, "Load custom card", () =>
            {
                LoadCard();
                return "Custom card loaded!";
            });

            Buttons.RegisterCustomButton(this, "Load custom segment", () =>
            {
                LoadCardSegment();
                return "Custom segment loaded!";
            });

            ResetCard = Config.Bind("Card",
             "Reset the card",
             false,
             "Reset the card");

            showType = Config.Bind("Card",
             "Show type",
             "",
             "Show type");
            size = Config.Bind("Card",
             "Size",
            "",
             "Card size");
            rivalFed = Config.Bind("Card",
             "Rival fed",
            "",
             "Rival fed ID");
            attendance = Config.Bind("Card",
             "Attendance",
            "",
             "Attendance");
            territory = Config.Bind("Card",
             "Territory",
            "",
             "Territory ID");



            SegmentID = Config.Bind("Segment",
             "Segment ID",
             "",
             "Card segment ID");
            SegmentMatch = Config.Bind("Segment",
             "Segment Match Name",
            "",
             "The segment match name displayed on the card");
            SegmentLeftID = Config.Bind("Segment",
             "Left wrestler ID",
            "",
             "Left wrestler ID");
            SegmentRightID = Config.Bind("Segment",
             "Right wrestler ID",
            "",
             "Right wrestler ID");
            SegmentLeftName = Config.Bind("Segment",
             "Left wrestler name",
            "",
             "Left wrestler name displayed on the card");
            SegmentRightName = Config.Bind("Segment",
             "Right wrestler name",
            "",
             "Right wrestler name displayed on the card");
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
            
            if (Progress.card[Progress.date] == null)
            {
                Logger.LogInfo("Card is null, so reloading it");
                Progress.PDBLBLFNDHE(Progress.date);
            }
            if (rivalFed.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting rival fed " + int.Parse(rivalFed.Value));
                    Progress.rivalFed = int.Parse(rivalFed.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse rival fed: " + rivalFed.Value);
                    Logger.LogError(e);
                }

            }
            if (showType.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting show type " + int.Parse(showType.Value));
                    if (showType.Value == "4")
                    {
                        Progress.card[Progress.date].PIMGMPBCODM(Progress.date, 6);
                        Progress.MBGDIFJMIHA(Progress.date);
                    }
                    else
                    {
                        Progress.show[Progress.date] = int.Parse(showType.Value);
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse show type: " + showType.Value);
                    Logger.LogError(e);
                }
            }
            if (ResetCard.Value == true)
            {
                Logger.LogInfo("Resetting the card");
                World.KKJICBHPPEL();
                World.NIKGKKEJCFL(0, Progress.date, 1);
                World.CIEFJFNEEFJ();
                Progress.card[Progress.date].PIMGMPBCODM(Progress.date, Progress.card[Progress.date].size);
            }
            if(size.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting card size " + size.Value);
                    List<Segment> savedSegments = new List<Segment>();
                    int smallersize = Progress.card[Progress.date].size < int.Parse(size.Value) ? Progress.card[Progress.date].size : int.Parse(size.Value);
                    for (int i = 1; i <= smallersize; i++)
                    {
                        savedSegments.Add(Progress.card[Progress.date].segment[i]);
                    }
                    Progress.card[Progress.date].PIMGMPBCODM(Progress.date, int.Parse(size.Value));
                    foreach (Segment segment in savedSegments)
                    {
                        Progress.card[Progress.date].segment[segment.id] = segment;
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse size: " + size.Value);
                    Logger.LogError(e);
                }
            }
            if (attendance.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting attendance " + attendance.Value);
                    Progress.attendance[Progress.date] = int.Parse(attendance.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse attendance: " + attendance.Value);
                    Logger.LogError(e);
                }

            }
            if (territory.Value != "")
            {
                try
                {
                    Logger.LogInfo("Setting territory " + territory.Value);
                    Progress.territory[Progress.date] = int.Parse(territory.Value);
                }
                catch (Exception e)
                {
                    Logger.LogError("Error trying to parse territory: " + territory.Value);
                    Logger.LogError(e);
                }

            }

        }
        
        public void LoadCardSegment()
        {
            try
            {
                if (SegmentID.Value != "")
                {
                    Segment customSegment = new Segment();
                    Logger.LogInfo("setting match segment: ");
                    Logger.LogInfo("match_id " + SegmentID.Value);
                    customSegment.id = int.Parse(SegmentID.Value);
                    if (Progress.card[Progress.date] == null)
                    {
                        Logger.LogInfo("Card is null, so reloading it");
                        Progress.PDBLBLFNDHE(Progress.date);
                    }
                    if (Progress.card[Progress.date].segment[customSegment.id] == null)
                    {
                        Logger.LogInfo("Segment is null, so reloading it");
                        Progress.card[Progress.date].segment[customSegment.id] = new Segment();
                    }
                    try
                    {
                        if (SegmentMatch.Value != "")
                        {
                            Logger.LogInfo("match \"" + SegmentMatch.Value + "\"");
                            customSegment.match = SegmentMatch.Value;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Error while trying to parse Segment Match: " + SegmentMatch.Value);
                        Logger.LogError(e);
                    }
                    try
                    {
                        if (SegmentLeftName.Value != "")
                        {
                            Logger.LogInfo("left_name \"" + SegmentLeftName.Value + "\"");
                            customSegment.leftName = SegmentLeftName.Value;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Error while trying to parse Segment Left Name: " + SegmentLeftName.Value);
                        Logger.LogError(e);
                    }
                    try
                    {
                        if (SegmentRightName.Value != "")
                        {
                            Logger.LogInfo("right_name \"" + SegmentRightName.Value + "\"");
                            customSegment.rightName = SegmentRightName.Value;
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Error while trying to parse Segment Right Name: " + SegmentRightName.Value);
                        Logger.LogError(e);
                    }
                    try
                    {
                        if (SegmentLeftID.Value != "")
                        {
                            Logger.LogInfo("left_char " + SegmentLeftID.Value);
                            customSegment.leftChar = int.Parse(SegmentLeftID.Value);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Error while trying to parse Segment Left ID: " + SegmentLeftID.Value);
                        Logger.LogError(e);
                    }
                    try
                    {
                        if (SegmentRightID.Value != "")
                        {
                            Logger.LogInfo("right_char " + SegmentRightID.Value);
                            customSegment.rightChar = int.Parse(SegmentRightID.Value);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.LogError("Error while trying to parse Segment right ID: " + SegmentRightID.Value);
                        Logger.LogError(e);
                    }

                    Progress.card[Progress.date].segment[customSegment.id] = customSegment;
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Error trying to parse custom segment:");
                Logger.LogError(e);
            }
        }
    }
}

//generic 
//Progress.PDBLBLFNDHE(Progress.date); - create card if not exists
//Progress.show[Progress.date];  - type
//Progress.card[Progress.date].PIMGMPBCODM(Progress.date, 6); - reset card
//World.NIKGKKEJCFL(Progress.fed, Progress.date, 1); - reset attendance + arena?

/*
 * World.KKJICBHPPEL(); -save arena
World.NIKGKKEJCFL(0, Progress.date, 1); -reset attendance + arena
World.CIEFJFNEEFJ(); - load arena*/
/* Cross promotion: 
 * Progress.rivalFed = 2; -opp fed. can't be the same as you!
    Progress.card[Progress.date].PIMGMPBCODM(Progress.date, 6);
    Progress.MBGDIFJMIHA(Progress.date);
 * 
 * 
 */
/*
 * format
[date]: x
rival_fed: x
card_type: x
reset_card: true
card_size: 6/10
attendance: 100
territory: 10
match_id: x [id]
match: Prebooked match
leftName: Game modders
rightName: The game
leftChar: 1
rightChar: 2
match_end

int Segment = 5;
Log(Progress.card[Progress.date].segment[Segment].id);
Log(Progress.card[Progress.date].segment[Segment].match);
Log(Progress.card[Progress.date].segment[Segment].leftName);
Log(Progress.card[Progress.date].segment[Segment].rightName);
Log(Progress.card[Progress.date].segment[Segment].result);
Log(Progress.card[Progress.date].segment[Segment].leftChar);
Log(Progress.card[Progress.date].segment[Segment].rightChar);
Log(Progress.card[Progress.date].segment[Segment].winner);
Log(Progress.card[Progress.date].segment[Segment].rating);
Log(Progress.card[Progress.date].segment[Segment].violence);
Log(Progress.card[Progress.date].segment[Segment].time);
*/