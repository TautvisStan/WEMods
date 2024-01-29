//TODO: Exp barbed wire ropes?

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using WECCL.API;

namespace TimeBomb
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "GeeEm.WrestlingEmpire.TimeBomb";
        public const string PluginName = "TimeBomb";
        public const string PluginVer = "1.0.0";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<float> configSize;
        public static ConfigEntry<int> configBurstSize;
        public static ConfigEntry<float> configTimeInBurst;
        public static ConfigEntry<bool> configRepeat;
     //   public static ConfigEntry<int> configSeconds;
        public static ConfigEntry<bool> configMatch;
        public static int LastExplosion = 0;

        public static int? TimeBombID = null;
        public static bool active = false;
        public static int time;
        public static int oldtime;


        public static int BombTimerButton = -1;
        public static int BombRepeatButton = -1;
        public static bool ButtonsActive = false;

        public static int RepeatMinutes = 5;
        public static int RepeatSeconds = 0;


        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);

            configSize = Config.Bind("General",
             "ExplosionSize",
             30.5f,
             "Base size of an explosion");
            configBurstSize = Config.Bind("General",
             "BurstSize",
             2,
             "Amount of spawned explosions in the burst");
            configTimeInBurst = Config.Bind("General",
             "BurstExplosionTime",
             0.5f,
             "Time between explosions in a single burst");
            configRepeat = Config.Bind("General",
             "Repeat",
             true,
             "Repeat explosions on the interval");
 /*           configSeconds = Config.Bind("General",
             "ExplosionInterval",
             10,
             "Time in seconds between explosions");*/
        }
        private void Start()
        {
            TimeBombID = WECCL.API.CustomMatch.RegisterCustomPreset("TimeBomb", true);
            if (TimeBombID == null)
            {
                Logger.LogError("Failed to connect to WECCL! Disabling mod.");
                this.enabled = false;
            }
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
       [HarmonyPatch(typeof(Scene_Match_Setup), nameof(Scene_Match_Setup.Update))]
       [HarmonyPostfix]
       public static void Scene_Match_Setup_Update()
        {

            if (FFCEGMEAIBP.CBIPLGLDCAG != TimeBombID)
            {
                if (ButtonsActive)
                {
                    LIPNHOMGGHF.ICGNAJFLAHL();
                    
                }
                ButtonsActive = false;
                return;
            }
            if (LIPNHOMGGHF.CHLJMEPFJOK == 2)
            {
                if(!ButtonsActive)
                {
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(3, "Bomb Timer", -375f, 230f, 1.6f, 1.6f);
                    LIPNHOMGGHF.DFLLBNMHHIH();
                    LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Repeat Explosions", 375f, 230f, 1.6f, 1.6f);
                    ButtonsActive = true;
                    for(int i = 1; i < LIPNHOMGGHF.FKANHDIMMBJ.Count(); i++)
                    {
                        AKFIIKOMPLL menu = LIPNHOMGGHF.FKANHDIMMBJ[i];
                        if (menu.NKEDCLBOOMJ == "Bomb Timer")
                        {
                            BombTimerButton = menu.PLFGKLGCOMD;
                        }
                        if (menu.NKEDCLBOOMJ == "Repeat Explosions")
                        {
                            BombRepeatButton = menu.PLFGKLGCOMD;
                        }
                    }
                }



              if(FFCEGMEAIBP.NBAFIEALMHN == 0)
                {
                    RepeatMinutes = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[BombTimerButton].ODONMLDCHHF(RepeatMinutes, 1f, 10f, 1f, 30f, 0));
                    RepeatSeconds = RepeatMinutes * 60;
                }
                else
                {
                    RepeatMinutes = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[BombTimerButton].ODONMLDCHHF(RepeatMinutes, 1f, 10f, 1f, (float)FFCEGMEAIBP.NBAFIEALMHN, 0));
                    RepeatSeconds = RepeatMinutes * 60;
                }
                LIPNHOMGGHF.FKANHDIMMBJ[BombTimerButton].FFCNPGPALPD = RepeatMinutes.ToString() + " minutes";
                //repeat explosions
            }
            else
            {
                ButtonsActive = false;
            }
        }
        private void Update()
        {
            if (FFCEGMEAIBP.CBIPLGLDCAG != TimeBombID) return;
            if (FFCEGMEAIBP.LOBDMDPMFLK != 2) return;
            //FFCEGMEAIBP.LOBDMDPMFLK match state, 2 - in match
            int minutes = FFCEGMEAIBP.IBGAIDBHGED;
            int seconds = FFCEGMEAIBP.LCLHNINHLHO;
            time = minutes * 60 + seconds;
            if (time % RepeatSeconds == 0 && time != 0)
            {
                if (time != oldtime)
                {
                    StartCoroutine(MakeExplosions());
                    if (!configRepeat.Value)
                    {
                        active = false;
                    }
                }
            }
            oldtime = time;
        }
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.JELMGJMKKEK))]
        [HarmonyPrefix]
        public static bool FFCEGMEAIBP_JELMGJMKKEK_Prefix(int FHNBKPCHIAM)
        {
            if (FHNBKPCHIAM == TimeBombID)
            {
                FFCEGMEAIBP.EBMPAEBEMNE = NAEEIFNFBBO.CBMHGKFFHJE;
                FFCEGMEAIBP.OHBEGHIIHJB = 0;
                FFCEGMEAIBP.AEKLGCEFIHM = 0;
                if (NAEEIFNFBBO.CBMHGKFFHJE > 0)
                {
                    FFCEGMEAIBP.JMBGHDFADHN = 0;
                }
                else
                {
                    FFCEGMEAIBP.JMBGHDFADHN = -1;
                }
                FFCEGMEAIBP.CBIPLGLDCAG = FHNBKPCHIAM;

                FFCEGMEAIBP.CMECDGMCMLC = "Time Bomb";
                FFCEGMEAIBP.LCCCCENGFOK = 2;
                FFCEGMEAIBP.JPBHIEOKODO = 1;
                FFCEGMEAIBP.BPJFLJPKKJK = 1;
                FFCEGMEAIBP.CADLONHABMC = 0;
                FFCEGMEAIBP.OLJFOJOLLOM = 0;
                FFCEGMEAIBP.LGHMLHICAFL = 2;
                FFCEGMEAIBP.DOLNEDHNKMM = 0;
                FFCEGMEAIBP.GDKCEGBINCM = 0;
                FFCEGMEAIBP.NBAFIEALMHN = 0;
                RepeatMinutes = 5;
                //RepeatMinutes
                //RepeatBomb



                if (FFCEGMEAIBP.NBAFIEALMHN > 0 && FFCEGMEAIBP.OLJFOJOLLOM >= 0)
                {
                    if (FFCEGMEAIBP.OLJFOJOLLOM == 2 && NAEEIFNFBBO.LPNCPCPAIAF == 2 && FFCEGMEAIBP.NBAFIEALMHN >= 10)
                    {
                        FFCEGMEAIBP.NBAFIEALMHN += 5;
                    }
                    if (NAEEIFNFBBO.LPNCPCPAIAF >= 3)
                    {
                        FFCEGMEAIBP.NBAFIEALMHN += 5;
                    }
                }
                if (FFCEGMEAIBP.LCCCCENGFOK > NAEEIFNFBBO.ILLMCDIFFON)
                {
                    FFCEGMEAIBP.LCCCCENGFOK = NAEEIFNFBBO.ILLMCDIFFON;
                }
                if (NAEEIFNFBBO.BNCCMMLOIML == 0 || FFCEGMEAIBP.LCCCCENGFOK >= NAEEIFNFBBO.ILLMCDIFFON)
                {
                    FFCEGMEAIBP.JPBHIEOKODO = 0;
                }
                FFCEGMEAIBP.FFABCMJINFF = FFCEGMEAIBP.LCCCCENGFOK;
                return false;
            }

            return true;
        }
        /*       [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
               [HarmonyPostfix]
               public static void ICGNAJFLAHL_Patch()
               {
                   if (LIPNHOMGGHF.FAKHAFKOBPB == 14)
                   {
                       if (LIPNHOMGGHF.CHLJMEPFJOK == 2)
                       {
                           LIPNHOMGGHF.DFLLBNMHHIH();
                           LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(3, "Bomb timer", -260f, 230f, 1.6f, 1.6f);
                       }
                   }
               }*/
        IEnumerator MakeExplosions()
        {
            for (int i = 0; i < configBurstSize.Value; i++)
            {
                ALIGLHEIAGO.MDFJMAEDJMG(1, 2, new Color(1f, 1f, 1f), Plugin.configSize.Value * World.ringSize, null, 0f, 15f, 0f, 0f, 0f, 0f, 1);
                yield return new WaitForSeconds(configTimeInBurst.Value);
            }
        }
    }
}