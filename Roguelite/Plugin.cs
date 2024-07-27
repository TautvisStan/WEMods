using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using WECCL.API;

namespace Roguelite
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.Roguelite";
        public const string PluginName = "Roguelite";
        public const string PluginVer = "0.9.9";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;


        static int RogueliteButton;
        static readonly int RogueliteNum = -123655;

        public static RogueliteSave save { get; set; } = null;

        public static ConfigEntry<string> RandomizerSeed { get; set; }
        public static ConfigEntry<bool> AdvancedDisplay { get; set; }
        public static ConfigEntry<int> MatchGenerationMethod { get; set; }
        public static Randomizer rng { get; set; } = null;
        public static int TotalMatches { get; set; } = 0;
        public static int CurrentMatch { get; set; } = 0;
        public static string DefaultSaveName { get; set; } = "RogueliteSave.json";
        private static bool Won { get; set; } = false;
        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            RandomizerSeed = Config.Bind("General",
             "Randomizer Seed",
             "",
             "If you set a seed here, the matches should always be the same based on the seed, as long as you pick the same character. Leave empty for no set seed.");

            AdvancedDisplay = Config.Bind("General",
             "Advanced Display",
             true,
             "If enabled, it will display the match counter next to the timer.");

            MatchGenerationMethod = Config.Bind("General",
             "Match generation method",
             1,
             new ConfigDescription("0 = all matches will generated at the start; 1 = matches will be generated during the run.", new AcceptableValueRange<int>(0, 1)));


            Buttons.RegisterCustomButton(this, "Delete Save", () =>
            {
                DeleteSave(DefaultSaveName);
                return "Save Deleted!";
            }, true);

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

        public static bool InCustomMode()
        {
            return NAEEIFNFBBO.CBMHGKFFHJE == RogueliteNum || LIPNHOMGGHF.BCKLOCJPIMD == RogueliteNum;
        }

        //adding new button to the main menu
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ICGNAJFLAHL_Patch()
        {
            if (LIPNHOMGGHF.FAKHAFKOBPB == 1)
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == 1)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(1, "Rogue Rumble", 200f, -300f, 1.5f, 1.5f);
                    RogueliteButton = LIPNHOMGGHF.HOAOLPGEBKJ;
                }
            }
        }

        //Setting up the main menu button redirect
        [HarmonyPatch(typeof(Scene_Titles), nameof(Scene_Titles.Update))]
        [HarmonyPostfix]
        public static void Scene_Titles_Update_Patch()
        {
            if (LIPNHOMGGHF.PIEMLEPEDFN >= 5 && LIPNHOMGGHF.ODOAPLMOJPD == 1 && NAEEIFNFBBO.EKFJCKLKELN != 1)
            {
                if (LIPNHOMGGHF.NNMDEFLLNBF == RogueliteButton)
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = RogueliteNum;
                    LIPNHOMGGHF.BCKLOCJPIMD = RogueliteNum;

                    save = LoadFromFile(DefaultSaveName);
                    if (save == null)
                        LIPNHOMGGHF.PMIIOCMHEAE(11); //char select
                    else
                    {
                        if (rng == null)
                        {
                            rng = new(save.seed.ToString());
                            rng.CatchUp(save.nums);
                        }

                        LIPNHOMGGHF.PMIIOCMHEAE(14); //match setup
                    }
                }
            }
        }

        //Scene nav stuff
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.PMIIOCMHEAE))]
        [HarmonyPrefix]
        public static void LIPNHOMGGHF_PMIIOCMHEAE_Patch(ref int KBEAJEIMNMI)
        {
            //Doing stuff when going match setup -> game
            if (SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == RogueliteNum && KBEAJEIMNMI == 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = RogueliteNum;
                FFCEGMEAIBP.EBMPAEBEMNE = 0;
                return;
            }

            //Doing stuff when exiting match setup
            if (SceneManager.GetActiveScene().name == "Match_Setup" && NAEEIFNFBBO.CBMHGKFFHJE == RogueliteNum && KBEAJEIMNMI != 50)
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
                LIPNHOMGGHF.BCKLOCJPIMD = 0;
                KBEAJEIMNMI = 1;   //titles
                return;
            }

            //Doing stuff when exiting match
            if (SceneManager.GetActiveScene().name == "Game" && LIPNHOMGGHF.BCKLOCJPIMD == RogueliteNum)
            {
                if(!Won)
                { 
                    NAEEIFNFBBO.CBMHGKFFHJE = RogueliteNum;
                    LIPNHOMGGHF.BCKLOCJPIMD = RogueliteNum;
                    KBEAJEIMNMI = 14;  //match setup redirect
                }
                else
                {
                    Won = false;
                    NAEEIFNFBBO.CBMHGKFFHJE = 0;
                    LIPNHOMGGHF.BCKLOCJPIMD = 0;
                    KBEAJEIMNMI = 1;   //titles
                }
                return;
            }

        }
        //disabling character switch in matches
        [HarmonyPatch(typeof(BJMGCKGNCHO), nameof(BJMGCKGNCHO.FEEFDEKKMBM))]
        [HarmonyPrefix]
        public static void BJMGCKGNCHO_FEEFDEKKMBM_Prefix()
        {
            if(InCustomMode())
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 1;
            }
        }
        [HarmonyPatch(typeof(BJMGCKGNCHO), nameof(BJMGCKGNCHO.FEEFDEKKMBM))]
        [HarmonyPostfix]
        public static void BJMGCKGNCHO_FEEFDEKKMBM_Postfix()
        {
            if (InCustomMode())
            {
                NAEEIFNFBBO.CBMHGKFFHJE = 0;
            }
        }

        //Setting up the char select back button redirect
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.Update))]
        [HarmonyPostfix]
        public static void Scene_Select_Char_Update_Patch()
        {
            if (LIPNHOMGGHF.PIEMLEPEDFN <= -5)
            {
                if (LIPNHOMGGHF.BCKLOCJPIMD == RogueliteNum)
                {
                    NAEEIFNFBBO.CBMHGKFFHJE = 0;
                    LIPNHOMGGHF.BCKLOCJPIMD = 0;
                    LIPNHOMGGHF.PMIIOCMHEAE(1);
                }
            }
        }
        //adding stat numbers next to the timer
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.HLEDBJJDLIA))]
        [HarmonyPostfix]
        public static void FFCEGMEAIBP_HLEDBJJDLIA_Patch()
        {
            if (InCustomMode() && AdvancedDisplay.Value == true)
            {
                if (FFCEGMEAIBP.PDEHCNAKBCG.text != "")
                    FFCEGMEAIBP.PDEHCNAKBCG.text = "Time: " + FFCEGMEAIBP.PDEHCNAKBCG.text + " | <color=Orange>Match: " + CurrentMatch + "/" + TotalMatches + "</color>";
            }
        }
        //Progressing once the player wins
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.BAGEPNPJPLD))]
        [HarmonyPostfix]
        public static void FFCEGMEAIBP_BAGEPNPJPLD_Patch(int KJELLNJFNGO)
        {
            if (InCustomMode())
            {
                if (NJBJIIIACEP.OAAMGFLINOB[KJELLNJFNGO].LBCFAJGDKJP == 1 || NJBJIIIACEP.OAAMGFLINOB[KJELLNJFNGO].GOOKPABIPBC == save.SelectedCharacter)
                {
                    save.matchesCompleted++;
                    if (save.matchesCompleted == TotalMatches)
                    {
                        FFCEGMEAIBP.PGPFHDLODFG[1].text = "Congrats for beating the mode!";
                        save.matchesCompleted = 0;
                        string filename = "CompletedRogue " + DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH'-'mm'-'ss'-'fff") + ".json";
                        SaveToFile(save, filename);
                        DeleteSave(DefaultSaveName);
                        Won = true;
                    }
                    else
                    {
                        SaveToFile(save, DefaultSaveName);
                    }
                }
            }
        }
        //disable interference and simulate buttons;
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.OPBBKBAJJHA))]
        [HarmonyPostfix]
        public static void LIPNHOMGGHF_OPBBKBAJJHA_Patch()
        {
            if (InCustomMode())
            {
                if (LIPNHOMGGHF.ODOAPLMOJPD == 0)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[3].AHBNKMMMGFI = 0;
                    LIPNHOMGGHF.FKANHDIMMBJ[4].AHBNKMMMGFI = 0;
                }
            }
        }
 
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Update))]
        [HarmonyPostfix]
        public static void Scene_Match_Setup_Update_Patch()
        {
            if (NAEEIFNFBBO.CBMHGKFFHJE == RogueliteNum)
            {
                if (LIPNHOMGGHF.CHLJMEPFJOK == 1 || LIPNHOMGGHF.CHLJMEPFJOK == 2 || LIPNHOMGGHF.CHLJMEPFJOK == 4)        //disable tabs in the match setup
                {
                    for (int i = 1; i <= LIPNHOMGGHF.HOAOLPGEBKJ; i++)
                    {
                        LIPNHOMGGHF.FKANHDIMMBJ[i].AHBNKMMMGFI = 0;
                    }
                }
                if (LIPNHOMGGHF.CHLJMEPFJOK == 3)       //disable character selection in cast menu
                {
                    for (int i = 1; i < NJBJIIIACEP.OAAMGFLINOB.Length; i++)
                    {
                        if (NJBJIIIACEP.OAAMGFLINOB[i] != null)
                        {
                            NJBJIIIACEP.OAAMGFLINOB[i].AHBNKMMMGFI = 0;
                        }
                    }
                }
                if (LIPNHOMGGHF.CHLJMEPFJOK == 5)
                {
                    if (LIPNHOMGGHF.PIEMLEPEDFN < 14)
                    {
                        for (int i = 1; i < NJBJIIIACEP.OAAMGFLINOB.Length; i++)
                        {
                            if (NJBJIIIACEP.OAAMGFLINOB[i] != null && NJBJIIIACEP.OAAMGFLINOB[i].GOOKPABIPBC == save.SelectedCharacter)
                            {
                                NJBJIIIACEP.OAAMGFLINOB[i].AHBNKMMMGFI = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < NJBJIIIACEP.OAAMGFLINOB.Length; i++)
                        {
                            if (NJBJIIIACEP.OAAMGFLINOB[i] != null)
                            {
                                NJBJIIIACEP.OAAMGFLINOB[i].AHBNKMMMGFI = 1;
                            }
                        }
                    }
                }
            }
        }

        //disable cast tab buttons
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.AddRandom))]
        [HarmonyPrefix]
        public static bool Scene_Match_Setup_AddRandom(Scene_Match_Setup __instance)
        {
            if (InCustomMode())
            {
                return false;
            }
            return true;
        }
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.RemoveCast))]
        [HarmonyPrefix]
        public static bool Scene_Match_Setup_RemoveCast(Scene_Match_Setup __instance, int GKNIAFAOLJK)
        {
            if (InCustomMode())
            {
                return false;
            }
            return true;
        }

        //Setting up the char selection button redirect
        [HarmonyPatch(typeof(Scene_Select_Char), nameof(Scene_Select_Char.EGPFEGLDMJM))]
        [HarmonyPostfix]
        public static void Scene_Select_Char_EGPFEGLDMJM_Patch(Scene_Select_Char __instance, int GOOKPABIPBC)
        {
            if (LIPNHOMGGHF.BCKLOCJPIMD == RogueliteNum)
            {
                save = new(GOOKPABIPBC);

                if (RandomizerSeed.Value != "")
                {
                    rng = new(RandomizerSeed.Value);
                }
                else
                {
                    rng = new();
                }
                save.seed = rng.seed;


                List<int> opponents = MatchGenerator.RandomizeOpponents(GOOKPABIPBC, rng);
                save.generationMode = MatchGenerationMethod.Value;
                if (save.generationMode == 0)
                {
                    save.matches = MatchGenerator.GenerateRandomMatches(opponents, rng);
                }
                else
                {
                    save.OpponentPool = opponents;
                }
                SaveToFile(save, DefaultSaveName);

                LIPNHOMGGHF.PMIIOCMHEAE(14);  //match setup

            }
        }

        //setting up the match
        [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Start))]
        [HarmonyPostfix]
        public static void Scene_Match_Setup_Start_Postfix(Scene_Match_Setup __instance)
        {
            if (InCustomMode())
            {
                if (save.generationMode == 1 && save.matchesCompleted == save.matches.Count)
                {
                    save.matches.Add(MatchGenerator.GenerateSingleMatch(rng, save.matchesCompleted + 1, save.OpponentPool, save.TeammatePool));
                    SaveToFile(save, DefaultSaveName);
                }
                MatchGenerator.SetupMatchRules(save.matches[save.matchesCompleted]);
                if(save.SelectedCharacter > Characters.no_chars)
                {
                    Log.LogWarning($"WARNING! Player id {save.SelectedCharacter} is out of range! They will be replaced by a random character!");
                    save.SelectedCharacter = UnityEngine.Random.Range(1, Characters.no_chars + 1);
                }
                MatchGenerator.SetupParticipants(save.SelectedCharacter, save.matches[save.matchesCompleted]);
                CurrentMatch = save.matchesCompleted+1;
                TotalMatches = CalcTotalMatches();
            }
        }

        public static void SaveToFile(RogueliteSave save, string filename)
        {
            save.nums = rng.nums;
            save.seed = rng.seed;
            using (StreamWriter file = File.CreateText(Path.Combine(Paths.ConfigPath, filename)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, save);
            }
        }
        public static void DeleteSave(string filename)
        {
            save = null;
            rng = null;
            try
            {
                if (File.Exists(Path.Combine(Paths.ConfigPath, filename)))
                {
                    File.Delete(Path.Combine(Paths.ConfigPath, filename));
                }
            }
            catch (Exception e)
            {
                Log.LogError(e);
            }

        }
        public static int CalcTotalMatches()
        {
            if (save.generationMode == 0) return save.matches.Count;
            if (save.generationMode == 1)
            {
                int nthTeamMatch = 5;
                int preparedMatches = save.matches.Count;
                int distributedOpponents = Mathf.FloorToInt(preparedMatches / nthTeamMatch) * (nthTeamMatch + 1) + (preparedMatches % nthTeamMatch);
                int totalOpponents = distributedOpponents + save.OpponentPool.Count;
                int matches = (Mathf.FloorToInt(totalOpponents / (nthTeamMatch + 1)) * nthTeamMatch) + (totalOpponents % (nthTeamMatch + 1));
                return matches;
            }
            return 0;
        }
        public static RogueliteSave LoadFromFile(string filename)
        {
            RogueliteSave save = null;
            try
            {
                if (File.Exists(Path.Combine(Paths.ConfigPath, filename)))
                {
                    using (StreamReader file = File.OpenText(Path.Combine(Paths.ConfigPath, filename)))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        save = (RogueliteSave)serializer.Deserialize(file, typeof(RogueliteSave));
                        if (save == null)
                        {
                            Log.LogError("Failed to load the save file for an unknown reason.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogError("An error has occured while trying to load the save.");
                Log.LogError(e);
            }
            return save;
        }
    }
}