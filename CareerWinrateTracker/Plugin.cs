using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore;
using UnityEngine.UI;
using WECCL.Utils;


namespace CareerWinrateTracker
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.CareerWinrateTracker";
        public const string PluginName = "CareerWinrateTracker";
        public const string PluginVer = "1.0.3";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static GameObject WinRate;
        public static GameObject RateText;

        public static List<int> winners = new();
        public static List<int> losers = new();
        public static List<int> draws = new();
        public static bool readytosave = false;
        public static Dictionary<int, CharacterWinrate> Winrates;
        public static ConfigEntry<bool> AdvancedDisplay;

        public class CharacterWinrate
        {
            public IndividualTypesWinrate Special { get; set; }
            public IndividualTypesWinrate Singles { get; set; }
            public IndividualTypesWinrate Team { get; set; }
            public CharacterWinrate()
            {
                Special = new();
                Singles = new();
                Team = new();
            }
            public int GetTotalWins()
            {
                return Special.Wins + Singles.Wins + Team.Wins;
            }
            public int GetTotalDraws()
            {
                return Special.Draws + Singles.Draws + Team.Draws;
            }
            public int GetTotalLoses()
            {
                return Special.Loses + Singles.Loses + Team.Loses;
            }
        }
        public class IndividualTypesWinrate
        {
            public int Wins = 0;
            public int Draws = 0;
            public int Loses = 0;

        }



        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            Winrates = new();
            AdvancedDisplay = Config.Bind("General",
             "AdvancedDisplay",
             false,
             "Separate the winrate display into individual types (singles, teams, special)");
        }
        private void OnEnable()
        {
            try
            {
                Harmony.PatchAll();
                Logger.LogInfo($"Loaded {PluginName}!");
            }
            catch (Exception e)
            {
                Log.LogError("An error was found! Winrate trackers now require WECCL, make sure you have it too! Error:");
                Log.LogError(e);
                this.enabled = false;
            }
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }
        public static void SaveWinrateToFile()
        {
            using (StreamWriter file = File.CreateText(Path.Combine(Paths.ConfigPath, "CareerWinrates.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Winrates);
            }
        }
        public static void LoadWinrateFromFile()
        {
            try
            {
                if (File.Exists(Path.Combine(Paths.ConfigPath, "CareerWinrates.json")))
                {
                    using (StreamReader file = File.OpenText(Path.Combine(Paths.ConfigPath, "CareerWinrates.json")))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        Winrates = (Dictionary<int, CharacterWinrate>)serializer.Deserialize(file, typeof(Dictionary<int, CharacterWinrate>));
                        if (Winrates == null)
                        {
                            Debug.LogError("Failed to load the winrates for an unknown reason.");
                            Winrates = new();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogError("An error has occured while trying to load the winrates from a file. Character winrates will be reset.");
                Log.LogError(e);
                Winrates = new();
            }

        }
        public static void ReloadCharacterWinratesSingle()
        {
            if (Characters.wrestler == 0 || NAEEIFNFBBO.CBMHGKFFHJE != 1) return;
            LoadWinrateFromFile();
            if (!Winrates.ContainsKey(Characters.wrestler))
            {
                Winrates.Add(Characters.wrestler, new CharacterWinrate());
            }
            if(Winrates.Count > 1)
            {
                foreach (KeyValuePair<int, CharacterWinrate> characterWinrate in Winrates.ToList())
                {
                    if (characterWinrate.Key != Characters.wrestler)
                    {
                        Winrates.Remove(characterWinrate.Key);
                    }
                }
            }
            SaveWinrateToFile();

        }

        public static bool LoadWinrate(int id)
        {
            CharacterWinrate winrate;
            bool success = Winrates.TryGetValue(id, out winrate);
            if (success)
            {
                if (AdvancedDisplay.Value == true)
                {
                    RateText.GetComponent<Text>().text = "S: " + winrate.Singles.Wins + "-" + winrate.Singles.Draws + "-" + winrate.Singles.Loses + "; T: " + winrate.Team.Wins + "-" + winrate.Team.Draws + "-" + winrate.Team.Loses + "; Sp: " + winrate.Special.Wins + "-" + winrate.Special.Draws + "-" + winrate.Special.Loses;
                }
                else
                {
                    RateText.GetComponent<Text>().text = "W-D-L: " + winrate.GetTotalWins() + "-" + winrate.GetTotalDraws() + "-" + winrate.GetTotalLoses();
                }
            }
            return success;
        }
        public static void SetUpGameObjects()
        {
            WinRate = UnityEngine.GameObject.Instantiate(Characters.gProfile.transform.Find("Header").gameObject);
            foreach (Transform child in WinRate.transform)
            {
                UnityEngine.GameObject.Destroy(child.gameObject);
            }
            UnityEngine.GameObject.Destroy(WinRate.GetComponent<UnityEngine.UI.Image>());
            WinRate.transform.SetParent(Characters.gProfile.transform);
            WinRate.transform.position = Characters.gProfile.transform.Find("Header").position;
            WinRate.transform.localScale = Characters.gProfile.transform.Find("Header").localScale;
            WinRate.transform.localPosition = new Vector3(WinRate.transform.localPosition.x, -130, WinRate.transform.localPosition.z);

            RateText = UnityEngine.GameObject.Instantiate(Characters.gProfile.transform.Find("Header/Name").gameObject);
            RateText.transform.SetParent(WinRate.transform);
            RateText.GetComponent<UnityEngine.UI.Text>().text = "W-D-L: x-x-x";
            RateText.transform.localPosition = new Vector3(128, 0, 0);
            RateText.transform.localScale = Characters.gProfile.transform.Find("Header/Name").localScale;
        }
        [HarmonyPatch(typeof(WECCL.Utils.CharacterUtils), nameof(CharacterUtils.DeleteCharacter))]  //move winrate id on char deletion
        [HarmonyPrefix]
        static void CWinrate_UpdateOnDelete(int id)
        {
            ReloadCharacterWinratesSingle();
            if (Winrates.Count > 0)
            {
                if (id < Winrates.First().Key)
                {
                    int newId = Winrates.First().Key - 1;
                    var wins = Winrates.First().Value;
                    Winrates.Remove(Winrates.First().Key);
                    Winrates.Add(newId, wins);
                }
                
            }
            SaveWinrateToFile();
        }
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.Start))]  //setup the game objects
        [HarmonyPostfix]
        static void Scene_Select_Char_Setup()
        {
            SetUpGameObjects();
            ReloadCharacterWinratesSingle();
        }
        [HarmonyPatch(typeof(Scene_News), nameof(Scene_News.Start))]  //setup the game objects
        [HarmonyPostfix]
        static void Scene_News_Setup()
        {
            SetUpGameObjects();
            ReloadCharacterWinratesSingle();
        }
        [HarmonyPatch(typeof(Scene_Calendar), nameof(Scene_Calendar.Start))]  //setup the game objects
        [HarmonyPostfix]
        static void Scene_Calendar_Setup()
        {
            if(NAEEIFNFBBO.CBMHGKFFHJE != 1) return;
            SetUpGameObjects();
            ReloadCharacterWinratesSingle();
        }
        [HarmonyPatch(typeof(Scene_Calendar), nameof(Scene_Calendar.Update))]  //Update the score
        [HarmonyPostfix]
        static void Scene_Calendar_Update()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE != 1) return;
            if (Characters.star == 0 || NAEEIFNFBBO.CBMHGKFFHJE != 1)
            {
                WinRate.SetActive(false);
                return;
            }
            if (Characters.foc == Characters.star)
            {
                WinRate.SetActive(true);
                if (!LoadWinrate(Characters.c[Characters.foc].id)) WinRate.SetActive(false);
            }
            else
            {
                WinRate.SetActive(false);
            }
        }
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.Update))]  //Update the score
        [HarmonyPostfix]
        static void Scene_Select_Char_Update()
        {
            if (Characters.star == 0 || NAEEIFNFBBO.CBMHGKFFHJE != 1)
            {
                WinRate.SetActive(false);
                return;
            }
            if (Characters.foc == Characters.star)
            {
                WinRate.SetActive(true);
                if(!LoadWinrate(Characters.c[Characters.foc].id)) WinRate.SetActive(false);
            }
            else
            {
                WinRate.SetActive(false);
            }
        }
        [HarmonyPatch(typeof(Scene_News), nameof(Scene_Select_Char.Update))]  //Update the score
        [HarmonyPostfix]
        static void Scene_News_Update()
        {
            if (Characters.star == 0 || NAEEIFNFBBO.CBMHGKFFHJE != 1)
            {
                WinRate.SetActive(false);
                return;
            }
            if (Characters.c[IMNHOCBFGHJ.OLMOLOOOIJM[IMNHOCBFGHJ.ODOAPLMOJPD].GOOKPABIPBC[1]].id == Characters.star)
            {
                WinRate.SetActive(true);
                if (!LoadWinrate(Characters.c[IMNHOCBFGHJ.OLMOLOOOIJM[IMNHOCBFGHJ.ODOAPLMOJPD].GOOKPABIPBC[1]].id)) WinRate.SetActive(false);
            }
            else
            {
                WinRate.SetActive(false);
            }
        }
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.BAGEPNPJPLD))]  //Match end, might still restart
        [HarmonyPostfix]
        static void FFCEGMEAIBP_BAGEPNPJPLD_Postfix(int KJELLNJFNGO)  //
        {
            winners = new();
            draws = new();
            losers = new();
            if (NAEEIFNFBBO.CBMHGKFFHJE != 1) return;
            //if team mode, add winning team participants to winners, others to losers
            //if singles, add winner to winners, others to losers
            //if draw, add all to draws
            if(FFCEGMEAIBP.OOODPHNGHGD == 0) //draw
            {
                foreach (DFOGOCNBECG character in NJBJIIIACEP.OAAMGFLINOB)
                {
                    if(character.FIEMGOLBHIO == 1)
                    {
                        draws.Add(character.GOOKPABIPBC);
                    }
                }
            }
            else //someone won
            {
                foreach (DFOGOCNBECG character in NJBJIIIACEP.OAAMGFLINOB)
                {
                    if (character.FIEMGOLBHIO == 1)
                    {
                        if (FFCEGMEAIBP.OLJFOJOLLOM > 0)//team mode
                        {
                            if (character.LBCFAJGDKJP == NJBJIIIACEP.OAAMGFLINOB[KJELLNJFNGO].LBCFAJGDKJP)
                            {
                                winners.Add(character.GOOKPABIPBC);
                            }
                            else
                            {
                                losers.Add(character.GOOKPABIPBC);
                            }
                        }
                        else  //singles or countdowns
                        {
                            if(character == NJBJIIIACEP.OAAMGFLINOB[KJELLNJFNGO])
                            {
                                winners.Add(character.GOOKPABIPBC);
                            }
                            else
                            {
                                losers.Add(character.GOOKPABIPBC);
                            }
                        }
                    }
                }
            }
            //NJBJIIIACEP.OAAMGFLINOB[winner] //winner
        }
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.PMIIOCMHEAE))]
        [HarmonyPrefix]
        public static void LIPNHOMGGHF_PMIIOCMHEAE_Prefix()
        {
            if (Characters.star == 0 || NAEEIFNFBBO.CBMHGKFFHJE != 1) return;
            if (SceneManager.GetActiveScene().name != "Game") return;
            ReloadCharacterWinratesSingle();
            foreach(int i in winners)
            {
                if (Winrates.ContainsKey(i))
                {
                    if (FFCEGMEAIBP.OLJFOJOLLOM > 0)
                    {
                        Winrates[i].Team.Wins++;
                    }
                    else if (FFCEGMEAIBP.OLJFOJOLLOM == 0)
                    {
                        Winrates[i].Singles.Wins++;
                    }
                    else if (FFCEGMEAIBP.OLJFOJOLLOM < 0)
                    {
                        Winrates[i].Special.Wins++;
                    }
                }
            }
            foreach (int i in draws)
            {
                if (Winrates.ContainsKey(i))
                {
                    if (FFCEGMEAIBP.OLJFOJOLLOM > 0)
                    {
                        Winrates[i].Team.Draws++;
                    }
                    else if (FFCEGMEAIBP.OLJFOJOLLOM == 0)
                    {
                        Winrates[i].Singles.Draws++;
                    }
                    else if (FFCEGMEAIBP.OLJFOJOLLOM < 0)
                    {
                        Winrates[i].Special.Draws++;
                    }
                }
            }
            foreach (int i in losers)
            {
                if (Winrates.ContainsKey(i))
                {
                    if (FFCEGMEAIBP.OLJFOJOLLOM > 0)
                    {
                        Winrates[i].Team.Loses++;
                    }
                    else if (FFCEGMEAIBP.OLJFOJOLLOM == 0)
                    {
                        Winrates[i].Singles.Loses++;
                    }
                    else if (FFCEGMEAIBP.OLJFOJOLLOM < 0)
                    {
                        Winrates[i].Special.Loses++;
                    }
                }
            }
            SaveWinrateToFile();
            winners = new();
            draws = new();
            losers = new();
        }
    }

//NJBJIIIACEP.OAAMGFLINOB[1].FIEMGOLBHIO == 1 wrestler; 0 announcer; 2 manager?; 3 ref
}
/* Setup
 * 
 * GameObject WinRate = UnityEngine.GameObject.Instantiate(Characters.gProfile.transform.Find("Header").gameObject);
foreach (Transform child in WinRate.transform) {
     UnityEngine.GameObject.Destroy(child.gameObject);
 }
UnityEngine.GameObject.Destroy(WinRate.GetComponent<UnityEngine.UI.Image>());
WinRate.transform.SetParent(Characters.gProfile.transform);
WinRate.transform.position = Characters.gProfile.transform.Find("Header").position;
WinRate.transform.localScale = Characters.gProfile.transform.Find("Header").localScale;
WinRate.transform.localPosition = new Vector3(WinRate.transform.localPosition.x, -130, WinRate.transform.localPosition.z);
GameObject RateText = UnityEngine.GameObject.Instantiate(Characters.gProfile.transform.Find("Header/Name").gameObject);
RateText.transform.SetParent(WinRate.transform);
RateText.GetComponent<UnityEngine.UI.Text>().text = "W-D-L: x-x-x";
RateText.transform.localPosition = new Vector3(128, 0, 0);*/


/* winrate tracking?
 */

/* loading the winrate?
 */

/* other handling?*/