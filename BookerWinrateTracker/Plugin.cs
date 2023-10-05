using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BookerWinrateTracker
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.BookerWinrateTracker";
        public const string PluginName = "BookerWinrateTracker";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static GameObject WinRate;
        public static GameObject RateText;

        public static List<int> winners;
        public static List<int> losers;
        public static List<int> draws;
        public static bool readytosave = false;
        public static Dictionary<int, CharacterWinrate> Winrates;


       // public static List<CharacterWinrate> Winrates; /// <summary>
        /// ///////CHAJNGE INTO DICTIONARY
        /// </summary>

        public class CharacterWinrate
        {
            public int Wins;
            public int Draws;
            public int Loses;
            public CharacterWinrate()
            {
                Wins = 0; Draws = 0; Loses = 0;
            }
        }



        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
            Winrates = new();
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
        public static void SaveWinrateToFile()
        {
            using (StreamWriter file = File.CreateText(Path.Combine(PluginPath, "Winrates.json")))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Winrates);
            }
        }
        public static void LoadWinrateFromFile()
        {
            using (StreamReader file = File.OpenText(Path.Combine(PluginPath, "Winrates.json")))
            { 
                JsonSerializer serializer = new JsonSerializer();
                Winrates = (Dictionary<int, CharacterWinrate>)serializer.Deserialize(file, typeof(Dictionary<int, CharacterWinrate>));
                if (Winrates == null) Winrates = new();
            }
        }
        public static void ReloadCharacterWinrates()
        {
            if (Characters.booker == 0 || LFNJDEGJLLJ.NHDABIOCLFH != 2) return;
            LoadWinrateFromFile();
            foreach (KeyValuePair<int, CharacterWinrate> characterWinrate in Winrates.ToList())
            {
                if (Characters.c[characterWinrate.Key].fed != Characters.c[Characters.booker].fed)
                {
                    Winrates.Remove(characterWinrate.Key);
                }
            }
            foreach(int character in Characters.fedData[Characters.c[Characters.booker].fed].roster)
            {
                if(character != 0 && Characters.c[character].fed == Characters.c[Characters.booker].fed && !Winrates.ContainsKey(character))
                {
                    Winrates.Add(character, new CharacterWinrate());
                }
            }
            SaveWinrateToFile();

        }
        public static bool LoadWinrate(int id)
        {
            CharacterWinrate winrate;
            bool success = Winrates.TryGetValue(id, out winrate);
            if(success) RateText.GetComponent<Text>().text = "W-D-L: " + winrate.Wins + "-" + winrate.Draws + "-" + winrate.Loses;
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
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.Start))]  //setup the game objects
        [HarmonyPostfix]
        static void Scene_Select_Char_Setup()
        {
            SetUpGameObjects();
            ReloadCharacterWinrates();
        }
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.Update))]  //Update the score
        [HarmonyPostfix]
        static void Scene_Select_Char_Update()
        {
            if (Characters.booker == 0 || LFNJDEGJLLJ.NHDABIOCLFH != 2)
            {
                WinRate.SetActive(false);
                return;
            }
            if (Characters.c[Characters.foc].fed == Characters.c[Characters.booker].fed)
            {
                WinRate.SetActive(true);
                if(!LoadWinrate(Characters.c[Characters.foc].id)) WinRate.SetActive(false);
            }
            else
            {
                WinRate.SetActive(false);
            }
        }
        [HarmonyPatch(typeof(PHECEOMIMND), nameof(PHECEOMIMND.HDKPGMAKCLO))]  //Match end, might still restart
        [HarmonyPostfix]
        static void PHECEOMIMND_HDKPGMAKCLO_Postfix(int winner)  //
        {

            if (LFNJDEGJLLJ.NHDABIOCLFH != 2) return;
            //if team mode, add winning team participants to winners, others to losers
            //if singles, add winner to winners, others to losers
            //if draw, add all to draws
            if(PHECEOMIMND.PPCJLFLDAKP == 0) //draw
            {
                //PHECEOMIMND.DPALBBPCJKD - wrestler count? starters, not counting interference entrants
               // FFKMIEMAJML.FJCOPECCEKN
            }
            //FFKMIEMAJML.FJCOPECCEKN[winner] //winner
        }
    }


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