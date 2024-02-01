//TODO: Exp barbed wire ropes?; Fix the bomb timer applying? to career; Better timer display for bombs?; seconds in bomb timer?;
//FIX: Bomb timer in countdown matches; bomb not exploding in countdown matches  -> custom timer using FFCEGMEAIBP.OKNLFAFHAAF (miliseconds?);
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
        public const string PluginVer = "1.0.1";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        public static ConfigEntry<float> configSize;
        public static ConfigEntry<int> configBurstSize;
        public static ConfigEntry<float> configTimeInBurst;
     //   public static ConfigEntry<bool> configRepeat;
     //   public static ConfigEntry<int> configSeconds;
        public static ConfigEntry<bool> configMatch;
        public static int LastExplosion = 0;

        public static int? TimeBombID = null;
        public static bool active = false;
        public static int time;
        public static int oldtime;
        public static Plugin plugin;


        public static int BombTimerButton = -1;
        public static int BombRepeatButton = -1;
        public static bool ButtonsActive = false;

        public static int RepeatMinutes = 1;
        public static int RepeatSeconds = 60;
        public static int RepeatExplosions = 1;


        private void Awake()
        {
            Plugin.Log = base.Logger;
            plugin = this;
            PluginPath = Path.GetDirectoryName(Info.Location);

            configSize = Config.Bind("General",
             "ExplosionSize",
             30.5f,
             new ConfigDescription("Base size of an explosion (Scales with the ring size)")); //new AcceptableValueRange<float>(0.01f, float.MaxValue)));
            configBurstSize = Config.Bind("General",
             "BurstSize",
             3,
             new ConfigDescription("Amount of spawned explosions in the burst"));// new AcceptableValueRange<int>(1, int.MaxValue)));
            configTimeInBurst = Config.Bind("General",
             "BurstExplosionTime",
             0.5f,
             new ConfigDescription("Time in seconds between explosions in a single burst (might be slightly inaccurate during long bursts)"));// new AcceptableValueRange<float>(0.01f, float.MaxValue)));
            /*          configRepeat = Config.Bind("General",
                       "Repeat",
                       true,
                       "Repeat explosions on the interval");*/
            /*           configSeconds = Config.Bind("General",
                        "ExplosionInterval",
                        10,
                        "Time in seconds between explosions");*/
        }
        private void Start()
        {
            TimeBombID = CustomMatch.RegisterCustomPreset("TimeBomb", true);
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
        [HarmonyPatch(typeof(FFCEGMEAIBP), nameof(FFCEGMEAIBP.DMJFCHKLEFH))]
        [HarmonyPostfix]
        public static void FFCEGMEAIBP_DMJFCHKLEFH()
        {
            if (FFCEGMEAIBP.CBIPLGLDCAG == TimeBombID)
            {
                active = true;
            }
        }
        public static void SetupButtons()
        {
            LIPNHOMGGHF.DFLLBNMHHIH();
            LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(3, "Bomb Timer", -375f, 230f, 1.6f, 1.6f);
            BombTimerButton = LIPNHOMGGHF.HOAOLPGEBKJ;

            LIPNHOMGGHF.DFLLBNMHHIH();
            LIPNHOMGGHF.FKANHDIMMBJ[LIPNHOMGGHF.HOAOLPGEBKJ].ICGNAJFLAHL(2, "Repeat Explosions", 375f, 230f, 1.6f, 1.6f);
            BombRepeatButton = LIPNHOMGGHF.HOAOLPGEBKJ;

            ButtonsActive = true;
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
                    SetupButtons();
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
                RepeatExplosions = Mathf.RoundToInt(LIPNHOMGGHF.FKANHDIMMBJ[BombRepeatButton].ODONMLDCHHF(RepeatExplosions, 1f, 10f, 0f, 1f, 1));
                if (RepeatExplosions == 0)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[BombRepeatButton].FFCNPGPALPD = "No";
                }
                if (RepeatExplosions == 1)
                {
                    LIPNHOMGGHF.FKANHDIMMBJ[BombRepeatButton].FFCNPGPALPD = "Yes";
                }
            }
            else
            {
                ButtonsActive = false;
            }
        }
        [HarmonyPatch(typeof(Scene_Game), nameof(Scene_Game.Update))]
        [HarmonyPostfix]
        private static void Scene_Game_Update()
        {
            if (FFCEGMEAIBP.CBIPLGLDCAG != TimeBombID) return;
            if (!active) return;
          //  if (FFCEGMEAIBP.LOBDMDPMFLK != 2) return;
            //FFCEGMEAIBP.LOBDMDPMFLK match state, 2 - in match
            int minutes = FFCEGMEAIBP.IBGAIDBHGED;
            int seconds = FFCEGMEAIBP.LCLHNINHLHO;
            time = minutes * 60 + seconds;
            if (time % RepeatSeconds == 0 && time != 0)
            {
                if (time != oldtime)
                {
                    //if match time end, explosion happen after
                    plugin.ModMakeExplosions();
                    if (!Convert.ToBoolean(RepeatExplosions))
                    {
                        active = false;
                    }
                }
            }
            oldtime = time;
        }
        public void ModMakeExplosions()
        {
           StartCoroutine(MakeExplosions());
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
                RepeatMinutes = 1;
                RepeatSeconds = 60;
                RepeatExplosions = 1;
                FFCEGMEAIBP.JPBHIEOKODO = 0; //? refs
                active = true;


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
        [HarmonyPatch(typeof(FFCEGMEAIBP))]
        public static class FFCEGMEAIBP_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch("ANMHOOBBIPL")]
            public static bool ANMHOOBBIPL_PrePatch(ref string __result, int DOEEMNKCGCA)
            {
                if (FFCEGMEAIBP.CBIPLGLDCAG == TimeBombID)
                {
                    string text = FFCEGMEAIBP.CMECDGMCMLC;
                    string text2 = "";
                    if (FFCEGMEAIBP.CBIPLGLDCAG >= 2 && FFCEGMEAIBP.CBIPLGLDCAG != 12 && ((FFCEGMEAIBP.FFABCMJINFF > 2 && FFCEGMEAIBP.OLJFOJOLLOM <= 0) || FFCEGMEAIBP.FFABCMJINFF > 5))
                    {
                        text2 = FFCEGMEAIBP.FFABCMJINFF + "-Man ";
                        if (FFCEGMEAIBP.OLJFOJOLLOM == -2 && LIPNHOMGGHF.FAKHAFKOBPB != 50)
                        {
                            text2 = FFCEGMEAIBP.FFABCMJINFF * 2 + "-Man ";
                        }
                    }
                    if (FFCEGMEAIBP.BPJFLJPKKJK < 5 && FFCEGMEAIBP.FFABCMJINFF > 2 && FFCEGMEAIBP.OLJFOJOLLOM == 0 && FFCEGMEAIBP.CBIPLGLDCAG != 12)
                    {
                        text2 = FFCEGMEAIBP.FFABCMJINFF + "-Way ";
                    }
                    if (text2 != "")
                    {
                        text = ((FFCEGMEAIBP.CBIPLGLDCAG != 2) ? (text2 + text) : text2.Replace(" ", ""));
                        if ((FFCEGMEAIBP.CBIPLGLDCAG == 2 || FFCEGMEAIBP.CBIPLGLDCAG == 12) && FFCEGMEAIBP.OLJFOJOLLOM > 0)
                        {
                            if (FFCEGMEAIBP.OLJFOJOLLOM == 1)
                            {
                                text = text2 + "Team";
                            }
                            if (FFCEGMEAIBP.OLJFOJOLLOM == 2)
                            {
                                text = text2 + "Tag Team";
                            }
                        }
                    }
                    else if ((FFCEGMEAIBP.CBIPLGLDCAG == 2 || FFCEGMEAIBP.CBIPLGLDCAG == 12) && FFCEGMEAIBP.OLJFOJOLLOM > 0)
                    {
                        if (FFCEGMEAIBP.OLJFOJOLLOM == 1)
                        {
                            text = "Team";
                        }
                        if (FFCEGMEAIBP.OLJFOJOLLOM == 2)
                        {
                            text = "Tag Team";
                        }
                    }

                    if (DOEEMNKCGCA > 0)
                    {
                        string text3 = FFCEGMEAIBP.IHGGIEKCDCB();
                        if (text3 != "")
                        {
                            text = ((!(text == "Singles") && !(text == text3) && (!text.Contains("Team") || !text3.Contains("Team"))) ? ("'" + text3 + "' " + text) : ("'" + text3 + "'"));
                            if (text.Contains("Championship"))
                            {
                                text = text.Replace("'", "");
                            }
                        }
                        string text4 = NAEEIFNFBBO.GGEAMHLEAHN(text).ToLower();
                        if (text4 != "fight" && text4 != "confrontation" && text4 != "signing" && text4 != "session" && text4 != "royal" && text4 != "contest" && text4 != "war")
                        {
                            text += " match";
                        }
                    }
                    else
                    {
                        text = text2 + "Time Bomb Match";
                    }

                    __result = text;
                    return false; // Skip the original method call
                }

                return true;
            }
        }
        [HarmonyPatch(typeof(LIPNHOMGGHF), nameof(LIPNHOMGGHF.ICGNAJFLAHL))]
        [HarmonyPostfix]
        public static void ICGNAJFLAHL_Patch()
        {
            ButtonsActive = false;
            /*   if (LIPNHOMGGHF.FAKHAFKOBPB == 14)
               {
                    if (LIPNHOMGGHF.CHLJMEPFJOK == 2)
                       {

                     }
               }*/

        }
        static IEnumerator MakeExplosions()
        {
            
            for (int i = 0; i < configBurstSize.Value; i++)
            {
                while (LIPNHOMGGHF.GCJKOBOBIGA == 1) yield return null;
                ALIGLHEIAGO.MDFJMAEDJMG(1, 2, new Color(1f, 1f, 1f), Plugin.configSize.Value * World.ringSize, null, 0f, 15f, 0f, 0f, 0f, 0f, 1);
                float waittime = configTimeInBurst.Value;
                if (Time.timeScale == 2) waittime *= 2;
                yield return new WaitForSeconds(waittime);
            }
        }
        [HarmonyPatch(typeof(LIPNHOMGGHF))]
        public static class LIPNHOMGGHF_Patch
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(LIPNHOMGGHF.PMIIOCMHEAE))]
            public static void Prefix()
            {
                Plugin.plugin.StopAllCoroutines();
            }
        }
    }
}