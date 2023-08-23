using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
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
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;
        public static ConfigEntry<KeyCode> loadCardButton;
        public struct CardStruct
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
        }
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            loadCardButton = Config.Bind("Controls",
             "Load custom card",
             KeyCode.KeypadPlus,
             "Button that loads the custom card");
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
            if(Input.GetKeyDown(loadCardButton.Value) && LFNJDEGJLLJ.NHDABIOCLFH == 2 && (SceneManager.GetActiveScene().name == "Calendar" || SceneManager.GetActiveScene().name == "Atlas"))
            {
                LoadCard();
            }    
        }
        public void LoadCard()
        {
            CardStruct card = new CardStruct();
            List<Segment> segments = new List<Segment>();
            Segment CustomSegment = new Segment();
            string[] lines = File.ReadAllLines(Path.Combine(PluginPath, "CustomCard.txt"));
            int matchid = 0;
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
                    matchid = int.Parse(line.Substring(9).Trim());
                    CustomSegment = new Segment();
                    CustomSegment.id = matchid;
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
                Progress.MOHIFAFEPFE(card.date);
            }
            if (card.rivalFed != -1)
            {
                Progress.rivalFed = card.rivalFed;
            }
            if (card.type != -1)
            {
                if (card.type == 4)
                {
                    Progress.card[card.date].FFGHCMGIDOB(card.date, 6);
                    Progress.BBBGMKCJJBB(card.date);
                }
                else
                {
                    Progress.show[card.date] = card.type;
                }
            }
            if (card.resetCard)
            {
                World.BCGKOALHLFO();
                World.GIBGMPFBBOF(0, card.date, 1);
                World.PCPFMFFBKCP();
                Progress.card[card.date].FFGHCMGIDOB(card.date, Progress.card[card.date].size);
            }


            if (card.size != -1)
            {
                List<Segment> savedSegments = new List<Segment>();
                for (int i = 0; i < 6; i++) 
                {
                    savedSegments.Add(Progress.card[card.date].segment[i]);
                }
                Progress.card[card.date].FFGHCMGIDOB(card.date, card.size);
                for (int i = 0; i < 6; i++)
                {
                    Progress.card[card.date].segment[i] = savedSegments[i];
                }
            }
            if (card.attendance != -1)
            {
                Progress.attendance[card.date] = card.attendance;
            }
            if (card.territory != -1)
            {
                Progress.territory[card.date] = card.territory;
            }
            foreach (Segment segment in segments)
            {
                Progress.card[card.date].segment[segment.id] = segment;
            }
        }
        
    }
}
//todo figure out a way to change card size without fuckups


//generic 
//Progress.MOHIFAFEPFE(Progress.date); - create card if not exists
//Progress.show[Progress.date];  - type
//Progress.card[Progress.date].FFGHCMGIDOB(Progress.date, 6); - reset card
//World.GIBGMPFBBOF(Progress.fed, Progress.date, 1); - reset attendance + arena?

/*
 * World.BCGKOALHLFO(); -save arena
World.GIBGMPFBBOF(0, Progress.date, 1); -reset attendance + arena
World.PCPFMFFBKCP(); - load arena*/
/* Cross promotion: 
 * Progress.rivalFed = 2; -opp fed. can't be the same as you!
    Progress.card[Progress.date].FFGHCMGIDOB(Progress.date, 6);
    Progress.BBBGMKCJJBB(Progress.date);
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