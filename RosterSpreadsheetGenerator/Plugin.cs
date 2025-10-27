using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using WECCL.API;

namespace RosterSpreadsheetGenerator
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.RosterSpreadsheetGenerator";
        public const string PluginName = "RosterSpreadsheetGenerator";
        public const string PluginVer = "1.1.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);


            Buttons.RegisterCustomButton(this, "Generate Character CSV", () =>
            {
                GenerateCharacterCSV();
                return "Character CSV spreadsheet generated!";
            });

            Buttons.RegisterCustomButton(this, "Generate Promotion CSV", () =>
            {
                GeneratePromotionCSV();
                return "Promotion CSV spreadsheet generated!";
            });

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
        public static void GeneratePromotionCSV()
        {
            using (var stream = new StreamWriter(Path.Combine(PluginPath, "PromotionCSV.csv")))
            {
                stream.WriteLine("ID,Name,Owner,Booker,Home territory,Ratings,Bank,Size,1st belt,1st champ,2nd belt,2nd champ,3rd belt,3rd champ,4th belt,4th champ1,4th champ2");
                for (int i = 0; i < Characters.fedData.Length; i++)
                {
                    var fed = Characters.fedData[i];
                    int id = fed.id;
                    string name = fed.name;
                    string owner = fed.owner == 0 ? "None" : Characters.c[fed.owner].name;
                    string booker = fed.booker == 0 ? "None" : Characters.c[fed.booker].name;
                    string territory = Atlas.name[fed.home];
                    float rating = fed.rating;
                    int bank = fed.bank;
                    int size = fed.size;
                    string b1 = fed.beltName[1];
                    string c1 = fed.champ[1,1] == 0 ? "None" : Characters.c[fed.champ[1, 1]].name;
                    string b2 = fed.beltName[2];
                    string c2 = fed.champ[2, 1] == 0 ? "None" : Characters.c[fed.champ[2, 1]].name;
                    string b3 = fed.beltName[3];
                    string c3 = fed.champ[3, 1] == 0 ? "None" : Characters.c[fed.champ[3, 1]].name;
                    string b4 = fed.beltName[4];
                    string c4 = fed.champ[4, 1] == 0 ? "None" : Characters.c[fed.champ[4, 1]].name;
                    string c5 = fed.champ[5, 1] == 0 ? "None" : Characters.c[fed.champ[5, 1]].name;
                    stream.WriteLine($"{id},{name},{owner},{booker},{territory},{rating},{bank},{size},{b1},{c1},{b2},{c2},{b3},{c3},{b4},{c4},{c5}");
                }
            }

        }
        public static void GenerateCharacterCSV()
        {
            using (var stream = new StreamWriter(Path.Combine(PluginPath, "CharacterCSV.csv")))
            {
                stream.WriteLine("ID,Name,Height,Weight,Promotion,Role,Allegiance,Territory,Manager,Partner,Story friend,Story enemy,Front special,Rear special,Popularity,Strength,Skill,Agility,Stamina,Atitude");
                for (int i = 1; i < Characters.c.Length; i++)
                {
                    var c = Characters.c[i];
                    int id = c.id;
                    string name = c.name;
                    string height = c.DBAHDHJBCLN();
                    string weight = c.LFEPPKMEOEM();
                    string promotion = Characters.fedData[c.fed].name;
                    string role = "";
                    if (c.role == 1)
                    {
                        role = "Wrestler";
                    }
                    if (c.role == 2)
                    {
                        role = "Manager";
                    }
                    if (c.role == 3)
                    {
                        role = "Ref";
                    }
                    string align = "";
                    if (c.heel == 1)
                    {
                        align = "Heel";
                    }
                    else
                    {
                        align = "Face";
                    }
                    string territory = Atlas.name[c.home];
                    string manager = c.relationship[1] == 0 ? "None" : Characters.c[c.relationship[1]].name;
                    string partner = c.relationship[2] == 0 ? "None" : Characters.c[c.relationship[2]].name;
                    string st_f = c.relationship[3] == 0 ? "None" : Characters.c[c.relationship[3]].name;
                    string st_e = c.relationship[4] == 0 ? "None" : Characters.c[c.relationship[4]].name;
                    string f_sp = MBLIOKEDHHB.DDIJBPJLEBF(c.moveFront[0]);
                    string b_sp = MBLIOKEDHHB.DDIJBPJLEBF(c.moveBack[0]);
                    float Popularity = c.stat[1];
                    float Strength = c.stat[2];
                    float Skill = c.stat[3];
                    float Agility = c.stat[4];
                    float Stamina = c.stat[5];
                    float Atitude = c.stat[6];
                    stream.WriteLine($"{id},{name},{height},{weight},{promotion},{role},{align},{territory},{manager},{partner},{st_f},{st_e},{f_sp},{b_sp},{Popularity},{Strength},{Skill},{Agility},{Stamina},{Atitude}");
                }
            }

        }
    }
}