using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KeepBeltObjects
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.KeepBeltObjects";
        public const string PluginName = "KeepBeltObjects";
        public const string PluginVer = "1.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<bool> ForceTitleDefence;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);


            ForceTitleDefence = Config.Bind(
                "General",
                "Force Title Defences",
                false,
                "(Experimental) If enabled, the mod will force champions to defend all their titles (even from other feds) in any match they're in. Make sure to disable this option if you want to defend only a specific belt or for non-title matches. The belts will also get moved in case of a DQ win.");
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
        [HarmonyPatch(typeof(DFOGOCNBECG), nameof(DFOGOCNBECG.AMPHLBAOCKC))]
        [HarmonyPrefix]  //Belts on the character preview?
        static void DFOGOCNBECG_AMPHLBAOCKC_Prefix(DFOGOCNBECG __instance)
        {
            for (int i = 1; i <= Characters.no_feds; i++)
            {
                if (__instance.EMDMDLNJFKP.fed != i)
                {
                    for (int j = 1; j <= 4; j++)
                    {
                        if (Characters.fedData[i].champ[j, 1] == __instance.EMDMDLNJFKP.id)
                        {
                            __instance.PHFEKOGCLCL(-(i * 10 + j), 99);
                        }
                    }
                    if (Characters.fedData[i].champ[5, 1] == __instance.EMDMDLNJFKP.id)
                    {
                        __instance.PHFEKOGCLCL(-(i * 10 + 4), 99);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(JFLEBEBCGFA), nameof(JFLEBEBCGFA.AMPHLBAOCKC))]
        [HarmonyPrefix] //Giving the actual match character belts to carry
        static void JFLEBEBCGFA_AMPHLBAOCKC_Prefix()
        {
            for (int c = 1; c <= NJBJIIIACEP.NBBBLJDBLNM; c++)
            {
                DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[c];
                for (int i = 1; i <= Characters.no_feds; i++)
                {
                    if (dfogocnbecg.EMDMDLNJFKP.fed != i)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            if (Characters.fedData[i].champ[j, 1] == dfogocnbecg.EMDMDLNJFKP.id && FFCEGMEAIBP.JMBGHDFADHN != i)
                            {
                                dfogocnbecg.PHFEKOGCLCL(-(i * 10 + j), 99);
                            }

                        }
                        if ((Characters.fedData[i].champ[4, 1] == dfogocnbecg.EMDMDLNJFKP.id || Characters.fedData[i].champ[5, 1] == dfogocnbecg.EMDMDLNJFKP.id) && FFCEGMEAIBP.JMBGHDFADHN != 4)
                        {
                            dfogocnbecg.PHFEKOGCLCL(-(i * 10 + 4), 99);
                        }
                    }
                }
            }
        }
        public struct ChampBelt
        {
            public int charID;
            public int fedID;
            public int beltID;
        }
        public static List<ChampBelt> BeltsToDefend = new();
        public static Dictionary<int, List<ChampBelt>> Champs = new();

        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.OOMNLEIFFHM))]
        [HarmonyPostfix]  //early start of the match
        static void FFCEGMEAIBP_OOMNLEIFFHM_Postfix()
        {
            if (ForceTitleDefence.Value && SceneManager.GetActiveScene().name == "Game")
            {
                Champs.Clear();
                BeltsToDefend.Clear();
                if (FFCEGMEAIBP.OLJFOJOLLOM <= 0) // non-teams format
                {
                    for (int i = 1; i <= Characters.no_feds; i++)
                    {
                        for (int j = 1; j <= 3; j++)
                        {
                            int champ = Characters.fedData[i].champ[j, 1];
                            ChampBelt belt = new()
                            {
                                fedID = i,
                                beltID = j,
                                charID = champ
                            };
                            Champs.TryAdd(belt.charID, new List<ChampBelt>());
                            Champs[champ].Add(belt);
                        }
                    }
                    for (int c = 1; c <= NJBJIIIACEP.NBBBLJDBLNM; c++)
                    {
                        DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[c];
                        if (dfogocnbecg.FIEMGOLBHIO == 1 && Champs.ContainsKey(dfogocnbecg.EMDMDLNJFKP.id))
                        {
                            BeltsToDefend.AddRange(Champs[dfogocnbecg.EMDMDLNJFKP.id]);
                        }
                    }
                }
                else  //team format
                {

                    List<int> t1 = [];
                    List<int> t2 = [];
                    for (int c = 1; c <= NJBJIIIACEP.NBBBLJDBLNM; c++)
                    {
                        DFOGOCNBECG dfogocnbecg = NJBJIIIACEP.OAAMGFLINOB[c];
                        if (dfogocnbecg.FIEMGOLBHIO == 1 && dfogocnbecg.LBCFAJGDKJP == 1)
                        {
                            t1.Add(dfogocnbecg.GOOKPABIPBC);
                        }
                        if (dfogocnbecg.FIEMGOLBHIO == 1 && dfogocnbecg.LBCFAJGDKJP == 2)
                        {
                            t2.Add(dfogocnbecg.GOOKPABIPBC);
                        }
                    }
                    if (t1.Count != 2 && t2.Count != 2) return;
                    for (int i = 1; i <= Characters.no_feds; i++)
                    {
                        int b1 = Characters.fedData[i].champ[4, 1];
                        int b2 = Characters.fedData[i].champ[5, 1];
                        if (CheckTeamChamps(b1, b2, t1[0], t1[1]) || CheckTeamChamps(b1, b2, t2[0], t2[1]))
                        {
                            int champ = Characters.fedData[i].champ[4, 1];
                            ChampBelt belt = new()
                            {
                                fedID = i,
                                beltID = 4,
                                charID = champ
                            };
                            BeltsToDefend.Add(belt);
                        }
                    }

                }
                foreach (var item in BeltsToDefend)
                {
                    UnityEngine.Debug.LogWarning($"{item.fedID} {item.beltID}");
                }
            }
        }
        static bool CheckTeamChamps(int b1, int b2, int t1, int t2)
        {
            if (b1 == t1 && b2 == t2) return true;
            if (b1 == t2 && b2 == t1) return true;
            return false;
        }
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.BAGEPNPJPLD))]
        [HarmonyPostfix]  //match end
        static void FFCEGMEAIBP_BAGEPNPJPLD_Postfix(int KJELLNJFNGO)
        {
            if (ForceTitleDefence.Value && KJELLNJFNGO > 0)
            {
                DFOGOCNBECG winner = NJBJIIIACEP.OAAMGFLINOB[KJELLNJFNGO];
                if (FFCEGMEAIBP.OLJFOJOLLOM <= 0) // non-teams format
                {
                    
                    foreach (var item in BeltsToDefend)
                    {
                        if (Characters.fedData[item.fedID].champ[item.beltID, 1] != winner.EMDMDLNJFKP.id)
                        {
                            Characters.fedData[item.fedID].EGIGCIPNDCJ(item.beltID, winner.EMDMDLNJFKP.id, 0);
                            ALIGLHEIAGO.LOAEIDIDECL();
                        }
                    }
                }
                else //team format
                {
                    int partner = winner.GOOKPABIPBC;
                    int i = 1;
                    while (i <= NJBJIIIACEP.NBBBLJDBLNM)
                    {
                        if (i != FFCEGMEAIBP.OOODPHNGHGD && winner.NEMJMNEGAAH(i) > 0 && NJBJIIIACEP.OAAMGFLINOB[i].FIEMGOLBHIO == 1 && partner == winner.GOOKPABIPBC)
                        {
                            partner = NJBJIIIACEP.OAAMGFLINOB[i].GOOKPABIPBC;
                        }
                        i++;
                    }
                    foreach (var item in BeltsToDefend)
                    {
                        int b1 = Characters.fedData[item.fedID].champ[4, 1];
                        int b2 = Characters.fedData[item.fedID].champ[5, 1];
                        int t1 = winner.EMDMDLNJFKP.id;
                        int t2 = partner;
                        if (!CheckTeamChamps(b1, b2, t1, t2))
                        {
                            SetNewChamps(Characters.fedData[item.fedID], 4, winner.EMDMDLNJFKP.id, partner);
                            ALIGLHEIAGO.LOAEIDIDECL();
                        }
                    }
                }
            }
        }
        public static void SetNewChamps(Roster roster, int NKEDCLBOOMJ, int GOOKPABIPBC, int AJBBDOOCFDG = 0)  //Copied from vanilla, removed character.FBIIPBMKAGK(4) == 0 checks
        {
            if (GOOKPABIPBC > 0)
            {
                Character character = Characters.c[GOOKPABIPBC];
                if ((NKEDCLBOOMJ <= 3 && roster.champ[NKEDCLBOOMJ, 1] != GOOKPABIPBC) || (NKEDCLBOOMJ >= 4) || (NKEDCLBOOMJ >= 4 && AJBBDOOCFDG > 0))
                {
                    for (int i = 10; i >= 2; i--)
                    {
                        roster.champ[NKEDCLBOOMJ, i] = roster.champ[NKEDCLBOOMJ, i - 1];
                        if (NKEDCLBOOMJ == 4)
                        {
                            roster.champ[NKEDCLBOOMJ + 1, i] = roster.champ[NKEDCLBOOMJ + 1, i - 1];
                        }
                        roster.champDate[NKEDCLBOOMJ, i] = roster.champDate[NKEDCLBOOMJ, i - 1];
                    }
                    roster.champ[NKEDCLBOOMJ, 1] = GOOKPABIPBC;
                    if (NKEDCLBOOMJ == 0)
                    {
                        roster.booker = GOOKPABIPBC;
                    }
                    if (NKEDCLBOOMJ == 4)
                    {
                        roster.champ[NKEDCLBOOMJ + 1, 1] = AJBBDOOCFDG;
                    }
                    roster.champDate[NKEDCLBOOMJ, 1] = Progress.CNAMECMHPLK(Progress.date, Progress.year, 0);
                    if (NAEEIFNFBBO.CBMHGKFFHJE == 1 && (GOOKPABIPBC == Characters.wrestler || AJBBDOOCFDG == Characters.wrestler) && NKEDCLBOOMJ >= 1 && NKEDCLBOOMJ <= 4 && character.fed > 0)
                    {
                        Progress.titles[character.fed, NKEDCLBOOMJ]++;
                        Progress.trophies[character.fed, NKEDCLBOOMJ]++;
                        Debug.Log("Recording title #" + Progress.titles[character.fed, NKEDCLBOOMJ].ToString() + " for star at fed=" + character.fed.ToString());
                    }
                }
            }
        }
    }
}