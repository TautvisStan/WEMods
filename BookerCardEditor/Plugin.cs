using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BookerCardEditor
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.BookerCardEditor";
        public const string PluginName = "BookerCardEditor";
        public const string PluginVer = "1.0.3";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static ConfigEntry<KeyCode> loadCardButton;
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

        /*    public struct CardStruct
            {
                public int date { get; set; } = Progress.date;
                public int type { get; set; } = -1;
                public int size { get; set; } = -1;
                public int rivalFed { get; set; } = -1;
                public bool resetCard { get; set; } = false;
                public int attendance { get; set; } = -1;
                public int territory { get; set; } = -1;

                public CardStruct()
                {

                }
            }*/
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            loadCardButton = Config.Bind("Controls",
             "Load custom card",
             KeyCode.KeypadPlus,
             "Button that loads the custom card");

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
        private void Update()
        {
            if(Input.GetKeyDown(loadCardButton.Value))
            {
                if (NAEEIFNFBBO.CBMHGKFFHJE == 2)
                {
                    bool loaded = false;
                    if (SceneManager.GetActiveScene().name == "Calendar")
                    {
                        Logger.LogInfo("Loading custom card.");
                        try
                        {
                            loaded = true;
                            LoadCard();
                        }
                        catch (Exception e)
                        {
                            Logger.LogError("Error while loading custom card: ");
                            Logger.LogError(e);
                        }
                        finally
                        {
                            Logger.LogInfo("Done loading custom card.");
                        }
                    }
                    if (SceneManager.GetActiveScene().name == "Card")
                    {
                        Logger.LogInfo("Loading custom segment.");
                        try
                        {
                            loaded = true;
                            LoadCardSegment();
                        }
                        catch (Exception e)
                        {
                            Logger.LogError("Error while loading custom card segment: ");
                            Logger.LogError(e);
                        }
                        finally
                        {
                            Logger.LogInfo("Done loading custom card segment.");
                        }
                    }
                    if(loaded == false)
                    {
                        Logger.LogInfo("Didn't load, not in the correct menu menu.");
                    }
                }
                else
                {
                    Logger.LogInfo("Didn't load, not in the booker mode.");
                }

            }    
        }
        public void LoadCard()
        {
            /*   CardStruct card = new CardStruct();
               List<Segment> segments = new List<Segment>();
               Segment CustomSegment = new Segment();
               string[] lines = File.ReadAllLines(Path.Combine(PluginPath, "CustomCard.txt"));
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

                   if (line.ToLower().StartsWith("card_type:"))
                   {
                       card.type = int.Parse(line.Substring(10).Trim());
                       continue;
                   }
                   if (line.ToLower().StartsWith("rival_fed:"))
                   {
                       card.rivalFed = int.Parse(line.Substring(10).Trim());
                       continue;
                   }
                   if (line.ToLower().StartsWith("card_size:"))
                   {
                       card.size = int.Parse(line.Substring(10).Trim());
                       continue;
                   }
                   if (line.ToLower().StartsWith("reset_card:"))
                   {
                       bool.TryParse(line.Substring(11).Trim(), out bool reset);
                       card.resetCard = reset;
                       continue;
                   }
                   if (line.ToLower().StartsWith("attendance:"))  
                   {
                       card.attendance = int.Parse(line.Substring(11).Trim());
                       continue;
                   }
                   if (line.ToLower().StartsWith("territory:"))
                   {
                       card.territory = int.Parse(line.Substring(10).Trim());
                       continue;
                   }
                   if (line.ToLower().StartsWith("match_id:"))
                   {
                       CustomSegment = new Segment();
                       CustomSegment.id = int.Parse(line.Substring(9).Trim());
                       continue;
                   }
                   if (line.ToLower().StartsWith("match_name:"))   
                   {
                       CustomSegment.match = line.Substring(11).Trim();
                       continue;
                   }
                   if (line.ToLower().StartsWith("left_name:"))   
                   {
                       CustomSegment.leftName = line.Substring(10).Trim();
                       continue;
                   }
                   if (line.ToLower().StartsWith("right_name:"))   
                   {
                       CustomSegment.rightName = line.Substring(11).Trim();
                       continue;
                   }
                   if (line.ToLower().StartsWith("left_id:"))   
                   {
                       CustomSegment.leftChar = int.Parse(line.Substring(8).Trim());
                       continue;
                   }
                   if (line.ToLower().StartsWith("right_id:"))   
                   {
                       CustomSegment.rightChar = int.Parse(line.Substring(9).Trim());
                       continue;
                   }
                   if (line.ToLower().StartsWith("match_end"))   
                   {
                       segments.Add(CustomSegment);
                       continue;
                   }
               }

               if (Progress.card[card.date] == null)
               {
                   Logger.LogInfo("Card is null, so reloading it");
                   Progress.PDBLBLFNDHE(card.date);
               }
               if (card.rivalFed != -1)
               {
                   Logger.LogInfo("setting rival_fed " + card.rivalFed);
                   Progress.rivalFed = card.rivalFed;
               }
               if (card.type != -1)
               {
                   Logger.LogInfo("setting card_type " + card.type);
                   if (card.type == 4)
                   {
                       Progress.card[card.date].PIMGMPBCODM(card.date, 6);
                       Progress.MBGDIFJMIHA(card.date);
                   }
                   else
                   {
                       Progress.show[card.date] = card.type;
                   }
               }
               if (card.resetCard)
               {
                   Logger.LogInfo("resetting the card");
                   World.KKJICBHPPEL();
                   World.NIKGKKEJCFL(0, card.date, 1);
                   World.CIEFJFNEEFJ();
                   Progress.card[card.date].PIMGMPBCODM(card.date, Progress.card[card.date].size);
               }


               if (card.size != -1)
               {
                   Logger.LogInfo("setting card_size " + card.size);
                   List<Segment> savedSegments = new List<Segment>();
                   int smallersize = Progress.card[card.date].size < card.size ? Progress.card[card.date].size : card.size;
                   for (int i = 1; i <= smallersize; i++) 
                   {
                       savedSegments.Add(Progress.card[card.date].segment[i]);
                   }
                   Progress.card[card.date].PIMGMPBCODM(card.date, card.size);
                   foreach (Segment segment in savedSegments)
                   {
                       Progress.card[card.date].segment[segment.id] = segment;
                   }
               }
               if (card.attendance != -1)
               {
                   Logger.LogInfo("setting attendance " + card.attendance);
                   Progress.attendance[card.date] = card.attendance;
               }
               if (card.territory != -1)
               {
                   Logger.LogInfo("setting territory " + card.territory);
                   Progress.territory[card.date] = card.territory;
               }
               foreach (Segment segment in segments)
               {
                   Logger.LogInfo("setting match segment: ");
                   Logger.LogInfo("match_id " + segment.id);
                   Logger.LogInfo("match \"" + segment.match + "\"");
                   Logger.LogInfo("left_name \"" + segment.leftName + "\"");
                   Logger.LogInfo("right_name \"" + segment.rightName + "\"");
                   Logger.LogInfo("left_char " + segment.leftChar);
                   Logger.LogInfo("right_char " + segment.rightChar);
                   Progress.card[card.date].segment[segment.id] = segment;
                   Logger.LogInfo("=====================================");
               }*/
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
                    if (SegmentMatch.Value != "")
                    {
                        Logger.LogInfo("match \"" + SegmentMatch.Value + "\"");
                        customSegment.match = SegmentMatch.Value;
                    }
                    if (SegmentLeftName.Value != "")
                    {
                        Logger.LogInfo("left_name \"" + SegmentLeftName.Value + "\"");
                        customSegment.leftName = SegmentLeftName.Value;
                    }
                    if (SegmentRightName.Value != "")
                    {
                        Logger.LogInfo("right_name \"" + SegmentRightName.Value + "\"");
                        customSegment.rightName = SegmentRightName.Value;
                    }
                    if (SegmentLeftID.Value != "")
                    {
                        Logger.LogInfo("left_char " + SegmentLeftID.Value);
                        customSegment.leftChar = int.Parse(SegmentLeftID.Value);
                    }
                    if (SegmentRightID.Value != "")
                    {
                        Logger.LogInfo("right_char " + SegmentRightID.Value);
                        customSegment.rightChar = int.Parse(SegmentRightID.Value);
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