using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DiscordRichPresence
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.DiscordRichPresence";
        public const string PluginName = "DiscordRichPresence";
        public const string PluginVer = "1.0.3.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public Discord.Discord discord;
        int interval = 10;

        static long TimeStamp;

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
        }
        private void OnEnable()
        {
            try
            {
                Harmony.PatchAll();
                discord = new Discord.Discord(1227585524668170372, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
                var activityManager = discord.GetActivityManager();
                TimeStamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                var activity = new Discord.Activity
                {
                    Details = "Starting the game",
                    // State = "State123456",
                    Assets = new() { LargeImage = "logo" },
                    Timestamps = new() { Start = TimeStamp }
                };
                activityManager.UpdateActivity(activity, (res) =>
                {
                    if (res == Discord.Result.Ok)
                    {
                        // Debug.LogError("Everything is fine!");
                    }
                    else
                    {
                        Log.LogError(res);
                        this.enabled = false;
                    }
                });

                Logger.LogInfo($"Loaded {PluginName}!");
            }
            catch (Exception e)
            {
                Log.LogError(e);
                this.enabled = false;
            }
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }
        // Update is called once per frame
        float timePassed = 0f;
        void Update()
        {
            timePassed += Time.deltaTime;
            if (timePassed > 10f)
            {
                UpdateStatus();
                timePassed = 0f;
            }
            if(discord != null)
                discord.RunCallbacks();
            else
            {
                Log.LogError("Failed to connect to Discord!");
                this.enabled = false;
            }
        }
        private void UpdateStatus()
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity {Assets = new() { LargeImage = "logo"}, Timestamps = new() {Start = TimeStamp } };



            //FFCEGMEAIBP.LOBDMDPMFLK 0 roam 1 ent 2 match <0 match end
            //NAEEIFNFBBO.CBMHGKFFHJE 0 exhi 1 carr 2 booker
            //NAEEIFNFBBO.EPNLHPHFEOB 1 roam???????
            if(SceneManager.GetActiveScene().name == "Game" || SceneManager.GetActiveScene().name == "Map")
            {
                switch (NAEEIFNFBBO.CBMHGKFFHJE)
                {
                    case 0:
                        activity.Details = "In Exhibition Mode";
                        break;
                    case 1:
                        activity.Details = "In Career Mode";
                        break;
                    case 2:
                        activity.Details = "In Booker Mode";
                        break;
                }
                switch (FFCEGMEAIBP.LOBDMDPMFLK)
                {
                    case 0:
                        activity.State = "Free Roaming in " + World.CIHOMEHEECL(World.location);
                        break;
                    case 1:
                        activity.State = "In " + FFCEGMEAIBP.IHGGIEKCDCB() + " " + FFCEGMEAIBP.ANMHOOBBIPL() + " Match Entrance";
                        break;
                    case 2:
                        activity.State = "In " + FFCEGMEAIBP.IHGGIEKCDCB() + " " + FFCEGMEAIBP.ANMHOOBBIPL() + " Match";
                        break;
                    case <0:
                        activity.State = "In Post " + FFCEGMEAIBP.IHGGIEKCDCB() + " " + FFCEGMEAIBP.ANMHOOBBIPL() + " Match";
                        break;
                }

                int PlayerNum = GetMainPlayerNum();
                if (PlayerNum != 0)
                {
                    activity.Assets.LargeText = "Playing As: " + NJBJIIIACEP.OAAMGFLINOB[PlayerNum].EMDMDLNJFKP.name;
                    string champ = CheckChamp(NJBJIIIACEP.OAAMGFLINOB[PlayerNum].EMDMDLNJFKP);
                    activity.Assets.SmallText = "In: " + Characters.fedData[NJBJIIIACEP.OAAMGFLINOB[PlayerNum].EMDMDLNJFKP.fed].name + " " + champ;
                    activity.Assets.SmallImage = "fed" + NJBJIIIACEP.OAAMGFLINOB[PlayerNum].EMDMDLNJFKP.fed.ToString("00");
                }
                else
                {
                    activity.Assets.LargeText = "Spectating the match";
                }
            }
            else
            {
                activity.Details = "In " + SceneManager.GetActiveScene().name + " Menu";
                switch (SceneManager.GetActiveScene().name)
                {
                    case "Editor":
                        activity.State = "Editing " + Characters.c[Characters.edit].name;
                        break;
                    case "Roster_Editor":
                        activity.State = "Editing " + Characters.fedData[Characters.fed].name;
                        break;
                }
            }


            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res == Discord.Result.Ok)
                {
                    //Debug.LogError("Everything is fine!");
                }
                else
                {
                    Log.LogError(res);

                }
            });
        }

        public string CheckChamp(Character character)
        {
            Roster fed = Characters.fedData[character.fed];
            for (int i = 0; i < fed.champ.GetLength(0); i++)
            {
                // Console.WriteLine(fed.champ[i, 1]);
                if (fed.champ[i, 1] == character.id)
                {

                    return "As " + fed.beltName[i==5? 4 : i] + " Champion";
                }
            }
            return "";
        }
        public int GetMainPlayerNum()
        {
            for (int j = 0; j <= HKJOAJOKOIJ.NGCNKGDDKGF; j++)
            {
                if (HKJOAJOKOIJ.NAADDLFFIHG[j].AHBNKMMMGFI > 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].BPJFLJPKKJK >= 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP > 0 && HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP <= NJBJIIIACEP.NBBBLJDBLNM)
                {
                    return HKJOAJOKOIJ.NAADDLFFIHG[j].FOAPDJMIFGP;
                }
            }
            return 0;
        }
    }
}